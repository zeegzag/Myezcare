-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 2 March, 2020
-- Description:	This stored procedure get all latest downloaded ERAs from claim.md
-- =============================================
CREATE PROCEDURE [dbo].[HC_GetLatestERA]
(
	@LatestEraID bigint=0,
	@PayorID bigint=0,
	@StartDate datetime=null,
	@EndDate datetime=null,
	@IsDeleted bigint = -1,          
	@SortExpression NVARCHAR(100),            
	@SortType NVARCHAR(10),          
	@FromIndex INT,          
	@PageSize INT
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
             
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CheckNumber' THEN E.CheckNumber END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CheckNumber' THEN E.CheckNumber END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CheckType' THEN E.CheckType END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CheckType' THEN E.CheckType END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN convert(decimal, E.PaidAmount) END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN convert(decimal, E.PaidAmount) END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PaidDate' THEN convert(datetime, E.PaidDate) END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PaidDate' THEN convert(datetime, E.PaidDate) END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClaimProviderName' THEN E.ClaimProviderName END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClaimProviderName' THEN E.ClaimProviderName END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'DownTime' THEN E.DownTime END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'DownTime' THEN E.DownTime END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EraID' THEN E.EraID END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EraID' THEN E.EraID END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'RecievedTime' THEN CONVERT(DATETIME, E.RecievedTime) END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'RecievedTime' THEN CONVERT(DATETIME, E.RecievedTime) END END DESC
			) AS ROW,
			E.LatestEraID,E.CheckNumber,E.CheckType,E.ClaimProviderName,E.DownTime,E.EraID,E.PaidAmount,E.PaidDate,E.PayerName,E.PayerID,E.ProviderName,
			E.ProviderNPI,E.ProviderTaxID,E.RecievedTime,E.Source,E.IsDeleted
			FROM LatestERAs E
			INNER JOIN OrganizationSettings OS ON OS.Submitter_NM109_IdCode = E.ProviderNPI
			INNER JOIN Payors P ON P.AgencyNPID = E.PayerID
			WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR E.IsDeleted=@IsDeleted) 
					AND ((@LatestEraID IS NULL OR @LatestEraID=0) OR E.LatestEraID = @LatestEraID)
					AND ((@PayorID IS NULL OR @PayorID=0) OR P.PayorID = @PayorID)
					AND ((@StartDate IS NULL OR convert(datetime, E.PaidDate) >= @StartDate) and (@EndDate IS NULL OR convert(datetime, E.PaidDate) <= @EndDate))  
					AND P.IsDeleted=0 AND E.IsDeleted=0
		) AS E1
	) SELECT * FROM CTELatestErasList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)
END