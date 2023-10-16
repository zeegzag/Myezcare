CREATE PROCEDURE [dbo].[CHK_SaveChecklistItems]
@UDTChecklistItems UDT_ChecklistItem READONLY,
@LoggedInUserId BIGINT,  
@SystemID VARCHAR(20)  
AS      
BEGIN

	MERGE SavedChecklistItems SCLI
	USING @UDTChecklistItems CLI
	ON
		SCLI.ChecklistItemID = CLI.ChecklistItemID
		AND SCLI.ChecklistItemTypePrimaryID = CLI.ChecklistItemTypePrimaryID
	WHEN NOT MATCHED BY TARGET THEN
	INSERT  
	(
		ChecklistItemTypeID,
		ChecklistItemID,
		ChecklistItemTypePrimaryID,
		IsChecked,
		CreatedBy,
		CreatedDate,
		UpdatedDate,
		UpdatedBy,
		SystemID,
		IsDeleted
	)
	VALUES
	(
		ChecklistItemTypeID,
		ChecklistItemID,
		ChecklistItemTypePrimaryID,
		IsChecked,
		@LoggedInUserId,
		GETDATE(),
		GETDATE(),
		@LoggedInUserId,
		@SystemID,
		0
	)
	WHEN MATCHED THEN
	UPDATE SET
		SCLI.IsChecked = CLI.IsChecked,
		SCLI.UpdatedDate = GETDATE(),
		SCLI.UpdatedBy = @LoggedInUserId;
	--WHEN NOT MATCHED BY SOURCE  
	--	AND SCLI.ChecklistItemTypePrimaryID = CLI.ChecklistItemTypePrimaryID THEN  
	--DELETE;  

	/*
	INSERT INTO SavedChecklistItems
	(
		ChecklistItemTypeID,
		ChecklistItemID,
		ChecklistItemTypePrimaryID,
		IsChecked,
		CreatedBy,
		CreatedDate,
		UpdatedDate,
		UpdatedBy,
		SystemID,
		IsDeleted
	)
	SELECT
		ChecklistItemTypeID,
		ChecklistItemID,
		ChecklistItemTypePrimaryID,
		IsChecked,
		@LoggedInUserId,
		GETDATE(),
		GETDATE(),
		@LoggedInUserId,
		@SystemID,
		0
	FROM
		@UDTChecklistItems
	*/
		
	SELECT   
		1 AS TransactionResultId,  
		1 AS TablePrimaryId
END
