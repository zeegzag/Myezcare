CREATE PROCEDURE [dbo].[API_GetPastVisitDetail]            
 @FromIndex INT,            
 @ToIndex INT,            
 @SortExpression NVARCHAR(100),            
 @SortType NVARCHAR(10),            
 @ReferralID BIGINT,            
 @EmployeeID BIGINT,            
 @IsCompletedVisit BIT,            
 @CurrentDateTime NVARCHAR(100)                              
AS                                                      
BEGIN   
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 Declare @Permission int                                                   
 IF(@SortExpression IS NULL OR @SortExpression ='')                                                      
 BEGIN                                                      
  SET @SortExpression = 'StartDate'                                                      
  SET @SortType='DESC'                                                      
 END                                                      
          
--Modified by Pallav to address the issue to showing only the user's timesheet who has logged in        
                                                       
 ;WITH CTEVisitList AS                                                          
 (                                                               
  SELECT ROW_NUMBER() OVER                                                       
      (ORDER BY                                                      
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartDate' THEN T1.StartDate END                                                      
       END ASC,                                                      
       CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartDate' THEN T1.StartDate         
  END                                                      
       END DESC                                                    
       --CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FirstName' THEN e.FirstName END                                                      
       --END ASC,                    
       --CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FirstName' THEN e.FirstName END                    
       --END DESC                    
                                                             
      ) AS Row,*,COUNT(T1.ScheduleID) OVER() AS Count                                                      
  FROM                                                      
  (                                                      
   SELECT * FROM (SELECT TOP 50 RankOrder=ROW_NUMBER() OVER (PARTITION BY SM.EmployeeTSDateID,SM.ReferralTSDateID ORDER BY SM.CreatedDate DESC),                    
 sm.ScheduleID,EmployeeName = dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),e.FirstName,ss.ScheduleStatusName,ev.ClockInTime,ev.ClockOutTime,sm.StartDate,sm.EndDate,sm.ReferralID,sm.EmployeeID,                              
   ImageURL=e.ProfileImagePath,rtd.IsDenied                                        
 FROM dbo.ScheduleMasters sm                                                                        
 INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID                                                        
 INNER JOIN ScheduleStatuses ss ON ss.ScheduleStatusID=sm.ScheduleStatusID                              
 INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID                              
 LEFT JOIN ReferralTimeSlotDates rtd ON rtd.ReferralTSDateID=sm.ReferralTSDateID                                  
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID                                             
 WHERE sm.ReferralID=@ReferralID             
 AND ((@IsCompletedVisit=1  AND ev.IsPCACompleted=1) OR (@IsCompletedVisit=0 AND             
 ev.ClockInTime IS NOT NULL AND ev.ClockOutTime IS NOT NULL AND (ev.IsPCACompleted=0))            
 AND ((rtd.IsDenied=0 AND sm.IsDeleted=0) OR (rtd.IsDenied=1 AND sm.IsDeleted=1) OR (rtd.IsDenied=NULL))) AND CONVERT(DATETIME,sm.StartDate)<=GETDATE()                                                         
 and (@employeeid Is null or sm.employeeid=@employeeid)        
 ORDER BY sm.StartDate DESC) T WHERE T.RankOrder=1                    
  ) AS T1                    
 )                  
                                                         
  SELECT * FROM CTEVisitList WHERE ROW BETWEEN @FromIndex AND @ToIndex                                                      
                                                        
                                                        
END 