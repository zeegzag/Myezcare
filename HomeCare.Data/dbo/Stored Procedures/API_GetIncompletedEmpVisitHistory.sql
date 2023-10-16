--EXEC API_GetIncompletedEmpVisitHistory @ServerCurrentDate = N'2021-02-26', @EmployeeID = N'86', @Name = NULL
CREATE PROCEDURE [dbo].[API_GetIncompletedEmpVisitHistory]                  
 @EmployeeID BIGINT,                          
 @ServerCurrentDate DATE,                          
 @Name NVARCHAR(100)=null,                          
 @StartDate DATE=null,                          
 @EndDate DATE=null                          
AS                                                      
BEGIN                            
                    
 SELECT ClockInTime=SM.StartDate,ClockOutTime=SM.EndDate,R.FirstName,R.LastName,EV.EmployeeVisitID,SM.StartDate,SM.ReferralID,SM.ScheduleID,                        
 ImageURL=R.ProfileImagePath,Editable=1--CASE WHEN CONVERT(DATE,SM.StartDate)>=CONVERT(DATE,dateadd(day,-7,@ServerCurrentDate)) THEN 1 ELSE 0 END                      
 FROM EmployeeVisits EV                          
 INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID AND SM.EmployeeID=@EmployeeId AND EV.IsDeleted=0                          
 INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID                          
 WHERE                   
 (EV.ClockInTime IS NOT NULL or EV.ClockOutTime IS NOT NULL )AND                  
 (EV.IsPCACompleted=0) AND EV.IsDeleted=0 --AND EV.SurveyCompleted=1            
 AND CONVERT(DATE,SM.EndDate) <= @ServerCurrentDate                          
 AND                               
   ((@Name IS NULL OR LEN(@Name)=0)                                 
   OR (                                
       (R.FirstName LIKE '%'+@Name+'%' )OR                                  
    (R.LastName  LIKE '%'+@Name+'%') OR                                  
    (R.FirstName +' '+R.LastName like '%'+@Name+'%') OR                                  
    (R.LastName +' '+R.FirstName like '%'+@Name+'%') OR                                  
    (R.FirstName +', '+R.LastName like '%'+@Name+'%') OR                                  
    (R.LastName +', '+R.FirstName like '%'+@Name+'%')))                          
 AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(DATE,sm.StartDate) >= @StartDate)                                  
   AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(DATE,sm.EndDate) <= @EndDate)                                                              
                                                         
  order by  sm.StartDate   desc                                                  
                                                        
END  