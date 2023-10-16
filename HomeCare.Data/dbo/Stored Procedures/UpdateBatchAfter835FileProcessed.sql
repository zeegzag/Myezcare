
CREATE Procedure [dbo].[UpdateBatchAfter835FileProcessed]
AS
BEGIN
declare @Table1 table (BatchNoteID bigint, BatchID bigint, NoteID bigint);
declare @Table2 table (BatchNoteID bigint, BatchID bigint, NoteID bigint);

INSERT INTO  @Table1 Select Max(BatchNoteID),BatchID,NoteID From BatchNotes Group BY BatchID,NoteID
INSERT INTO  @Table2 Select Min(BatchNoteID),BatchID,NoteID From BatchNotes Group BY BatchID,NoteID

Update B SET 
B.LoadDate=A.LoadDate,
B.ReceivedDate=A.ReceivedDate,
B.ProcessedDate=A.ProcessedDate,
B.CLP02_ClaimStatusCode=A.CLP02_ClaimStatusCode,
B.CAS01_ClaimAdjustmentGroupCode=A.CAS01_ClaimAdjustmentGroupCode,
B.CAS02_ClaimAdjustmentReasonCode=A.CAS02_ClaimAdjustmentReasonCode,
B.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount=A.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount,
B.AMT01_ServiceLineAllowedAmount_AllowedAmount=A.AMT01_ServiceLineAllowedAmount_AllowedAmount,
B.SVC03_LineItemProviderPaymentAmoun_PaidAmount=A.SVC03_LineItemProviderPaymentAmoun_PaidAmount
FROM (select * from BatchNotes WHERE BatchNoteID IN (Select BatchNoteID FROM @Table1)) AS A
INNER JOIN BatchNotes B ON B.BatchID=A.BatchID AND B.NoteID=A.NoteID
WHERE B.BatchNoteID IN (Select BatchNoteID FROM @Table2)
END