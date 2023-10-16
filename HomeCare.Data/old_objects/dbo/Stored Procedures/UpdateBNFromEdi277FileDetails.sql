CREATE PROCEDURE [dbo].[UpdateBNFromEdi277FileDetails]
@Edi277FileID BIGINT
AS
BEGIN

		  UPDATE BN
		  SET
		  BN.S277 = (CASE WHEN (LEN(PayorClaimNumber)=0 OR PayorClaimNumber IS NULL) THEN EFD.Action ELSE NULL END),  
		  BN.S277CA = (CASE WHEN (LEN(PayorClaimNumber)>0 AND PayorClaimNumber IS NOT NULL) THEN EFD.Action ELSE NULL END)
		  FROM BatchNotes AS BN
		  INNER JOIN Edi277FileDetails AS EFD
		  ON (BN.BatchNoteID= EFD.BatchNoteID) OR (BN.ParentBatchNoteID= EFD.BatchNoteID)
		  WHERE EFD.Edi277FileID=@Edi277FileID;
	
	
END
