
    
CREATE PROCEDURE [dbo].[GetUserPermission]      
 @RoleID bigint    
AS    
BEGIN      
    
SELECT PermissionID=CASE WHEN PermissionCode IS NULL THEN CONVERT(VARCHAR(100),P.PermissionID) ELSE PermissionCode END  from RolePermissionMapping RPM    
INNER JOIN Permissions P on P.PermissionID=RPM.PermissionID    
WHERE RPM.IsDeleted=0 AND RPM.RoleID=@RoleID AND CompanyHasAccess=1 --AND PermissionCode IS NOT NULL    
    
END