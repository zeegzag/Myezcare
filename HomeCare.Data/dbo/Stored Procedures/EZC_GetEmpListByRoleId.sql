--CreatedBy              CreatedDate      UpdatedDate    description    
--vikas srivastava       13-6-2019            ""          Get Employee List by Role id    
    
--exec EZC_GetEmpListByRoleId '1,19'  
    
CREATE PROCEDURE [dbo].[EZC_GetEmpListByRoleId]      
  @RoleID nvarchar(200)    
AS      
BEGIN      
select EmployeeID,FirstName,LastName,RoleID from Employees where RoleID IN (SELECT * FROM SplitString(@RoleID, ',')) and IsDeleted=0  
END