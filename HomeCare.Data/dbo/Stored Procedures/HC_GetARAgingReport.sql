-- EXEC HC_GetARAgingReport  '4','',''              
CREATE PROCEDURE HC_GetARAgingReport                
@PayorIDs NVARCHAR(MAX) =NULL,           
@ReconcileStatus NVARCHAR(MAX)=NULL ,    
@ClientName VARCHAR(MAX)=null                                                
AS                                            
BEGIN                                 
        
DECLARE @AllActivePayorIDs NVARCHAR(MAX)=(SELECT  STUFF((SELECT ',' + Convert(varchar(50),ISNULL(PayorID,'')) FROM Payors WHERE IsDeleted=0 AND ShortName NOT IN ('MCP RBHA','Pro Bono') FOR XML PATH('')),1,1,''));        
              
 ;WITH CTEGetARAgingReport AS                                            
 (                                             
               
 SELECT COUNT(T2.PayorID) OVER() AS Count,PayorID,PayorShortName=ShortName,PayorName,PayorTypeName,          
        
 AllActivePayorIDs = @AllActivePayorIDs,        
        
 TotalPendingAmount= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN 0.00 ELSE TotalPendingAmount END,              
 PendingAmount0_60= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN  0.00 ELSE PendingAmount0_60 END,              
 PendingAmount61_90= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN  0.00 ELSE PendingAmount61_90 END,              
 PendingAmount91_120= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN  0.00 ELSE PendingAmount91_120 END,              
 PendingAmount121_180= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN  0.00 ELSE PendingAmount121_180 END,              
 PendingAmount181_270= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN  0.00 ELSE PendingAmount181_270 END,              
 PendingAmount271_365= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN  0.00 ELSE PendingAmount271_365 END--,              
 --PendingAmount365_Plus= CASE WHEN ShortName IN ('MCP RBHA','Pro Bono') THEN  0.00  ELSE PendingAmount365_Plus END              
               
               
 FROM                                             
  (                                            
  SELECT ROW_NUMBER() OVER (ORDER BY ShortName ASC ) AS Row,               
  *,TotalPendingAmount=PendingAmount0_60+PendingAmount121_180+PendingAmount181_270+PendingAmount271_365--+PendingAmount365_Plus           
           
 FROM (              
 SELECT PayorID,ShortName,PayorName,PayorTypeName,          
 PartitionRow = ROW_NUMBER() OVER(PARTITION BY PayorID Order BY PayorID ASC),              
 PendingAmount0_60 =SUM(CASE WHEN DayCount BETWEEN 0 AND 60 THEN CLM_BilledAmount ELSE 0 END) OVER(PARTITION BY PayorID Order BY PayorID ASC),              
 PendingAmount61_90 =SUM(CASE WHEN DayCount BETWEEN 61 AND 90 THEN CLM_BilledAmount ELSE 0 END) OVER(PARTITION BY PayorID Order BY PayorID ASC),              
 PendingAmount91_120 =SUM(CASE WHEN DayCount BETWEEN 91 AND 120 THEN CLM_BilledAmount ELSE 0 END) OVER(PARTITION BY PayorID Order BY PayorID ASC),              
 PendingAmount121_180 =SUM(CASE WHEN DayCount BETWEEN 121 AND 180 THEN CLM_BilledAmount ELSE 0 END) OVER(PARTITION BY PayorID Order BY PayorID ASC),              
 PendingAmount181_270 =SUM(CASE WHEN DayCount BETWEEN 181 AND 270 THEN CLM_BilledAmount ELSE 0 END) OVER(PARTITION BY PayorID Order BY PayorID ASC),              
 PendingAmount271_365 =SUM(CASE WHEN DayCount BETWEEN 271 AND 365 THEN CLM_BilledAmount ELSE 0 END) OVER(PARTITION BY PayorID Order BY PayorID ASC)--,              
 --PendingAmount365_Plus =SUM(CASE WHEN DayCount BETWEEN 366 AND 999 THEN CLM_BilledAmount ELSE 0 END) OVER(PARTITION BY PayorID Order BY PayorID ASC)              
              
 FROM (              
              
 SELECT ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY MarkAsLatest DESC,BN.BatchID DESC, BN.BatchNoteID Desc) AS RowNumber,              
 P.PayorID,P.ShortName,P.PayorName, BN.NoteID, BN.MarkAsLatest, BN.BatchID,BN.BatchNoteID,ClaimAdjustmentTypeID,Submitted_ClaimAdjustmentTypeID,PT.PayorTypeName,              
 DayCount=DATEDIFF(DAY, N.ServiceDate,GETDATE()) ,CLM_BilledAmount=CONVERT(DECIMAL(10,2), RTRIM(LTRIM(REPLACE(BN.CLM_BilledAmount, ',', '')))),              
 ClaimStatus=  CASE WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void'))   
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
   ELSE  NULL END                
 FROM BatchNotes BN              
 INNER JOIN Batches B ON B.BatchID=BN.BatchID AND B.IsSent=1  AND BN.IsUseInBilling=1 AND IsShowOnParentReconcile=1              
 INNER JOIN BatchNoteDetails N ON N.NoteID=BN.NoteID AND N.BatchID=BN.BatchID                    
 INNER JOIN Notes NT ON NT.NoteID = N.NoteID    
 INNER JOIN Referrals R ON R.ReferralID = NT.ReferralID    
 INNER JOIN Payors P ON P.PayorID=B.PayorID              
 LEFT JOIN PayorTypes PT ON PT.PayorTypeID=P.PayorTypeID          
 LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode              
 WHERE P.IsDeleted=0 AND ((@PayorIDs IS NULL OR LEN(@PayorIDs)=0) OR B.PayorID In (SELECT CONVERT(BIGINT,val) FROM GetCSVTable(@PayorIDs)))                           
--AND ((@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0) OR R.ReferralID In (SELECT CONVERT(BIGINT,val) FROM GetCSVTable(@ReferralIDs)))                              
AND                            
 (((@ClientName IS NULL) or (@ClientName='') or LEN(LTRIM(rtrim(@ClientName)))=0  )               
 OR (                            
 (R.FirstName LIKE '%'+@ClientName+'%' ) OR                            
 (R.LastName LIKE '%'+@ClientName+'%') OR                            
 (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                            
 (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                            
 (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                            
 (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))     
    
    
 ) AS T  WHERE RowNumber=1 AND       
 --(ClaimStatus !='Paid')      
 (               
   (@ReconcileStatus IS NULL OR LEN(@ReconcileStatus)=0 OR                           
    (RowNumber=1 AND   
     (  
    (@ReconcileStatus = 'InProcess' AND ClaimStatus IS NULL) OR   
    ((ClaimStatus IN (SELECT val FROM GetCSVTable(@ReconcileStatus)) ) OR   
    ('InProcess' IN (SELECT val FROM GetCSVTable(@ReconcileStatus)) AND ClaimStatus IS NULL  ))  
   )  
  )                          
   )                                        
  )         
  
 --AND (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID!='Void')             
 AND  (ClaimAdjustmentTypeID IS NULL OR ClaimAdjustmentTypeID NOT IN ('Write-Off'))            
              
 ) AS T1 WHERE T1.PartitionRow=1 ) AS T2 )--ORDER BY PayorName ASC              
              
              
SELECT * FROM CTEGetARAgingReport   ORDER BY PayorShortName            
-- EXEC GetARAgingReport                
                  
                                            
END 