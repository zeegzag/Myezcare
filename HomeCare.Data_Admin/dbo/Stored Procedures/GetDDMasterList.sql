CREATE PROCEDURE [dbo].[GetDDMasterList]
	@ItemType INT = NULL,  
	@Title nvarchar(2000) = NULL,
	@Value nvarchar(2000) = NULL,
	@IsDeleted BIGINT = -1,    
	@SortExpression NVARCHAR(100),                
	@SortType NVARCHAR(10),              
	@FromIndex INT,              
	@PageSize INT    
AS            
BEGIN            
	;WITH CTEDDMasterList AS                  
	(                   
		SELECT *,COUNT(T1.DDMasterID) OVER() AS Count FROM                   
		(                  
			SELECT ROW_NUMBER() OVER (ORDER BY
				CASE WHEN @SortType = 'ASC' THEN 
					CASE 
						WHEN @SortExpression = 'ItemType' THEN DT.Name
						WHEN @SortExpression = 'Title' THEN Title
						WHEN @SortExpression = 'Value' THEN Value
					END
				END ASC,
				CASE WHEN @SortType = 'DESC' THEN
					CASE
						WHEN @SortExpression = 'ItemType' THEN DT.Name
						WHEN @SortExpression = 'Title' THEN Title
						WHEN @SortExpression = 'Value' THEN Value
					END
				END DESC
			) AS Row,DDMasterID,ItemType=DT.Name,DT.DDMasterTypeID,Title,IsDeleted,D.[Value]
			FROM  DDMaster D  
			INNER JOIN lu_DDMasterTypes DT ON DT.DDMasterTypeID=D.ItemType  
			WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR D.IsDeleted=@IsDeleted)                  
			AND ((@ItemType IS NULL OR LEN(@ItemType)=0) OR (ItemType = @ItemType))
			AND ((@Title IS NULL OR LEN(@Title)=0) OR (Title LIKE '%' + @Title + '%'))
			AND ((@Value IS NULL OR LEN(@Value)=0) OR (D.[Value] LIKE '%' + @Value + '%'))
			) AS T1  
		)        
	SELECT * FROM CTEDDMasterList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                
END