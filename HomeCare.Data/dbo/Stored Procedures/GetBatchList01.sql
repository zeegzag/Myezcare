-- EXEC GetBatchList01 @IsSentStatus = '-1', @SortExpression = 'BatchID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'
CREATE PROCEDURE [dbo].[GetBatchList01]                                                                            
@BatchID bigint=0,                    
@BatchTypeID bigint=0,                    
@PayorID bigint=0,                    
@BillingProviderIDS varchar(4000)=null,                    
@StartDate date=null,                    
@EndDate date=null,                    
@IsDeleted BIGINT = -1,  
@IsSentStatus int=-1,           
@SORTEXPRESSION VARCHAR(100),                    
@SORTTYPE VARCHAR(10),                    
@FROMINDEX INT,                    
@PAGESIZE INT                    
AS                                                              
BEGIN 

-- EXEC CreateChildGroupNotes

-- EXEC RefreshBatchNotes
                                                             
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
    )AS ROW,   * FROM                                                           
    
	(
	SELECT BH.Comment,BH.BatchID,  BH.StartDate,BH.EndDate,BHT.BatchTypeName,                                                            
	P.PayorName, EMP.UserName as GatheredBy, BH.CreatedDate as GatherDate, BH.SentDate,                                 
	BH.StartDate  as ServiceStartDate,BH.EndDate as ServiceEndDate,
	BH.IsSent,EMPI.UserName as IsSentBy, SUM(Original_Amount) AS Amount, SUM((CASE WHEN BCH.IsUseInBilling=0 THEN 0 ELSE CONVERT(decimal(18,2),CLM_BilledAmount) END)) AS BillingAmount,

	COUNT(DISTINCT BCH.NoteID) as Gathered, SUM(CASE WHEN BCH.IsUseInBilling=1 THEN 1 ELSE 0 END) as BillingGathered,
	SUM(Original_Unit) AS Unit, SUM((CASE WHEN BCH.IsUseInBilling=0 THEN 0 ELSE CONVERT(BIGINT,CLM_Unit) END)) AS BillingUnit,
	
	(SELECT  STUFF((SELECT ', ' + FC.FacilityName                                                          
	FROM BatchApprovedFacility F join Facilities Fc on F.BillingProviderID=FC.FacilityID                                                         
	WHERE F.BatchID=BH.BatchID                                                        
	FOR XML PATH('')),1,1,'')) AS FacilityName --,
		
 --  (SELECT COUNT(Temp.NoteID) FROM (
	--SELECT DISTINCT Temp_BN.NoteID,Temp_BN.BatchID,Temp_BN.CLP02_ClaimStatusCode,Temp_BN.ClaimAdjustmentTypeID, ROW_NUMBER() OVER ( PARTITION BY Temp_BN.NoteID,Temp_BN.BatchID ORDER BY Temp_BN.BatchNoteID DESC) AS RowNumber FROM BatchNotes Temp_BN
	-- WHERE Temp_BN.BatchID=BH.BatchID 
	--) AS Temp   WHERE Temp.RowNumber=1 AND Temp.CLP02_ClaimStatusCode IS NOT NULL AND Temp.CLP02_ClaimStatusCode NOT IN (4,22) AND ClaimAdjustmentTypeID IS NULL) as Cleard 

	
	  FROM                                                                      
	  Batches BH        
	  INNER JOIN BatchTypes BHT on BH.BatchTypeID= BHT.BatchTypeID                           
	  LEFT JOIN BatchNotes BCH on BCH.BatchID=BH.BatchID  AND    (BCH.IsFirstTimeClaimInBatch IS NULL OR BCH.IsFirstTimeClaimInBatch=1)                      
	  LEFT JOIN Payors P on P.PayorID=BH.PayorID                                                                          
	  LEFT JOIN Employees EMP on EMP.EmployeeID=BH.CreatedBy                                                    
	  LEFT JOIN Employees EMPI on EMPI.EmployeeID=BH.IsSentBy      
	  WHERE BH.IsDeleted=0 -- AND    (BCH.IsFirstTimeClaimInBatch IS NULL OR BCH.IsFirstTimeClaimInBatch=1)
		AND  ((CAST(@IsSentStatus AS BIGINT)=-1) OR BH.IsSent=@IsSentStatus)  
		AND ((@BatchTypeID =0) or BH.BatchTypeID= @BatchTypeID)  
		AND ((@PayorID=0) or BH.PayorID= @PayorID)  
	 -- AND ((@BillingProviderIDS is null OR BH.BatchID in(select BatchID from BatchApprovedFacility where BillingProviderID in(select val from GetCSVTable(@BillingProviderIDS)))))                          
		AND ((@StartDate is null OR BH.StartDate >= @StartDate) and (@EndDate is null OR BH.EndDate<= @EndDate))       
		GROUP BY BH.Comment,BH.BatchID,BCH.BatchID,BH.StartDate,BH.EndDate,BHT.BatchTypeName,P.PayorName,BH.CreatedDate ,BH.SentDate,EMP.UserName,BH.IsSent,EMPI.UserName                                                
	) AS T2                   
  ) AS T1                                                                                      
 )                                                                                        
 SELECT * FROM CTEBillingBatch  WHERE ROW BETWEEN ((@PAGESIZE *(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                                                                                            
END