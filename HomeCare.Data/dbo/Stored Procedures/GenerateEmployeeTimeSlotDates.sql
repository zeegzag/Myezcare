-- EXEC GenerateEmployeeTimeSlotDates '2018-01-01','2018-03-15'
CREATE PROCEDURE [dbo].[GenerateEmployeeTimeSlotDates]
@StartDate AS DATETIME,
@EndDate AS DATETIME,
@EmployeeID BIGINT=0
AS
BEGIN

-- SET @StartDate='2018-03-01';

SET @StartDate=CONVERT(DATE,@StartDate);
SET @EndDate=CONVERT(DATE,@EndDate);

-- SELECT * FROM EmployeeTimeSlotDates ORDER BY 1 DESC
-- DELETE FROM  EmployeeTimeSlotDates
-- DELETE FROM  EmployeeTimeSlotDetails
-- DELETE FROM EmployeeTimeSlotMaster
-- TRUNCATE TABLE EmployeeTimeSlotDates
-- TRUNCATE TABLE EmployeeTimeSlotDetails
-- TRUNCATE TABLE EmployeeTimeSlotMaster


INSERT INTO EmployeeTimeSlotDates 
SELECT T.EmployeeID,T.EmployeeTimeSlotMasterID, T.EmployeeTSDate,T.EmployeeTSStartTime,T.EmployeeTSEndTime,T.DayNumber,T.EmployeeTimeSlotDetailID FROM (
SELECT E.EmployeeID,ETM.EmployeeTimeSlotMasterID, EmployeeTSDate=IndividualDate,ETMEndDate=ETM.EndDate,
EmployeeTSStartTime = TS.TSStartTime,  
EmployeeTSEndTime = CASE WHEN TS.TSStartTime <= TS.TSEndTime AND ISNULL(ETSD.Is24Hrs, 0) = 0 THEN TS.TSEndTime ELSE TS.TSEndTimeNextDay END, 
DayNumber=T1.DayNameInt,ETSD.EmployeeTimeSlotDetailID
FROM DateRange(@StartDate, @EndDate) T1
INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.Day=T1.DayNameInt AND ETSD.IsDeleted=0
CROSS APPLY (
	SELECT
		TSStartTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + ISNULL(CONVERT(char(8), StartTime, 108), '00:00:00')),  
		TSEndTime = CONVERT(datetime, CONVERT(char(8), T1.IndividualDate, 112) + ' ' + ISNULL(CONVERT(char(8), EndTime, 108), '23:59:59')), 
		TSEndTimeNextDay = CONVERT(datetime, CONVERT(char(8), DATEADD(D, 1,T1.IndividualDate), 112) + ' ' + ISNULL(CONVERT(char(8), EndTime, 108), '11:59:59'))
) TS
INNER JOIN EmployeeTimeSlotMaster ETM ON ETM.EmployeeTimeSlotMasterID=ETSD.EmployeeTimeSlotMasterID  AND ETM.IsDeleted=0
INNER JOIN Employees E ON E.EmployeeID=ETM.EmployeeID
) AS T
LEFT JOIN EmployeeTimeSlotDates ETSDT ON ETSDT.EmployeeTSStartTime= T.EmployeeTSStartTime AND ETSDT.EmployeeTSEndTime= T.EmployeeTSEndTime 
AND ETSDT.EmployeeID=T.EmployeeID
WHERE  ETSDT.EmployeeTSDateID IS NULL  AND T.EmployeeTSDate <= ISNULL(ETMEndDate,@EndDate) AND (@EmployeeID=0 OR T.EmployeeID=@EmployeeID)
-- EndDate
ORDER BY T.EmployeeID ASC, T.EmployeeTimeSlotMasterID ASC


END