--CreatedBy: Akhilesh Kamal
--CreatedDate:30 jan 2020
--Description: For get permissions list
-- exec GetPermissionsList ''
CREATE Procedure [dbo].[SetPermissionsList]
@PermissionName NVARCHAR = NULL,
@IsDeleted bigint=0,
@FromIndex bigint=0,
@pageSize bigint=0,
@SortExpression NVARCHAR=NULL,
@SortType  NVARCHAR=NULL
AS
BEGIN
SELECT PermissionName FROM Permissions where IsDeleted=0

END