--  exec [EmployeeList]  
  
CREATE PROCEDURE [dbo].[EmployeeList]          
         
AS                            
BEGIN          
         
	DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()            
  SELECT E.EmployeeID, dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat) AS EmployeeName FROM Employees E   where IsDeleted=0    order by  EmployeeName   
         
END 