-- EXEC HC_GetBatchList 0,3,null,null,'',0,'','ASC',1,100      
CREATE PROCEDURE [dbo].[HC_GetBatchList]                                                                                      
@BatchTypeID bigint=0,                              
@PayorID bigint=0,                              
@StartDate date=null,                              
@EndDate date=null,         
@Comment VARCHAR(max)=null,         
@IsSentStatus int=-1,       
                    
@SORTEXPRESSION VARCHAR(100),                              
@SORTTYPE VARCHAR(10),                              
@FROMINDEX INT,                              
@PAGESIZE INT                              
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
 SELECT BH.Comment,BH.BatchID,  BH.StartDate,BH.EndDate,BHT.BatchTypeName,                                                                      
 P.PayorName,P.PayorBillingType, EMP.UserName as GatheredBy, BH.CreatedDate as GatherDate, BH.SentDate,                                           
 BH.StartDate  as ServiceStartDate,BH.EndDate as ServiceEndDate,          
 BH.IsSent,EMPI.UserName as IsSentBy,       
 SUM(Original_Amount) AS Amount,       
 SUM((CASE WHEN BCH.IsUseInBilling=0 THEN 0 ELSE CONVERT(decimal(18,2),CLM_BilledAmount) END)) AS BillingAmount,          
 COUNT(DISTINCT BCH.NoteID) as Gathered, SUM(CASE WHEN BCH.IsUseInBilling=1 THEN 1 ELSE 0 END) as BillingGathered,          
 SUM(Original_Unit) AS Unit, SUM((CASE WHEN BCH.IsUseInBilling=0 THEN 0 ELSE CONVERT(BIGINT,CLM_Unit) END)) AS BillingUnit        
 FROM                                                                                
 Batches BH                  
 INNER JOIN BatchTypes BHT on BH.BatchTypeID= BHT.BatchTypeID                                     
 LEFT JOIN BatchNotes BCH on BCH.BatchID=BH.BatchID  AND  (BCH.IsFirstTimeClaimInBatch IS NULL OR BCH.IsFirstTimeClaimInBatch=1)                                
 LEFT JOIN Payors P on P.PayorID=BH.PayorID                                                                                    
 LEFT JOIN Employees EMP on EMP.EmployeeID=BH.CreatedBy                                                              
 LEFT JOIN Employees EMPI on EMPI.EmployeeID=BH.IsSentBy      
                
 WHERE BH.IsDeleted=0       
 AND  ((CAST(@IsSentStatus AS BIGINT)=-1) OR BH.IsSent=@IsSentStatus)            
 AND ((@BatchTypeID =0) or BH.BatchTypeID= @BatchTypeID)            
 AND ((@PayorID=0) or BH.PayorID= @PayorID)            
 AND ((@StartDate is null OR BH.StartDate >= @StartDate) and (@EndDate is null OR BH.EndDate<= @EndDate))                 
 AND ((@Comment is null or @Comment ='') or BH.Comment like '%'+@Comment+'%')                  
 GROUP BY BH.Comment,BH.BatchID,BCH.BatchID,BH.StartDate,BH.EndDate,BHT.BatchTypeName,P.PayorName,P.PayorBillingType,  
 BH.CreatedDate ,BH.SentDate,EMP.UserName,BH.IsSent,EMPI.UserName                                                        
 ) AS T2                             
 ) AS T1                                                                                                
 )                                                                                                  
 SELECT * FROM CTEBillingBatch  WHERE ROW BETWEEN ((@PAGESIZE *(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                                                                        
END
