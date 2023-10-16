CREATE PROCEDURE [dbo].[DeleteReferralMonthlySummary]
	@ReferralID bigint,
	@ClientName varchar(100),
	@CreatedBy bigint=0,
	@StartDate Date =null,
	@EndDate Date=null,
	@FacilityID bigint=0, 
	@RegionID bigint=0,            
	@SORTEXPRESSION NVARCHAR(100),   
	@IsDeleted BIGINT = -1,            
	@SORTTYPE NVARCHAR(10),            
	@FROMINDEX INT,                            
	@PAGESIZE INT,
	@ListOfIdsInCsv varchar(300),
	@IsShowList bit,
	@loggedInID BIGINT
AS
BEGIN

	IF(LEN(@ListOfIdsInCsv)>0)
	BEGIN		
			UPDATE ReferralMonthlySummaries SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() 
			WHERE ReferralMonthlySummariesID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))		  	
		END

	IF(@IsShowList=1)
	BEGIN
		EXEC GetReferralMonthlySummaryList @ReferralID, @ClientName, @CreatedBy, @StartDate, @EndDate, @FacilityID, @RegionID,@IsDeleted, @SORTEXPRESSION, @SORTTYPE, @FROMINDEX, @PAGESIZE 
	END
END

