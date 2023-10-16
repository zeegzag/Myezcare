--EXEC HC_ViewBatchDetails @BatchID = '33'
-- exec [HC_ViewBatchDetails] 135          
CREATE PROCEDURE [dbo].[HC_ViewBatchDetails]                    
@BatchID bigint                                                         
AS                                                  
BEGIN                       
         DECLARE @MedicaidType VARCHAR(50);
SET @MedicaidType = 'Medicaid'   
      
select count(N.NoteID) as TotalClaims,N.ReferralID,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName --,R.FirstName,R.LastName                
,R.AHCCCSID,RPM.BeneficiaryNumber CISNumber,R.Dob,BN.BatchID ,P.PayorID ,P.PayorBillingType ,B.StartDate,B.EndDate      
from BatchNotes BN          
INNER JOIN Notes N on N.NoteID = BN.NoteID and BN.BatchID=@BatchID          
INNER JOIN Batches B on B.BatchID=BN.BatchID
INNER JOIN Referrals R on R.referralid =N.referralid    
LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID
INNER JOIN dbo.Payors P ON p.PayorID = N.PayorID and p.PayorID = rpm.PayorID
LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType

GROUP BY N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,RPM.BeneficiaryNumber,R.Dob,BN.BatchID,P.PayorID  ,P.PayorBillingType  ,B.StartDate,B.EndDate           
ORDER BY PatientName
END