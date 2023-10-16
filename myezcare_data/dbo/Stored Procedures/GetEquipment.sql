-- GetEquipment '', 10, 1
CREATE PROCEDURE [dbo].[GetEquipment]
@SearchText varchar(50)=null,
@IgnoreIDs VARCHAR(MAX),  
@PageSize int,
@ItemTypeID BIGINT
AS      
BEGIN    
	SELECT DISTINCT TOP(@PageSize)
		DDMasterID EquipmentID,
		[Title] EquipmentName
	FROM  
		DDMaster  
	WHERE 
		(@SearchText IS NULL) OR ([Title] LIKE '%'+@SearchText+'%' ) 
		AND ItemType = @ItemTypeID
		AND IsDeleted = 0
		AND DDMasterID NOT IN (SELECT val FROM [dbo].[GetCSVTable](@IgnoreIDs))
END
