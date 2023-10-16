-- =============================================
-- Author:		Pallav Saxena
-- Create date: 05/24/2020
-- Description:	Claims Report for list all the claims with summary
-- =============================================
CREATE PROCEDURE [rpt].[ClaimsReport] 
	-- Add the parameters for the stored procedure here
	@OrgID int = null, 
	@PayorID int = null,
	@ReferralID int=null,
	@BilledStartDate date=null,
	@BilledEndDate date=null,
	@BatchID int =null,
	@ClaimID int=null,
	@ClaimStatus varchar(4000)=null,
	@isSent bit =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT        N.ServiceCode, N.ServiceName, N.ServiceDate, N.UnitType, N.StartTime, N.EndTime, N.CalculatedUnit, N.MarkAsComplete, N.PayorName, R.FirstName, R.LastName, BNS.BatchNoteStatusName, BN.BatchID, 
                         BN.NoteID, B.BatchID AS B_BatchID, B.BatchTypeID, B.PayorID, B.StartDate, B.EndDate, B.IsSent, B.IsSentBy, B.SentDate, BN.CheckDate, BN.CheckAmount, BN.CheckNumber, BN.Deductible, BN.ReceivedDate, 
                         BN.MYEZCARE_ClaimStatus, BN.CLM_BilledAmount, N.CalculatedAmount, N.CalculatedServiceTime, BCN.CalculatedUnit AS BCN_CalculatedUnit, BCN.CalculatedAmount AS BCN_CalculatedAmount
FROM            Notes AS N INNER JOIN
                         Referrals AS R ON N.ReferralID = R.ReferralID INNER JOIN
                         BatchNotes AS BN ON N.NoteID = BN.NoteID INNER JOIN
                         Batches AS B ON BN.BatchID = B.BatchID INNER JOIN
                         BatchNoteStatus AS BNS ON BN.BatchNoteStatusID = BNS.BatchNoteStatusID INNER JOIN
                         BatchChildNotes AS BCN ON BN.NoteID = BCN.NoteID	
Where (@PayorID is null or B.PayorID=@PayorID) and (@ClaimStatus is null or BN.MYEZCARE_ClaimStatus=@ClaimStatus ) and (@isSent is null or IsSent=@isSent) and (b.SentDate between @BilledStartDate and @BilledEndDate)
	
END