-- EXEC GetBillingSummary @StartDate = '2017/07/04', @EndDate = '2017/07/13'
CREATE PROCEDURE [dbo].[GetBillingSummary]
@StartDate DATE=NULL,
@EndDate DATE=NULL
AS
BEGIN
SELECT 
                                ClientName= R.LastName+', '+R.FirstName,R.Gender, R.AHCCCSID, R.CISNumber,Payor=N.PayorShortName,
                                BT.BatchTypeName,
                                ClaimStatus=CSC.ClaimStatusName,ClaimNumber=BN.CLP01_ClaimSubmitterIdentifier,PayorClaimNumber=BN.CLP07_PayerClaimControlNumber,
                                BilledAmount= BN.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount,AllowedAmount=BN.AMT01_ServiceLineAllowedAmount_AllowedAmount, PaidAmount=BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount,
                                ClaimAdjustmentGroupCode=CAGC.ClaimAdjustmentGroupCodeID+' : '+CAGC.ClaimAdjustmentGroupCodeName,
								ClaimAdjustmentReason=CARC.ClaimAdjustmentReasonCodeID+' : '+CARC.ClaimAdjustmentReasonDescription,
								--ProcessedDate,ReceivedDate,
								CONVERT(VARCHAR(10),CONVERT(datetime,LoadDate,1),111) AS LoadDate
                                FROM BatchNotes BN       
                                INNER JOIN Batches B on B.BatchID=BN.BatchID  AND B.IsSent=1     
                                INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID
                                INNER JOIN Employees EG on EG.EmployeeID=B.CreatedBy
                                LEFT JOIN Employees ES on ES.EmployeeID=B.IsSentBy
                                INNER JOIN Notes N ON N.NoteID=BN.NoteID        
                                LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode        
                                LEFT JOIN ClaimAdjustmentGroupCodes CAGC  ON CAGC.ClaimAdjustmentGroupCodeID=BN.CAS01_ClaimAdjustmentGroupCode        
                                LEFT JOIN ClaimAdjustmentReasonCodes CARC  ON CARC.ClaimAdjustmentReasonCodeID=BN.CAS02_ClaimAdjustmentReasonCode        
                                LEFT JOIN Referrals R ON R.ReferralID=N.ReferralID        
                                LEFT JOIN Modifiers MD ON MD.ModifierID=N.ModifierID       
                                WHERE 1=1 --AND BN.ParentBatchNoteID IS NULL         
                                          AND N.IsBillable=1 AND n.IsDeleted=0 AND  BN.CLP02_ClaimStatusCode IS NOT NULL
										  AND ((@StartDate is null OR CONVERT(Date,LoadDate) >= @StartDate)) and ((@EndDate is null OR Convert(date,LoadDate) <= @EndDate))



END

                                                   

                        



