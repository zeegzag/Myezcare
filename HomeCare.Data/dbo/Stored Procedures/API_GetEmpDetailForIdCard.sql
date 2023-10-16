CREATE PROCEDURE [dbo].[API_GetEmpDetailForIdCard]                        
 @EmployeeID BIGINT                        
AS                        
BEGIN       
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 DECLARE @OrganizationName NVARCHAR(100);    
 DECLARE @SiteLogo NVARCHAR(100);    
 SET @OrganizationName = (SELECT TOP 1 SiteName FROM OrganizationSettings)     
  SET @SiteLogo = (SELECT TOP 1 SiteLogo FROM OrganizationSettings)     
          
 SELECT e.EmployeeID,Name=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),Role=r.RoleName,e.EmployeeUniqueID,@OrganizationName AS OrganizationName,          
 es.SignaturePath AS EmployeeSignatureURL,e.ProfileImagePath AS EmployeeProfileImgURL,NPIID=e.HHA_NPI_ID,JoiningDate=CONVERT(DATE,e.CreatedDate),      
 e.IsActive,@SiteLogo AS SiteLogo      
 FROM Employees e            
 INNER JOIN Roles r ON r.RoleID=e.RoleID            
 LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID                        
 WHERE e.EmployeeID=@EmployeeID            
END 