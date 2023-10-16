CREATE PROCEDURE [dbo].[HC_ViewBatchDetails]                                                     
 @BatchID bigint                                                     
AS                                                                                   
BEGIN 
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
DECLARE @MedicaidType VARCHAR(50);                                 
SET @MedicaidType = 'Medicaid'                                    
                    
DECLARE @IsSent BIT=0;                
                
SELECT @IsSent = IsSent FROM Batches WHERE BatchID = @BatchID                
                
IF(@IsSent = 0)                           
BEGIN                
SELECT                     
BatchID,ReferralID,PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType ,RowNumber, --StartDate,EndDate,                    
TotalClaims= COUNT(NoteID),                    
TotalAmount= SUM(CalculatedAmount),                    
TotalBilledAmount=SUM (CLM_BilledAmount),                          
TotalAllowedAmount=SUM(AMT01_ServiceLineAllowedAmount_AllowedAmount),                    
TotalPaidAmount=SUM (SVC03_LineItemProviderPaymentAmoun_PaidAmount)   ,      
TotalMPP_AdjustedAmount = SUM(MPP_AdjustmentAmount)      
                    
FROM (                        
                    
SELECT N.ReferralID,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName,R.AHCCCSID,RPM.BeneficiaryNumber CISNumber,R.Dob,BN.BatchID ,P.PayorID ,P.PayorBillingType ,                    
--B.StartDate,B.EndDate,                    
N.NoteID,                    
                    
-- Inital Note Amount                    
CalculatedAmount =  CONVERT(DECIMAL(10,2), N.CalculatedAmount),                    
                    
-- Capture Amount in Batch - Should same as Initial Note Amount                    
CLM_BilledAmount =  CONVERT(DECIMAL(10,2),    CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END ) ,                     
                    
-- Payor Allowed Amount Per Service Line Item                    
AMT01_ServiceLineAllowedAmount_AllowedAmount =  CONVERT(DECIMAL(10,2), CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0                     
THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END ),                    
                    
-- Payor Paid Amount Per Service Line Item                    
SVC03_LineItemProviderPaymentAmoun_PaidAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount IS NULL OR LEN(BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount)=0                     
THEN '0' ELSE BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount END),        
      
      
MPP_AdjustmentAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BN.MPP_AdjustmentAmount IS NULL OR LEN(BN.MPP_AdjustmentAmount)=0                     
THEN '0' ELSE BN.MPP_AdjustmentAmount END),        
                    
--RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID,BN.BatchID Order BY BN.MarkAsLatest DESC, BN.BatchNoteID DESC)                     
--RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY BN.MarkAsLatest DESC, BN.BatchNoteID DESC)                     
RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, BN.MarkAsLatest DESC, BN.BatchNoteID DESC)                
                    
FROM BatchNotes BN                                           
INNER JOIN Notes N on N.NoteID = BN.NoteID --and BN.BatchID=@BatchID                                                     
INNER JOIN Batches B on B.BatchID=BN.BatchID                                 
INNER JOIN Referrals R on R.referralid =N.referralid                                     
--LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID                                 
--INNER JOIN dbo.Payors P ON p.PayorID = N.PayorID and p.PayorID = rpm.PayorID                                 
INNER JOIN dbo.Payors P ON p.PayorID = B.PayorID                                
LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  AND RPM.PayorID=P.PayorID                                 
AND (CONVERT(date,N.ServiceDate) between RPM.PayorEffectiveDate and RPM.PayorEffectiveEndDate)  and rpm.IsDeleted=0                             
INNER JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title =  @MedicaidType                              
WHERE BN.NoteID IN (SELECT DISTINCT NoteID FROM BatchNotes WHERE BatchID=@BatchID)    AND BN.BatchID=@BatchID                 
                    
) AS TEMP  WHERE RowNumber = 1                    
                    
--GROUP BY ReferralID,PatientName,AHCCCSID,CISNumber,Dob,BatchID ,PayorID ,PayorBillingType ,StartDate,EndDate,RowNumber                    
GROUP BY BatchID,ReferralID,PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType ,RowNumber                    
ORDER BY PatientName                        
END                
                
ELSE IF(@IsSent = 1)                           
BEGIN                
                
SELECT                     
BatchID,ReferralID,PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType ,RowNumber, --StartDate,EndDate,                    
TotalClaims= COUNT(NoteID),                    
TotalAmount= SUM(CalculatedAmount),                    
TotalBilledAmount=SUM (CLM_BilledAmount),                          
TotalAllowedAmount=SUM(AMT01_ServiceLineAllowedAmount_AllowedAmount),                    
TotalPaidAmount=SUM (SVC03_LineItemProviderPaymentAmoun_PaidAmount)              ,      
TotalMPP_AdjustedAmount = SUM(MPP_AdjustmentAmount)      
                    
FROM (                        
                    
SELECT N.ReferralID,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName,R.AHCCCSID,RPM.BeneficiaryNumber CISNumber,R.Dob,BN.BatchID ,P.PayorID ,P.PayorBillingType ,                    
--B.StartDate,B.EndDate,                    
N.NoteID,                    
                    
-- Inital Note Amount                    
CalculatedAmount =  CONVERT(DECIMAL(10,2), N.CalculatedAmount),                    
                    
-- Capture Amount in Batch - Should same as Initial Note Amount                    
CLM_BilledAmount =  CONVERT(DECIMAL(10,2),    CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END ) ,                     
                    
-- Payor Allowed Amount Per Service Line Item                    
AMT01_ServiceLineAllowedAmount_AllowedAmount =  CONVERT(DECIMAL(10,2), CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0                     
THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END ),                    
                    
-- Payor Paid Amount Per Service Line Item                    
SVC03_LineItemProviderPaymentAmoun_PaidAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount IS NULL OR LEN(BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount)=0                     
THEN '0' ELSE BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount END),                    
      
MPP_AdjustmentAmount= CONVERT(DECIMAL(10,2),    CASE WHEN BN.MPP_AdjustmentAmount IS NULL OR LEN(BN.MPP_AdjustmentAmount)=0                     
THEN '0' ELSE BN.MPP_AdjustmentAmount END),        
                    
--RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID,BN.BatchID Order BY BN.MarkAsLatest DESC, BN.BatchNoteID DESC)                     
--RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY BN.MarkAsLatest DESC, BN.BatchNoteID DESC)                     
RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, BN.MarkAsLatest DESC, BN.BatchNoteID DESC)                
                    
FROM BatchNotes BN                                           
--INNER JOIN BatchNoteDetails N on N.NoteID = BN.NoteID --and BN.BatchID=@BatchID                                     
INNER JOIN Notes N on N.NoteID = BN.NoteID --and BN.BatchID=@BatchID                                                     
INNER JOIN Batches B on B.BatchID=BN.BatchID             
INNER JOIN Referrals R on R.referralid =N.referralid                                     
--LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID                                 
--INNER JOIN dbo.Payors P ON p.PayorID = N.PayorID and p.PayorID = rpm.PayorID                                 
INNER JOIN dbo.Payors P ON p.PayorID = B.PayorID                                
LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  AND RPM.PayorID=P.PayorID                                 
AND (CONVERT(date,N.ServiceDate) between RPM.PayorEffectiveDate and RPM.PayorEffectiveEndDate)  and rpm.IsDeleted=0                             
--INNER JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title =  @MedicaidType                              
WHERE BN.NoteID IN (SELECT DISTINCT NoteID FROM BatchNotes WHERE BatchID=@BatchID) AND BN.BatchID=@BatchID                               
                 
) AS TEMP  WHERE RowNumber = 1                    
                    
--GROUP BY ReferralID,PatientName,AHCCCSID,CISNumber,Dob,BatchID ,PayorID ,PayorBillingType ,StartDate,EndDate,RowNumber                    
GROUP BY BatchID,ReferralID,PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType ,RowNumber                    
ORDER BY PatientName                        
END                
                    
                                               
END 