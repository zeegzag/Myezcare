-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 02-05-2020
-- Description:	To update batch parent  reconcile
-- =============================================
CREATE PROCEDURE [dbo].[HC_UpdateBatchReconcile]
(
	@BatchNoteID bigint,
	@PaidAmount varchar(max),
	@ClaimStatusID bigint,
	@ClaimStatusCodeID bigint
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE BatchNotes
	SET 
		CLP04_TotalClaimPaymentAmount = @PaidAmount,
		MYEZCARE_ClaimStatus = CASE WHEN @ClaimStatusID IS NOT NULL AND @ClaimStatusID != 0 THEN @ClaimStatusID ELSE MYEZCARE_ClaimStatus END,
		CLP02_ClaimStatusCode = CASE WHEN @ClaimStatusCodeID IS NOT NULL AND @ClaimStatusCodeID !=0 THEN @ClaimStatusCodeID ELSE CLP02_ClaimStatusCode END
	WHERE BatchNoteID = @BatchNoteID;
END
