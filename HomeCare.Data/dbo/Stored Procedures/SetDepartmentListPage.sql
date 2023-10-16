  
CREATE PROCEDURE [dbo].[SetDepartmentListPage]   
AS  
BEGIN     
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 -- THIS WILL RETURN THE DEPARTMENTS  
 SELECT D.DepartmentID,D.DepartmentName  
 FROM Departments D order by DepartmentName ASC --WHERE D.IsDeleted = 0   
  
   
 -- THIS WILL RETURN THE MANAGER  
 SELECT E.EmployeeID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) as Manager   
 FROM Employees E  
 INNER JOIN Departments D ON D.DepartmentID = E.DepartmentID  
 WHERE E.IsDepartmentSupervisor = 1 order by E.LastName ASC-- AND E.IsDeleted = 0 AND D.IsDeleted = 0  
  
 SELECT 0;  
  
 SELECT 0;  
END  
  