CREATE PROCEDURE [dbo].[HC_EmployeeDayOffPage]    
AS    
BEGIN    
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()  
SELECT EmployeeID, Name=dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat), IsDeleted FROM Employees ORDER BY LastName  
    
END  