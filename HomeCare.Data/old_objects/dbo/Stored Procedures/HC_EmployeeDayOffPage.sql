CREATE PROCEDURE [dbo].[HC_EmployeeDayOffPage]  
AS  
BEGIN  
  
SELECT EmployeeID, Name=dbo.GetGeneralNameFormat(FirstName,LastName), IsDeleted FROM Employees ORDER BY LastName
  
END
