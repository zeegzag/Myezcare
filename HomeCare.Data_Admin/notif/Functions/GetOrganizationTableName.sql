-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 03 Jul 2020
-- Description:	This SVF is used to get organization table name.
-- =============================================
CREATE FUNCTION [notif].[GetOrganizationTableName]
(
	@OrganizationID BIGINT,
	@TableName NVARCHAR(200)
)
RETURNS NVARCHAR(600)
AS
BEGIN
	DECLARE @DBName NVARCHAR(400);
	DECLARE @OrgTableName NVARCHAR(600);
	DECLARE @ObjID INT;
	SELECT @DBName = [DBName] FROM [dbo].[Organizations] WHERE [OrganizationID] = @OrganizationID;
	IF (@DBName IS NOT NULL)
		BEGIN
			SELECT @OrgTableName = '[' + @DBName + '].' + @TableName;
			SELECT @ObjID = OBJECT_ID(@OrgTableName)
			IF (@ObjID IS NULL)
				BEGIN
					RETURN 'Error: Object ' + @OrgTableName + ' not found.';
				END
		END
	ELSE
		BEGIN
			RETURN 'Error: Invalid Organization DBName.';
		END

	RETURN @OrgTableName;
END