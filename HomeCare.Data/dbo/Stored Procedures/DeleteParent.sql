CREATE PROCEDURE [dbo].[DeleteParent]
	 @Name VARCHAR(MAX) = NULL,  
	 @Email VARCHAR(MAX) = NULL,  
	 @Address VARCHAR(MAX) = NULL,  
	 @City VARCHAR(MAX) = NULL,  
	 @ZipCode VARCHAR(MAX) = NULL,  
	 @Phone VARCHAR(MAX) = NULL,  
	 @ContactTypeID BIGINT = 0,  
	 @IsDeleted BIGINT = -1,  
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
		UPDATE Contacts SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END , UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE ContactID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))		  	
	END

	IF(@IsShowList=1)
	BEGIN
		EXEC GetParentList @Name, @Email,@Address,@City,@ZipCode,@Phone,@ContactTypeID,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize
	END
END