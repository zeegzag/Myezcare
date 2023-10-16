CREATE PROCEDURE [dbo].[HC_GetEmployeesForEmpCalender]  
@EmployeeIDs VARCHAR(MAX)  
AS  
BEGIN  
  
SELECT DISTINCT EmployeeID=E.EmployeeID, FirstName, LastName,   
SUM(DATEDIFF(mi, SM.StartDate,SM.EndDate) / 60.0) OVER ( PARTITION BY E.EmployeeID ORDER BY E.EmployeeID ASC) AS EmployeeUsedHours  
FROM Employees E   
LEFT JOIN ScheduleMasters SM ON SM.EmployeeID = E.EmployeeID AND SM.IsDeleted=0  
WHERE (@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0 OR E.EmployeeID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs)))          
ORDER BY LastName ASC  
  
END  
