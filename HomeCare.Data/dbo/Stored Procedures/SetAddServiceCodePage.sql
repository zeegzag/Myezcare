CREATE PROCEDURE [dbo].[SetAddServiceCodePage]
@ServiceCodeID BIGINT=0
AS
BEGIN

SELECT * FROM ServiceCodes WHERE ServiceCodeID=@ServiceCodeID

SELECT Name=ModifierName,Value=ModifierID FROM Modifiers

SELECT Name=ServiceCodeTypeName,Value=ServiceCodeTypeID FROM ServiceCodeTypes  WHERE ServiceCodeTypeID!=3
 


END