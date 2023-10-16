CREATE PROCEDURE [dbo].[GetAuthorizationLinkupList]
  @ReferralBillingAuthorizationID bigint

AS
BEGIN

  DECLARE @StartDate date,
          @EndDate date;

  SELECT
    @StartDate =
                CASE
                  WHEN StartDate < GETDATE() THEN CONVERT(date, GETDATE())
                  ELSE StartDate
                END,
    @EndDate = EndDate
  FROM ReferralBillingAuthorizations
  WHERE
    ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID;


  ;
  WITH Schedules
  AS
  (
    SELECT
      SM.ScheduleID,
      SM.EmployeeID,
      RTSD.ReferralTimeSlotMasterID,
      RTSD.ReferralTimeSlotDetailID,
      SM.ReferralTSDateID,
      SM.CareTypeId,
      SM.ReferralBillingAuthorizationID,
      SM.PayorID
    FROM ReferralBillingAuthorizations RBA
    INNER JOIN ScheduleMasters SM
      ON SM.IsDeleted = 0
      AND CONVERT(date, SM.StartDate) BETWEEN @StartDate AND @EndDate
      AND CONVERT(date, SM.EndDate) BETWEEN @StartDate AND @EndDate
	  AND SM.ReferralID = RBA.ReferralID
      AND (SM.PayorID <> RBA.PayorID
      OR SM.ReferralBillingAuthorizationID <> RBA.ReferralBillingAuthorizationID
      OR SM.CareTypeId <> RBA.CareType)
    INNER JOIN ReferralTimeSlotDates RTSD
      ON RTSD.ReferralTSDateID = SM.ReferralTSDateID
    WHERE
      RBA.ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID
      AND RBA.IsDeleted = 0
  ),
  RTSMasterMed
  AS
  (
    SELECT
      S.EmployeeID,
      S.ReferralTimeSlotMasterID,
      RTSDT.StartTime,
      RTSDT.EndTime,
      S.PayorID,
      S.CareTypeId,
      S.ReferralBillingAuthorizationID,
      S.ReferralTimeSlotDetailID,
      STRING_AGG(S.ScheduleID, ',') ScheduleIDs,
      COUNT(S.ScheduleID) Cnt
    FROM Schedules S
    INNER JOIN ReferralTimeSlotDetails RTSDT
      ON RTSDT.ReferralTimeSlotDetailID = S.ReferralTimeSlotDetailID
      AND RTSDT.IsDeleted = 0
    GROUP BY S.EmployeeID,
             S.ReferralTimeSlotMasterID,
             RTSDT.StartTime,
             RTSDT.EndTime,
             S.PayorID,
             S.CareTypeId,
             S.ReferralBillingAuthorizationID,
             S.ReferralTimeSlotDetailID
  ),
  RTSMaster
  AS
  (
    SELECT
      RTM.EmployeeID,
      RTM.ReferralTimeSlotMasterID,
      RTM.StartTime,
      RTM.EndTime,
      RTM.PayorID,
      RTM.CareTypeId,
      RTM.ReferralBillingAuthorizationID,
      STRING_AGG(RTM.ReferralTimeSlotDetailID, ',') ReferralTimeSlotDetailIDs,
      STRING_AGG(RTM.ScheduleIDs, ',') ScheduleIDs,
      SUM(Cnt) Cnt
    FROM RTSMasterMed RTM
    GROUP BY RTM.EmployeeID,
             RTM.ReferralTimeSlotMasterID,
             RTM.StartTime,
             RTM.EndTime,
             RTM.PayorID,
             RTM.CareTypeId,
             RTM.ReferralBillingAuthorizationID
  )
  SELECT
    dbo.GetGeneralNameFormat(E.FirstName, E.LastName) Employee,
    RTSM.StartDate,
    RM.StartTime,
    ISNULL(RTSM.EndDate, @EndDate) EndDate,
    RM.EndTime,
    P.PayorName,
    C.Title CareType,
    RBA.AuthorizationCode,
    RM.ScheduleIDs,
    RM.Cnt
  FROM RTSMaster RM
  INNER JOIN ReferralTimeSlotMaster RTSM
    ON RTSM.ReferralTimeSlotMasterID = RM.ReferralTimeSlotMasterID
    AND RTSM.IsDeleted = 0
  LEFT JOIN Employees E
    ON E.EmployeeID = RM.EmployeeID
  LEFT JOIN Payors P
    ON P.PayorID = RM.PayorID
  LEFT JOIN DDMaster C
    ON C.DDMasterID = RM.CareTypeId
  LEFT JOIN ReferralBillingAuthorizations RBA
    ON RBA.ReferralBillingAuthorizationID = RM.ReferralBillingAuthorizationID;

END