CREATE FUNCTION [dbo].[GetCommaSepCategories]
(
    @ID bigint
)
RETURNS varchar(max) -- or whatever length you need
AS
BEGIN
   DECLARE @Title VARCHAR(max) 
SELECT @Title = COALESCE(@Title + ',', '') + d.Title from EmployeeGroup eg,DDMaster d where d.DDMasterID=eg.val and eg.EmployeeID= @ID
return @Title


END