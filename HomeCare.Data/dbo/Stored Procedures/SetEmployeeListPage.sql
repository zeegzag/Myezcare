CREATE PROCEDURE [dbo].[SetEmployeeListPage]       
AS      
BEGIN          
 -- return list of department ID and Name for department dropdownlist      
      
 Select RoleID, RoleName from Roles order by RoleName;      
 SELECT * from EmployeeCredentials    
 DECLARE @TempCount BIGINT;      
 SET @TempCount=(SELECT DISTINCT COUNT(d.DepartmentID) FROM Departments d LEFT JOIN  Employees e ON e.DepartmentID=d.DepartmentID);-- WHERE e.IsDeleted=0);      
       
 IF @TempCount=0      
 BEGIN       
  SELECT 0;      
 END      
 ELSE      
 BEGIN      
  SELECT DISTINCT d.DepartmentID,d.DepartmentName FROM Departments d LEFT JOIN  Employees e ON e.DepartmentID=d.DepartmentID --WHERE e.IsDeleted=0;      
 END      
 -- If we will not "return 0", error has been occurred in "GetMultipleEntity"      
 SELECT 0;      
      
 SELECT 0;      
 SELECT 0;        

 select dm.DDMasterID as DesignationID  ,dm.Title AS DesignationName 
from ddmaster dm  
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID = dm.ItemType  
where lu.Name='Designation'   and dm.IsDeleted=0
END