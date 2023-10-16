CREATE procedure [dbo].[GetPatientClockInOutList]                        
@StartDate DATE =NULL,                              
@EndDate  DATE=NULL,                      
@PatientName VARCHAR(200),                      
@SortExpression NVARCHAR(100),                                                
@SortType NVARCHAR(10),                                              
@FromIndex INT,                                              
@PageSize INT                                                             
AS                                              
BEGIN                                              
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                
;WITH CTEGetPatientClockInOutList AS                                              
 (                                               
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                                               
  (                                              
   SELECT ROW_NUMBER() OVER (ORDER BY                                               
                                       
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN CONVERT(varchar(50),t.LastName) END END ASC,                                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN CONVERT(varchar(50),t.LastName) END END DESC,                                   
                               
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleStartTime' THEN  CONVERT(date, t.ScheduleStartTime, 105) END END ASC,                                                              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleStartTime' THEN  CONVERT(date, t.ScheduleStartTime, 105) END END DESC,                              
                               
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ScheduleEndTime' THEN  CONVERT(date, t.ScheduleEndTime, 105) END END ASC,                                                              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ScheduleEndTime' THEN  CONVERT(date, t.ScheduleEndTime, 105) END END DESC,     
     
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Attendence' THEN CONVERT(varchar(50),t.IsPatientAttendedSchedule) END END ASC,                                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Attendence' THEN CONVERT(varchar(50),t.IsPatientAttendedSchedule) END END DESC,                                         
                                       
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockIn' THEN CAST(t.ClockIn AS BIT) END END ASC,                                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockIn' THEN CAST(t.ClockIn AS BIT) END END DESC,                                                                    
                              
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockOut' THEN CAST(t.ClockOut AS BIT) END END ASC,                                                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockOut' THEN CAST(t.ClockOut AS BIT) END END DESC                                   
                                      
   ) AS ROW,                                              
   t.* FROM                                              
   (                                    
                              
                              
SELECT DISTINCT R.ReferralID, E.EmployeeID, R.FirstName,R.LastName, Patient = dbo.GetGenericNameFormat(R.FirstName,R.MiddleName, R.LastName,@NameFormat), 
SM.ScheduleID, SM.ScheduleStatusID,SM.IsPatientAttendedSchedule,    
CASE WHEN SM.IsPatientAttendedSchedule = 1 THEN 'Present' WHEN SM.IsPatientAttendedSchedule = 0 THEN 'Absent' ELSE 'NA' END AS Attendence,     
SM.AbsentReason,  ScheduleStartTime= SM.StartDate,ScheduleEndTime=SM.EndDate, 
EmpFirstName=E.FirstName, EmpLastName=E.LastName, Employee = dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat),   
ClockIn=CASE WHEN EV.ClockInTime IS NULL THEN 0 ELSE 1 END ,                              
ClockOut=CASE WHEN EV.ClockOutTime IS NULL THEN 0 ELSE 1 END,    
EV.EmployeeVisitID, EV.IsSelf, EV.Name, EV.Relation,    
F.FacilityName, F.Address, F.City, F.State, F.ZipCode, F.Phone    
  FROM ScheduleMasters SM          
  INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID     
  LEFT JOIN Employees E ON E.EmployeeID=SM.EmployeeID          
  LEFT JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID AND EV.IsDeleted = 0    
  LEFT JOIN Facilities F ON F.FacilityID= SM.FacilityID                          
WHERE ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0 AND R.IsDeleted=0 AND SM.FacilityID > 0   
AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND CONVERT(DATE,SM.StartDate) >= @StartDate) OR                     
(@EndDate IS NOT NULL AND @StartDate IS NULL AND CONVERT(DATE,SM.EndDate) <= @EndDate) OR                    
(@StartDate IS NOT NULL AND @EndDate IS NOT NULL AND (CONVERT(DATE,SM.StartDate) >= @StartDate AND CONVERT(DATE,SM.EndDate) <= @EndDate)) OR                    
(@StartDate IS NULL AND @EndDate IS NULL)                    
)                  
                  
AND                           
   ((@PatientName IS NULL OR LEN(R.LastName)=0)                             
   OR (                            
       (R.FirstName LIKE '%'+@PatientName+'%' )OR                              
    (R.LastName  LIKE '%'+@PatientName+'%') OR                              
    (R.FirstName +' '+R.LastName like '%'+@PatientName+'%') OR                              
    (R.LastName +' '+R.FirstName like '%'+@PatientName+'%') OR                              
    (R.FirstName +', '+R.LastName like '%'+@PatientName+'%') OR                              
    (R.LastName +', '+R.FirstName like '%'+@PatientName+'%')))                         
  )                                 
  AS T)  AS T1 )                                  
                                  
SELECT * FROM CTEGetPatientClockInOutList                                                                    
 outer apply(select count(ap.ReferralID) as TotalPatientSchedule from CTEGetPatientClockInOutList ap  ) as TotalPatientSchedule     
  outer apply(select count(ap.ReferralID) as TotalPresent from CTEGetPatientClockInOutList ap where ap.IsPatientAttendedSchedule=1) as TotalPresent     
  outer apply(select count(ap.ReferralID) as TotalAbsent from CTEGetPatientClockInOutList ap where ap.IsPatientAttendedSchedule=0) as TotalAbsent     
            
WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   order by    ScheduleStartTime asc                                         
END  