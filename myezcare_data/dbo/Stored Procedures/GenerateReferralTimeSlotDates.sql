-- EXEC GenerateReferralTimeSlotDates  '2018-04-07','2019-03-15'  
-- EXEC GenerateReferralTimeSlotDates @StartDate = '2018/04/06', @EndDate = '2019/04/01'  
CREATE PROCEDURE [dbo].[GenerateReferralTimeSlotDates]  
@StartDate AS DATETIME,  
@EndDate AS DATETIME,  
@ReferralID BIGINT=0  
AS  
BEGIN  
--SET @StartDate='2018-03-01';  
  
SET @StartDate=CONVERT(DATE,@StartDate);  
SET @EndDate=CONVERT(DATE,@EndDate);  
  
-- SELECT * FROM ReferralTimeSlotDates ORDER BY ReferralTSDate DESC  
-- DELETE FROM  ReferralTimeSlotDates  
-- DELETE FROM  ReferralTimeSlotDetails  
-- DELETE FROM ReferralTimeSlotMaster  
-- TRUNCATE TABLE ReferralTimeSlotDates  
-- TRUNCATE TABLE ReferralTimeSlotDetails  
-- TRUNCATE TABLE ReferralTimeSlotMaster  
  
  
  
INSERT INTO ReferralTimeSlotDates   
SELECT T.ReferralID,T.ReferralTimeSlotMasterID, T.ReferralTSDate,T.ReferralTSStartTime,T.ReferralTSEndTime,  
T.UsedInScheduling,T.Notes,T.DayNumber,T.ReferralTimeSlotDetailID,0,NULL,0
  
FROM (  
SELECT E.ReferralID,ETM.ReferralTimeSlotMasterID, ReferralTSDate=IndividualDate,ETMEndDate=ETM.EndDate,  
ReferralTSStartTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), StartTime, 108)),  
ReferralTSEndTime=CONVERT(DATETIME, CONVERT(CHAR(8), T1.IndividualDate, 112) + ' ' + CONVERT(CHAR(8), EndTime, 108)),  
ETSD.UsedInScheduling,ETSD.Notes,DayNumber=T1.DayNameInt,ETSD.ReferralTimeSlotDetailID  
FROM DateRange(@StartDate, @EndDate) T1  
INNER JOIN ReferralTimeSlotDetails ETSD ON ETSD.Day=T1.DayNameInt  AND ETSD.IsDeleted=0  
INNER JOIN ReferralTimeSlotMaster ETM ON ETM.ReferralTimeSlotMasterID=ETSD.ReferralTimeSlotMasterID  AND ETM.IsDeleted=0  
INNER JOIN Referrals E ON E.ReferralID=ETM.ReferralID  
) AS T  
LEFT JOIN ReferralTimeSlotDates ETSDT ON ETSDT.ReferralTSStartTime= T.ReferralTSStartTime AND ETSDT.ReferralTSEndTime= T.ReferralTSEndTime   
AND ETSDT.ReferralID=T.ReferralID  
WHERE  ETSDT.ReferralTSDateID IS NULL  AND T.ReferralTSDate <= ISNULL(ETMEndDate,@EndDate) AND (@ReferralID=0 OR T.ReferralID=@ReferralID)  
ORDER BY T.ReferralID ASC, T.ReferralTimeSlotMasterID ASC  
  
  
  
  
DECLARE @InfinateDate DATE='2099-12-31';  
  
UPDATE RTD SET RTD.UsedInScheduling=0,RTD.Notes=RH.PatientOnHoldReason, RTD.OnHold=1, ReferralOnHoldDetailID=RH.ReferralOnHoldDetailID  
--SELECT RTD.*, RH.*  
 FROM ReferralOnHoldDetails RH  
 INNER JOIN ReferralTimeSlotDates RTD ON RH.ReferralID=RTD.ReferralID AND RTD.ReferralTSDate BETWEEN RH.StartDate AND ISNULL(RH.EndDate,@InfinateDate)  
 WHERE (@ReferralID=0 OR (RTD.ReferralID=@ReferralID AND RH.ReferralID=@ReferralID)) AND RTD.OnHold=0 AND RTD.UsedInScheduling=1 AND  
  
(  
(RH.StartDate BETWEEN @StartDate AND @EndDate OR ISNULL(RH.EndDate,@InfinateDate) BETWEEN @StartDate AND @EndDate ) OR  
(@StartDate BETWEEN RH.StartDate AND ISNULL(RH.EndDate,@InfinateDate) OR @EndDate BETWEEN RH.StartDate AND ISNULL(RH.EndDate,@InfinateDate) )  
)  
  
  
  
  
--INSERT INTO ReferralTimeSlotDates   
--SELECT E.ReferralID,ETM.ReferralTimeSlotMasterID, IndividualDate,  
--CONVERT(DATETIME, CONVERT(CHAR(8), IndividualDate, 112) + ' ' + CONVERT(CHAR(8), StartTime, 108)),  
--CONVERT(DATETIME, CONVERT(CHAR(8), IndividualDate, 112) + ' ' + CONVERT(CHAR(8), EndTime, 108)), ETSD.UsedInScheduling,ETSD.Notes  
--FROM DateRange(@StartDate, @EndDate) T  
--INNER JOIN ReferralTimeSlotDetails ETSD ON ETSD.Day=T.DayNameInt  AND ETSD.IsDeleted=0  
--INNER JOIN ReferralTimeSlotMaster ETM ON ETM.ReferralTimeSlotMasterID=ETSD.ReferralTimeSlotMasterID  AND ETM.IsDeleted=0  
--INNER JOIN Referrals E ON E.ReferralID=ETM.ReferralID  
--LEFT JOIN ReferralTimeSlotDates ETSDT ON ETSDT.ReferralTSDate= IndividualDate AND ETSDT.ReferralID=ETM.ReferralID  
--WHERE  ETSDT.ReferralTSDateID IS NULL  AND IndividualDate <= ISNULL(ETM.EndDate,@EndDate)  
---- EndDate  
--ORDER BY ReferralID ASC, ReferralTimeSlotMasterID ASC  
  
  
END
