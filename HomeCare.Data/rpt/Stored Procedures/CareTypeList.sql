--   EXEC [rpt].[CareTypeList] 
CREATE PROCEDURE [rpt].[CareTypeList] 

AS
BEGIN
	SET NOCOUNT ON;
    
	WITH CTE AS (
	SELECT 0 AS CareTypeID,'All' AS CareTypeName
	UNION ALL
	SELECT DISTINCT DDMasterID AS CareTypeID,Title AS CareTypeName 
	FROM dbo.DDMaster DDM WHERE IsDeleted=0 AND ItemType=1)
	SELECT * FROM CTE
	WHERE CareTypeName IS NOT NULL
	ORDER BY CASE WHEN CareTypeName = 'All' THEN '0'
		ELSE CareTypeName END ASC;
END