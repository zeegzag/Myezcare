--   EXEC GetEmpClockInOutListWithOutStatus @StartDate = '2021/06/01', @EndDate = '2021/06/20'      
CREATE procedure [dbo].[GetEmpClockInOutListWithOutStatus]
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
AS
BEGIN
    ;WITH CTEGetEmpClockInOutList
       AS (SELECT *,
                  COUNT(T.ReferralID) OVER () AS Count
             FROM (   SELECT DISTINCT SM.ScheduleID,
                             EV.EmployeeVisitid,
                             R.ReferralID,
                             E.EmployeeID,
                             ClockIn = CASE
                                            WHEN EV.ClockInTime IS NULL THEN 0
                                            ELSE 1 END,
                             ClockOut = CASE
                                             WHEN EV.ClockOutTime IS NULL THEN 0
                                             ELSE 1 END,
                             ISNULL(EV.IsPCACompleted,0) IsPCACompleted
                        FROM ScheduleMasters SM
                       INNER JOIN Referrals R
                          ON R.ReferralID          = SM.ReferralID
                       INNER JOIN Employees E
                          ON E.EmployeeID          = SM.EmployeeID
                        Left JOIN EmployeeVisits EV
                          ON EV.ScheduleID         = SM.ScheduleID
                         AND EV.IsDeleted          = 0
                       WHERE ISNULL(SM.OnHold, 0)                    = 0
                         AND SM.IsDeleted                            = 0
                         AND R.ReferralStatusID                      = 1
                         AND (   (   @StartDate IS NOT NULL
                               AND   @EndDate IS NULL
                               AND   CONVERT(DATE, SM.StartDate)     >= @StartDate)
                            OR   (   @EndDate IS NOT NULL
                               AND   @StartDate IS NULL
                               AND   CONVERT(DATE, SM.EndDate)       <= @EndDate)
                            OR   (   @StartDate IS NOT NULL
                               AND   @EndDate IS NOT NULL
                               AND   (   CONVERT(DATE, SM.StartDate) >= @StartDate
                                   AND   CONVERT(DATE, SM.EndDate)   <= @EndDate))
                            OR   (   @StartDate IS NULL
                               AND   @EndDate IS NULL))) AS T )
    SELECT *
      FROM CTEGetEmpClockInOutList
     outer apply (   select count(ap.EmployeeID) as TotalSchedule
                       from CTEGetEmpClockInOutList ap) as TotalSchedule
     outer apply (   select count(ap.EmployeeID) as Inprogress
                       from CTEGetEmpClockInOutList ap
                      where ap.ClockIn  = 1
                        and ap.ClockOut = 0) as Inprogress
     outer apply (   select count(ap.EmployeeID) as ClocOutnDone
                       from CTEGetEmpClockInOutList ap
                      where ap.ClockIn  = 1
                        and ap.ClockOut = 1) as ClocOutnDone
     outer apply (   select count(ap.EmployeeID) as MissedSchedule
                       from CTEGetEmpClockInOutList ap
                      where ap.EmployeeVisitid IS NULL
                        AND ap.ScheduleID IS NOT NULL) as MissedSchedule
     outer apply (   select count(ap.EmployeeID) as TotalComplete
                       from CTEGetEmpClockInOutList ap
                      where ap.IsPCACompleted = 1) as TotalComplete
END