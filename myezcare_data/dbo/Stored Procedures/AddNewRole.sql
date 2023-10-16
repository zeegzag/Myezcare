
CREATE PROCEDURE [dbo].[AddNewRole]    
 @EmployeeID BIGINT,    
 @RoleName VARCHAR(100),    
 @SystemID VARCHAR(100),    
 @PermissionID bigint    
     
AS    
BEGIN        
 Declare @RoleID BIGINT=0;    
 Declare @IsDeleted BIT=1; -- Set default entries for new role    
 DECLARE @CurrentDate DATETIME = (SELECT GETDATE());    
     
 DECLARE @CheckDuplicateRole VARCHAR(100);    
    
 IF @RoleName IS NOT NULL OR LEN(@RoleName)>0    
 BEGIN     
  SET @CheckDuplicateRole=(select COUNT(RoleID) FROM Roles WHERE rolename=@RoleName);    
  IF @CheckDuplicateRole=0    
  BEGIN    
   INSERT INTO Roles(RoleName,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)      
    VALUES(@RoleName,@CurrentDate,@EmployeeID,@CurrentDate,@EmployeeID,@SystemID)    
   SET @RoleID=SCOPE_IDENTITY();    
    
   INSERT INTO RolePermissionMapping    
       (RoleID,PermissionID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)    
    select     
    @RoleID,PermissionID,@CurrentDate,@EmployeeID,@CurrentDate,@EmployeeID,@SystemID,@IsDeleted from Permissions WHERE PermissionID!=@PermissionID AND IsDeleted=0 AND UsedInHomeCare=1   
   SELECT @RoleID AS RoleID,@RoleName AS RoleName;    
   END    
   ELSE    
   BEGIN    
    SELECT 0;    
   END    
 END    
 ELSE    
 BEGIN    
  SELECT 0;    
 END    
     
END    

