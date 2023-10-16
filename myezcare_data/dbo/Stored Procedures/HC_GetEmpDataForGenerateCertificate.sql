-- HC_GetEmpDataForGenerateCertificate 621  
CREATE PROCEDURE [dbo].[HC_GetEmpDataForGenerateCertificate]  
@EmployeeID bigint  
AS  
BEGIN  
 SELECT e.*, 
 EmployeeFullName = dbo.GetGeneralNameFormat(e.FirstName,e.LastName),  
 str_Role=r.RoleName,
 str_Designation=d.Title
 FROM dbo.Employees e 
 LEFT JOIN dbo.DDMaster d ON d.DDMasterID = e.Designation AND d.ItemType=18 
 LEFT JOIN dbo.Roles r ON r.RoleID = e.RoleID  
 WHERE e.EmployeeID = @EmployeeID  
END
