-- EXEC HC_GetLatestERA @SortExpression = 'RecievedTime', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'        
            
-- =============================================            
-- Author:  Kundan Kumar Rai            
-- Create date: 2 March, 2020            
-- Description: This stored procedure get all latest downloaded ERAs from claim.md            
-- =============================================            
CREATE PROCEDURE [dbo].[HC_GetLatestERA]              
(              
 @LatestEraID bigint=0,              
 @PayorID bigint=0,              
               
 @IsDeleted bigint = -1,                        
 @SortExpression NVARCHAR(100),                          
 @SortType NVARCHAR(10),                        
 @FromIndex INT,                        
 @PageSize INT   ,
 
 @CheckNumber NVARCHAR(MAX)='',
 @EraId NVARCHAR(MAX)='',
 @PaidStartDate DATETIME=NULL,              
 @PaidEndDate DATETIME=NULL,
 @ReceivedStartDate DATETIME=NULL,              
 @ReceivedEndDate DATETIME=NULL
)              
AS              
BEGIN              
 ;WITH CTELatestErasList AS              
 (              
  SELECT *, COUNT(E1.LatestEraID) OVER() AS Count FROM              
  (              
   SELECT ROW_NUMBER() OVER (ORDER BY              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'LatestEraID' THEN convert(bigint, LatestEraID) END END ASC,                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'LatestEraID' THEN convert(bigint, LatestEraID) END END DESC,                        
                           
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CheckNumber' THEN CheckNumber END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CheckNumber' THEN CheckNumber END END DESC,              
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CheckType' THEN CheckType END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CheckType' THEN CheckType END END DESC,              
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN convert(decimal, PaidAmount) END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN convert(decimal, PaidAmount) END END DESC,              
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PaidDate' THEN convert(datetime, PaidDate) END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PaidDate' THEN convert(datetime, PaidDate) END END DESC,              
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClaimProviderName' THEN ClaimProviderName END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClaimProviderName' THEN ClaimProviderName END END DESC,              
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'DownTime' THEN DownTime END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'DownTime' THEN DownTime END END DESC,              
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EraID' THEN EraID END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EraID' THEN EraID END END DESC,              
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReceivedTime' THEN CONVERT(DATETIME, RecievedTime) END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReceivedTime' THEN CONVERT(DATETIME, RecievedTime) END END DESC              ,

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayerName' THEN  PayerName END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayerName' THEN  PayerName END END DESC              
   ) AS ROW,              
             
   * FROM (          
   
   SELECT E.LatestEraID,E.CheckNumber,E.CheckType,E.ClaimProviderName,E.DownTime,E.EraID,E.PaidAmount,E.PaidDate,E.PayerName,E.PayerID,
   E.ProviderName, E.ProviderNPI,E.ProviderTaxID,E.RecievedTime,E.Source,E.IsDeleted, U.Upload835FileProcessStatus, U.ReadableFilePath,
   E.ValidationMessage, U.EraMappedBatches , U.LogFilePath,          
   RowNumber = ROW_NUMBER() OVER ( PARTITION BY E.LatestEraID Order BY U.Upload835FileID DESC)    
   FROM LatestERAs E              
   --INNER JOIN OrganizationSettings OS ON OS.Submitter_NM109_IdCode = E.ProviderNPI  
   LEFT JOIN Payors P ON P.EraPayorID = E.PayerID AND P.IsDeleted=0              
   LEFT JOIN Upload835Files U ON U.EraID = E.EraID             
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR E.IsDeleted=@IsDeleted)               
     AND ((@LatestEraID IS NULL OR @LatestEraID=0) OR E.LatestEraID = @LatestEraID)              
     AND ((@PayorID IS NULL OR @PayorID=0) OR P.PayorID = @PayorID)              

AND ( LEN(ISNULL(@CheckNumber,'')) =0 OR E.CheckNumber= @CheckNumber)              
AND (  LEN(ISNULL(@EraId,'')) =0 OR E.EraID = @EraId)              
AND ((@PaidStartDate IS NULL OR CONVERT(DATETIME, E.PaidDate) >= @PaidStartDate) AND (@PaidEndDate IS NULL OR convert(DATETIME, E.PaidDate) <= @PaidEndDate))
AND ((@ReceivedStartDate IS NULL OR CONVERT(DATETIME, E.RecievedTime) >= @ReceivedStartDate) 
			AND (@ReceivedEndDate IS NULL OR convert(DATETIME, E.RecievedTime) <= @ReceivedEndDate))
     
-- EXEC HC_GetLatestERA @CheckNumber = '', @EraId = '', @SortExpression = 'RecievedTime', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'	 
	 
	 AND E.IsDeleted=0-- AND U.EraID='17707198'             
  ) AS TEMP WHERE TEMP.RowNumber= 1          
  ) AS E1              
 ) SELECT * FROM CTELatestErasList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)              
END 