 CREATE PROCEDURE [dbo].[SetServiceCodeListPage]
AS
BEGIN
 SELECT Name=ModifierName,Value=ModifierID FROM Modifiers
 SELECT Name=ServiceCodeTypeName,Value=ServiceCodeTypeID FROM ServiceCodeTypes

END 
