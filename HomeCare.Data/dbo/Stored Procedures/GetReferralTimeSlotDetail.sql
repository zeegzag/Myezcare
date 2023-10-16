--EXEC GetReferralTimeSlotDetail @ReferralID = '43', @ReferralTimeSlotMasterID = '240', @ClientName = '', @SortExpression = 'ReferralID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'  
  
    
CREATE  PROCEDURE [dbo].[GetReferralTimeSlotDetail]                  
@ReferralID BIGINT =0,     
@ReferralTimeSlotMasterID BIGINT =0,         
@StartDate date=null,                                                  
@EndDate date=null,                           
@ClientName VARCHAR(MAX)=null,    
 @SortExpression NVARCHAR(100)=NULL,                                                
 @SortType NVARCHAR(10)=NULL,                                              
 @FromIndex INT=1,                                              
 @PageSize INT =50                 
              
AS  
BEGIN                        
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 ;WITH List AS                                                    
 (                                                     
  SELECT *,COUNT(T1.ReferralTSDateID) OVER() AS Count FROM                                                     
  (                                                    
   SELECT ROW_NUMBER() OVER (ORDER BY                                                     
                                                
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralTSDateID' THEN t.ReferralTSDateID END END ASC,                                              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralTSDateID' THEN t.ReferralTSDateID END END DESC                          
                                                  
                                            
   ) AS ROW,                                                    
   t.*  FROM     (                                          
                                
SELECT  r.ReferralID, rtm.ReferralTimeSlotMasterID,rtds.ReferralTSDate,rtds.ReferralTSStartTime,rtds.ReferralTSEndTime,    
sm.EmployeeID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) as EmployeeName,    
ISNULL(sm.ReferralTSDateID,0) ReferralTSDateID,ISNULL(sm.ScheduleID,0) ScheduleID, sm.ScheduleStatusID,rtds.DayNumber , [dbo].[GetWeekDay](rtds.DayNumber) DayName    
FROM ReferralTimeSlotMaster rtm    
INNER JOIN ReferralTimeSlotDates rtds on rtds.ReferralTimeSlotMasterID=rtm.ReferralTimeSlotMasterID AND rtds.IsDenied = 0 --AND  rtds.ReferralTSDate BETWEEN rtm.StartDate AND rtm.EndDate --rtm.StartDate>=rtds.ReferralTSDate AND rtm.EndDate <=rtds.ReferralTSDate    
INNER JOIN ReferralTimeSlotDetails rtd on rtd.ReferralTimeSlotMasterID=rtm.ReferralTimeSlotMasterID and rtds.DayNumber = rtd.Day    
INNER JOIN Referrals r on r.ReferralID=rtm.ReferralID    
LEFT JOIN ScheduleMasters sm on sm.ReferralTSDateID= rtds.ReferralTSDateID     
LEFT JOIN Employees e on e.EmployeeID=sm.EmployeeID    
where rtm.ReferralID = @ReferralID AND rtm.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID      
AND ReferralTSDate >= CONVERT(DATE,GETDATE())      
AND ((@StartDate is null OR rtds.ReferralTSDate >= @StartDate) AND (@EndDate is null OR rtds.ReferralTSDate <= @EndDate))              
AND (((@ClientName IS NULL) or (@ClientName='') or LEN(LTRIM(rtrim(@ClientName)))=0  )             
 OR (                          
 (e.FirstName LIKE '%'+@ClientName+'%' ) OR                          
 (e.LastName LIKE '%'+@ClientName+'%') OR                          
 (e.FirstName +' '+e.LastName like '%'+@ClientName+'%') OR                          
 (e.LastName +' '+e.FirstName like '%'+@ClientName+'%') OR                          
 (e.FirstName +', '+e.LastName like '%'+@ClientName+'%') OR                          
 (e.LastName +', '+e.FirstName like '%'+@ClientName+'%')))     
        
  ) t                              
                              
)  AS T1 )                                        
                                        
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)     
                        
END    