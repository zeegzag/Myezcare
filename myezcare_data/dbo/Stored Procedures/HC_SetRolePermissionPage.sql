﻿-- EXEC HC_SetRolePermissionPage @RoleID = '18', @PermissionID = '1001'      
-- EXEC SetRolePermissionPage 1,0        
CREATE PROCEDURE [dbo].[HC_SetRolePermissionPage]         
@RoleID BIGINT,        
@PermissionID bigint        
AS        
BEGIN            
         
 IF @RoleID = 0         
 BEGIN        
  SELECT * FROM Roles ORDER BY RoleName ASC;        
  --SELECT * FROM Permissions;          
  SELECT PermissionID AS id, CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent, PermissionName AS TEXT, Description  FROM Permissions        
  WHERE IsDeleted=0 AND UsedInHomeCare=1 AND CompanyHasAccess=1 AND LOWER(PermissionPlatform)='web'      
  ORDER BY OrderID ASC        
      
      
   SELECT PermissionID AS id, CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent, PermissionName AS TEXT, Description  FROM Permissions        
  WHERE IsDeleted=0 AND UsedInHomeCare=1 AND CompanyHasAccess=1 AND LOWER(PermissionPlatform)='mobile'      
  ORDER BY OrderID ASC        
      
  --WHERE  PermissionID!=@PermissionID        
        
  SET @RoleID=(SELECT TOP 1 RoleID FROM Roles);        
 END         
 ELSE        
 BEGIN        
  SELECT NULL;        
  SELECT NULL;        
  SELECT NULL;        
 END;        
      
         
 SELECT rpm.RolePermissionMappingID AS RolePermissionMappingID, r.RoleID AS RoleID, r.RoleName, p.PermissionID AS PermissionID, p.PermissionName        
 FROM Permissions p        
  INNER JOIN RolePermissionMapping rpm ON p.PermissionID=rpm.PermissionID         
  INNER JOIN Roles r ON r.RoleID=rpm.RoleID         
  WHERE r.RoleID = @RoleID AND rpm.IsDeleted=0 AND p.IsDeleted=0 AND  p.CompanyHasAccess=1 --and p.PermissionID!=@PermissionID        
  ORDER BY OrderID ASC        
         
  -- for search model        
 SELECT RoleID as RoleID, RoleName FROM Roles WHERE RoleID=@RoleID;            
            
END
