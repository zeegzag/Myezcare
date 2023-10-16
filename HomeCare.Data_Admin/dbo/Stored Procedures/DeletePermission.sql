--CreatedBy Satya
-- CreatedDate: 06/02/2020
--Description: Form Delete permissions
-- Exec DeletePermission 1
CREATE PROCEDURE [dbo].[DeletePermission]
@PermissionID BIGINT
AS                          
 BEGIN                          
  UPDATE Permissions SET IsDeleted=1 WHERE PermissionID=@PermissionID
  SELECT 1; RETURN;
END