USE [Kundan_Admin]
GO
/****** Object:  StoredProcedure [dbo].[SaveOrganizationPermission]    Script Date: 2/19/2020 10:40:29 PM ******/
--Created by Satya
--Purpose : Save organisation Permission

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveOrganizationPermission]                 
@EmployeeID BIGINT,                
@OrganizationID BIGINT,                              
@ListOfPermissionIdInCsvSelected nVARCHAR(max),   
@ListOfPermissionIdInCsvNotSelected nVARCHAR(max),           
@SystemID VARCHAR(50)                
AS                
BEGIN                    
 DECLARE @CurrentDate DATETIME = (SELECT GETDATE());                
 UPDATE OrganizationPermission SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0             
  WHERE OrganizationID=@OrganizationID and PermissionID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfPermissionIdInCsvSelected))            
               
  UPDATE OrganizationPermission set UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1               
  where OrganizationID=@OrganizationID and PermissionID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfPermissionIdInCsvNotSelected))            
          
 Insert into OrganizationPermission                 
  (OrganizationID,PermissionID,                
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy, IsDeleted)                
 (select                 
  @OrganizationID OrganizationID,T.Val PermissionID,                
  GETUTCDATE() CreatedDate,@EmployeeID CreatedBy,GETUTCDATE() UpdatedDate,@EmployeeID UpdatedBy,0 IsDeleted                 
  from                 
  (SELECT CAST(Val AS BIGINT) as Val FROM GetCSVTable(@ListOfPermissionIdInCsvSelected)) as T                  
  where T.Val not in  (select PermissionID from OrganizationPermission where OrganizationID=@OrganizationID)                
 )                
          
Insert into OrganizationPermission                 
  (OrganizationID,PermissionID,                
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsDeleted)                
 (select                 
  @OrganizationID OrganizationID,T.Val PermissionID,                
  GETUTCDATE() CreatedDate,@EmployeeID CreatedBy,GETUTCDATE() UpdatedDate,@EmployeeID UpdatedBy,1 IsDeleted                 
  from                 
  (SELECT CAST(Val AS BIGINT) as Val FROM GetCSVTable(@ListOfPermissionIdInCsvNotSelected)) as T                  
  where T.Val not in  (select PermissionID from OrganizationPermission where OrganizationID=@OrganizationID)                
 )         
 --SELECT 1 AS IsSuccess;               
       
 SELECT rpm.OrgPermissionId AS RolePermissionMappingID, r.OrganizationID AS OrganizationID, r.CompanyName, p.PermissionID AS PermissionID, p.PermissionName            
 FROM Permissions p            
  INNER JOIN OrganizationPermission rpm ON p.PermissionID=rpm.PermissionID             
  INNER JOIN Organizations r ON r.OrganizationID=rpm.OrganizationID             
  WHERE r.OrganizationID = @OrganizationID AND rpm.IsDeleted=0 AND p.IsDeleted=0          
  ORDER BY OrderID ASC       
                    
END
