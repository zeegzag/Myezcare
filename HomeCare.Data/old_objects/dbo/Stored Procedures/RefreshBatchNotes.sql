-- EXEC RefreshBatchNotes
CREATE PROCEDURE [dbo].[RefreshBatchNotes]
AS
BEGIN

		UPDATE BN
		SET CLM_UNIT =  N.CalculatedUnit, Original_Unit =N.CalculatedUnit,
		CLM_BilledAmount = N.CalculatedAmount, Original_Amount =  N.CalculatedAmount
		, CLP03_TotalClaimChargeAmount =  N.CalculatedAmount
		, IsUseInBilling = CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END
		FROM BatchNotes BN
		INNER JOIN Batches B ON BN.BatchID = B.BatchID AND B.IsSent = 0
		INNER JOIN NOTES N ON N.NoteID = BN.NoteID
		WHERE ISFirstTimeClaimInBatch =1 AND IsNewProcess = 1


		UPDATE BN
		SET CLM_UNIT =  A.TempCalculatedUnit,
		CLM_BilledAmount = A.TempCalculatedAmount,
		CLP03_TotalClaimChargeAmount =  A.TempCalculatedAmount
		FROM BatchNotes BN 
		INNER JOIN Batches B ON BN.BatchID = B.BatchID AND B.IsSent = 0
		INNER JOIN (
			SELECT  BN.BatchNoteID, 
			TempCalculatedUnit= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit) THEN PSM.BillingUnitLimit ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END , 
			TempCalculatedAmount= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit) 
			THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit ) 
			ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END
			FROM BatchNotes BN
			INNER JOIN Batches B ON BN.BatchID = B.BatchID AND B.IsSent = 0
			INNER JOIN NOTES N ON N.NoteID = BN.NoteID AND (ISFirstTimeClaimInBatch IS NULL OR ISFirstTimeClaimInBatch =1) 
			INNER JOIN ChildNotes CN ON CN.ParentNoteID = N.NoteID
			INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID
			GROUP BY BN.BatchID,BN.BatchNoteID,N.NoteID, CN.ParentNoteID, PSM.BillingUnitLimit
		) AS A ON A.BatchNoteID = BN.BatchNoteID AND (ISFirstTimeClaimInBatch IS NULL OR ISFirstTimeClaimInBatch =1)  AND IsNewProcess = 1 AND IsUseInBilling = 1

END
