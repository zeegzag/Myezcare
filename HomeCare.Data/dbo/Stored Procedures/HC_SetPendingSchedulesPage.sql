CREATE PROCEDURE [dbo].[HC_SetPendingSchedulesPage]  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
SELECT EmployeeID, Name=dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat)  FROM Employees  
END  