CREATE procedure [dbo].[GetEmpClockInOutList]        
@StartDate DATE =NULL,            
@EndDate  DATE=NULL,    
@EmployeeName VARCHAR(200),    
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
                        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN t.EmpFirstName END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN t.EmpFirstName END END DESC,                       
                     
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN CONVERT(varchar(50),t.LastName) END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN CONVERT(varchar(50),t.LastName) END END DESC,                 
             
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleStartTime' THEN  CONVERT(date, t.ScheduleStartTime, 105) END END ASC,                                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleStartTime' THEN  CONVERT(date, t.ScheduleStartTime, 105) END END DESC,            
             
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleEndTime' THEN  CONVERT(date, t.ScheduleEndTime, 105) END END ASC,                                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleEndTime' THEN  CONVERT(date, t.ScheduleEndTime, 105) END END DESC,                      
                     
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockIn' THEN CAST(t.ClockIn AS BIT) END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockIn' THEN CAST(t.ClockIn AS BIT) END END DESC,                                                  
            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockOut' THEN CAST(t.ClockOut AS BIT) END END ASC,                                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockOut' THEN CAST(t.ClockOut AS BIT) END END DESC                 
                    
   ) AS ROW,                            
   t.* FROM                            
   (                  
            
            
SELECT DISTINCT R.ReferralID, E.EmployeeID,            
EmpFirstName=E.FirstName, EmpLastName=E.LastName,            
R.FirstName, R.LastName,             
ScheduleStartTime= SM.StartDate,            
ScheduleEndTime= SM.EndDate,            
ClockIn=CASE WHEN EV.ClockInTime IS NULL THEN 0 ELSE 1 END ,            
ClockOut=CASE WHEN EV.ClockOutTime IS NULL THEN 0 ELSE 1 END              
FROM ScheduleMasters SM            
INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID            
INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID  
INNER JOIN ReferralTimeSlotDates RTSD ON RTSD.ReferralTSDateID=SM.ReferralTSDateID AND RTSD.IsDenied=0  
LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID AND EV.IsDeleted=0        
WHERE SM.IsDeleted=0 AND (EV.ClockInTime IS NULL OR EV.ClockOutTime IS NULL)        
--AND (((@StartDate is null OR SM.StartDate >= @StartDate) and (@EndDate is null OR SM.EndDate <= @EndDate))      
--    OR(@StartDate is null OR CONVERT(DATE,SM.StartDate)=@StartDate) OR(@EndDate is null OR CONVERT(DATE,SM.EndDate)=@EndDate))    

AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND CONVERT(DATE,SM.StartDate) >= @StartDate) OR   
(@EndDate IS NOT NULL AND @StartDate IS NULL AND CONVERT(DATE,SM.EndDate) <= @EndDate) OR  
(@StartDate IS NOT NULL AND @EndDate IS NOT NULL AND (CONVERT(DATE,SM.StartDate) >= @StartDate AND CONVERT(DATE,SM.EndDate) <= @EndDate)) OR  
(@StartDate IS NULL AND @EndDate IS NULL)  
)

AND         
   ((@EmployeeName IS NULL OR LEN(e.LastName)=0)           
   OR (          
       (E.FirstName LIKE '%'+@EmployeeName+'%' )OR            
    (E.LastName  LIKE '%'+@EmployeeName+'%') OR            
    (E.FirstName +' '+E.LastName like '%'+@EmployeeName+'%') OR            
    (E.LastName +' '+E.FirstName like '%'+@EmployeeName+'%') OR            
    (E.FirstName +', '+E.LastName like '%'+@EmployeeName+'%') OR            
    (E.LastName +', '+E.FirstName like '%'+@EmployeeName+'%')))       
    
--AND (@StartDate IS NULL OR @EndDate IS NULL OR (SM.StartDate BETWEEN @StartDate AND @EndDate OR SM.EndDate BETWEEN @StartDate AND @EndDate))        
 --SM.StartDate < GETDATE()          
  )               
  AS T)  AS T1 )                
                
SELECT * FROM CTEGetEmpClockInOutList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                             
END
