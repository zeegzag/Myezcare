
--CreatedBy: Satya
--CreatedDate:06 Feb 2020
--Description: For get permission data by Id
-- exec GetPermissionsData 2001
CREATE Procedure [dbo].[GetPermissionsData]
@PermissionID bigint=0
AS
BEGIN
SELECT * FROM Permissions where IsDeleted=0 and PermissionID=@PermissionID

END