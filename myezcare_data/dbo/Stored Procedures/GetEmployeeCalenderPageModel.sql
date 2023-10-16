CREATE PROCEDURE [dbo].[GetEmployeeCalenderPageModel]  
AS  
BEGIN  
  
SELECT EmployeeID=EmployeeID, FirstName, LastName FROM Employees WHERE IsDeleted=0 ORDER BY LastName ASC  
  
END
