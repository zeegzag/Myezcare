CREATE PROCEDURE [dbo].[API_GetEmpVisitHistory]                            
 @FromIndex INT,                  
 @ToIndex INT,                  
 @SortExpression NVARCHAR(100),                  
 @SortType NVARCHAR(10),                  
 @EmployeeID BIGINT,                  
 @ServerCurrentDate DATE,                  
 @Name NVARCHAR(100)=null,                  
 @StartDate DATE=null,                  
 @EndDate DATE=null                  
AS                                              
BEGIN                    
                                            
 IF(@SortExpression IS NULL OR @SortExpression ='')                                              
 BEGIN                                              
  SET @SortExpression = 'StartDate'                                              
  SET @SortType='DESC'                                              
 END                                              
                                               
 ;WITH CTEVisitHistoryList AS                                                  
 (                                                       
  SELECT ROW_NUMBER() OVER                                               
      (ORDER BY                                              
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartDate' THEN T.StartDate END                                              
       END ASC,                                              
       CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartDate' THEN T.StartDate END                                              
       END DESC                                            
       --CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FirstName' THEN e.FirstName END                                              
       --END ASC,                                              
       --CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FirstName' THEN e.FirstName END                                              
       --END DESC                                              
                                                     
      ) AS Row,*,COUNT(T.EmployeeVisitID) OVER() AS Count                                              
  FROM                                              
  (                                              
 SELECT ClockInTime=SM.StartDate,ClockOutTime=SM.EndDate,R.FirstName,R.LastName,EV.EmployeeVisitID,SM.StartDate,SM.ReferralID,SM.ScheduleID,                
 ImageURL=R.ProfileImagePath,Editable=0--CASE WHEN CONVERT(DATE,SM.StartDate)>=CONVERT(DATE,dateadd(day,-7,GETDATE())) THEN 1 ELSE 0 END              
 FROM EmployeeVisits EV                  
 INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID AND SM.EmployeeID=@EmployeeId AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0                  
 INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID                  
 WHERE --EV.SurveyCompleted=1  AND
 EV.IsPCACompleted=1 AND EV.IsDeleted=0                  
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
  ) AS T                                              
 )                                              
                                                 
  SELECT * FROM CTEVisitHistoryList WHERE ROW BETWEEN @FromIndex AND @ToIndex                  
                                                
                                              
END 