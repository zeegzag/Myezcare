-- Note: Please also do required changes in GetEmployeeVisitsHoursSummary.sql
-- Similar query is available there for getting EmpHours with additional filters and grouping
CREATE FUNCTION [dbo].[EmpHours] (
  @EmployeeIDs nvarchar(max),
  @StartDate datetime,
  @EndDate datetime,
  @WeekStartDay int)
RETURNS @EmpHours TABLE (
  EmployeeID bigint,
  DWM nvarchar(100),
  IndividualDate date,
  RegularMinutes int,
  EmployeeTSDateIDs nvarchar(max),
  AllocatedMinutes int,
  ScheduleIDs nvarchar(max),
  ScheduledMinutes int,
  VisitIDs nvarchar(max),
  VisitedMinutes int,
  PTOIDs nvarchar(max),
  PTOMinutes int
)
AS
BEGIN

  INSERT INTO @EmpHours
  (
    EmployeeID,
    DWM,
    IndividualDate,
    RegularMinutes,
    EmployeeTSDateIDs,
    AllocatedMinutes,
    ScheduleIDs,
    ScheduledMinutes,
    VisitIDs,
    VisitedMinutes,
    PTOIDs,
    PTOMinutes
  )
    SELECT
      E.EmployeeID,
      ET.DWM,
      DR.IndividualDate,
      CASE ROW_NUMBER() OVER (PARTITION BY E.EmployeeID, ET.DWM ORDER BY DR.IndividualDate)
        WHEN 1 THEN CAST(CASE WHEN ISNULL(E.RegularHours, 0) = 0 THEN 8 ELSE ISNULL(E.RegularHours, 0) END * 60 AS int)
        ELSE 0
      END RegularMinutes,
      EDT.EmployeeTSDateIDs,
      EDT.AllocatedMinutes,
      S.ScheduleIDs,
      S.ScheduledMinutes,
      V.VisitIDs,
      V.VisitedMinutes,
      PTO.PTOIDs,
      PTO.PTOMinutes
    FROM dbo.Employees E
    CROSS JOIN DateRange(@StartDate, @EndDate) DR
    CROSS APPLY
    (
      SELECT
        DATEDIFF(DAY, '1900-01-01', DR.IndividualDate) + 1 DayNum,
        DATEDIFF(WEEK, '1900-01-01', DATEADD(D, -1 * @WeekStartDay, DR.IndividualDate)) + 1 WeekNum,
        DATEDIFF(MONTH, '1900-01-01', DR.IndividualDate) + 1 MonthNum,
        CONVERT(datetime2, DR.IndividualDate) IndividualStartDateTime,
        DATEADD(D, 1, CONVERT(datetime2, DR.IndividualDate)) IndividualEndDateTime
    ) DT
    CROSS APPLY
    (
      SELECT
        CASE ISNULL(E.RegularHourType, 1)
          WHEN 1 THEN 'D' + CONVERT(varchar(max), DT.DayNum)
          WHEN 2 THEN 'W' + CONVERT(varchar(max), DT.WeekNum)
          WHEN 3 THEN 'M' + CONVERT(varchar(max), DT.MonthNum)
        END DWM
    ) ET
    OUTER APPLY
    (
      SELECT
        STRING_AGG(ETSD.EmployeeTSDateID, ',') EmployeeTSDateIDs,
        SUM(ISNULL(CASE
          WHEN ETSD.EmployeeTSStartTime < ETSD.EmployeeTSEndTime THEN DATEDIFF(MINUTE, ETSD.EmployeeTSStartTime, ETSD.EmployeeTSEndTime)
          ELSE 1440 + DATEDIFF(MINUTE, ETSD.EmployeeTSStartTime, ETSD.EmployeeTSEndTime)
        END, 0)) AllocatedMinutes
      FROM dbo.EmployeeTimeSlotDates ETSD
      WHERE
        ETSD.EmployeeID = E.EmployeeID
        AND ETSD.EmployeeTSDate = DR.IndividualDate
    ) EDT
    OUTER APPLY
    (
      SELECT
        STRING_AGG(SM.ScheduleID, ',') ScheduleIDs,
        SUM(ISNULL(CASE
          WHEN SM.StartDate < SM.EndDate THEN DATEDIFF(MINUTE, SM.StartDate, SM.EndDate)
          ELSE 1440 + DATEDIFF(MINUTE, SM.StartDate, SM.EndDate)
        END, 0)) ScheduledMinutes
      FROM dbo.ScheduleMasters SM
      WHERE
        SM.EmployeeID = E.EmployeeID
		AND ISNULL(SM.AnyTimeClockIn, 0) = 0
		AND DR.IndividualDate BETWEEN CONVERT(DATE, SM.StartDate) AND CONVERT(DATE, SM.EndDate)
		AND SM.EmployeeTSDateID IN (SELECT [val] FROM GetCSVTable(EDT.EmployeeTSDateIDs))
        AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0
    ) S
    OUTER APPLY
    (
      SELECT
        STRING_AGG(EV.EmployeeVisitID, ',') VisitIDs,
        SUM(ISNULL(CASE
          WHEN EV.ClockInTime < EV.ClockOutTime THEN DATEDIFF(MINUTE, EV.ClockInTime, EV.ClockOutTime)
          ELSE 1440 + DATEDIFF(MINUTE, EV.ClockInTime, EV.ClockOutTime)
        END, 0)) VisitedMinutes
      FROM dbo.ScheduleMasters SM
      INNER JOIN dbo.EmployeeVisits EV
        ON EV.ScheduleID = SM.ScheduleID
        AND EV.IsPCACompleted = 1 AND EV.IsDeleted = 0
      WHERE
        SM.EmployeeID = E.EmployeeID
		AND SM.ScheduleID IN (SELECT [val] FROM GetCSVTable(S.ScheduleIDs))
        AND DR.IndividualDate BETWEEN CONVERT(DATE, SM.StartDate) AND CONVERT(DATE, SM.EndDate)
        AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0
    ) V
    OUTER APPLY
    (
      SELECT
        STRING_AGG(EDO.EmployeeDayOffID, ',') PTOIDs,
        SUM(DATEDIFF(MINUTE, DO.StartTime, DO.EndTime)) PTOMinutes
      FROM EmployeeDayOffs EDO
      CROSS APPLY
      (
        SELECT
          CASE
            WHEN EDO.StartTime > DT.IndividualStartDateTime THEN EDO.StartTime
            ELSE DT.IndividualStartDateTime
          END StartTime,
          CASE
            WHEN EDO.EndTime < DT.IndividualEndDateTime THEN EDO.EndTime
            ELSE DT.IndividualEndDateTime
          END EndTime
      ) DO
      WHERE
        EDO.IsDeleted = 0
        AND EDO.DayOffStatus = 'Approved'
		AND EDO.EmployeeID = E.EmployeeID
        AND DR.IndividualDate BETWEEN CONVERT(DATE, EDO.StartTime) AND CONVERT(DATE, EDO.EndTime)
    ) PTO
    WHERE
      ((@EmployeeIDs IS NULL
          AND E.IsDeleted = 0)
        OR E.EmployeeID IN
      (
        SELECT
          EL.[val]
        FROM dbo.GetCSVTable(@EmployeeIDs) EL
      )
      )
	  AND DR.IndividualDate IS NOT NULL
    ORDER BY E.EmployeeID, ET.DWM, DR.IndividualDate
  RETURN
END