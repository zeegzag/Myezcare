-- EXEC SetRolePermissionPage @ServicePlanID = 4  
CREATE PROCEDURE [dbo].[SetRolePermissionPage]  
@ServicePlanID BIGINT  
AS      
BEGIN          
 -- Web Permissions 
 select 0; 
 SELECT   
  PermissionID AS id,   
  CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent,   
  PermissionName AS TEXT, Description   
 FROM   
  [Permissions]  
 WHERE   
  IsDeleted=0   
  AND LOWER(PermissionPlatform)='web'    
 ORDER BY   
  OrderID ASC   
   
 -- Mobile Permissions   
 SELECT   
  PermissionID AS id,   
  CASE WHEN ParentID=0 THEN '#' ELSE CAST(ParentID AS VARCHAR(100)) END AS parent,   
  PermissionName AS TEXT, Description   
 FROM   
  [Permissions]  
 WHERE   
  IsDeleted=0   
  AND LOWER(PermissionPlatform)='mobile'    
 ORDER BY   
  OrderID ASC   
    
    -- Role Wise Permissions  
 SELECT   
  P.PermissionID,  
  P.PermissionName  
 FROM  
  ServicePlanPermissions SPP  
  INNER JOIN [Permissions] P ON SPP.PermissionID = P.PermissionID  
 WHERE  
  SPP.ServicePlanID = @ServicePlanID  
       
 -- For Search Model  
 SELECT 1;          
END