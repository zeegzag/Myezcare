-- EXEC [rpt].[PayorsList]

CREATE PROCEDURE [rpt].[PayorsList] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	WITH CTE AS (
	SELECT 0 AS PayorID,'All' AS PayorName
	UNION ALL
	SELECT DISTINCT PayorID,PayorName 
	FROM dbo.Payors e where IsDeleted=0)
	SELECT * FROM CTE
	WHERE PayorName IS NOT NULL
	ORDER BY CASE WHEN PayorName = 'All' THEN '0'
		ELSE PayorName END ASC;
END