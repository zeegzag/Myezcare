CREATE PROCEDURE [dbo].[TellusAggregatorVisitData]
    @EventName nvarchar(max),
    @ScheduleID bigint,
    @ReasonCode nvarchar(max),
    @ActionCode nvarchar(max),
    @OrganizationID bigint,
    @TellusAggregator nvarchar(max)
AS
BEGIN

    IF (@EventName = 'EditSchedule-ClockOut')
    BEGIN
        EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'','', 1
    END

    DECLARE @IsDelete bit = (
                                SELECT CASE WHEN @EventName = 'DeleteSchedule' THEN 1 ELSE 0 END
                            )
    DECLARE @Result int = 0,
            @EmployeeID bigint,
            @Payor nvarchar(max),
            @Aggregator nvarchar(max),
            @IsBillVisit BIT,
            @NA varchar(5) = '-NA-'

    SELECT @EmployeeID = AGI.EmployeeID,
           @Payor = ISNULL(AGI.PayorName, @NA),
           @Aggregator = ISNULL(AGI.ClaimProcessor, @NA),
           @IsBillVisit = CAST(CASE WHEN AGI.VisitBilledBy = @TellusAggregator THEN 1 ELSE 0 END AS BIT)
    FROM [dbo].[GetAggregatorInfo](@ScheduleID) AGI

    DECLARE @ProviderTaxID nvarchar(max),
            @ProviderName nvarchar(max),
            @OrgState nvarchar(max),
            @LenProviderTaxID int

    SELECT @ProviderTaxID = BillingProvider_REF02_ReferenceIdentification,
           @ProviderName = BillingProvider_NM103_NameLastOrOrganizationName,
           @OrgState = OrganizationState,
           @LenProviderTaxID = LEN(ISNULL(BillingProvider_REF02_ReferenceIdentification, ''))
    FROM OrganizationSettings

    IF (
           @Aggregator = @TellusAggregator
           AND ISNULL(@EmployeeID, 0) > 0
           AND @LenProviderTaxID > 0
       )
    BEGIN
        EXEC [dbo].[CreateAndSaveBatchForSchedule] @ScheduleID = @ScheduleID

        ;
        WITH Visit
        AS (SELECT
                (
                    SELECT 'MYEZ' SourceSystem,
                           @OrgState Jurisdiction,
                           AGI.PayorShortName Payer,
                           'NONE' [Plan],
                                                                 --NULL Program										,
                           'FFFS' DeliverySystem,
                           @ProviderName ProviderName,
                           AGI.CareTypeValue ProviderMedicaidId,
                           AGI.NPINumber ProviderNPI,
                                                                 --NULL ProviderNPITaxonomy							,
                                                                 --NULL ProviderNPIZipCode							,
                           AGI.AgencyTaxNumber ProviderEin,
                           E.FirstName CaregiverFirstName,
                           E.LastName CaregiverLastName,
                           E.ProfessionalLicenseNumber CaregiverLicenseNumber,
                           AGI.BeneficiaryNumber RecipientMedicaidId,
                           AGI.MemberID RecipientMemberId,
                           R.FirstName RecipientFirstName,
                           R.LastName RecipientLastName,
                           R.Dob RecipientDob,
                           RC.[Address] ServiceAddress1,
                           RC.ApartmentNo ServiceAddress2,
                           RC.City ServiceCity,
                           RC.[State] ServiceState,
                           RC.ZipCode ServiceZip,
                           CONVERT(NVARCHAR(MAX), @OrganizationID) + CONVERT(NVARCHAR(MAX), SM.ScheduleID) VisitId,
                           AGI.ServiceCode ServiceCode,
                           M.MOD1 ServiceCodeMod1,
                           M.MOD2 ServiceCodeMod2,
                           M.MOD3 ServiceCodeMod3,
                           M.MOD4 ServiceCodeMod4,
                           (SELECT TOP 1 DC.DXCodeName FROM ReferralDXCodeMappings RDCM 
						   INNER JOIN DXCodes DC ON DC.DXCodeID = RDCM.DXCodeID
						   WHERE RDCM.ReferralID = SM.ReferralID AND RDCM.Precedence = 1) DiagnosisCode1								,
                                                                 --NULL DiagnosisCode2								,
                                                                 --NULL DiagnosisCode3								,
                                                                 --NULL DiagnosisCode4								,
                           EVVIN.[CheckInMethod] StartVerificationType,
                           EVVOUT.[CheckOutMethod] EndVerificationType,
                           SM.StartDate ScheduledStartDateTime,
                           SM.EndDate ScheduledEndDateTime,
                           CONVERT(NVARCHAR(MAX), RC.Latitude) ScheduledLatitude,
                           CONVERT(NVARCHAR(MAX), RC.Longitude) ScheduledLongitude,
                           EVVIN.[CheckInDateTime] ActualStartDateTime,
                           EVVOUT.[CheckOutDateTime] ActualEndDateTime,
                           EVVIN.[CheckInLat] ActualStartLatitude,
                           EVVIN.[CheckInLong] ActualStartLongitude,
                           EVVOUT.[CheckOutLat] ActualEndLatitude,
                           EVVOUT.[CheckOutLong] ActualEndLongitude,
                           CONVERT(NVARCHAR(MAX), @OrganizationID) + CONVERT(NVARCHAR(MAX), BD.[InvoiceNumber]) UserField1,
                                                                 --NULL UserField2									,
                                                                 --NULL UserField3									,
                                                                 --NULL ReasonCode1									,
                                                                 --NULL ReasonCode2									,
                                                                 --NULL ReasonCode3									,
                                                                 --NULL ReasonCode4									,
                           'NEWY' TimeZone,
                           SM.Comments VisitNote,
                                                                 --NULL EndAddress1									,
                                                                 --NULL EndAddress2									,
                                                                 --NULL EndCity										,
                                                                 --NULL EndState									,
                                                                 --NULL EndZip										,
                           CASE
                               WHEN @IsDelete = 1 THEN
                                   'UNSR' 
                               WHEN TS.IsCompleted = 1 THEN
                                   'COMP'
                               WHEN EVVOUT.[CheckOutDateTime] IS NOT NULL THEN
                                   'HOLD'
                               WHEN EVVIN.[CheckInDateTime] IS NOT NULL THEN
                                   'INPR'
                               ELSE
                                   'NEWS'
                           END VisitStatus,
                                                                 --NULL MissedVisitReason							,
                                                                 --NULL MissedVisitActionTaken						,
                           BD.TotalUnitsBilled InvoiceUnits,
                           BD.TotalBilledAmount InvoiceAmount,
                           BD.ContractRate InvoiceRate,
                                                                 --NULL ScheduledEndLatitude						,
                                                                 --NULL ScheduledEndLongitude						,
                                                                 --NULL PaidAmount									,
                                                                 --NULL CareDirectionType							,
                                                                 --NULL Tasks										,
                                                                 --NULL CaregiverType								,
                                                                 --NULL StartAddressType							,
                                                                 --NULL EndAddressType								,
                                                                 --NULL ReferringPhysicianFirstName					,
                                                                 --NULL ReferringPhysicianLastName					,
                                                                 --NULL ReferringPhysicianNpi						,
                                                                 --NULL ReferringPhysicianNpiTaxonomy				,
                                                                 --NULL ProviderAddress								,
                                                                 --NULL ProviderAddress2							,
                                                                 --NULL ProviderCity								,
                                                                 --NULL ProviderState								,
                                                                 --NULL ProviderZip									,
                           R.Gender RecipientGender,
                    --NULL AuthorizationID,
                    --NULL ICNPayerClaimNumber							,
                    --NULL ProviderInvoiceNumber						,
                    --NULL TransactionControlNumber					,
                    '' ClaimType,
                    --BD.InvoiceStartDateTime,
                    --BD.InvoiceEndDateTime,
                    --NULL CaregiverEvvId								,
                    --NULL CaregiverDob								,
                    --NULL CaregiverSsn								,
                    --NULL RecipientSsn								,
                    NEWID() TransactionId	--	,
                    --NULL CaregiverEmail								,
                    --NULL TPLPaid										,
                    --NULL SOCPaid										,
                    --NULL TPLPaidDate									,
                    --NULL TPLPayerType								,
                    --NULL TPLPayerName								,
                    --NULL TPLPayerID									,
                    --NULL TPLPayerAddress1							,
                    --NULL TPLPayerAddress2							,
                    --NULL TPLPayerCity								,
                    --NULL TPLPayerState								,
                    --NULL TPLPayerZip									,
                    --NULL TPLDeduct									,
                    --NULL TPLDeductDate								,
                    --NULL TPLDeniedAmount								,
                    --NULL TPLDeniedDate								
                    FROM ScheduleMasters SM
                        --       OUTER APPLY
                        --       (
                        --         SELECT TOP 1
                        --           *
                        --         FROM OrganizationSettings
                        --       ) OS
                        OUTER APPLY
                    (SELECT * FROM [dbo].[GetAggregatorInfo](SM.ScheduleID) ) AGI
                        LEFT JOIN EmployeeVisits EV
                            ON EV.ScheduleID = SM.ScheduleID
                               AND EV.IsDeleted = 0
                        LEFT JOIN Employees E
                            ON SM.EmployeeID = E.EmployeeID
                        LEFT JOIN Referrals R
                            ON SM.ReferralID = R.ReferralID
                    CROSS APPLY (
                        SELECT CASE WHEN (ISNULL(EV.IsApprovalRequired, 0) = 0 OR ISNULL(EV.IsApproved, 1) = 1)
							AND EV.IsPCACompleted = 1 AND @EventName != 'EditSchedule-ClockOut' THEN 1 ELSE 0 END IsCompleted
                    ) TS
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
                        SELECT EV.ClockInTime [CheckInDateTime],
                               CASE
                                   WHEN IVRClockIn = 1 THEN
                                       'IVR'
                                   ELSE
                                       'GPS'
                               END [CheckInMethod],
                               CASE
                                   WHEN IVRClockIn <> 1 THEN
                                       CONVERT(NVARCHAR(MAX), ISNULL(EV.Latitude, RC.Latitude))
                               END [CheckInLat],
                               CASE
                                   WHEN IVRClockIn <> 1 THEN
                                       CONVERT(NVARCHAR(MAX), ISNULL(EV.Longitude, RC.Longitude))
                               END [CheckInLong],
                               CASE
                                   WHEN IVRClockIn = 1 THEN
                                       E.MobileNumber
                               END [CheckInIVRPhoneNumber]
                        WHERE EV.ClockInTime IS NOT NULL
                    ) EVVIN
                    OUTER APPLY
                    (
                        SELECT *
                        FROM  
                        (
                            SELECT
	                        'MOD' + CONVERT(VARCHAR(Max), SCM.id) Field,
                            M.ModifierCode
                        FROM Modifiers M
                        INNER JOIN GetCSVTable(AGI.ModifierID) SCM ON SCM.val = M.ModifierID
                        ) AS SourceTable  
                        PIVOT  
                        (  
                            MIN(ModifierCode)  
                            FOR Field IN ([MOD1], [MOD2], [MOD3], [MOD4])  
                        ) AS PivotTable
                    ) M
                    OUTER APPLY
                    (
                        SELECT EV.ClockOutTime [CheckOutDateTime],
                               CASE
                                   WHEN IVRClockOut = 1 THEN
                                       'IVR'
                                   ELSE
                                       'GPS'
                               END [CheckOutMethod],
                               CASE
                                   WHEN IVRClockOut <> 1 THEN
                                       CONVERT(NVARCHAR(MAX), ISNULL(EV.Latitude, RC.Latitude))
                                   ELSE
                                       NULL
                               END [CheckOutLat],
                               CASE
                                   WHEN IVRClockOut <> 1 THEN
                                       CONVERT(NVARCHAR(MAX), ISNULL(EV.Longitude, RC.Longitude))
                                   ELSE
                                       NULL
                               END [CheckOutLong],
                               CASE
                                   WHEN IVRClockOut = 1 THEN
                                       E.MobileNumber
                               END [CheckOutIVRPhoneNumber]
                        WHERE EV.ClockOutTime IS NOT NULL
                    ) EVVOUT
                        --       OUTER APPLY
                        --       (
                        --         SELECT
                        --           STRING_AGG(VT.VisitTaskDetail, '~') [CarePlanTasksCompleted]
                        --         FROM EmployeeVisitNotes EVN
                        --         INNER JOIN ReferralTaskMappings RTM
                        --           ON EVN.ReferralTaskMappingID = RTM.ReferralTaskMappingID
                        --         INNER JOIN VisitTasks VT
                        --           ON VT.VisitTaskID = RTM.VisitTaskID
                        --         WHERE
                        --           EVN.EmployeeVisitID = EV.EmployeeVisitID
                        --       ) EVN
                        --OUTER APPLY
                        --(
                        --  SELECT
                        --    CAST(1 AS bit) [missedVisit.missed],
                        --    CASE
                        --      WHEN LEN(ISNULL(@ReasonCode, '')) > 0 THEN @ReasonCode
                        --      ELSE ''
                        --    END [missedVisit.reasonCode],
                        --    CASE
                        --      WHEN LEN(ISNULL(@ActionCode, '')) > 0 THEN @ActionCode
                        --      ELSE ''
                        --    END [missedVisit.actionCode],
                        --    NULL [missedVisit.notes]
                        --  WHERE
                        --    1 = 2
                        --) MSV
                        --OUTER APPLY
                        --(
                        --  SELECT
                        --    CAST(1 AS bit) [edited.editVisit],
                        --    CASE
                        --      WHEN LEN(ISNULL(@ReasonCode, '')) > 0 THEN @ReasonCode
                        --      ELSE ''
                        --    END [editVisit.reasonCode],
                        --    CASE
                        --      WHEN LEN(ISNULL(@ActionCode, '')) > 0 THEN @ActionCode
                        --      ELSE ''
                        --    END [editVisit.actionCode],
                        --    NULL [editVisit.notes]
                        --  WHERE
                        --    1 = 2
                        --) EDV
                        OUTER APPLY
                    (
                        SELECT TOP 1
                            BND.BatchID [InvoiceNumber],
                            BND.CLM_BilledAmount [TotalBilledAmount],
                            BND.CLM_UNIT [TotalUnitsBilled],
                            AGI.Rate [ContractRate]--,
							--EV.ClockInTime [InvoiceStartDateTime],
							--EV.ClockOutTime [InvoiceEndDateTime]
                        FROM Notes SN
                            INNER JOIN BatchNotes BND
                                ON BND.NoteID = SN.NoteID
                        WHERE SN.EmployeeVisitID = EV.EmployeeVisitID
                              AND SN.IsDeleted = 0
                              AND TS.IsCompleted = 1
                              AND @IsBillVisit = 1
                        ORDER BY BND.NoteID DESC,
                                 BND.BatchID DESC
                    ) BD
                    WHERE SM.EmployeeID > 0
                          AND SM.ScheduleID = @ScheduleID
                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                ) [Data]
           )
        SELECT @EventName [EventName],
               @ScheduleID [ScheduleID],
               @OrganizationID [OrganizationID],
               @ProviderTaxID [ProviderTaxID],
               V.[Data] [Visit]
        FROM Visit V;
        SET @Result = 1;
    END
    ELSE
    BEGIN
        SELECT NULL;
    END

    SELECT @Payor [Payor],
           @Aggregator [Aggregator],
           ISNULL(@EmployeeID, 0) [EmployeeID],
           @LenProviderTaxID [ProviderTaxIDLength];
    SELECT @Result [ResultID];
END
GO