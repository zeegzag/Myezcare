-- EXEC HC_GetReferralForScheduling01 @ReferralName = '', @StartDate = '2018/03/19', @EndDate = '2018/03/25'    
CREATE PROCEDURE [dbo].[HC_GetReferralForScheduling01]    
@ReferralName VARCHAR(MAX)='',    
@StartDate DATE,--='2018-03-01',    
@EndDate DATE--='2018-03-31'    
AS    
BEGIN    
    
  DECLARE @TempReferral TABLE(    
  ReferralID BIGINT,    
  FirstName VARCHAR(50),    
  LastName VARCHAR(50),    
  NewAllocatedHrs DECIMAL(10, 2)    
 )    
     
  
  INSERT INTO @TempReferral    
  SELECT DISTINCT  R.ReferralID, R.FirstName, R.LastName,    
  NewAllocatedHrs= SUM(DATEDIFF(second, RTD.ReferralTSStartTime, RTD.ReferralTSEndTime) / 3600.0) OVER(PARTITION BY RTD.ReferralID)    
  FROM Referrals R    
  LEFT JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=R.ReferralID AND (RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate)    
  WHERE  R.IsDeleted=0 AND R.ReferralStatusID=1 --AND R.ReferralID=1953    
  AND ((@ReferralName IS NULL OR LEN(@ReferralName)=0)               
  OR              
    ((R.FirstName LIKE '%'+@ReferralName+'%' )OR                
     (R.LastName  LIKE '%'+@ReferralName+'%') OR                
     (R.FirstName +' '+R.LastName like '%'+@ReferralName+'%') OR                
     (R.LastName +' '+R.FirstName like '%'+@ReferralName+'%') OR                
     (R.FirstName +', '+R.LastName like '%'+@ReferralName+'%') OR                
     (R.LastName +', '+R.FirstName like '%'+@ReferralName+'%'))              
  )     
 ORDER BY LASTNAME ASC    

 --SELECT * FROM @TempReferral  

 -- EXEC HC_GetReferralForScheduling01 @ReferralName = '', @StartDate = '2018/07/02', @EndDate = '2018/07/08'    
    
     
	DECLARE @TempReferral01 TABLE(    
	  ReferralID BIGINT,    
	  FirstName VARCHAR(50),    
	  LastName VARCHAR(50),    
	  NewAllocatedHrs DECIMAL(10, 2),    
	  NewUsedHrsReal DECIMAL(10, 2)    
	 )    
    
	INSERT @TempReferral01    
	SELECT DISTINCT  R.*,--SM.ScheduleID,    
	NewUsedHrsReal =CONVERT(DECIMAL(10, 2), SUM(DATEDIFF(second, RTD.ReferralTSStartTime, RTD.ReferralTSEndTime) / 3600.0) OVER(PARTITION BY SM.ReferralID) )    
	FROM @TempReferral R    
	LEFT JOIN ScheduleMasters SM ON SM.ReferralID = R.ReferralID AND SM.IsDeleted=0    
	LEFT JOIN ReferralTimeSlotDates RTD ON    
	RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate    
	AND (RTD.UsedInScheduling=1 AND RTD.ReferralID=SM.ReferralID AND RTD.ReferralTSDateID = SM.ReferralTSDateID)    
	WHERE 1=1 ORDER BY LASTNAME ASC    
    
	SELECT * FROM @TempReferral01  
    
    

SELECT DISTINCT T.*
,     
NewUsedHrs =
ISNULL(T.NewUsedHrsReal,0) + 
ISNULL(
CONVERT( DECIMAL(10, 2), 
SUM( DATEDIFF(second, RTD.ReferralTSStartTime, RTD.ReferralTSEndTime) / 3600.0 ) OVER(PARTITION BY RTD.ReferralID)
)
,0)    
FROM @TempReferral01 T    
LEFT JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=T.ReferralID AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate 
AND RTD.UsedInScheduling=0

   EXEC HC_GetReferralForScheduling01 @ReferralName = '', @StartDate = '2018/07/02', @EndDate = '2018/07/08'    

--For Testing
--select *  
--FROM @TempReferral01 T   
--left Join ReferralTimeSlotDates RTD on RTD.ReferralID=T.ReferralID  
--AND ReferralTSDate BETWEEN '2018-07-02' AND '2018-07-08' and RTD.UsedInScheduling=0 

END
