CREATE PROCEDURE [dbo].[SetClaimAdjustmentFlag01]
@ClaimAdjustmentType as varchar(50)=null,
@ClaimAdjustmentReason as varchar(MAX)=null,
@BatchID as bigint,
@NoteID as bigint
AS
BEGIN

DECLARE @IsValidForUpdate BIT= 1;

IF(LEN(@ClaimAdjustmentType)=0)
 BEGIN
  SET @ClaimAdjustmentType=NULL;
  SET @ClaimAdjustmentReason=NULL;
 END
ELSE IF(@ClaimAdjustmentType='Void' OR @ClaimAdjustmentType='Replacement')
  BEGIN
   
   IF NOT EXISTS(SELECT BatchNoteID FROM BatchNotes WHERE BatchID=@BatchID AND NoteID=@NoteID AND CLP07_PayerClaimControlNumber IS NOT NULL)
    SET @IsValidForUpdate = 0;

  END


--PRINT @BatchID
--PRINT @NoteID
--PRINT @IsValidForUpdate

 IF(@IsValidForUpdate=1)
 BEGIN


		DECLARE @PayorClaimNumber VARCHAR(MAX);
		SELECT TOP 1 @PayorClaimNumber=CLP01_ClaimSubmitterIdentifier   FROM BatchNotes BN
		 WHERE BN.BatchID =@BatchID AND BN.NoteID = @NoteID    
			
		Declare @TempBatchID BIGINT;
		Declare @TempNoteID BIGINT;

		DECLARE db_cursor CURSOR FOR  SELECT DISTINCT BatchID, NoteID FROM BatchNotes BN WHERE BN.CLP01_ClaimSubmitterIdentifier=@PayorClaimNumber
		OPEN db_cursor   
		FETCH NEXT FROM db_cursor INTO @TempBatchID,@TempNoteID   

		WHILE @@FETCH_STATUS = 0   
		BEGIN   
			
				 UPDATE BatchNotes SET ClaimAdjustmentTypeID=@ClaimAdjustmentType, ClaimAdjustmentReason=@ClaimAdjustmentReason WHERE BatchNoteID IN (SELECT BatchNoteID FROM ( 
					SELECT DISTINCT BN.BatchNoteID,
					-- Order BY BN.BatchID DESC, MarkAsLatest DESC, BN.BatchNoteID Desc
					BN.BatchID, BN.NoteID, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID,BN.BatchID ORDER BY MarkAsLatest DESC,BN.BatchNoteID DESC) AS RowNumber
					FROM BatchNotes BN       
					WHERE BN.BatchID = CAST(@TempBatchID AS BIGINT) AND BN.NoteID = CAST(@TempNoteID AS BIGINT)       
				) AS t WHERE RowNumber=1 )

			 FETCH NEXT FROM db_cursor INTO @TempBatchID,@TempNoteID   
		END   

		CLOSE db_cursor   
		DEALLOCATE db_cursor


	
 END

END