-- GetServicePlanComponent '', 10, 1
CREATE PROCEDURE [dbo].[GetServicePlanComponent]
@SearchText varchar(50)=null,
@PageSize int,
@ItemTypeID BIGINT
AS      
BEGIN    
	SELECT DISTINCT TOP(@PageSize)
		DDMasterID,
		[Title]
	FROM  
		DDMaster  
	WHERE 
		(@SearchText IS NULL) OR ([Title] LIKE '%'+@SearchText+'%' ) 
		AND ItemType = @ItemTypeID
		AND IsDeleted = 0
END