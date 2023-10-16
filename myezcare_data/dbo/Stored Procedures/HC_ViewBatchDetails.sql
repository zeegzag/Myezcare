-- exec [HC_ViewBatchDetails] 135        
CREATE PROCEDURE [dbo].[HC_ViewBatchDetails]                  
@BatchID bigint                                                       
AS                                                
BEGIN                     
              
select count(N.NoteID) as TotalClaims,N.ReferralID,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName --,R.FirstName,R.LastName              
,R.AHCCCSID,R.CISNumber,R.Dob,BN.BatchID ,P.PayorID ,P.PayorBillingType     
from BatchNotes BN        
INNER JOIN Notes N on N.NoteID = BN.NoteID and BN.BatchID=@BatchID        
INNER JOIN dbo.Payors P ON p.PayorID = N.PayorID      
INNER JOIN Referrals R on R.referralid =N.referralid         
GROUP BY N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob,BN.BatchID,P.PayorID  ,P.PayorBillingType     
ORDER BY PatientName             
                              
END
