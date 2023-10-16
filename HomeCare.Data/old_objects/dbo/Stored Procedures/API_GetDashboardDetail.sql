--EXEC API_GetDashboardDetail @ServerCurrentDate = N'2021-01-26', @EmployeeId = N'166'                                              
CREATE PROCEDURE [dbo].[API_GetDashboardDetail] @ServerCurrentDate DATE,
  @EmployeeId BIGINT
AS
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from                                                              
  -- interfering with SELECT statements.      
  DECLARE @OrganizationTwilioNumber NVARCHAR(20)

  SET NOCOUNT ON;

  SELECT TOP 1 @OrganizationTwilioNumber = TwilioFromNo
  FROM OrganizationSettings

  SELECT e.EmployeeID,
    EmployeeName = dbo.GetGeneralNameFormat(e.FirstName, e.LastName),
    e.FirstName,
    e.LastName,
    e.Email,
    e.PhoneWork,
    e.PhoneHome,
    e.MobileNumber,
    e.IVRPin,
    @OrganizationTwilioNumber AS OrganizationTwilioNumber
  FROM dbo.Employees e
  WHERE e.EmployeeID = @EmployeeId

  --Get Today's Visit                                                            
  SELECT *
  FROM (
    SELECT TOP 25 RankOrder = ROW_NUMBER() OVER (
        PARTITION BY SM.EmployeeTSDateID,
        SM.ReferralTSDateID ORDER BY SM.CreatedDate DESC
        ),
      r.ReferralID,
      sm.ScheduleID,
      sm.EmployeeID,
      FullName = dbo.GetGeneralNameFormat(r.FirstName, r.LastName),
      r.FirstName,
      sm.StartDate,
      sm.EndDate,
      ev.ClockInTime,
      ev.ClockOutTime,
      ImageURL = r.ProfileImagePath,
      rtd.IsDenied,
      ISNULL(sm.AnyTimeClockIn, 0) AnyTimeClockIn
    --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied                              
    FROM dbo.ScheduleMasters sm
    INNER JOIN dbo.Referrals r
      ON sm.ReferralID = r.ReferralID
        AND r.IsDeleted = 0
    INNER JOIN dbo.Employees e
      ON sm.EmployeeID = e.EmployeeID
    INNER JOIN ReferralTimeSlotDates rtd
      ON rtd.ReferralTSDateID = sm.ReferralTSDateID
    LEFT JOIN EmployeeVisits ev
      ON ev.ScheduleID = sm.ScheduleID
    WHERE --sm.IsDeleted=0 AND                          
      (
        (
          sm.IsDeleted = 0
          AND rtd.IsDenied = 0
          )
        OR (
          sm.IsDeleted = 1
          AND rtd.IsDenied = 1
          )
        )
      AND sm.EmployeeID = @EmployeeId
      AND CONVERT(DATE, sm.StartDate) = @ServerCurrentDate
      AND sm.ScheduleStatusID = 2
      AND (
        ev.IsSigned = 0
        OR ev.IsSigned IS NULL
        )
      AND (
        ev.IsPCACompleted = 0
        OR ev.IsPCACompleted IS NULL
        )
    ORDER BY sm.StartDate DESC
    
    UNION
    
    SELECT TOP 25 RankOrder = ROW_NUMBER() OVER (
        PARTITION BY sm.ScheduleID ORDER BY SM.CreatedDate DESC
        ),
      r.ReferralID,
      sm.ScheduleID,
      sm.EmployeeID,
      FullName = dbo.GetGeneralNameFormat(r.FirstName, r.LastName),
      r.FirstName,
      sm.StartDate,
      sm.EndDate,
      ev.ClockInTime,
      ev.ClockOutTime,
      ImageURL = r.ProfileImagePath,
      CAST(0 AS BIT) IsDenied,
      ISNULL(sm.AnyTimeClockIn, 0) AnyTimeClockIn
    --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied                              
    FROM dbo.ScheduleMasters sm
    INNER JOIN dbo.Referrals r
      ON sm.ReferralID = r.ReferralID
        AND r.IsDeleted = 0
    INNER JOIN dbo.Employees e
      ON sm.EmployeeID = e.EmployeeID
    LEFT JOIN EmployeeVisits ev
      ON ev.ScheduleID = sm.ScheduleID
    WHERE sm.IsDeleted = 0
      AND sm.EmployeeID = @EmployeeId
      AND CONVERT(DATE, sm.StartDate) = @ServerCurrentDate
      AND sm.ScheduleStatusID = 2
      AND Sm.EmployeeTSDateID IS NULL
      AND SM.ReferralTSDateID IS NULL
      AND ISNULL(ev.IsSigned, 0) = 0
      AND ISNULL(ev.IsPCACompleted, 0) = 0
    ORDER BY sm.StartDate DESC
    ) T
  WHERE T.RankOrder = 1

  --Get Tomorrows's Visit                                  
  SELECT r.ReferralID,
    sm.ScheduleID,
    sm.EmployeeID,
    FullName = dbo.GetGeneralNameFormat(r.FirstName, r.LastName),
    r.FirstName,
    sm.StartDate,
    sm.EndDate,
    ev.ClockInTime,
    ev.ClockOutTime,
    ImageURL = r.ProfileImagePath,
    sm.AnyTimeClockIn
  FROM dbo.ScheduleMasters sm
  INNER JOIN dbo.Referrals r
    ON sm.ReferralID = r.ReferralID
      AND r.IsDeleted = 0
  INNER JOIN dbo.Employees e
    ON sm.EmployeeID = e.EmployeeID
  LEFT JOIN EmployeeVisits ev
    ON ev.ScheduleID = sm.ScheduleID
  WHERE sm.IsDeleted = 0
    AND sm.EmployeeID = @EmployeeId
    AND CONVERT(DATE, sm.StartDate) <= DATEADD(day, 7, (convert(DATE, @ServerCurrentDate)
        ))
    AND CONVERT(DATE, sm.StartDate) >= DATEADD(day, 1, (convert(DATE, @ServerCurrentDate)
        ))
    AND (
      ev.IsPCACompleted = 0
      OR ev.IsPCACompleted IS NULL
      )
  --AND sm.ScheduleStatusID=2          --AND (ev.SurveyCompleted=0 OR ev.SurveyCompleted is null) AND (ev.IsSigned=0 OR ev.IsSigned is null)              
  ORDER BY sm.StartDate DESC
END
