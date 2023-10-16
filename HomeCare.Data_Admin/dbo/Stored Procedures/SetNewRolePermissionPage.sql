-- EXEC SetRolePermissionPage @ServicePlanID = 4  
CREATE PROCEDURE [dbo].[SetNewRolePermissionPage]  
@OrganizationID BIGINT  
AS      
BEGIN          
 -- Web Permissions  
 SELECT   
  PermissionID AS id,   
  CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent,   
  PermissionName AS TEXT, Description ,
  (CASE when ((select Count(*) from OrganizationPermission where OrganizationId=@OrganizationID) >=1) then 1 else 0 end) as IsSelected
 FROM   
  [Permissions]  
  
 WHERE   
  IsDeleted=0   and ParentId=0
  AND LOWER(PermissionPlatform)='web'    
 ORDER BY   
  OrderID ASC   
   
 -- Mobile Permissions   
 SELECT   
  PermissionID AS id,   
  CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent,   
  PermissionName AS TEXT, Description ,
  
  (CASE when ((select Count(*) from OrganizationPermission where OrganizationId=@OrganizationID) >=1) then 1 else 0 end) as IsSelected  
 FROM   
  [Permissions]  
 WHERE   
  IsDeleted=0    and ParentId=0
  AND LOWER(PermissionPlatform)='mobile'    
 ORDER BY   
  OrderID ASC   
    
 
 -- For Search Model  
 SELECT 1;          
END