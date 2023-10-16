-- EXEC GetReferralCaseLoadList @ReferralID =1951, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'                        
CREATE PROCEDURE [dbo].[GetReferralCaseLoadList]
	@ReferralID BIGINT = 0,
	@StartDate DATE = NULL,
	@EndDate DATE = NULL,
	@IsDeleted int=-1,
	@SortExpression NVARCHAR(100),                          
	@SortType NVARCHAR(10),
	@FromIndex INT,
	@PageSize INT
AS
BEGIN
	;WITH CTERclMasterList AS
	(                         
		SELECT *,COUNT(t1.ReferralCaseLoadID) OVER() AS Count FROM
		(                        
			SELECT ROW_NUMBER() OVER
			(
				ORDER BY
				CASE WHEN 1=1 THEN TBL1.EndDate ELSE TBL1.StartDate END ASC,
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralCaseLoadID' THEN TBL1.ReferralCaseLoadID END END ASC,                        
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralCaseLoadID' THEN TBL1.ReferralCaseLoadID END END DESC,                        
	                     
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeName' THEN TBL1.EmployeeName END END ASC,                        
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeName' THEN TBL1.EmployeeName END END DESC,
				
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CaseLoadType' THEN TBL1.CaseLoadType END END ASC,                        
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CaseLoadType' THEN TBL1.CaseLoadType END END DESC,
	                        
				CASE WHEN @SortType = 'ASC' THEN                        
					CASE                         
						WHEN @SortExpression = 'StartDate' THEN TBL1.StartDate                        
					END                         
				END ASC,                        
				CASE WHEN @SortType = 'DESC' THEN                        
					CASE                         
						WHEN @SortExpression = 'StartDate' THEN TBL1.StartDate                        
					END                        
				END DESC,
				
				CASE WHEN @SortType = 'ASC' THEN                        
					CASE                         
						WHEN @SortExpression = 'EndDate' THEN TBL1.EndDate                        
					END                         
				END ASC,                        
				CASE WHEN @SortType = 'DESC' THEN                        
					CASE                         
						WHEN @SortExpression = 'EndDate' THEN TBL1.EndDate                        
					END                        
				END DESC         
			) AS Row, * 
			FROM 
			(     
			   SELECT DISTINCT 
					RCL.ReferralCaseLoadID,
					RCL.ReferralID,
					RCL.CaseLoadType,
					RCL.StartDate,
					RCL.EndDate,
					RCL.IsDeleted,
					E.EmployeeID,
					EmployeeName = dbo.GetGeneralNameFormat(E.FirstName,E.LastName)
			   FROM
					ReferralCaseLoads RCL
					INNER JOIN Referrals R on R.ReferralID=RCL.ReferralID
					INNER JOIN Employees E on E.EmployeeID=RCL.EmployeeID
			   WHERE 
					((CAST(@IsDeleted AS BIGINT)=-1) OR RCL.IsDeleted=@IsDeleted)                        
					AND ((@ReferralID =0 OR LEN(@ReferralID)=0) OR RCL.ReferralID=@ReferralID)                
					AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR RCL.StartDate LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')                        
					AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR RCL.EndDate LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')                                
			) AS TBL1
		) AS t1
	)
                   
	SELECT * FROM CTERclMasterList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                        
END