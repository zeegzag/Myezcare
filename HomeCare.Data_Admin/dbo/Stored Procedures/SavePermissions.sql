--CreatedBy Akhilesh kamal
-- CreatedDate: 30/01/2020
--Description: Form save and update permissions

-- Exec SavePermissions 'Test11','',1,0,'','',0

CREATE PROCEDURE [dbo].[SavePermissions]  
@PermissionID BIGINT=0,  
@PermissionName NVARCHAR(200),  
@Description NVARCHAR(MAX),  
@ParentID BIGINT=0,  
@OrderID BIGINT=0,  
@PermissionCode NVARCHAR(100),  
@PermissionPlatform NVARCHAR(100)
--@IsDeleted BIGINT
AS  
BEGIN  
-- IF EXISTS (SELECT TOP 1 PermissionID FROM Permissions WHERE  PermissionID != @PermissionID) 
--  BEGIN                    
-- SELECT -1 RETURN; 
--END
DECLARE @PID BIGINT
 SET @PID =  (SELECT  MAX(PermissionID) FROM Permissions) +1
 PRINT @PID
END
IF(@PermissionID=0)                         
 BEGIN                          
  INSERT INTO Permissions                          
  (PermissionID,PermissionName,Description,ParentID,OrderID,IsDeleted,PermissionCode,PermissionPlatform)  
  VALUES                          
  (@PID,@PermissionName,@Description,@ParentID,@OrderID,0,@PermissionCode,@PermissionPlatform);        

  SELECT 1; RETURN;
END
ELSE
BEGIN                          
  UPDATE Permissions  
  SET PermissionName=@PermissionName, Description=@Description,PermissionCode=@PermissionCode,
  PermissionPlatform=@PermissionPlatform, OrderID=@OrderID
  Where PermissionID=@PermissionID                  
  
  SELECT 1; RETURN;
END