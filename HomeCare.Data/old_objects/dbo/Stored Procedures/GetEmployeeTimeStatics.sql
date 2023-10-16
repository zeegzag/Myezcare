-- EXEC GetEmployeeTimeStatics
CREATE PROCEDURE [dbo].[GetEmployeeTimeStatics]
@StartDate DATE =NULL,
@EndDate  DATE=NULL
AS
BEGIN

IF(@StartDate IS NULL)
 SET @StartDate=DATEADD(DAY, -30, CONVERT(date, GETDATE())) 

 IF(@EndDate IS NULL)
 SET @EndDate=DATEADD(DAY, 1, CONVERT(date, GETDATE())) 
 ELSE
 SET @EndDate=DATEADD(DAY, 1, @EndDate) 

DECLARE @TotalDays BIGINT;
SET @TotalDays = DATEDIFF(DAY, @StartDate, @EndDate) 

PRINT @StartDate;
PRINT @EndDate;
PRINT @TotalDays;

-- EXEC GetEmployeeTimeStatics


SELECT DISTINCT 
EmployeeID,FirstName,LastName,
AvgDelay=CONVERT(DECIMAL(10,2),  SUM(CAST(TotalDelay AS float)/CAST(60 AS float)) OVER (PARTITION BY EmployeeID)  /  CAST(@TotalDays AS float) )
FROM (

SELECT --SM.*,
E.EmployeeID, E.FirstName, E.LastName,
TotalDelay= CASE WHEN  SM.StartDate < EV.ClockInTime THEN  DATEDIFF(MI, SM.StartDate, EV.ClockInTime) ELSE 0 END +
 CASE WHEN  SM.EndDate < EV.ClockOutTime THEN  DATEDIFF(MI, SM.EndDate, EV.ClockOutTime) ELSE 0 END

-- FROM Employees E
--LEFT JOIN ScheduleMasters SM ON E.EmployeeID=SM.EmployeeID
--LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID AND SM.IsDeleted=0 AND EV.IsDeleted=0
--WHERE SM.StartDate BETWEEN @StartDate AND @EndDate

FROM ScheduleMasters SM
INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID
INNER JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID AND SM.IsDeleted=0 AND EV.IsDeleted=0
WHERE SM.StartDate BETWEEN @StartDate AND @EndDate

) AS TEMO





END


--SELECT CAST(23 AS float) / CAST(3 AS float)
