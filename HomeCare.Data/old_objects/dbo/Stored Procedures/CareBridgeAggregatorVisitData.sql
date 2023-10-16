CREATE PROCEDURE [dbo].[CareBridgeAggregatorVisitData]
  @EventName nvarchar(max),
  @ScheduleID bigint,
  @ReasonCode nvarchar(max),
  @ActionCode nvarchar(max),
  @OrganizationID bigint,
  @CareBridgeAggregator nvarchar(max)
AS
BEGIN

  DECLARE @IsDelete bit = (SELECT CASE WHEN @EventName = 'DeleteSchedule' THEN 1 ELSE 0 END)	
  DECLARE @Result int = 0,
          @EmployeeID bigint,
          @Payor nvarchar(max),
          @Aggregator nvarchar(max),
          @NA varchar(5) = '-NA-'

  SELECT
    @EmployeeID = SM.EmployeeID,
    @Payor = ISNULL(P.PayorName, @NA),
    @Aggregator = ISNULL(P.ClaimProcessor, @NA)
  FROM ScheduleMasters SM
  LEFT JOIN ReferralBillingAuthorizations RBA
    ON RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
  LEFT JOIN Payors P
    ON P.PayorID = RBA.PayorID
  WHERE
    SM.ScheduleID = @ScheduleID

  DECLARE @ProviderTaxID nvarchar(max),
          @LenProviderTaxID int,
          @LenCareBridgeAuth bit

  SELECT
    @ProviderTaxID = BillingProvider_REF02_ReferenceIdentification,
    @LenProviderTaxID = LEN(ISNULL(BillingProvider_REF02_ReferenceIdentification, '')),
    @LenCareBridgeAuth = LEN(ISNULL(HHAXClientId, '') + ISNULL(HHAXClientId, ''))
  FROM OrganizationSettings

  IF (@Aggregator = @CareBridgeAggregator
    AND ISNULL(@EmployeeID, 0) > 0
    AND @LenProviderTaxID > 0
    AND @LenCareBridgeAuth > 0)
  BEGIN

    ;
    WITH Visit
    AS
    (
      SELECT
        (
          SELECT
            OS.BillingProvider_NM103_NameLastOrOrganizationName [VendorName],
            NEWID() [TransactionID],
            GETDATE() [TransactionDateTime],

            P.PayorIdentificationNumber [ProviderID],
            P.PayorName [ProviderName],
            P.NPINumber [ProviderNPI],
            P.AgencyTaxNumber [ProviderEIN],
            P.AgencyNPID [ProviderMedicaidID],

            SM.ScheduleID [ApptID],

            E.FirstName [CaregiverFName],
            E.LastName [CaregiverLName],
            SM.EmployeeID [CaregiverID],
            E.ProfessionalLicenseNumber [CaregiverLicenseNumber],
            E.DateOfBirth [CaregiverDateOfBirth],

            R.FirstName [MemberFName],
            R.LastName [MemberLName],
            RPM.BeneficiaryNumber [MemberMedicaidID],
            RPM.MemberID [MemberID],
            R.Dob [MemberDateOfBirth],

            SM.StartDate [ApptStartDateTime],
            SM.EndDate [ApptEndDateTime],
            CASE WHEN @IsDelete = 1 THEN 'C' ELSE NULL END [ApptCancelled],

            EVVIN.[CheckInDateTime],
            EVVIN.[CheckInMethod],
            EVVIN.[CheckInStreetAddress],
            EVVIN.[CheckInStreetAddress2],
            EVVIN.[CheckInCity],
            EVVIN.[CheckInState],
            EVVIN.[CheckInZip],
            EVVIN.[CheckInLat],
            EVVIN.[CheckInLong],

            EVVOUT.[CheckOutDateTime],
            EVVOUT.[CheckOutMethod],
            EVVOUT.[CheckOutStreetAddress],
            EVVOUT.[CheckOutStreetAddress2],
            EVVOUT.[CheckOutCity],
            EVVOUT.[CheckOutState],
            EVVOUT.[CheckOutZip],
            EVVOUT.[CheckOutLat],
            EVVOUT.[CheckOutLong],


            RBA.AuthorizationCode [AuthRefNumber],
            SC.ServiceCode [ServiceCode],
            M.*,
            'US/Eastern' [TimeZone],
            EVVIN.[CheckInIVRPhoneNumber],
            EVVOUT.[CheckOutIVRPhoneNumber],
            SM.Comments [ApptNote],
            NULL [DiagnosisCode],
            'MA1000' [ApptAttestation],
            0 [Rate],
            NULL [ManualReason],
            NULL [LateReason],
            NULL [LateAction],
            NULL [MissedReason],
            NULL [MissedAction],
            EVN.[CarePlanTasksCompleted],
            NULL [CarePlanTasksNotCompleted],
            NULL [CaregiverSurveyQuestions],
            NULL [CaregiverSurveyResponses],
            'V' [ClaimAction],
            P.ShortName [MCOID],
            E.SocialSecurityNumber [CaregiverSSN],
            E.EmpGender [CaregiverGender],
            'Both' [CaregiverType],
            E.HireDate [CaregiverHireDate]
          FROM ScheduleMasters SM
          OUTER APPLY
          (
            SELECT TOP 1
              *
            FROM OrganizationSettings
          ) OS
          LEFT JOIN ReferralBillingAuthorizations RBA
            ON RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
          LEFT JOIN Payors P
            ON P.PayorID = RBA.PayorID
          LEFT JOIN ReferralPayorMappings RPM
            ON RPM.ReferralID = SM.ReferralID
			AND RPM.PayorID = P.PayorID
            AND RPM.IsDeleted = 0
          LEFT JOIN ServiceCodes SC
            ON SC.ServiceCodeID = RBA.ServiceCodeID
          LEFT JOIN EmployeeVisits EV
            ON EV.ScheduleID = SM.ScheduleID
            AND EV.IsDeleted = 0
          LEFT JOIN Employees E
            ON SM.EmployeeID = E.EmployeeID
          LEFT JOIN Referrals R
            ON SM.ReferralID = R.ReferralID
          OUTER APPLY
          (
            SELECT TOP 1
              C.*
            FROM ContactMappings CM
            INNER JOIN Contacts C
              ON C.ContactID = CM.ContactID
            WHERE
              CM.ContactTypeID = 1
              AND CM.ReferralID = SM.ReferralID
          ) RC
          OUTER APPLY
          (
            SELECT
              EV.ClockInTime [CheckInDateTime],
              CASE
                WHEN IVRClockIn = 1 THEN 'E'
                ELSE 'M'
              END [CheckInMethod],
              RC.[Address] [CheckInStreetAddress],
              RC.ApartmentNo [CheckInStreetAddress2],
              RC.City [CheckInCity],
              RC.[State] [CheckInState],
              RC.ZipCode [CheckInZip],
              CASE
                WHEN IVRClockIn = 1 THEN EV.Latitude
              END [CheckInLat],
              CASE
                WHEN IVRClockIn = 1 THEN EV.Longitude
              END [CheckInLong],
              CASE
                WHEN IVRClockIn = 1 THEN NULL
                ELSE E.MobileNumber
              END [CheckInIVRPhoneNumber]
            WHERE
              EV.ClockInTime IS NOT NULL
          ) EVVIN
          OUTER APPLY
          (
            SELECT
              *
            FROM
            (
              SELECT TOP 2
                'Modifier ' + CONVERT(varchar(1), SCM.id) [Col],
                M.ModifierCode [Val]
              FROM Modifiers M
              INNER JOIN GetCSVTable(SC.ModifierID) SCM
                ON SCM.val = M.ModifierID
            ) AS Data
            PIVOT
            (MAX([Val]) FOR [Col] IN ([Modifier 1], [Modifier 2])) AS Final
          ) M
          OUTER APPLY
          (
            SELECT

              EV.ClockOutTime [CheckOutDateTime],
              CASE
                WHEN IVRClockOut = 1 THEN 'E'
                ELSE 'I'
              END [CheckOutMethod],
              RC.[Address] [CheckOutStreetAddress],
              RC.ApartmentNo [CheckOutStreetAddress2],
              RC.City [CheckOutCity],
              RC.[State] [CheckOutState],
              RC.ZipCode [CheckOutZip],
              CASE
                WHEN IVRClockOut = 1 THEN EV.Latitude
                ELSE NULL
              END [CheckOutLat],
              CASE
                WHEN IVRClockOut = 1 THEN EV.Longitude
                ELSE NULL
              END [CheckOutLong],
              CASE
                WHEN IVRClockOut = 1 THEN NULL
                ELSE E.MobileNumber
              END [CheckOutIVRPhoneNumber]
            WHERE
              EV.ClockOutTime IS NOT NULL
          ) EVVOUT
          OUTER APPLY
          (
            SELECT
              STRING_AGG(VT.VisitTaskDetail, '~') [CarePlanTasksCompleted]
            FROM EmployeeVisitNotes EVN
            INNER JOIN ReferralTaskMappings RTM
              ON EVN.ReferralTaskMappingID = RTM.ReferralTaskMappingID
            INNER JOIN VisitTasks VT
              ON VT.VisitTaskID = RTM.VisitTaskID
            WHERE
              EVN.EmployeeVisitID = EV.EmployeeVisitID
          ) EVN
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
          WHERE
            SM.EmployeeID > 0
            AND SM.ScheduleID = @ScheduleID

        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
        )
        [Data]
    )

    SELECT
      @EventName [EventName],
      @ScheduleID [ScheduleID],
      @OrganizationID [OrganizationID],
      @ProviderTaxID [ProviderTaxID],
      'NJ' [StateInitial],
      V.[Data] [Visit]
    FROM Visit V;
    SET @Result = 1;
  END
  ELSE
  BEGIN
    SELECT
      NULL;
  END

  SELECT
    @Payor [Payor],
    @Aggregator [Aggregator],
    ISNULL(@EmployeeID, 0) [EmployeeID],
    @LenProviderTaxID [ProviderTaxIDLength],
    @LenCareBridgeAuth [CareBridgeAuthLength];
  SELECT
    @Result [ResultID];
END