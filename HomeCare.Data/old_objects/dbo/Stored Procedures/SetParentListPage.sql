-- EXEC [SetParentListPage]
CREATE PROCEDURE [dbo].[SetParentListPage]	
AS
BEGIN
	
	SELECT * FROM ContactTypes ORDER BY ContactTypeName ASC

END

