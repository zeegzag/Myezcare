-- EXEC CHK_GetChecklistItems 1, 24254
CREATE PROCEDURE [dbo].[CHK_GetChecklistItems]
	@ChecklistItemTypeID INT,
	@ChecklistItemTypePrimaryID BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	IF OBJECT_ID('tempdb..#CurrentChecklistItems') IS NOT NULL 
		DROP TABLE #CurrentChecklistItems
		
	IF OBJECT_ID('tempdb..#ChecklistItemStatus') IS NOT NULL 
		DROP TABLE #ChecklistItemStatus

	IF OBJECT_ID('tempdb..#ChecklistItemDocuments') IS NOT NULL 
		DROP TABLE #ChecklistItemDocuments		

	IF OBJECT_ID('tempdb..#SavedChecklistItems') IS NOT NULL 
		DROP TABLE #SavedChecklistItems	

	CREATE TABLE #ChecklistItemStatus (ChecklistItemID BIGINT, IsDocumentUploaded BIT, DocumentName VARCHAR(MAX))
	CREATE TABLE #ChecklistItemDocuments (ChecklistItemID BIGINT, ComplianceID BIGINT)

	SELECT
		ChecklistItemID,
		IsDocumentRequired
	INTO
		#CurrentChecklistItems
	FROM
		ChecklistItems
	WHERE
		ChecklistItemTypeID = @ChecklistItemTypeID
		-- AND ChecklistItemID IN (50,52)
		
		
	WHILE EXISTS (SELECT * FROM #CurrentChecklistItems)
	BEGIN
		DECLARE @CurrentChecklistItemID BIGINT
		DECLARE @CurrentIsDocumentRequired BIT = 0
		DECLARE @CurrentChecklistItemIsDocumentUploaded BIT = 0
		DECLARE @DocumentName VARCHAR(MAX) = ''

		DELETE FROM #ChecklistItemDocuments
	
		SELECT TOP 1
			@CurrentChecklistItemID = ChecklistItemID,
			@CurrentIsDocumentRequired = IsDocumentRequired
		FROM
			#CurrentChecklistItems
		ORDER BY
			ChecklistItemID ASC
		-- PRINT('ItemID ' + CAST(@CurrentChecklistItemID AS VARCHAR))
		IF(@CurrentIsDocumentRequired = 1)
		BEGIN

			INSERT INTO #ChecklistItemDocuments
			SELECT
				ChecklistItemID,
				ComplianceID
			FROM
				ChecklistItemDocuments
			WHERE
				ChecklistItemID = @CurrentChecklistItemID
			-- SELECT * FROM #ChecklistItemDocuments
			WHILE EXISTS (SELECT * FROM #ChecklistItemDocuments)
			BEGIN

				DECLARE @CurrentDocumentID BIGINT
				DECLARE @CurrentDocumentName VARCHAR(500)
				
				SELECT TOP 1
					@CurrentDocumentID = ComplianceID
				FROM
					#ChecklistItemDocuments
				WHERE
					ChecklistItemID = @CurrentChecklistItemID
				ORDER BY
					ChecklistItemID ASC
				-- PRINT('DocumentID ' + CAST(@CurrentDocumentID AS VARCHAR))
				IF EXISTS(SELECT 1 FROM EbriggsFormMppings WHERE SubSectionID = @CurrentDocumentID AND ReferralID = @ChecklistItemTypePrimaryID)
				BEGIN
					-- PRINT('EBriggs')
					SET @CurrentChecklistItemIsDocumentUploaded = 1
					SELECT
						@CurrentDocumentName = C.DocumentName
					FROM
						EbriggsFormMppings EFM
						INNER JOIN Compliances C ON EFM.SubSectionID = C.ComplianceID
						-- INNER JOIN Admin_Stage_Myezcare.dbo.EBForms EF ON EFM.OriginalEBFormID = EF.EBFormID
					WHERE
						SubSectionID = @CurrentDocumentID AND ReferralID = @ChecklistItemTypePrimaryID					
				END
				ELSE IF EXISTS(SELECT 1 FROM ReferralDocuments WHERE ComplianceID = @CurrentDocumentID AND UserID = @ChecklistItemTypePrimaryID)
				BEGIN
					SET @CurrentChecklistItemIsDocumentUploaded = 1
					SELECT
						@CurrentDocumentName = C.DocumentName
					FROM
						ReferralDocuments RD
						INNER JOIN Compliances C ON RD.ComplianceID = C.ComplianceID
					WHERE 
						RD.ComplianceID = @CurrentDocumentID AND UserID = @ChecklistItemTypePrimaryID
				END
				ELSE
				BEGIN
					SET @CurrentChecklistItemIsDocumentUploaded = 0
					BREAK;
				END
				-- PRINT(@CurrentChecklistItemIsDocumentUploaded)
				-- PRINT('DocumentName ' + @DocumentName + ' CurrDocumentName ' + @CurrentDocumentName)
				SET @DocumentName = @DocumentName + ', ' + @CurrentDocumentName
				DELETE FROM #ChecklistItemDocuments WHERE ComplianceID = @CurrentDocumentID
			END -- While End
		END -- IF Document Required

		INSERT INTO #ChecklistItemStatus
		SELECT @CurrentChecklistItemID, @CurrentChecklistItemIsDocumentUploaded, @DocumentName
			
		DELETE FROM #CurrentChecklistItems WHERE ChecklistItemID = @CurrentChecklistItemID
	END

	SELECT
		ChecklistItemID,
		IsChecked
	INTO
		#SavedChecklistItems
	FROM
		dbo.SavedChecklistItems sci
	WHERE
		sci.ChecklistItemTypePrimaryID = @ChecklistItemTypePrimaryID
		AND sci.ChecklistItemTypeID = @ChecklistItemTypeID
	
	SELECT 
		CLS.ChecklistItemID,
		CLS.IsDocumentUploaded,
		CASE WHEN SCLI.ChecklistItemID IS NOT NULL THEN SCLI.IsChecked ELSE CLS.IsDocumentUploaded END AS IsChecked,
		LTRIM(STUFF(CLS.DocumentName,1,1,'')) DocumentName,
		CLI.ChecklistItemTypeID,
		CLI.StepName,
		CLI.StepDescription,
		CLI.ChecklistTypeControl,
		CLI.IsDocumentRequired,
		CLI.IsMandatory,
		CLI.IsAutomatic,
		@ChecklistItemTypePrimaryID AS ChecklistItemTypePrimaryID
	FROM 
		#ChecklistItemStatus CLS
		INNER JOIN ChecklistItems CLI ON CLS.ChecklistItemID = CLI.ChecklistItemID
		LEFT JOIN #SavedChecklistItems SCLI ON CLS.ChecklistItemID = SCLI.ChecklistItemID
	ORDER BY
		CLI.SortOrder
		
	EXEC CHK_GetIsChecklistRemaining @ChecklistItemTypeID, @ChecklistItemTypePrimaryID
END
