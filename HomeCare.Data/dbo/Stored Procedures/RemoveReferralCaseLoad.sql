-- EXEC RemoveReferralCaseLoad @ReferralID =1951, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'                        
CREATE PROCEDURE [dbo].[RemoveReferralCaseLoad]
	@ListOfIdsInCsv VARCHAR(MAX),
	@CaseLoadType VARCHAR(100),
	@PermanenetCaseLoadType VARCHAR(100),
	@IsShowList BIT = 1,
	@LoggedInID BIGINT,
	@ReferralID BIGINT = 0,
	@StartDate DATE = NULL,
	@EndDate DATE = NULL,
	@IsDeleted int=-1,
	@SortExpression NVARCHAR(100),                          
	@SortType NVARCHAR(10),
	@FromIndex INT,
	@PageSize INT
AS
BEGIN
	IF(LEN(@ListOfIdsInCsv) > 0)
	BEGIN
		
		IF(@CaseLoadType = @PermanenetCaseLoadType)
		BEGIN
			UPDATE 
				ReferralCaseLoads
			SET
				EndDate = GETDATE(),
				UpdatedBy = @LoggedInID,
				UpdatedDate = GETDATE()
			WHERE
				ReferralCaseLoadID IN (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))				
		END
		
		IF(@CaseLoadType != @PermanenetCaseLoadType)
		BEGIN		
			DELETE FROM
				ReferralCaseLoads
			WHERE
				ReferralCaseLoadID IN (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))	
		END
		
	END

	IF(@IsShowList=1)
	BEGIN
		EXEC GetReferralCaseLoadList @ReferralID,@StartDate,@EndDate,@IsDeleted,@SortExpression,@SortType,@FromIndex,@PageSize
	END                   
END