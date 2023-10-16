CREATE PROCEDURE [dbo].[GetAdminDatabseTableData]
@TableName VARCHAR(200)
AS
BEGIN
DECLARE @AdminDatabaseName VARCHAR(200)
SELECT @AdminDatabaseName=AdminDatabaseName FROM OrganizationSettings
DECLARE @Query VARCHAR(MAX)
SET @Query= 'SELECT * FROM '+@AdminDatabaseName+'.dbo.'+@TableName
EXEC (@Query)
END