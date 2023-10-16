CREATE PROCEDURE [dbo].[SetClaimAdjustmentFlag]
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

IF(@ClaimAdjustmentType='Void' OR @ClaimAdjustmentType='Replacement')
  BEGIN
   
   IF NOT EXISTS(SELECT BatchNoteID FROM BatchNotes WHERE BatchID=@BatchID AND NoteID=@NoteID AND CLP07_PayerClaimControlNumber IS NOT NULL)
    SET @IsValidForUpdate = 0;

  END


--PRINT @BatchID
--PRINT @NoteID
--PRINT @IsValidForUpdate

 IF(@IsValidForUpdate=1)
 BEGIN
	UPDATE BatchNotes SET ClaimAdjustmentTypeID=@ClaimAdjustmentType, ClaimAdjustmentReason=@ClaimAdjustmentReason WHERE BatchNoteID IN (SELECT BatchNoteID FROM ( 
				SELECT DISTINCT BN.BatchNoteID,
				BN.BatchID, BN.NoteID, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID,BN.BatchID ORDER BY BN.BatchNoteID DESC) AS RowNumber
				FROM BatchNotes BN       
				WHERE BN.BatchID = CAST(@BatchID AS BIGINT) AND BN.NoteID = CAST(@NoteID AS BIGINT)       
			) AS t WHERE RowNumber=1 )
 END

END