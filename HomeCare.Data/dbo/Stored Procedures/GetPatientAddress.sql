-- EXEC [GetPatientAddress] @StartDate = '2021/06/05', @EndDate = '2021/06/05', @EmployeeName = '', @SortExpression = 'ScheduleStartTime', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'  
             
CREATE procedure [dbo].[GetPatientAddress]                        
@StartDate DATE =NULL,                              
@EndDate  DATE=NULL,                      
@EmployeeName VARCHAR(200),                      
@SortExpression NVARCHAR(100),                                                
@SortType NVARCHAR(10),                                              
@FromIndex INT=100,                                              
@PageSize INT                                  
                                  
AS                                              
BEGIN    
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
--Set @StartDate= getdate()-1                  
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
     
SELECT DISTINCT SM.ScheduleID,EV.EmployeeVisitid,R.ReferralID, E.EmployeeID,EmpFirstName=E.FirstName, EmpLastName=E.LastName,Employee = dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat),
R.FirstName, R.LastName,  Patient = dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),                             
ScheduleStartTime= SM.StartDate,ScheduleEndTime= SM.EndDate, ClockIn=CASE WHEN EV.ClockInTime IS NULL THEN 0 ELSE 1 END , ClockOut=CASE WHEN EV.ClockOutTime IS NULL THEN 0 ELSE 1 END,EV.IsPCACompleted,                              
CONCAT(c.Address,c.ApartmentNo,c.City,c.ZipCode) As Address,c.Latitude as lat,c.Longitude as lng,c.Phone1  as Phone,FORMAT(sm.StartDate,'hh:mm tt') as StartTime,FORMAT(sm.EndDate,'hh:mm tt') as EndTime      
FROM ScheduleMasters SM                              
INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID                              
INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID                    
INNER JOIN ReferralTimeSlotDates RTSD ON RTSD.ReferralTSDateID=SM.ReferralTSDateID AND RTSD.IsDenied=0                    
Left JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID AND EV.IsDeleted=0        
inner join ContactMappings cm on cm.referralid=R.referralid AND cm.IsEmergencyContact=0       
inner join Contacts c on c.ContactID=cm.ContactID AND c.IsDeleted=0       
WHERE ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0                      
   AND R.ReferralStatusID=1                
AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND CONVERT(DATE,SM.StartDate) >= @StartDate) OR                     
(@EndDate IS NOT NULL AND @StartDate IS NULL AND CONVERT(DATE,SM.EndDate) <= @EndDate) OR                    
(@StartDate IS NOT NULL AND @EndDate IS NOT NULL AND (CONVERT(DATE,SM.StartDate) >= @StartDate AND CONVERT(DATE,SM.EndDate) <= @EndDate)) OR                    
(@StartDate IS NULL AND @EndDate IS NULL)                    
)                  
                  
                         
                      
--AND (@StartDate IS NULL OR @EndDate IS NULL OR (SM.StartDate BETWEEN @StartDate AND @EndDate OR SM.EndDate BETWEEN @StartDate AND @EndDate))                          
 --SM.StartDate < GETDATE()                            
  )                                 
  AS T)  AS T1 )                                  
                                  
SELECT * FROM CTEGetEmpClockInOutList                                                                    
                                                                                          
         
WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   order by    ScheduleStartTime asc                                         
END  