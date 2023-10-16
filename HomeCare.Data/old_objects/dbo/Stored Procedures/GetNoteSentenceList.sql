CREATE PROCEDURE [dbo].[GetNoteSentenceList]
	@NoteSentenceTitle VARCHAR(100) = NULL,
	@NoteSentenceDetails VARCHAR(100) = NULL,
	@IsDeleted BIGINT = -1,
	@SortExpression NVARCHAR(100),  
	@SortType NVARCHAR(10),
	@FromIndex INT,
	@PageSize INT
AS
BEGIN
	;WITH CTENoteSentenceList AS
	( 
		SELECT *,COUNT(T1.NoteSentenceID) OVER() AS Count FROM 
		(
			SELECT ROW_NUMBER() OVER (ORDER BY 
			CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NoteSentenceID' THEN NoteSentenceID END END ASC,
			CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NoteSentenceID' THEN NoteSentenceID END END DESC,
				CASE WHEN @SortType = 'ASC' THEN
					 CASE 						
						WHEN @SortExpression = 'NoteSentenceTitle' THEN NoteSentenceTitle	
						WHEN @SortExpression = 'NoteSentenceDetails' THEN NoteSentenceDetails	
					 END 
				END ASC,
				CASE WHEN @SortType = 'DESC' THEN
					 CASE 						
						WHEN @SortExpression = 'NoteSentenceTitle' THEN NoteSentenceTitle		
						WHEN @SortExpression = 'NoteSentenceDetails' THEN NoteSentenceDetails		
					 END
				END DESC
				
				

		) AS Row,
			NS.NoteSentenceID,NS.NoteSentenceTitle,NS.NoteSentenceDetails,NS.IsDeleted
			FROM  NoteSentences NS  
			WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR NS.IsDeleted=@IsDeleted)
			AND ((@NoteSentenceTitle IS NULL OR LEN(@NoteSentenceTitle)=0) OR (NS.NoteSentenceTitle LIKE '%' + @NoteSentenceTitle + '%'))	
			AND ((@NoteSentenceDetails IS NULL OR LEN(@NoteSentenceDetails)=0) OR (NS.NoteSentenceDetails LIKE '%' + @NoteSentenceDetails + '%'))						
		) AS T1		
	)
	
	SELECT * FROM CTENoteSentenceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)	
END

