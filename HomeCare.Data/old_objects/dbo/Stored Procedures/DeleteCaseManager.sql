
CREATE PROCEDURE [dbo].[DeleteCaseManager]
	@Name varchar(100)=NULL,
	@Email varchar(10)=NULL,
	@Phone VARCHAR(20) = NULL,
	@AgencyID BIGINT = 0,
	@AgencyLocationID BIGINT = 0,
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
		--IF EXISTS (SELECT * FROM Referrals WHERE CaseManagerID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV)))
		--BEGIN 
		--	SELECT NULL;
		--	RETURN NULL;
		--END
		--ELSE
			UPDATE CaseManagers SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE CaseManagerID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))		  	
		END

	IF(@IsShowList=1)
	BEGIN
		EXEC GetCaseManagerList @Name, @Email,@Phone,@AgencyID,@AgencyLocationID,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize
	END
END

