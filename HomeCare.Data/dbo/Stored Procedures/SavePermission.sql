/*
 Changed by Sagar 17 Dec :  commented the condition
*/
CREATE PROCEDURE [dbo].[SavePermission]                       
@EmployeeID BIGINT,                      
@RoleID BIGINT,                                    
@ListOfPermissionIdInCsvSelected nVARCHAR(max),         
@ListOfPermissionIdInCsvNotSelected nVARCHAR(max),                 
@SystemID VARCHAR(50)                      
AS                      
BEGIN                          
 DECLARE @CurrentDate DATETIME = (SELECT GETDATE());                      
 UPDATE rolepermissionmapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                      
  WHERE RoleID=@RoleID and PermissionID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfPermissionIdInCsvSelected))                  
                  
 --IF(@ListOfPermissionIdInCsvSelected like '%3008%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                      
 -- WHERE RoleID=@RoleID and PermissionID IN (3015)                  
                  
 --IF(@ListOfPermissionIdInCsvSelected like '%3012%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                      
 -- WHERE RoleID=@RoleID and PermissionID IN (3018)                  
                  
 --IF(@ListOfPermissionIdInCsvSelected like '%3015%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                      
 -- WHERE RoleID=@RoleID and PermissionID IN (3008)                  
                  
 --IF(@ListOfPermissionIdInCsvSelected like '%3018%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                      
 -- WHERE RoleID=@RoleID and PermissionID IN (3012)                  
                  
 --IF(@ListOfPermissionIdInCsvSelected like '%3016%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                  
 -- WHERE RoleID=@RoleID and PermissionID IN (3015,3018,3017)                  
                  
 --IF(@ListOfPermissionIdInCsvSelected like '%3017%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                  
 -- WHERE RoleID=@RoleID and PermissionID IN (3008,3012,3016)                 
                      
                     
  UPDATE rolepermissionmapping set UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                       
  where RoleID=@RoleID and PermissionID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfPermissionIdInCsvNotSelected))                  
                  
  --IF(@ListOfPermissionIdInCsv like '%3008%')                  
  --UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                      
  --WHERE RoleID=@RoleID and PermissionID IN (3015)                  
                  
 --IF(@ListOfPermissionIdInCsv like '%3012%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                      
 -- WHERE RoleID=@RoleID and PermissionID IN (3018)                  
                  
 --IF(@ListOfPermissionIdInCsv like '%3015%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                      
 -- WHERE RoleID=@RoleID and PermissionID IN (3008)                  
                  
 --IF(@ListOfPermissionIdInCsv like '%3018%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                      
 -- WHERE RoleID=@RoleID and PermissionID IN (3012)                  
                  
 --IF(@ListOfPermissionIdInCsv like '%3016%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                  
 -- WHERE RoleID=@RoleID and PermissionID IN (3015,3018,3017)                  
                  
 --IF(@ListOfPermissionIdInCsv like '%3017%')                  
 -- UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                  
 -- WHERE RoleID=@RoleID and PermissionID IN (3008,3012,3016)                  
                  
                       
                
                
 --UPDATE RolePermissionMapping SET               
 --IsDeleted = CASE WHEN Exists(SELECT 1 FROM RolePermissionMapping WHERE RoleID=@RoleID AND PermissionID IN (3008,3012) AND IsDeleted=1) THEN 1 ELSE 0 END              
 --WHERE RoleID=@RoleID AND PermissionID = 3016              
              
 --UPDATE RolePermissionMapping SET               
 --IsDeleted = CASE WHEN Exists(SELECT 1 FROM RolePermissionMapping WHERE RoleID=@RoleID AND PermissionID IN (3015,3018) AND IsDeleted=1) THEN 1 ELSE 0 END              
 --WHERE RoleID=@RoleID AND PermissionID = 3017              
                
                
 Insert into RolePermissionMapping                       
  (RoleID,PermissionID,                      
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,                      
  SystemID,IsDeleted)                      
 (select                       
  @RoleID RoleID,T.Val PermissionID,                      
  GETUTCDATE() CreatedDate,@EmployeeID CreatedBy,GETUTCDATE() UpdatedDate,@EmployeeID UpdatedBy,                    
  @SystemID SystemID,0 IsDeleted                       
  from                       
  (SELECT CAST(Val AS BIGINT) as Val FROM GetCSVTable(@ListOfPermissionIdInCsvSelected)) as T                        
  where T.Val not in  (select PermissionID from RolePermissionMapping where RoleID=@RoleID)                      
 )                      
                
Insert into RolePermissionMapping                       
  (RoleID,PermissionID,                      
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,                      
  SystemID,IsDeleted)                      
 (select                       
  @RoleID RoleID,T.Val PermissionID,                      
  GETUTCDATE() CreatedDate,@EmployeeID CreatedBy,GETUTCDATE() UpdatedDate,@EmployeeID UpdatedBy,                    
  @SystemID SystemID,1 IsDeleted                       
  from                       
  (SELECT CAST(Val AS BIGINT) as Val FROM GetCSVTable(@ListOfPermissionIdInCsvNotSelected)) as T                        
  where T.Val not in  (select PermissionID from RolePermissionMapping where RoleID=@RoleID)                      
 )               
 --SELECT 1 AS IsSuccess;                     
             
 SELECT rpm.RolePermissionMappingID AS RolePermissionMappingID, r.RoleID AS RoleID, r.RoleName, p.PermissionID AS PermissionID, p.PermissionName                  
 FROM Permissions p                  
  INNER JOIN RolePermissionMapping rpm ON p.PermissionID=rpm.PermissionID                   
  INNER JOIN Roles r ON r.RoleID=rpm.RoleID                   
  WHERE r.RoleID = @RoleID AND rpm.IsDeleted=0 AND p.IsDeleted=0 AND  p.CompanyHasAccess=1 --and p.PermissionID!=@PermissionID                  
  ORDER BY OrderID ASC             
                          
END