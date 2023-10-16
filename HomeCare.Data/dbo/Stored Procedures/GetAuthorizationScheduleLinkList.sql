CREATE PROCEDURE [dbo].[GetAuthorizationScheduleLinkList]  
  @ReferralBillingAuthorizationID bigint,  
  @StartDate date = NULL,  
  @EndDate date = NULL  
AS  
BEGIN  
  
  DECLARE @RBAStartDate date,  
          @RBAEndDate date;  
  
  SELECT  
    @RBAStartDate = StartDate,  
    @RBAEndDate = EndDate  
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
   SM.StartDate,  
   SM.EndDate,  
      SM.PayorID  
    FROM ReferralBillingAuthorizations RBA  
    INNER JOIN ScheduleMasters SM  
      ON SM.IsDeleted = 0  
      AND CONVERT(date, SM.StartDate) BETWEEN @RBAStartDate AND @RBAEndDate  
      AND CONVERT(date, SM.EndDate) BETWEEN @RBAStartDate AND @RBAEndDate  
   AND (@StartDate IS NULL OR CONVERT(date, SM.StartDate) >= @StartDate)  
   AND (@EndDate IS NULL OR CONVERT(date, SM.StartDate) <= @EndDate)  
   AND SM.ReferralID = RBA.ReferralID  
      AND (SM.PayorID = RBA.PayorID  
      OR SM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID  
      OR SM.CareTypeId = RBA.CareType)  
 LEFT JOIN EmployeeVisits EV  
   ON EV.ScheduleID = SM.ScheduleID  
   AND EV.IsDeleted = 0  
    INNER JOIN ReferralTimeSlotDates RTSD  
      ON RTSD.ReferralTSDateID = SM.ReferralTSDateID  
    WHERE  
      RBA.ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID  
      AND RBA.IsDeleted = 0  
   AND ISNULL(EV.IsPCACompleted, 0) = 0  
  )  
  SELECT  
    dbo.GetGeneralNameFormat(E.FirstName, E.LastName) Employee,  
    S.StartDate,  
 CONVERT(TIME, S.StartDate) StartTime,  
    S.EndDate,  
    CONVERT(TIME, S.EndDate) EndTime,  
    P.PayorName,  
    C.Title CareType,  
    RBA.AuthorizationCode,  
    S.ScheduleID ScheduleIDs  
  FROM Schedules S  
  LEFT JOIN Employees E  
    ON E.EmployeeID = S.EmployeeID  
  LEFT JOIN Payors P  
    ON P.PayorID = S.PayorID  
  LEFT JOIN DDMaster C  
    ON C.DDMasterID = S.CareTypeId  
  LEFT JOIN ReferralBillingAuthorizations RBA  
    ON RBA.ReferralBillingAuthorizationID = S.ReferralBillingAuthorizationID;  
  
END
GO

