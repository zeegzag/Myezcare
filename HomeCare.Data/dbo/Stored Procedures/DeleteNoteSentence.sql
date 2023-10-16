CREATE PROCEDURE [dbo].[DeleteNoteSentence]
	@NoteSentenceTitle VARCHAR(100) = NULL,
	@NoteSentenceDetails VARCHAR(100) = NULL,
	@IsDeleted int=-1,
	@SortExpression NVARCHAR(100),  
	@SortType NVARCHAR(10),
	@FromIndex INT,
	@PageSize INT,
	@ListOfIdsInCsv varchar(300),
	@IsShowList bit,
	@loggedInID BIGINT
AS
BEGIN

	IF(LEN(@ListOfIdsInCsv)>0)
	BEGIN		
			UPDATE NoteSentences SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE NoteSentenceID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))		  	
		END

	IF(@IsShowList=1)
	BEGIN
		EXEC GetNoteSentenceList @NoteSentenceTitle,@NoteSentenceDetails,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize
	END
END