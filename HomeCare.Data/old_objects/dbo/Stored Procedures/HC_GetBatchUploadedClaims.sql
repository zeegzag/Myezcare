
-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 6/7/2020 
-- Description:	Get and filter manage claims data
-- =============================================
CREATE PROCEDURE [dbo].[HC_GetBatchUploadedClaims]
              
@ReferralID bigint = 0,
@BatchID bigint = 0,
@PayorID bigint = 0,
@INS_Number bigint = 0,
@StartDate date = null,
@EndDate date = null,

@SORTEXPRESSION NVARCHAR(100),                 
@SORTTYPE NVARCHAR(10),                
@FROMINDEX INT,                                
@PAGESIZE INT     
AS
BEGIN

;WITH CTEBatchUploadedClaims AS                            
 (                                 
  SELECT *,COUNT(T1.BatchUploadedClaimID) OVER() AS COUNT FROM                            
  (                                
   SELECT ROW_NUMBER() OVER (ORDER BY                            
                            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PatientName' THEN PatientName END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PatientName' THEN PatientName END END DESC,   
 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BatchID' THEN BatchID END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BatchID' THEN BatchID END END DESC,   
 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Payor' THEN Payer END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Payor' THEN Payer END END DESC,                
                                                                         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'INS_Number' THEN INS_Number END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'INS_Number' THEN INS_Number END END DESC, 
 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FDOS' THEN FDOS END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FDOS' THEN FDOS END END DESC, 
 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Total_Charge' THEN Total_Charge END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Total_Charge' THEN Total_Charge END END DESC,
 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Status' THEN Status END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Status' THEN Status END END DESC
                           
   ) AS ROW,
               
   BUC.BatchUploadedClaimID, BUC.PatientName, BUC.BatchID, BUC.Payer, BUC.INS_Number,BUC.BatchTypeID,BUC.PayorID,BUC.ReferralID, 
   BUC.FDOS,BUC.Total_Charge as Charges, BUC.BillingProvider,BUC.CreatedDate as AddedDate, BUC.Status             
   FROM  BatchUploadedClaims BUC   
   WHERE ((@ReferralID = 0) OR BUC.ReferralID = @ReferralID)
		AND ((@BatchID = 0) OR BUC.BatchID = @BatchID)
		AND ((@PayorID = 0) OR BUC.PayorID = @PayorID)
		AND ((@INS_Number = 0) OR BUC.INS_Number = @INS_Number)
		AND ((@StartDate IS NULL OR @StartDate = '' OR CAST(BUC.FDOS AS DATE) >= @StartDate) AND (@EndDate IS NULL OR @EndDate = '' OR CAST(BUC.FDOS AS DATE) <= @EndDate))                 
   ) AS T1
 )            
 SELECT * FROM CTEBatchUploadedClaims WHERE  
 ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)
 ORDER BY CTEBatchUploadedClaims.FDOS DESC
END