CREATE PROCEDURE [dbo].[CHK_GetIsChecklistRemaining]
	@ChecklistItemTypeID INT,
	@ChecklistItemTypePrimaryID BIGINT
AS      
BEGIN
	IF EXISTS
	(
		SELECT
			1
		FROM
			SavedChecklistItems
		WHERE
			ChecklistItemTypeID = @ChecklistItemTypeID
			AND ChecklistItemTypePrimaryID = @ChecklistItemTypePrimaryID
	)
	BEGIN
		SELECT 0 AS TransactionResultId
	END
	ELSE
	BEGIN
		SELECT 1 AS TransactionResultId
	END	
END