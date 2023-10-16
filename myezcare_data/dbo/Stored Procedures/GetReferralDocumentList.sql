-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,	>
-- Description:	<Description,,>
-- =============================================
-- EXEC GetReferralDocumentList @FromIndex = '1', @ToIndex = '10', @SortExpression = 'ReferralDocumentID', @SortType = 'DESC', @CustomWhere = 'Where (1=1)and (ReferralID =2)'
CREATE PROCEDURE [dbo].[GetReferralDocumentList] 
	-- Add the parameters for the stored procedure here
	@FromIndex INT,
	@ToIndex INT,
	@SortExpression VARCHAR(100),  
	@SortType VARCHAR(10),	
	@CustomWhere VARCHAR(MAX)
AS
BEGIN
	DECLARE @Sql VARCHAR(MAX)

    SET @SQL = ';WITH ReferralDocumentList AS
				(
					SELECT *,ROW_NUMBER() OVER (ORDER BY ' + @SortExpression + ' ' + @SortType + ') AS Row,COUNT(t.ReferralDocumentID) OVER() AS Count FROM 
					(	
						SELECT * FROM 
						(
							select RD.*,D.DocumentTypeName,e.LastName + '', ''+ e.FirstName as Name from ReferralDocuments RD
							inner join DocumentTypes D on D.DocumentTypeID=RD.DocumentTypeID
							inner join Employees e on e.EmployeeID=RD.CreatedBy
						) AS T1 ' + @CustomWhere + '
					) AS t
				)
				SELECT * FROM ReferralDocumentList WHERE ROW BETWEEN ' + CONVERT(VARCHAR(100),@FromIndex) + ' AND ' + CONVERT(VARCHAR(100),@ToIndex) + ' '
	
	PRINT(@SQL)		
	EXEC(@SQL)
END

