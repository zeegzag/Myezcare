CREATE PROCEDURE [dbo].[DeleteRecord]
@CustomWhere varchar(max),
@TableName varchar(max)
AS
BEGIN
declare @sql varchar(max)='';

SET @sql=N'DELETE FROM '+@TableName+' WHERE '+@CustomWhere;
print(@sql);
exec(@sql);
SELECT 1
END
