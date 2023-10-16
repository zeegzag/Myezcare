-- EXEC GetReconcileBatchNoteDetails @ParentBatchNoteID = '30699', @BatchID = '53', @NoteID = '65881', @Upload835FileID = '0'  
CREATE Procedure [dbo].[GetReconcileBatchNoteDetails]   
@ParentBatchNoteID as bigint,  
@BatchID as bigint,  
@NoteID as bigint,  
@Upload835FileID bigint=0  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  
Select *,  
Status=CASE WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void')) AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName IS NOT NULL THEN 'Denied'    
      WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName='Reversal of Previous Payment' THEN 'Paid'    
   WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName!='Reversal of Previous Payment' THEN 'Denied'    
   WHEN CSC.ClaimStatusName IS NULL THEN NULL ELSE  'Paid' END,  
  
ClaimNumber=BN.CLP01_ClaimSubmitterIdentifier,PayorClaimNumber=BN.CLP07_PayerClaimControlNumber,AdjustmentAmount=BN.CAS03_ClaimAdjustmentAmount,  
-- BilledAmount=CASE WHEN BN.CLP03_TotalClaimChargeAmount IS NULL OR LEN(BN.CLP03_TotalClaimChargeAmount)=0 THEN '0' ELSE BN.CLP03_TotalClaimChargeAmount END,  
BilledAmount=CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END,  
AllowedAmount=CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0 THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END,   
PaidAmount=CASE WHEN BN.CLP04_TotalClaimPaymentAmount IS NULL OR LEN(BN.CLP04_TotalClaimPaymentAmount)=0 THEN '0' ELSE BN.CLP04_TotalClaimPaymentAmount END,  
Payor=BN.N102_PayorName,ExtractDate=B.CreatedDate      
from BatchNotes BN  
INNER JOIN Batches B ON B.BatchID=BN.BatchID AND B.IsSent=1  
LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode  
LEFT JOIN ClaimAdjustmentGroupCodes CAGC  ON CAGC.ClaimAdjustmentGroupCodeID=BN.CAS01_ClaimAdjustmentGroupCode  
LEFT JOIN ClaimAdjustmentReasonCodes CARC  ON CARC.ClaimAdjustmentReasonCodeID=BN.CAS02_ClaimAdjustmentReasonCode  
Where  BN.BatchID=@BatchID AND NoteID=@NoteID AND (( CAST(@Upload835FileID AS BIGINT)=0) OR BN.Upload835FileID= CAST(@Upload835FileID AS BIGINT))  --AND ParentBatchNoteID=@ParentBatchNoteID  
Order BY MarkAsLatest DESC, BatchNoteID Desc;  
  
  
Select   
R.ReferralID,ClientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),R.FirstName,R.LastName,C.Address,C.City,C.State,C.ZipCode, R.Gender, ClientDob=CONVERT(VARCHAR(10), R.Dob, 101),R.AHCCCSID,R.CISNumber,  
R.Population, R.Title,N.NoteID, N.NoteDetails, N.Assessment, N.ActionPlan, N.ServiceCode,N.PosID,N.POSDetail ,N.ServiceDate, N.BillingProviderName, N.BillingProviderNPI,N.BillingProviderEIN,N.BillingProviderAddress,N.BillingProviderCity,  
N.BillingProviderState, N.BillingProviderZipcode ,N.RenderingProviderName, N.RenderingProviderNPI,N.RenderingProviderEIN,N.RenderingProviderAddress,N.RenderingProviderCity,  
N.RenderingProviderState, N.RenderingProviderZipcode,   
B.BatchID,BatchStartDate=B.StartDate, BatchEndDate=B.EndDate,B.SentDate,SentBy=SB.UserName,GatherDate= B.CreatedDate,  
GatheredBy=CB.UserName, BatchPayorName=BP.PayorName, BatchPayorShortName=BP.ShortName , BT.BatchTypeName,  
  
   (SELECT  STUFF((SELECT TOP 1 ', ' + F.DXCodeWithoutDot  
    FROM NoteDXCodeMappings F where F.ReferralID=N.ReferralID   Order BY F.Precedence  
    FOR XML PATH('')),1,1,'')) AS ContinuedDX  
  
  
from Referrals R  
INNER JOIN ContactMappings CM ON CM.ReferralID=R.ReferralID AND (CM.IsPrimaryPlacementLegalGuardian=1 OR CM.IsDCSLegalGuardian=1)  
INNER JOIN Contacts C ON C.ContactID=CM.ContactID  
INNER JOIN Notes N on N.ReferralID=R.ReferralID  AND N.NoteID=@NoteID  
INNER JOIN BatchNotes BN on BN.NoteID=N.NoteID --AND ParentBatchNoteID Is NULL AND -- BN.BatchNoteID=@ParentBatchNoteID  
INNER JOIN Batches B on B.BatchID=BN.BatchID AND B.BatchID=@BatchID AND B.IsSent=1  
INNER JOIN Employees CB on CB.EmployeeID=B.CreatedBy  
INNER JOIN Employees SB on SB.EmployeeID=B.IsSentBy  
INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID  
INNER JOIN Payors BP on BP.PayorID=B.PayorID   
Where 1=1 AND (( CAST(@Upload835FileID AS BIGINT)=0) OR BN.Upload835FileID= CAST(@Upload835FileID AS BIGINT))             
  
  
END  