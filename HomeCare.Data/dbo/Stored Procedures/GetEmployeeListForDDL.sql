CREATE PROCEDURE [dbo].[GetEmployeeListForDDL]  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
SELECT E.EmployeeID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName ,IsDeleted FROM Employees E  WHERE E.IsActive = 1 order by E.LastName ASC  
END  