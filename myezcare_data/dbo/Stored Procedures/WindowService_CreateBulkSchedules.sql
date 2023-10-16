-- EXEC WindowService_CreateBulkSchedules @StartDate='2018-11-22',@EndDate='2018-11-28'
CREATE PROCEDURE [dbo].[WindowService_CreateBulkSchedules]
@StartDate DATETIME,
@EndDate DATETIME

AS

BEGIN

DECLARE @TempTable TABLE (
 ReferralID BIGINT,
 EmployeeID BIGINT,
 EmployeeTimeSlotDetailID VARCHAR(MAX),
 StartDate DATETIME,
 DayDiff BIGINT
)


INSERT INTO @TempTable
SELECT * FROM (
SELECT DISTINCT  R.ReferralID,SM.EmployeeID,EmployeeTimeSlotDetailID=CONVERT(VARCHAR(MAX),ETD.EmployeeTimeSlotDetailID), StartDate= MAX(SM.StartDate) OVER(PARTITION BY R.ReferralID),
DayDiff=ISNULL(DATEDIFF(DAY, GETDATE(), MAX(SM.EndDate) OVER(PARTITION BY SM.ReferralID,SM.EmployeeID)),0) 
FROM Referrals R
LEFT JOIN ScheduleMasters SM ON SM.ReferralID=R.ReferralID
LEFT JOIN EmployeeTimeSlotDates ETD ON ETD.EmployeeTSDateID=SM.EmployeeTSDateID
WHERE R.IsDeleted=0 
) AS T
WHERE DayDiff < 180 AND StartDate IS NOT NULL


SELECT * FROM @TempTable


DECLARE @SystemID VARCHAR(100)=CONVERT(VARCHAR(100),CONNECTIONPROPERTY('local_net_address'));
DECLARE @loggedInId BIGINT=1;
DECLARE @ScheduleStatusID BIGINT=2;
DECLARE @SameDateWithTimeSlot BIT=0;
--DECLARE @StartDate DATETIME='2018-09-25';
--DECLARE @EndDate DATETIME='2018-10-04';


DECLARE @ReferralID BIGINT;
DECLARE @EmployeeID BIGINT;
DECLARE @EmployeeTimeSlotDetailID VARCHAR(MAX);
DECLARE  @TempStartDate DATETIME;

DECLARE cur CURSOR FOR SELECT StartDate, ReferralID, EmployeeID, EmployeeTimeSlotDetailID FROM @TempTable
OPEN cur

FETCH NEXT FROM cur INTO @TempStartDate,@ReferralID, @EmployeeID,@EmployeeTimeSlotDetailID

WHILE @@FETCH_STATUS = 0 BEGIN
	
	IF(@StartDate < @TempStartDate)
	SET @StartDate=@TempStartDate

	--SELECT @ReferralID, @EmployeeID,@EmployeeTimeSlotDetailID
    EXEC CreateBulkSchedules @ReferralID, @EmployeeID , 0, @EmployeeTimeSlotDetailID, @StartDate , @EndDate, @ScheduleStatusID,@SameDateWithTimeSlot, @loggedInId , @SystemID 
    
	FETCH NEXT FROM cur INTO  @TempStartDate,@ReferralID, @EmployeeID,@EmployeeTimeSlotDetailID
END

CLOSE cur    
DEALLOCATE cur


END
