CREATE PROCEDURE [dbo].[HC_PrivateDuty_GetReferralForScheduling]  
@ReferralName VARCHAR(MAX)='',                  
@StartDate DATE, --='2018-03-01',                  
@EndDate DATE, --='2018-03-31'                  
@SchStatus int = 2          
          
AS                  
BEGIN                  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()         
  DECLARE @TempReferral TABLE(                  
  ReferralID BIGINT,                  
  FirstName VARCHAR(5000),                    
  LastName VARCHAR(5000),   
  ReferralName VARCHAR(5000),                  
  NewAllocatedHrs DECIMAL(10, 2)                  
 )                  
                   
  INSERT INTO @TempReferral                  
  SELECT DISTINCT  R.ReferralID, R.FirstName, R.LastName,dbo.GetGenericNameFormat(R.FirstName,R.MiddleName, R.LastName,@NameFormat),                   
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
 --SELECT * FROM @TempReferral                  
                  
 -- EXEC HC_GetReferralForScheduling @ReferralName = 'NIXON Aco', @StartDate = '2018/03/26', @EndDate = '2018/03/26'                  
  select T1.* from                
(SELECT DISTINCT T.*,                  
NewUsedHrs = ISNULL(NewUsedHrsReal,0) + ISNULL( CONVERT(DECIMAL(10, 2), SUM(DATEDIFF(second, RTD.ReferralTSStartTime, RTD.ReferralTSEndTime) / 3600.0) OVER(PARTITION BY RTD.ReferralID) ),0)                    
--NewUsedHrs =NewUsedHrsReal + ISNULL( CONVERT(DECIMAL(10, 2), SUM(DATEDIFF(second, RTD.ReferralTSStartTime, RTD.ReferralTSEndTime) / 3600.0) OVER(PARTITION BY RTD.ReferralTSDateID) ),0)               
           
 FROM (                    
 SELECT DISTINCT  R.*,--SM.ScheduleID,                  
    NewUsedHrsReal =CONVERT(DECIMAL(10, 2), SUM(DATEDIFF(second, RTD.ReferralTSStartTime, RTD.ReferralTSEndTime) / 3600.0) OVER(PARTITION BY SM.ReferralID) )                  
    FROM @TempReferral R                  
    LEFT JOIN ScheduleMasters SM ON SM.ReferralID = R.ReferralID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0                  
    LEFT JOIN ReferralTimeSlotDates RTD ON                  
    RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate                  
    AND (RTD.UsedInScheduling=1 AND RTD.ReferralID=SM.ReferralID AND RTD.ReferralTSDateID = SM.ReferralTSDateID)                  
    --OR                  
    -- (RTD.UsedInScheduling=0 AND RTD.ReferralID=R.ReferralID)                  
    WHERE 1=1                  
                  
) AS T                       
LEFT JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=T.ReferralID AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate AND RTD.UsedInScheduling=0                        
 )AS T1          
 where ((@SchStatus= 1 and NewAllocatedHrs = NewUsedHrs) or (@SchStatus = 2 and NewAllocatedHrs != NewUsedHrs) or (@SchStatus = -1 or @SchStatus is null))  and NewAllocatedHrs is not null          
 ORDER BY LASTNAME ASC        
 --(@SchStatus= -1 OR @SchStatus IS NULL)           
                 
                  
END                  
