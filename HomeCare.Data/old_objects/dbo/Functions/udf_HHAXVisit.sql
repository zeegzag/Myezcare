CREATE FUNCTION dbo.udf_HHAXVisit (
  @EventName nvarchar(max),
  @ProviderTaxID nvarchar(max),
  @ScheduleID bigint,
  @ReasonCode nvarchar(max),
  @ActionCode nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN
  RETURN
  (
    SELECT
      @ProviderTaxID [providerTaxID],

      'FederalTaxID' [office.qualifier],
      P.NPINumber [office.identifier],

      'MedicaidID' [member.qualifier],
      RPM.BeneficiaryNumber [member.identifier],

      'ExternalID' [caregiver.qualifier],
      SM.EmployeeID [caregiver.identifier],

      P.ShortName [payerId],
      SM.ScheduleID [externalVisitId],
      SD.HHAXEvvmsID [evvmsid],
      SC.ServiceCode [procedureCode],
      MC.Codes [procedureModifierCode],
      'US/Eastern' [timezone],
      SM.StartDate [scheduleStartTime],
      SM.EndDate [scheduleEndTime],
      EV.ClockInTime [visitStartDateTime],
      EV.ClockOutTime [visitEndDateTime],
      EV.IsApprovalRequired [timesheetRequired],
      CAST(CASE
        WHEN EV.ActionTaken = 2 THEN 1
        ELSE 0
      END AS bit) [timesheetApproved],

      EVVIN.*,

      EVVOUT.*,

      MSV.*,

      EDV.*,

      BD.*

    FROM ScheduleMasters SM
    LEFT JOIN ScheduleDetails SD
      ON SD.ScheduleID = SM.ScheduleID
    LEFT JOIN ReferralBillingAuthorizations RBA
      ON RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
    LEFT JOIN Payors P
      ON P.PayorID = RBA.PayorID
    LEFT JOIN ReferralPayorMappings RPM
      ON RPM.ReferralID = SM.ReferralID
      AND RPM.IsDeleted = 0
    LEFT JOIN ServiceCodes SC
      ON SC.ServiceCodeID = RBA.ServiceCodeID
    OUTER APPLY
    (
      SELECT
        JSON_QUERY(
        (
          SELECT
            CONCAT('["', STRING_AGG(M.ModifierCode, '","'), '"]')

          FROM Modifiers M
          INNER JOIN GetCSVTable(SC.ModifierID) SCM
            ON SCM.val = M.ModifierID
        )
        ) [Codes]
    ) MC
    LEFT JOIN EmployeeVisits EV
      ON EV.ScheduleID = SM.ScheduleID
      AND EV.IsDeleted = 0
    LEFT JOIN Employees E
      ON SM.EmployeeID = E.EmployeeID
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
        EV.ClockInTime [evv.clockIn.callDateTime],
        CASE
          WHEN EV.IVRClockIn = 1 THEN 'Mobile'
          ELSE 'Telephony'
        END [evv.clockIn.callType],
        CASE
          WHEN EV.IVRClockIn = 1 THEN RC.Latitude
        END [evv.clockIn.callLatitude],
        CASE
          WHEN EV.IVRClockIn = 1 THEN RC.Longitude
        END [evv.clockIn.callLongitude],
        CASE
          WHEN EV.IVRClockIn = 1 THEN NULL
          ELSE E.MobileNumber
        END [evv.clockIn.originatingPhoneNumber],

        RC.[Address] [evv.clockIn.serviceAddress.addressLine1],
        RC.ApartmentNo [evv.clockIn.serviceAddress.addressLine2],
        RC.City [evv.clockIn.serviceAddress.city],
        RC.[State] [evv.clockIn.serviceAddress.state],
        RC.ZipCode [evv.clockIn.serviceAddress.zipcode]
      WHERE
        EV.ClockInTime IS NOT NULL
    ) EVVIN
    OUTER APPLY
    (
      SELECT
        (
          SELECT DISTINCT
            VT.TaskCode [code]
          FROM EmployeeVisitNotes EVN
          INNER JOIN ReferralTaskMappings RTM
            ON EVN.ReferralTaskMappingID = RTM.ReferralTaskMappingID
          INNER JOIN VisitTasks VT
            ON VT.VisitTaskID = RTM.VisitTaskID
          WHERE
            EVN.EmployeeVisitID = EV.EmployeeVisitID
          FOR JSON PATH
        )
        TaskCodes
    ) EVN
    OUTER APPLY
    (
      SELECT
        EV.ClockOutTime [evv.clockOut.callDateTime],
        CASE
          WHEN EV.IVRClockOut = 1 THEN 'Mobile'
          ELSE 'Telephony'
        END [evv.clockOut.callType],
        CASE
          WHEN EV.IVRClockOut = 1 THEN RC.Latitude
          ELSE NULL
        END [evv.clockOut.callLatitude],
        CASE
          WHEN EV.IVRClockOut = 1 THEN RC.Longitude
          ELSE NULL
        END [evv.clockOut.callLongitude],
        CASE
          WHEN EV.IVRClockOut = 1 THEN NULL
          ELSE E.MobileNumber
        END [evv.clockOut.originatingPhoneNumber],
        EVN.TaskCodes [evv.clockOut.performedTasks],
        NULL [evv.clockOut.refusedTasks],

        RC.[Address] [evv.clockOut.serviceAddress.addressLine1],
        RC.ApartmentNo [evv.clockOut.serviceAddress.addressLine2],
        RC.City [evv.clockOut.serviceAddress.city],
        RC.[State] [evv.clockOut.serviceAddress.state],
        RC.ZipCode [evv.clockOut.serviceAddress.zipcode]
      WHERE
        EV.ClockOutTime IS NOT NULL
    ) EVVOUT
    OUTER APPLY
    (
      SELECT
        CAST(1 AS bit) [missedVisit.missed],
        CASE
          WHEN LEN(ISNULL(@ReasonCode, '')) > 0 THEN @ReasonCode
          ELSE '609'
        END [missedVisit.reasonCode],
        CASE
          WHEN LEN(ISNULL(@ActionCode, '')) > 0 THEN @ActionCode
          ELSE '53'
        END [missedVisit.actionCode],
        NULL [missedVisit.notes]
      WHERE
        @EventName = 'missed'
    ) MSV
    OUTER APPLY
    (
      SELECT
        CAST(1 AS bit) [edited.editVisit],
        CASE
          WHEN LEN(ISNULL(@ReasonCode, '')) > 0 THEN @ReasonCode
          ELSE '222'
        END [editVisit.reasonCode],
        CASE
          WHEN LEN(ISNULL(@ActionCode, '')) > 0 THEN @ActionCode
          ELSE '25'
        END [editVisit.actionCode],
        NULL [editVisit.notes]
      WHERE
        @EventName = 'edit'
        AND EV.ClockInTime IS NOT NULL
    ) EDV
    OUTER APPLY
    (
      SELECT
	    BND.BatchID [billing.externalInvoiceNumber],
	    BND.BilledAmount [billing.totalBilledAmount],
	    BND.BilledUnit [billing.totalUnitsBilled],
	    BND.Rate [billing.contractRate],
	    BND.ContinuedDX [billing.diagnosisCodes]
	  FROM Notes SN
	  INNER JOIN BatchNoteDetails BND
	    ON BND.NoteID = SN.NoteID
		   AND BND.IsDeleted = 0
	  WHERE
	    SN.EmployeeVisitID = EV.EmployeeVisitID
		AND SN.IsDeleted = 0
    ) BD
    WHERE
      SM.ScheduleID = @ScheduleID

    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
  )
END;