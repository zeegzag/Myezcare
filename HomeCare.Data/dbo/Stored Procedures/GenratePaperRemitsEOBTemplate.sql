--  EXEC GenratePaperRemitsEOBTemplate @BatchID = '3'
CREATE Procedure [dbo].[GenratePaperRemitsEOBTemplate]
@BatchID as bigint
AS
BEGIN

	IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=1)
	BEGIN
			SELECT BN.NoteID,BN.BatchNoteID,BN.BatchID, R.ReferralID, R.LastName,R.FirstName,R.Dob, R.AHCCCSID,R.CISNumber, P.ShortName AS Payor,
			N.BillingProviderName, N.BillingProviderNPI, N.RenderingProviderName, N.RenderingProviderNPI, CONVERT(VARCHAR(10), N.ServiceDate, 101) AS ServiceDate ,N.PosID,N.ServiceCode,M.ModifierCode,
			BilledUnit=BN.CLM_UNIT,BilledAmount=CONVERT(DECIMAL(10,2),BN.CLM_BilledAmount)
			From Batches B 
			INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID 
			INNER JOIN BatchNoteDetails N ON N.BatchNoteID=BN.BatchNoteID
			INNER JOIN Referrals R on R.ReferralID=N.ReferralID
			INNER JOIN Payors P ON P.PayorID = B.PayorID
			LEFT JOIN Modifiers M on M.ModifierID=N.ModifierID
			WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) AND BN.IsUseInBilling=1 
	END

END