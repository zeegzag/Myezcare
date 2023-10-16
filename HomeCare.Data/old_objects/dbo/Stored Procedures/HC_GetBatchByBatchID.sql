-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE HC_GetBatchByBatchID

@BatchID bigint
	
AS
BEGIN
	
	
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
                
 WHERE BH.IsDeleted=0  and BH.BatchID=@BatchID                   
 GROUP BY BH.Comment,BH.BatchID,BCH.BatchID,BH.StartDate,BH.EndDate,BHT.BatchTypeName,P.PayorName,P.PayorBillingType,  
 BH.CreatedDate ,BH.SentDate,EMP.UserName,BH.IsSent,EMPI.UserName        

END