CREATE FUNCTION [dbo].[udf_HHAXVisit] (
  @EventName nvarchar(max),
  @ProviderTaxID nvarchar(max),
  @ScheduleID bigint,
  @ReasonCode nvarchar(max),
  @ActionCode nvarchar(max),
  @IsBillVisit BIT)
RETURNS nvarchar(max)
AS
BEGIN
  RETURN
  (
    SELECT
      @ProviderTaxID [providerTaxID],

      'NPI' [office.qualifier],
      AGI.NPINumber [office.identifier],

      'MedicaidID' [member.qualifier],
      AGI.BeneficiaryNumber [member.identifier],

      'ExternalID' [caregiver.qualifier],
      SM.EmployeeID [caregiver.identifier],

      AGI.PayorShortName [payerId],
      SM.ScheduleID [externalVisitId],
      SD.HHAXEvvmsID [evvmsid],
      AGI.ServiceCode [procedureCode],
      MC.Codes [procedureModifierCode],
      'US/Eastern' [timezone],
      [dbo].[GetUTCDateTimeForOrg](SM.StartDate) [scheduleStartTime],
      [dbo].[GetUTCDateTimeForOrg](SM.EndDate) [scheduleEndTime],
      [dbo].[GetUTCDateTimeForOrg](EV.ClockInTime) [visitStartDateTime],
      [dbo].[GetUTCDateTimeForOrg](EV.ClockOutTime) [visitEndDateTime],
      EV.IsApprovalRequired [timesheetRequired],
      CAST(CASE
        WHEN EV.IsApprovalRequired = 1 AND EV.ActionTaken = 2 THEN 1
		WHEN EV.IsApprovalRequired = 0 AND EV.IsPCACompleted = 1 THEN 1
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
    OUTER APPLY 
	(
	  SELECT * FROM [dbo].[GetAggregatorInfo](SM.ScheduleID) 
	) AGI
    OUTER APPLY
    (
      SELECT
        JSON_QUERY(
        (
          SELECT
            CASE WHEN NULLIF(STRING_AGG(M.ModifierCode, ''), '') IS NULL THEN NULL ELSE CONCAT('["', STRING_AGG(M.ModifierCode, '","'), '"]') END

          FROM Modifiers M
          INNER JOIN GetCSVTable(AGI.ModifierID) SCM
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
        [dbo].[GetUTCDateTimeForOrg](EV.ClockInTime) [evv.clockIn.callDateTime],
        CASE
          WHEN (EV.IVRClockIn = 1 AND EV.IVRClockOut = 1) THEN 'Telephony'
          ELSE 'Mobile'
        END [evv.clockIn.callType],
        CASE
          WHEN EV.IVRClockIn = 1 THEN RC.Latitude
        END [evv.clockIn.callLatitude],
        CASE
          WHEN EV.IVRClockIn = 1 THEN RC.Longitude
        END [evv.clockIn.callLongitude],
        CASE
          WHEN (EV.IVRClockIn = 1 AND EV.IVRClockOut = 1) THEN RC.Phone1
          ELSE NULL
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
            ISNULL(NULLIF(VT.TaskCode, ''), '203') [code]
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
        [dbo].[GetUTCDateTimeForOrg](EV.ClockOutTime) [evv.clockOut.callDateTime],
        CASE
          WHEN (EV.IVRClockIn = 1 AND EV.IVRClockOut = 1) THEN 'Telephony'
          ELSE 'Mobile'
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
          WHEN (EV.IVRClockIn = 1 AND EV.IVRClockOut = 1) THEN RC.Phone1
          ELSE NULL
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
      SELECT TOP 1
	    BND.BatchID [billing.externalInvoiceNumber],
	    BND.CLM_BilledAmount [billing.totalBilledAmount],
	    BND.CLM_UNIT [billing.totalUnitsBilled],
	    AGI.Rate [billing.contractRate],
	    DX.DXCodes [billing.diagnosisCodes]
	  FROM Notes SN
	  INNER JOIN BatchNotes BND
	    ON BND.NoteID = SN.NoteID
      CROSS APPLY (
		SELECT
        JSON_QUERY(
        (
            SELECT CONCAT('["', STRING_AGG(DC.DXCodeName, '","'), '"]')
            FROM ReferralDXCodeMappings RDCM 
			INNER JOIN DXCodes DC ON DC.DXCodeID = RDCM.DXCodeID
			WHERE RDCM.ReferralID = SM.ReferralID 
            HAVING NULLIF(STRING_AGG(DC.DXCodeName, '","'), '') IS NOT NULL
        )
        ) [DXCodes]
	  ) DX
	  WHERE
	    SN.EmployeeVisitID = EV.EmployeeVisitID
		AND SN.IsDeleted = 0
        AND EV.IsPCACompleted = 1
        AND @IsBillVisit = 1
	  ORDER BY BND.NoteID DESC, BND.BatchID DESC
    ) BD
    WHERE
      SM.ScheduleID = @ScheduleID

    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
  )
END;
GO

