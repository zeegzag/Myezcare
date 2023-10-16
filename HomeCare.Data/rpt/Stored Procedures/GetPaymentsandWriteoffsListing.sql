

-- =============================================
-- Author:		Kalpesh Patel
-- Create date: 06/24/2020
-- Description:	Claims Report for list all the claims with summary
-- EXEC rpt.GetPaymentsandWriteoffsListing
-- =============================================
create PROCEDURE [rpt].[GetPaymentsandWriteoffsListing] 
	-- Add the parameters for the stored procedure here
	@PayerID VARCHAR(4000)='0',
	@ReferralID VARCHAR(4000)='0',
	@BilledStartDate date=null,
	@BilledEndDate date=null,
	--@BatchID int =null,
	--@ClaimID int=null,
	--@ClaimStatus varchar(4000)=null,
	@isSent Varchar(10) = '0'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		R.ReferralID,
		dbo.GetGeneralNameFormat(r.FirstName,r.LastName) as PatientName,
		N.PayorName,
		DATEDIFF(Day,B.StartDate, B.EndDate) AS BillingPeriod,
		'' AS OriginalInvoice,
		BN.CLM_BilledAmount AS CurrentBalance,
		BN.CheckNumber,
		BN.CheckDate,
		0 AS Days,
		N.UnitType AS Type,
		BN.CheckAmount AS Amount,
		0 AS Allowance,
		0 AS WriteOff,
		0 AS InvoiceAdjustment
		--N.ServiceCode, N.ServiceName, N.ServiceDate, N.UnitType, N.StartTime, N.EndTime, N.CalculatedUnit, N.MarkAsComplete, N.PayorName, R.FirstName, R.LastName, BNS.BatchNoteStatusName, BN.BatchID, 
		--BN.NoteID, B.BatchID AS B_BatchID, B.BatchTypeID, B.PayorID, B.StartDate, B.EndDate, B.IsSent, B.IsSentBy, B.SentDate, BN.CheckDate, BN.CheckAmount, BN.CheckNumber, BN.Deductible, BN.ReceivedDate, 
		--BN.MYEZCARE_ClaimStatus, BN.CLM_BilledAmount, N.CalculatedAmount, N.CalculatedServiceTime, BCN.CalculatedUnit AS BCN_CalculatedUnit, BCN.CalculatedAmount AS BCN_CalculatedAmount
	FROM  Notes AS N 
	INNER JOIN Referrals AS R ON N.ReferralID = R.ReferralID 
	INNER JOIN BatchNotes AS BN ON N.NoteID = BN.NoteID 
	INNER JOIN Batches AS B ON BN.BatchID = B.BatchID 
	INNER JOIN BatchNoteStatus AS BNS ON BN.BatchNoteStatusID = BNS.BatchNoteStatusID 
	INNER JOIN BatchChildNotes AS BCN ON BN.NoteID = BCN.NoteID	
	Where 
	R.IsDeleted=0
	AND (@PayerID = '0' OR  TRY_CAst(N.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@PayerID, ',')))
	AND (@ReferralID = '0' OR  TRY_CAst(R.ReferralID AS varchar(100)) in (select Item from dbo.SplitString(@ReferralID, ',')))
	--and (@ClaimStatus is null or BN.MYEZCARE_ClaimStatus=@ClaimStatus ) 
	AND (@isSent = '0' or IsSent=@isSent) 
	AND ((@BilledStartDate is null or @BilledEndDate is null) or  b.SentDate BETWEEN @BilledStartDate AND @BilledEndDate)

END