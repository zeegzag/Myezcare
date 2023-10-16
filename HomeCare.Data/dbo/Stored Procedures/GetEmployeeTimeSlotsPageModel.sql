
CREATE PROCEDURE [dbo].[GetEmployeeTimeSlotsPageModel]  
AS  
BEGIN  
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
SELECT Value=EmployeeID, Name= dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat)  FROM Employees  WHERE IsDeleted=0  ORDER BY LastName ASC  
END