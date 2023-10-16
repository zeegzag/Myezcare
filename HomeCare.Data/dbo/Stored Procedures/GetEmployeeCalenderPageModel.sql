CREATE PROCEDURE [dbo].[GetEmployeeCalenderPageModel]      
AS      
BEGIN      
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()  
SELECT EmployeeID=EmployeeID, FirstName, LastName,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS EmployeeName FROM Employees WHERE IsDeleted=0 ORDER BY LastName ASC      
      
END 