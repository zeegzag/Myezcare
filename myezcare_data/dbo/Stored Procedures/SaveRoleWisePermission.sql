--EXEC SaveRoleWisePermission @EmployeeID = '1', @RoleID = '36', @IsSetToTrue = 'True', @ListOfPermissionIDInCsv = '2024,2090,2091,2093,2094,2095,2096,2097,2100,2101,2102,2104,2105,2106,2108,2109,2110,2113,2114,2115,2117,2118,2169,2170,2171,2173,2174,2175,2119,2121,2122,2123,2125,2126,2127,2129,2130,2131,2133,2134,2135,2137,2138,2043,2026,2027,2028', @SystemID = '::1'          
CREATE PROCEDURE [dbo].[SaveRoleWisePermission]             
@EmployeeID BIGINT,            
@RoleID BIGINT,            
@IsSetToTrue BIT,            
@ListOfPermissionIdInCsv nVARCHAR(max),            
@SystemID VARCHAR(50)            
AS            
BEGIN                
 DECLARE @CurrentDate DATETIME = (SELECT GETDATE());            
             
 IF(@IsSetToTrue=1)            
 BEGIN            
  UPDATE rolepermissionmapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID            
  WHERE RoleID=@RoleID and PermissionID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfPermissionIdInCsv))        
        
 IF(@ListOfPermissionIdInCsv like '%3008%')        
  UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID            
  WHERE RoleID=@RoleID and PermissionID IN (3015)        
        
 IF(@ListOfPermissionIdInCsv like '%3012%')        
  UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID            
  WHERE RoleID=@RoleID and PermissionID IN (3018)        
        
 IF(@ListOfPermissionIdInCsv like '%3015%')        
  UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID            
  WHERE RoleID=@RoleID and PermissionID IN (3008)        
        
 IF(@ListOfPermissionIdInCsv like '%3018%')        
  UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID            
  WHERE RoleID=@RoleID and PermissionID IN (3012)        
        
 IF(@ListOfPermissionIdInCsv like '%3016%')        
  UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID        
  WHERE RoleID=@RoleID and PermissionID IN (3015,3018,3017)        
        
 IF(@ListOfPermissionIdInCsv like '%3017%')        
  UPDATE RolePermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID        
  WHERE RoleID=@RoleID and PermissionID IN (3008,3012,3016)        
        
 END             
 ELSE            
 BEGIN            
  UPDATE rolepermissionmapping set UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID             
  where RoleID=@RoleID and PermissionID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfPermissionIdInCsv))        
        
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
        
 END            
      
      
 UPDATE RolePermissionMapping SET     
 IsDeleted = CASE WHEN Exists(SELECT 1 FROM RolePermissionMapping WHERE RoleID=@RoleID AND PermissionID IN (3008,3012) AND IsDeleted=1) THEN 1 ELSE 0 END    
 WHERE RoleID=@RoleID AND PermissionID = 3016    
    
 UPDATE RolePermissionMapping SET     
 IsDeleted = CASE WHEN Exists(SELECT 1 FROM RolePermissionMapping WHERE RoleID=@RoleID AND PermissionID IN (3015,3018) AND IsDeleted=1) THEN 1 ELSE 0 END    
 WHERE RoleID=@RoleID AND PermissionID = 3017    
      
      
 Insert into RolePermissionMapping             
  (RoleID,PermissionID,            
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,            
  SystemID,IsDeleted)            
 (select             
  @RoleID RoleID,T.Val PermissionID,            
  GETUTCDATE() CreatedDate,@EmployeeID CreatedBy,GETUTCDATE() UpdatedDate,@EmployeeID UpdatedBy,          
  @SystemID SystemID,(CASE when @IsSetToTrue=1 then 0 else 1 END) IsDeleted             
  from             
  (SELECT CAST(Val AS BIGINT) as Val FROM GetCSVTable(@ListOfPermissionIdInCsv)) as T              
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
