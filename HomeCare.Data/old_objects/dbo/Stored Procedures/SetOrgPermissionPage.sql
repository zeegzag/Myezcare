-- EXEC SetOrgPermissionPage @OrganizationID = '18', @PermissionID = '1001'      
CREATE PROCEDURE [dbo].[SetOrgPermissionPage]         
@OrganizationID BIGINT,        
@PermissionID bigint        
AS        
BEGIN            
         
 IF @OrganizationID = 0         
 BEGIN        
  SELECT * FROM Organizations ORDER BY CompanyName ASC;        
  --SELECT * FROM Permissions;          
  SELECT PermissionID AS id, CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent,
  PermissionName AS TEXT, Description  FROM Permissions        
  WHERE IsDeleted=0 
  AND LOWER(PermissionPlatform)='web' 
  ORDER BY OrderID ASC        
      
      
  SELECT PermissionID AS id, CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent,
  PermissionName AS TEXT, Description  FROM Permissions        
  WHERE IsDeleted=0 
  AND LOWER(PermissionPlatform)='mobile'      
  ORDER BY OrderID ASC       
  --WHERE  PermissionID!=@PermissionID        
        
  SET @OrganizationID=(SELECT TOP 1 OrganizationID FROM Organizations);        
 END         
 ELSE        
 BEGIN        
  SELECT NULL;        
  SELECT NULL;        
  SELECT NULL;        
 END;        
      
         
 SELECT rpm.OrgPermissionId AS RolePermissionMappingID, org.OrganizationID AS RoleID, org.CompanyName, p.PermissionID AS PermissionID, p.PermissionName        
 FROM Permissions p        
  INNER JOIN OrganizationPermission rpm ON p.PermissionID=rpm.PermissionID         
  INNER JOIN Organizations org ON org.OrganizationID=rpm.OrganizationId         
  WHERE org.OrganizationID = @OrganizationID AND p.IsDeleted=0 AND  rpm.IsDeleted=0 and p.PermissionID!=@PermissionID        
  ORDER BY OrderID ASC        
         
  -- for search model        
 SELECT OrganizationID as OrganizationID, CompanyName FROM Organizations where OrganizationID=@OrganizationID;            
            
END
