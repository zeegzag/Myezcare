CREATE PROCEDURE [dbo].[HC_GetEmployeesForEmpCalender]    
@EmployeeIDs VARCHAR(MAX)    
AS    
BEGIN    
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
SELECT DISTINCT EmployeeID=E.EmployeeID, FirstName, LastName,  dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS EmployeeName,   
SUM(DATEDIFF(mi, SM.StartDate,SM.EndDate) / 60.0) OVER ( PARTITION BY E.EmployeeID ORDER BY E.EmployeeID ASC) AS EmployeeUsedHours    
FROM Employees E     
LEFT JOIN ScheduleMasters SM ON SM.EmployeeID = E.EmployeeID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0    
WHERE (@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0 OR E.EmployeeID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs)))            
ORDER BY LastName ASC    
    
END  