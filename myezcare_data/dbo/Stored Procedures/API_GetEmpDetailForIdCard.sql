CREATE PROCEDURE [dbo].[API_GetEmpDetailForIdCard]                  
 @EmployeeID BIGINT                  
AS                  
BEGIN    
 DECLARE @OrganizationName NVARCHAR(100);    
 SET @OrganizationName = (SELECT TOP 1 SiteName FROM OrganizationSettings)    
    
 SELECT e.EmployeeID,Name=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),Role=r.RoleName,e.EmployeeUniqueID,@OrganizationName AS OrganizationName,    
 es.SignaturePath AS EmployeeSignatureURL,e.ProfileImagePath AS EmployeeProfileImgURL,NPIID=e.HHA_NPI_ID,JoiningDate=CONVERT(DATE,e.CreatedDate),
 e.IsActive
 FROM Employees e      
 INNER JOIN Roles r ON r.RoleID=e.RoleID      
 LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID                  
 WHERE e.EmployeeID=@EmployeeID      
END
