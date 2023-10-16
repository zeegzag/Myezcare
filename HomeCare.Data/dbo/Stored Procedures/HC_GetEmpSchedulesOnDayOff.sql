-- EXEC HC_GetEmpSchedulesOnDayOff @EmployeeDayOffID = '12', @EmployeeID = '4'  
CREATE PROCEDURE [dbo].[HC_GetEmpSchedulesOnDayOff]  
@EmployeeDayOffID BIGINT,  
@EmployeeID BIGINT  
AS  
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  
DECLARE @StartDate DATETIME;  
DECLARE @EndDate DATETIME;  
  
SELECT @StartDate=StartTime , @EndDate=EndTime FROM EmployeeDayOffs WHERE EmployeeDayOffID=@EmployeeDayOffID  
  
PRINT @StartDate  
PRINT @EndDate  
  
SELECT SM.StartDate, SM.EndDate, SM.ScheduleID, R.ReferralID, R.FirstName, R.LastName, dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS ReferralName,
E.EmployeeID, EmpFirstName=E.FirstName, EmpLastName=E.LastName,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName
FROM ScheduleMasters SM  
INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID AND SM.EmployeeID=@EmployeeID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0  
INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID  
WHERE 1=1 AND   
  
(SM.StartDate BETWEEN @StartDate AND @EndDate  OR SM.EndDate BETWEEN @StartDate AND @EndDate)   
OR  
(@StartDate  BETWEEN SM.StartDate AND SM.EndDate  OR @EndDate BETWEEN SM.StartDate AND SM.EndDate)  
  
END  
  
-- EXEC HC_GetEmpSchedulesOnDayOff @EmployeeDayOffID = '12', @EmployeeID = '4'  