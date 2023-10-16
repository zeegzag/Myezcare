

CREATE   PROCEDURE [dbo].[SandataAggregatorVisitData]
    @EventName NVARCHAR(MAX),
    @ScheduleID BIGINT,
    @ReasonCode NVARCHAR(MAX),
    @ActionCode NVARCHAR(MAX),
    @OrganizationID BIGINT,
    @SandataAggregator NVARCHAR(MAX)
AS
BEGIN

    DECLARE @IsDelete BIT = (
                                SELECT CASE WHEN @EventName = 'DeleteSchedule' THEN 1 ELSE 0 END
                            )
    DECLARE @Result INT = 0,
            @EmployeeID BIGINT,
            @Payor NVARCHAR(MAX),
            @Aggregator NVARCHAR(MAX),
            @IsBillVisit BIT,
            @NA VARCHAR(5) = '-NA-'

    SELECT @EmployeeID = agi.employeeid,
           @Payor = Isnull(agi.payorname, @NA),
           @Aggregator = Isnull(agi.claimprocessor, @NA),
           @IsBillVisit = CAST(CASE WHEN AGI.VisitBilledBy = @SandataAggregator THEN 1 ELSE 0 END AS BIT)
    FROM [dbo].[Getaggregatorinfo](@ScheduleID) AGI

    DECLARE @IsCA BIT,
	        @IsCO BIT,
            @ProviderQualifier NVARCHAR(MAX),
            @ProviderID NVARCHAR(MAX),
            @ProviderTaxID NVARCHAR(MAX),
            @LenProviderTaxID INT,
			@EmployeeQualifier NVARCHAR(MAX)

    SELECT @IsCA = CASE WHEN OrganizationState = 'CA' THEN 1 ELSE 0 END,
		   @IsCO = CASE WHEN OrganizationState = 'CO' THEN 1 ELSE 0 END,
           @ProviderQualifier = CASE WHEN OrganizationState in  ('CA','CO') THEN 'MedicaidID' ELSE 'Other' END,
           @ProviderID = sandatabusinessentitymedicaididentifier,
           @ProviderTaxID = billingprovider_ref02_referenceidentification,
           @LenProviderTaxID = Len(Isnull(billingprovider_ref02_referenceidentification, '')),
		   @EmployeeQualifier = CASE WHEN @IsCA = 1 OR @IsCO = 1 THEN 'EmployeeCustomID' ELSE 'EmployeeSSN' END
    FROM organizationsettings

    IF (
           @Aggregator = @SandataAggregator
           AND Isnull(@EmployeeID, 0) > 0
           AND @LenProviderTaxID > 0
       )
    BEGIN;
        EXEC [dbo].[CreateAndSaveBatchForSchedule] @ScheduleID = @ScheduleID

        ;WITH visits
        AS (SELECT
                (
                    SELECT @ProviderQualifier [ProviderIdentification.ProviderQualifier],
                           @ProviderID [ProviderIdentification.ProviderID],
                           r.FirstName [ClientFirstName],
                           LEFT(R.MiddleName, 1) [ClientMiddleInitial],
                           r.LastName [ClientLastName],
                           'ClientMedicaidID' [ClientQualifier],
                           AGI.BeneficiaryNumber [ClientMedicaidID],
                           AGI.BeneficiaryNumber [ClientIdentifier],
                           'False' [MissingMedicaidID],
                           CAST(FORMAT([dbo].[GetUTCDateTimeForOrg](GETDATE()), 'yyyyMMddHHmmssff') AS BIGINT) [SequenceID],
                           FORMAT(sm.ReferralID, '0000000000') [ClientOtherID],
                           AGI.TimezoneCode [ClientTimezone],
                           (
                               SELECT AGI.PayorSubmissionName [PayerID],
                                      AGI.CareTypeValue [PayerProgram],
                                      AGI.JurisdictionCode [JurisdictionID],
                                      AGI.ServiceCode [ProcedureCode],
                                      AGI.PayorShortName [ClientPayerID],
                                      FORMAT(RBA.StartDate, 'yyyy-MM-dd') [ClientEligibilityDateBegin],
                                      FORMAT(RBA.EndDate, 'yyyy-MM-dd') [ClientEligibilityDateEnd],
                                      FORMAT(RBA.StartDate, 'yyyy-MM-dd') [EffectiveStartDate],
                                      FORMAT(RBA.EndDate, 'yyyy-MM-dd') [EffectiveEndDate]
                               FROM ReferralBillingAuthorizations RBA
                               WHERE RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
                               FOR JSON PATH
                           ) AS ClientPayerInformation,
                           (
                               SELECT 'Home' [ClientAddressType],
                                      1 [ClientAddressIsPrimary],
                                      RC.[Address] [ClientAddressLine1],
                                      RC.ApartmentNo [ClientAddressLine2],
                                      NULL [ClientCounty],
                                      RC.City [ClientCity],
                                      RC.[State] [ClientState],
                                      LEFT(RC.ZipCode + '000000000', 9) [ClientZip],
                                      CASE WHEN @IsCA = 1 THEN NULL ELSE RC.Longitude END [ClientAddressLongitude],
                                      CASE WHEN @IsCA = 1 THEN NULL ELSE RC.Latitude END [ClientAddressLatitude]
                               FOR JSON PATH
                           ) AS ClientAddress
                    FROM schedulemasters SM
                        OUTER APPLY
                    (SELECT * FROM [dbo].[GetAggregatorInfo](SM.ScheduleID) ) AGI
                        LEFT JOIN Referrals R
                            ON sm.ReferralID = r.ReferralID
                        OUTER APPLY
                    (
                        SELECT TOP 1
                            C.*
                        FROM ContactMappings CM
                            INNER JOIN Contacts C
                                ON C.ContactID = CM.ContactID
                        WHERE CM.ContactTypeID = 1
                              AND CM.ReferralID = SM.ReferralID
                    ) RC
                    WHERE sm.employeeid > 0
                          AND sm.scheduleid = @ScheduleID
                    FOR JSON PATH
                ) [Client],
                (
                    SELECT @ProviderQualifier [ProviderIdentification.ProviderQualifier],
                           @ProviderID [ProviderIdentification.ProviderID],
                           @EmployeeQualifier [EmployeeQualifier],
                           CASE WHEN @IsCA = 1 THEN CONVERT(VARCHAR(MAX), e.EmployeeID) 
                                WHEN @IsCO = 1 THEN 'CON_IS_CO'
                                ELSE e.SocialSecurityNumber  END [EmployeeIdentifier],
                           FORMAT(sm.employeeid, '000000000') [EmployeeOtherID],
                           CAST(FORMAT([dbo].[GetUTCDateTimeForOrg](GETDATE()), 'yyyyMMddHHmmssff') AS BIGINT) [SequenceID],
                           e.SocialSecurityNumber [EmployeeSSN],
                           e.LastName [EmployeeLastName],
                           e.FirstName [EmployeeFirstName],
                           e.Email [EmployeeEmail]
                    FROM schedulemasters SM
                        LEFT JOIN employees E
                            ON sm.employeeid = e.employeeid
                    WHERE sm.employeeid > 0
                          AND sm.scheduleid = @ScheduleID
                    FOR JSON PATH
                ) [Employee],
                (
                    SELECT @ProviderQualifier [ProviderIdentification.ProviderQualifier],
                           @ProviderID [ProviderIdentification.ProviderID],
                           CONVERT(NVARCHAR(MAX), sm.scheduleid) [VisitOtherID],
                           CAST(FORMAT([dbo].[GetUTCDateTimeForOrg](GETDATE()), 'yyyyMMddHHmmssff') AS BIGINT) [SequenceID],
                           @EmployeeQualifier [EmployeeQualifier],
                           CASE WHEN @IsCA = 1 THEN CONVERT(VARCHAR(MAX), e.EmployeeID) 
                                WHEN @IsCO = 1 THEN 'CON_IS_CO'
                                ELSE e.SocialSecurityNumber  END [EmployeeIdentifier],
                           FORMAT(sm.employeeid, '000000000') [EmployeeOtherID],
                           'ClientMedicaidID' [ClientIDQualifier],
                           AGI.BeneficiaryNumber [ClientID],
                           FORMAT(sm.ReferralID, '0000000000') [ClientOtherID],

                                      --0 [VisitCancelledIndicator],  
                           AGI.PayorSubmissionName [PayerID],
                           AGI.CareTypeValue [PayerProgram],
                           AGI.ServiceCode [ProcedureCode],
                                      --NULL [Modifier1],  
                                      --NULL [Modifier2],  
                                      --NULL [Modifier3],  
                                      --NULL [Modifier4],  
                           AGI.TimezoneCode [VisitTimeZone],
                           CASE WHEN @IsCA = 1 THEN NULL ELSE 
                                FORMAT([dbo].[GetUTCDateTimeForOrg](SM.StartDate), 'yyyy-MM-ddTHH:mm:ss.fffZ') END [ScheduleStartTime],
                           CASE WHEN @IsCA = 1 THEN NULL ELSE 
                                FORMAT([dbo].[GetUTCDateTimeForOrg](SM.EndDate), 'yyyy-MM-ddTHH:mm:ss.fffZ') END [ScheduleEndTime],
                                      --'CP01' [ContingencyPlan],  
                           'No' [Reschedule],
                           CASE
                               WHEN @IsCA = 0 AND @IsCO = 0 AND [dbo].[GetUTCDateTimeForOrg](SM.StartDate) < GETUTCDATE() THEN
                                   FORMAT([dbo].[GetUTCDateTimeForOrg](SM.StartDate), 'yyyy-MM-ddTHH:mm:ss.fffZ')
                               ELSE
                                   NULL
                           END [AdjInDateTime],
                           CASE
                               WHEN @IsCA = 0 AND @IsCO = 0 AND [dbo].[GetUTCDateTimeForOrg](SM.EndDate) < GETUTCDATE() THEN
                                   FORMAT([dbo].[GetUTCDateTimeForOrg](SM.EndDate), 'yyyy-MM-ddTHH:mm:ss.fffZ')
                               ELSE
                                   NULL
                           END [AdjOutDateTime],
                           1 [BillVisit],
                           CASE
                               WHEN BD.BilledAmount > 0 THEN
                                   BD.BilledUnit
                               ELSE
                                   NULL
                           END [HoursToBill],
                           CASE
                               WHEN BD.BilledAmount > 0 THEN
                                   BD.BilledUnit
                               ELSE
                                   NULL
                           END [HoursToPay],
                                      --'This is a memo!' [Memo],  
                           EV.IsSigned [ClientVerifiedTimes],
                           EV.IsSigned [ClientVerifiedTasks],
                           EV.IsSigned [ClientVerifiedService],
                           EV.IsSigned [ClientSignatureAvailable],
                                      --1 [ClientVoiceRecording],  
                           (
                               SELECT *
                               FROM
                               (
                                   SELECT EV.EmployeeVisitID [CallExternalID],
                                          FORMAT(
                                                    [dbo].[GetUTCDateTimeForOrg](EV.ClockInTime),
                                                    'yyyy-MM-ddTHH:mm:ss.fffZ'
                                                ) [CallDateTime],
                                          'Time In' [CallAssignment],
                                          NULL [GroupCode],
                                          CASE
                                              WHEN EV.IVRClockIn = 1 THEN
                                                  'Telephony'
                                              WHEN EV.IsByPassClockIn = 1 AND EV.IsSigned = 0 THEN
                                                  'Manual'  
                                              ELSE
                                                  'Mobile'
                                          END [CallType],
                                          AGI.ServiceCode [ProcedureCode],
                                          FORMAT(sm.ReferralID, '0000000000') [ClientIdentifierOnCall],
                                          CASE
                                              WHEN EV.IVRClockIn = 1 OR (EV.IsByPassClockIn = 1 AND EV.IsSigned = 0) THEN
                                                  NULL
                                              ELSE
                                                  E.UserName
                                          END [MobileLogin],
                                          CASE
                                              WHEN EV.IVRClockIn = 1 OR (EV.IsByPassClockIn = 1 AND EV.IsSigned = 0) THEN
                                                  NULL
                                              ELSE
                                                  RC.Latitude
                                          END [CallLatitude],
                                          CASE
                                              WHEN EV.IVRClockIn = 1 OR (EV.IsByPassClockIn = 1 AND EV.IsSigned = 0) THEN
                                                  NULL
                                              ELSE
                                                  RC.Longitude
                                          END [CallLongitude],
                                          NULL [Location],
                                          CASE WHEN EV.IVRClockIn = 1 THEN E.IVRPin ELSE NULL END [TelephonyPIN],
                                          CASE
                                              WHEN EV.IVRClockIn = 1 THEN
                                                  RC.Phone1
                                              ELSE
                                                  NULL
                                          END [OriginatingPhoneNumber],
                                          CASE WHEN @IsCA = 1 THEN 1 ELSE NULL END [VisitLocationType]
                                   WHERE EV.ClockInTime IS NOT NULL
                                   UNION
                                   SELECT EV.EmployeeVisitID [CallExternalID],
                                          FORMAT(
                                                    [dbo].[GetUTCDateTimeForOrg](EV.ClockOutTime),
                                                    'yyyy-MM-ddTHH:mm:ss.fffZ'
                                                ) [CallDateTime],
                                          'Time Out' [CallAssignment],
                                          NULL [GroupCode],
                                          CASE
                                              WHEN EV.IVRClockOut = 1 THEN
                                                  'Telephony'
                                              WHEN EV.IsByPassClockIn = 1 AND EV.IsSigned = 0 THEN
                                                  'Manual'
                                              ELSE
                                                  'Mobile'
                                          END [CallType],
                                          AGI.ServiceCode [ProcedureCode],
                                          FORMAT(sm.ReferralID, '0000000000') [ClientIdentifierOnCall],
                                          CASE
                                              WHEN EV.IVRClockOut = 1 OR (EV.IsByPassClockIn = 1 AND EV.IsSigned = 0) THEN
                                                  NULL
                                              ELSE
                                                  E.UserName
                                          END [MobileLogin],
                                          CASE
                                              WHEN EV.IVRClockOut = 1 OR (EV.IsByPassClockIn = 1 AND EV.IsSigned = 0) THEN
                                                  NULL
                                              ELSE
                                                  RC.Latitude
                                          END [CallLatitude],
                                          CASE
                                              WHEN EV.IVRClockOut = 1 OR (EV.IsByPassClockIn = 1 AND EV.IsSigned = 0) THEN
                                                  NULL
                                              ELSE
                                                  RC.Longitude
                                          END [CallLongitude],
                                          NULL [Location],
                                          CASE WHEN EV.IVRClockOut = 1 THEN E.IVRPin ELSE NULL END [TelephonyPIN],
                                          CASE
                                              WHEN EV.IVRClockOut = 1 THEN
                                                  RC.Phone1
                                              ELSE
                                                  NULL
                                          END [OriginatingPhoneNumber],
                                          CASE WHEN @IsCA = 1 THEN 1 ELSE NULL END [VisitLocationType]
                                   WHERE EV.ClockOutTime IS NOT NULL
                               ) EVC
                               FOR JSON PATH
                           ) AS Calls,  
                    (  
                        SELECT CAST(FORMAT([dbo].[GetUTCDateTimeForOrg](GETDATE()),'yyyyMMddHHmmssff') AS BIGINT) [SequenceID],  
                               ISNULL(ev.UpdatedBy, sm.UpdatedBy) [ChangeMadeBy],  
                               FORMAT([dbo].[GetUTCDateTimeForOrg](GETDATE()), 'yyyy-MM-ddTHH:mm:ss.fffZ') [ChangeDateTime],  
                               NULL [GroupCode],
							   '14' [ReasonCode],  
                               NULL [ChangeReasonMemo],  
                               NULL [ResolutionCode] 
                        WHERE (ISNULL(SD.SandataID, '') <> '' OR (EV.IsByPassClockIn = 1 AND EV.IsSigned = 0))
                        FOR JSON PATH  
                    ) AS VisitChanges--,  
                    --(  
                    --    SELECT '321' [TaskID], '98.6' [TaskReading], 0 [TaskRefused] FOR JSON PATH  
                    --) AS Tasks  
                    FROM schedulemasters SM
                        OUTER APPLY
                    (SELECT * FROM [dbo].[GetAggregatorInfo](SM.ScheduleID) ) AGI
                        LEFT JOIN ScheduleDetails SD
                            ON SD.ScheduleID = SM.ScheduleID
                        LEFT JOIN EmployeeVisits EV
                            ON EV.ScheduleID = SM.ScheduleID
                               AND EV.IsDeleted = 0
                        LEFT JOIN employees E
                            ON sm.employeeid = e.employeeid
                        OUTER APPLY
                    (
                        SELECT TOP 1
                            C.*
                        FROM ContactMappings CM
                            INNER JOIN Contacts C
                                ON C.ContactID = CM.ContactID
                        WHERE CM.ContactTypeID = 1
                              AND CM.ReferralID = SM.ReferralID
                    ) RC
                        OUTER APPLY
                    (
                        --SELECT BND.BatchID,
                        --       BND.BilledAmount,
                        --       BND.BilledUnit,
                        --       BND.Rate,
                        --       BND.ContinuedDX
                        --FROM Notes SN
                        --    INNER JOIN BatchNoteDetails BND
                        --        ON BND.NoteID = SN.NoteID
                        --           AND BND.IsDeleted = 0
                        --WHERE SN.EmployeeVisitID = EV.EmployeeVisitID
                        --      AND SN.IsDeleted = 0

						
 SELECT TOP 1 BND.BatchID  ,
                              cast(BND.CLM_BilledAmount as float) [BilledAmount],
                               cast(BND.CLM_UNIT as int) BilledUnit
                              
                               
                        FROM Notes SN
                            INNER JOIN BatchNotes BND
                                ON BND.NoteID = SN.NoteID
  
	    WHERE
	    SN.EmployeeVisitID = EV.EmployeeVisitID
		AND SN.IsDeleted = 0
        AND EV.IsPCACompleted = 1
        AND @IsBillVisit = 1
	  ORDER BY BND.NoteID DESC, BND.BatchID DESC

                    ) BD
                    WHERE sm.employeeid > 0
                          AND sm.scheduleid = @ScheduleID
                          AND ( (@IsCA = 0 AND @IsCO = 0)
                                OR ((@IsCA = 1 OR @IsCO = 1) AND ev.IsPCACompleted = 1)
                          )
                    FOR JSON PATH
                ) [Visit]
           )
        SELECT @EventName [EventName],
               @ScheduleID [ScheduleID],
               @OrganizationID [OrganizationID],
               v.[Client],
               v.[Employee],
               v.[Visit]
        FROM visits V;


        SET @Result = 1;

    END
    ELSE
    BEGIN
        SELECT NULL;

    END
    SELECT @Payor [Payor],
           @Aggregator [Aggregator],
           Isnull(@EmployeeID, 0) [EmployeeID],
           @LenProviderTaxID [ProviderTaxIDLength];


    SELECT @Result [ResultID];

END