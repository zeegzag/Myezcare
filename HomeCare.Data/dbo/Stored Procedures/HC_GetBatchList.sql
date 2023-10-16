              
                    
-- EXEC HC_GetBatchList @IsSentStatus = '-1', @ClientName = '', @SortExpression = 'BatchID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'                
CREATE PROCEDURE [dbo].[HC_GetBatchList]                                      
@BatchID bigint=0,                                                        
@BatchTypeID bigint=0,                                                        
@PayorID bigint=0,                                                        
@StartDate date=null,                                                        
@EndDate date=null,                                   
@Comment VARCHAR(max)=null,                                   
@IsSentStatus int=-1,                                 
@ClientName VARCHAR(MAX)=null,                                                  
@SORTEXPRESSION VARCHAR(100),                                                        
@SORTTYPE VARCHAR(10),                                                        
@FROMINDEX INT,                                                        
@PAGESIZE INT ,      
      
@ClaimAdjustmentTypeID NVARCHAR(MAX)=NULL ,        
@ReconcileStatus NVARCHAR(MAX)=NULL      
AS                                                                                                  
BEGIN                                       
                                                                                                 
;WITH CTEBillingBatch AS                                                                                                   
 (                                                                                                  
  SELECT *,COUNT(T1.BatchID) OVER() AS COUNT FROM                                                                                                   
  (                                                                                                  
   SELECT ROW_NUMBER() OVER (ORDER BY                                                                                                  
                                                                                                  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BatchID' THEN CONVERT(BIGINT,BatchID) END END ASC,                                                                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BatchID' THEN CONVERT(BIGINT,BatchID)  END END DESC,                                                                            
                                                                                          
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'GatherDate' THEN  CONVERT(date, GatherDate, 105) END END ASC,                                                                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'GatherDate' THEN  CONVERT(date, GatherDate , 105) END END DESC,                                                                          
                                                                                                                    
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClaimCounts' THEN CAST(BatchID AS decimal)  END END ASC,                                                                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClaimCounts' THEN CAST(BatchID AS decimal)  END END DESC,                                                                          
                                                                              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SentDate' THEN  CONVERT(date, SentDate, 105) END END ASC,                                                                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SentDate' THEN  CONVERT(date, SentDate , 105) END END DESC,                                                                          
                                                                          
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Payor' THEN  PayorName  END END ASC,                                                                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Payor' THEN  PayorName END END DESC,                                                                          
                                                                           
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Type' THEN  BatchTypeName END END ASC,                                                                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Type' THEN BatchTypeName END END DESC,                       
                                                                              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'GatherBy' THEN  GatheredBy  END END ASC,                                                 
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'GatherBy' THEN   GatheredBy  END END DESC,                                                             
                                                                            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SentBy' THEN  IsSentBy END END ASC,                                                                 
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SentBy' THEN IsSentBy END END DESC                                                                          
    )AS ROW,                                   
 * FROM                                                                                               
 (                                    
                 
                 
                 
                
                 
                 
 SELECT Comment,BatchID,  StartDate,EndDate,BatchTypeName, BatchTypeID,                                                                                             
 PayorName,PayorBillingType, GatheredBy, GatherDate, SentDate,                                                                     
 ServiceStartDate,ServiceEndDate,IsSent,IsSentBy, EraIDs,                                
                 
 Amount = SUM(Amount)  ,                                 
 BillingAmount = SUM(BillingAmount) ,                
 Gathered = COUNT(DISTINCT Gathered),                
 BillingGathered = COUNT(DISTINCT Gathered),                                  
 Unit = SUM(Unit),                
 BillingUnit = SUM(BillingUnit),                
 AllowedAmount= SUM(AllowedAmount),                
 PaidAmount= SUM(PaidAmount)  ,  
 MPP_AdjustedAmount = SUM(MPP_AdjustedAmount)  
                        
 FROM                  
                 
                
 (                
                
 SELECT BH.Comment,BH.BatchID,  BH.StartDate,BH.EndDate,BHT.BatchTypeName, BHT.BatchTypeID,                                                                                             
 P.PayorName,P.PayorBillingType, EMP.UserName as GatheredBy, BH.CreatedDate as GatherDate, BH.SentDate,                                                                     
 BH.StartDate  as ServiceStartDate,BH.EndDate as ServiceEndDate,                                    
 BH.IsSent,EMPI.UserName as IsSentBy,BH.EraIDs,                
                 
 Amount = Original_Amount ,                 
 --BillingAmount = (CASE WHEN BCH.IsUseInBilling=0 THEN 0 ELSE CONVERT(decimal(18,2),CLM_BilledAmount) END),                 
 BillingAmount = CONVERT(decimal(18,2),CLM_BilledAmount),                 
 Gathered = BCH.NoteID ,                
 BillingGathered = CASE WHEN BCH.IsUseInBilling=1 THEN 1 ELSE 0 END,                
 Unit = Original_Unit,                
 BillingUnit = CASE WHEN BCH.IsUseInBilling=0 THEN 0 ELSE CONVERT(BIGINT,CLM_Unit) END,                 
                
 AllowedAmount =  CONVERT(DECIMAL(10,2), CASE WHEN BCH.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL OR LEN(BCH.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0                
    THEN '0' ELSE BCH.AMT01_ServiceLineAllowedAmount_AllowedAmount END ) ,                
 PaidAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BCH.SVC03_LineItemProviderPaymentAmoun_PaidAmount IS NULL OR LEN(BCH.SVC03_LineItemProviderPaymentAmoun_PaidAmount)=0                
    THEN '0' ELSE BCH.SVC03_LineItemProviderPaymentAmoun_PaidAmount END),                
  
  
MPP_AdjustedAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BCH.MPP_AdjustmentAmount IS NULL OR LEN(BCH.MPP_AdjustmentAmount)=0                
    THEN '0' ELSE BCH.MPP_AdjustmentAmount END),                
      
      
ClaimAdjustmentTypeID,      
                 
 ClaimStatus=CASE WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void'))       
   AND CONVERT(DECIMAL(18,2),ISNULL(BCH.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName IS NOT NULL       
   AND BCH.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Denied'       
         
   WHEN Submitted_ClaimAdjustmentTypeID = 'Void'       
   AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BCH.CLP04_TotalClaimPaymentAmount,0))= 0       
   AND CSC.ClaimStatusName='Reversal of Previous Payment'       
   AND BCH.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Paid'                     
        
   WHEN Submitted_ClaimAdjustmentTypeID = 'Void'       
   AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BCH.CLP04_TotalClaimPaymentAmount,0))= 0       
   AND CSC.ClaimStatusName!='Reversal of Previous Payment'       
   AND BCH.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Denied'        
         
   WHEN CSC.ClaimStatusName IS NULL  THEN NULL       
   WHEN  BCH.CLP07_PayerClaimControlNumber IS NOT NULL THEN 'Paid'       
   ELSE  NULL END    ,      
--RowNumber = ROW_NUMBER() OVER ( PARTITION BY BCH.BatchID,BCH.NoteID Order BY  BCH.MarkAsLatest DESC, BCH.BatchNoteID DESC)                      
RowNumber = CASE     
WHEN @ClaimAdjustmentTypeID IS NULL THEN ROW_NUMBER() OVER ( PARTITION BY BCH.BatchID,BCH.NoteID Order BY  BCH.MarkAsLatest DESC, BCH.BatchNoteID DESC)      
ELSE  ROW_NUMBER() OVER ( PARTITION BY BCH.NoteID Order BY  BCH.MarkAsLatest DESC, BCH.BatchNoteID DESC)   END    
                
 FROM             
                
 Batches BH                                            
 INNER JOIN BatchTypes BHT on BH.BatchTypeID= BHT.BatchTypeID                                                               
 LEFT JOIN BatchNotes BCH on BCH.BatchID=BH.BatchID  --AND  (BCH.IsFirstTimeClaimInBatch IS NULL OR BCH.IsFirstTimeClaimInBatch=1)       
 LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BCH.CLP02_ClaimStatusCode                                      
 LEFT JOIN Payors P on P.PayorID=BH.PayorID                                              
 LEFT JOIN Employees EMP on EMP.EmployeeID=BH.CreatedBy                        
 LEFT JOIN Employees EMPI on EMPI.EmployeeID=BH.IsSentBy                                
                    
 LEFT JOIN  Notes N On N.NoteID = BCH.NoteID                    
 LEFT JOIN Referrals R On R.ReferralID = N.ReferralID                      
                                          
 WHERE BH.IsDeleted=0                                 
 AND  ((CAST(@IsSentStatus AS BIGINT)=-1) OR BH.IsSent=@IsSentStatus)                                      
 AND ((@BatchID =0) or BH.BatchID= @BatchID)                                      
 AND ((@BatchTypeID =0) or BH.BatchTypeID= @BatchTypeID)                                      
 AND ((@PayorID=0) or BH.PayorID= @PayorID)                                      
 AND ((@StartDate is null OR @StartDate BETWEEN BH.StartDate AND BH.EndDate OR BH.StartDate >= @StartDate) AND (@EndDate is null OR @EndDate BETWEEN BH.StartDate AND BH.EndDate OR BH.EndDate <= @EndDate))                    
 AND ((@Comment is null or @Comment ='') or BH.Comment like '%'+@Comment+'%')                                            
 AND                                
 --((@ClientName IS NULL OR LEN(R.LastName)=0)                                
 (((@ClientName IS NULL) or (@ClientName='') or LEN(LTRIM(rtrim(@ClientName)))=0  )                   
 OR (                                
 (R.FirstName LIKE '%'+@ClientName+'%' ) OR                                
 (R.LastName LIKE '%'+@ClientName+'%') OR                                
 (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                                
(R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                                
 (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                                
 (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))                    
      
      
      
 ) AS TEMP WHERE     
     
     
 (@ClaimAdjustmentTypeID IS NULL AND RowNumber = 1                      )    
OR (                 
--AND         
(       
@ClaimAdjustmentTypeID IS NOT NULL AND    
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
              
 AND (ClaimAdjustmentTypeID IS NULL OR   LEN(ClaimAdjustmentTypeID)=0 OR (@ClaimAdjustmentTypeID IS NOT NULL AND ClaimAdjustmentTypeID NOT IN ('Write-Off')))     )               
                 
 GROUP BY Comment,BatchID,BatchID,StartDate,EndDate,BatchTypeName,BatchTypeID,PayorName,PayorBillingType,                            
 GatherDate ,SentDate,GatheredBy,IsSent,IsSentBy, EraIDs , ServiceStartDate, ServiceEndDate                       
                 
                 
                 
                 
 ) AS T2                                                       
 ) AS T1                                                                                      
 )                                                                                                                            
 SELECT * FROM CTEBillingBatch  WHERE ROW BETWEEN ((@PAGESIZE *(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                                                                                                  
END 