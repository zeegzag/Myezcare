﻿-- EXEC HC_GetChildReconcileList '234', '6972'                
CREATE Procedure [dbo].[HC_GetChildReconcileList]              
@BatchID as bigint,                
@NoteID as bigint,                
@Upload835FileID bigint=0                
AS                
BEGIN                
                
  
SELECT *,  
Status = CASE   
 WHEN Status_Temp = 'Paid' AND BilledAmount > PaidAmount THEN 'Partial Paid'  
 ELSE  Status_Temp END  
  
FROM (  
SELECT BN.*,B.BatchTypeID,B.StartDate,B.EndDate,B.IsSentBy,B.IsSent,B.SentDate,B.Comment,B.EraIDs,  
CSC.ClaimStatusCodeID,CSC.ClaimStatusName,CSC.ClaimStatusCodeDescription,  
CAGC.ClaimAdjustmentGroupCodeID,CAGC.ClaimAdjustmentGroupCodeName,CAGC.ClaimAdjustmentGroupCodeDescription,  
CARC.ClaimAdjustmentReasonCodeID,CARC.ClaimAdjustmentReasonDescription,  
  
  
Status_Temp=CASE WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void')) 
			AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName IS NOT NULL 
			AND BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Denied' 
			
			WHEN Submitted_ClaimAdjustmentTypeID = 'Void' 
			AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 
			AND CSC.ClaimStatusName='Reversal of Previous Payment' 
			AND BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Paid'               
  
			WHEN Submitted_ClaimAdjustmentTypeID = 'Void' 
			AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 
			AND CSC.ClaimStatusName!='Reversal of Previous Payment' 
			AND BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Denied'  
			
			WHEN CSC.ClaimStatusName IS NULL  THEN NULL 
			WHEN  BN.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Paid' 
			ELSE  NULL END,                
  
  
ClaimType=Submitted_ClaimAdjustmentTypeID,                
ClaimNumber=BN.CLP01_ClaimSubmitterIdentifier,PayorClaimNumber=BN.CLP07_PayerClaimControlNumber,AdjustmentAmount=BN.CAS03_ClaimAdjustmentAmount,                
-- BilledAmount=CASE WHEN BN.CLP03_TotalClaimChargeAmount IS NULL OR LEN(BN.CLP03_TotalClaimChargeAmount)=0 THEN '0' ELSE BN.CLP03_TotalClaimChargeAmount END,                
BilledAmount=CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END,                
AllowedAmount=CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0 THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END,                 
PaidAmount=CASE WHEN BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount IS NULL OR LEN(BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount)=0 THEN '0' ELSE BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount END,                
Payor=BN.N102_PayorName,ExtractDate=B.CreatedDate--, BN.BatchID    
  
  
from BatchNotes BN                
INNER JOIN Batches B ON B.BatchID=BN.BatchID --AND B.IsSent=1                
LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode                
LEFT JOIN ClaimAdjustmentGroupCodes CAGC  ON CAGC.ClaimAdjustmentGroupCodeID=BN.CAS01_ClaimAdjustmentGroupCode                
LEFT JOIN ClaimAdjustmentReasonCodes CARC  ON CARC.ClaimAdjustmentReasonCodeID=BN.CAS02_ClaimAdjustmentReasonCode                
Where  NoteID=@NoteID AND (( CAST(@Upload835FileID AS BIGINT)=0) OR BN.Upload835FileID= CAST(@Upload835FileID AS BIGINT))       
AND BN.BatchID= @BatchID --AND ParentBatchNoteID=@ParentBatchNoteID                
  
) AS T  
Order BY BatchID DESC,MarkAsLatest DESC,BatchNoteID Desc ;                
                
                
                
SELECT DISTINCT B.BatchID,BatchStartDate=B.StartDate, BatchEndDate=B.EndDate,B.SentDate,SentBy=SB.UserName,GatherDate= B.CreatedDate,                 
GatheredBy=CB.UserName, BatchPayorName=BP.PayorName, BatchPayorShortName=BP.ShortName , BT.BatchTypeName                
FROM Batches B                
INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID                
INNER JOIN Payors BP on BP.PayorID=B.PayorID                 
INNER JOIN Employees CB on CB.EmployeeID=B.CreatedBy          
LEFT JOIN Employees SB on SB.EmployeeID=B.IsSentBy                
INNER JOIN BatchNotes BN ON B.BatchID=BN.BatchID                  
WHERE BN.NoteID=@NoteID  -- AND B.IsSent=1      
ORDER BY B.BatchID DESC                
                
                
                
SELECT CreatedBy=CN.UserName,N.CreatedDate,UpdatedBy=UN.UserName,N.UpdatedDate,SignedBy=SN.UserName,SignedDate=N.SignatureDate FROM BatchNoteDetails N                
INNER JOIN Employees CN on CN.EmployeeID=N.CreatedBy                
INNER JOIN Employees UN on UN.EmployeeID=N.UpdatedBy                
LEFT JOIN SignatureLogs SL ON SL.NoteID=N.NoteID and SL.IsActive=1 and N.MarkAsComplete=1                
LEFT JOIN Employees SN on SN.EmployeeID=SL.SignatureBy                
WHERE N.NoteID=@NoteID                
                
                
                
END 