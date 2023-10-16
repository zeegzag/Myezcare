-- GetPatientNotScheduleList '','',1,10        
CREATE PROCEDURE [dbo].[GetPatientNotScheduleList]        
@StartDate DATE =NULL,        
@EndDate  DATE=NULL,                     
@SortExpression NVARCHAR(100),                          
@SortType NVARCHAR(10),                        
@FromIndex INT,                        
@PageSize INT            
            
AS                        
BEGIN                        
                        
;WITH CTEGetEmpClockInOutList AS                        
 (                         
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                         
  (                        
   SELECT ROW_NUMBER() OVER (ORDER BY                         
                    
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END DESC,                   
                 
         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WeeklyAllocatedHours' THEN  CONVERT(BIGINT, t.WeeklyAllocatedHours) END END ASC,      
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WeeklyAllocatedHours' THEN  CONVERT(BIGINT, t.WeeklyAllocatedHours) END END DESC,        
        
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WeeklyUsedHours' THEN  CONVERT(BIGINT, t.WeeklyUsedHours) END END ASC,      
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WeeklyUsedHours' THEN  CONVERT(BIGINT, t.WeeklyUsedHours) END END DESC,        
        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WeeklyRemainingHours' THEN  CONVERT(BIGINT, t.WeeklyRemainingHours) END END ASC,      
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WeeklyRemainingHours' THEN  CONVERT(BIGINT, t.WeeklyRemainingHours) END END DESC,      
       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UnusedHours' THEN  CONVERT(BIGINT, t.WeeklyUnusedHours) END END ASC,      
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UnusedHours' THEN  CONVERT(BIGINT, t.WeeklyUnusedHours) END END DESC        
                
   ) AS ROW,                        
   t.*  FROM     (              
        
SELECT *,        
WeeklyRemainingHours= CASE WHEN WeeklyAllocatedHours > WeeklyUsedHours + WeeklyUnusedHours THEN WeeklyAllocatedHours - WeeklyUsedHours - WeeklyUnusedHours ELSE 0 END        
 FROM (        
        
        
SELECT DISTINCT E.ReferralID,        
E.FirstName, LastName,        
WeeklyAllocatedHours = SUM(DATEDIFF(HOUR, ETD.ReferralTSStartTime, ETD.ReferralTSEndTime)) OVER(PARTITION BY ETD.ReferralID),        
WeeklyUsedHours = SUM(CASE WHEN SM.ScheduleID IS NULL THEN 0 ELSE DATEDIFF(HOUR, SM.StartDate, SM.EndDate) END) OVER(PARTITION BY ETD.ReferralID),      
WeeklyUnusedHours = SUM(CASE WHEN ETD.UsedInScheduling=1 THEN 0 ELSE  DATEDIFF(HOUR, ETD.ReferralTSStartTime, ETD.ReferralTSEndTime) END) OVER(PARTITION BY ETD.ReferralID)      
FROM ReferralTimeSlotDates ETD        
INNER JOIN Referrals E ON E.ReferralID= ETD.ReferralID AND E.IsDeleted=0        
INNER JOIN ReferralTimeSlotDetails RTD ON ETD.ReferralTimeSlotDetailID=RTD.ReferralTimeSlotDetailID --AND RTD.IsDeleted=0 -- AND RTD.UsedInScheduling=0      
LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID= ETD.ReferralTSDateID AND SM.IsDeleted=0      
        
WHERE 1=1  
--AND (((@StartDate is null OR ETD.ReferralTSDate >= @StartDate) and (@EndDate is null OR ETD.ReferralTSDate <= @EndDate))    
--    OR(@StartDate is null OR CONVERT(DATE,ETD.ReferralTSDate)=@StartDate) OR(@EndDate is null OR CONVERT(DATE,ETD.ReferralTSDate)=@EndDate))

AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND ETD.ReferralTSDate >= @StartDate) OR   
(@EndDate IS NOT NULL AND @StartDate IS NULL AND ETD.ReferralTSDate <= @EndDate) OR  
(@StartDate IS NOT NULL AND @EndDate IS NOT NULL AND (ETD.ReferralTSDate >= @StartDate AND ETD.ReferralTSDate <= @EndDate)) OR  
(@StartDate IS NULL AND @EndDate IS NULL)  
)

) AS TEMP WHERE TEMP.WeeklyAllocatedHours > TEMP.WeeklyUsedHours    
        
        
        
) AS T WHERE T.WeeklyRemainingHours > 0    
        
)  AS T1 )            
            
SELECT * FROM CTEGetEmpClockInOutList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                         
END
