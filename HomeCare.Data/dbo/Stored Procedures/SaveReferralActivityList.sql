CREATE PROCEDURE [dbo].[SaveReferralActivityList]
	@Type_ReferralActivityList [dbo].[Type_ReferralActivity] READONLY
	,@ReferralId NVARCHAR(MAX) = NULL,
	@Month NVARCHAR(200),@Year int
AS
BEGIN
	
	DECLARE @ReferralActivityMasterId BIGINT
	
	--SELECT @ReferralActivityMasterId=ReferralActivityMasterId FROM ReferralActivityMaster
	--WHERE [Month]=@Month AND [YEAR]=@Year AND ReferralId=@ReferralId
		
	DELETE FROM [ReferralActivityMaster] WHERE ReferralActivityMasterId=@ReferralActivityMasterId
	DELETE FROM [ReferralActivityList] WHERE ReferralActivityMasterId=@ReferralActivityMasterId
	DELETE FROM [ReferralActivityNotes] WHERE ReferralActivityMasterId=@ReferralActivityMasterId

	INSERT INTO [dbo].[ReferralActivityMaster]
	( [ReferralId], [Month], [Year], [CreatedBy], [CreatedDate])
	SELECT  @ReferralId, @Month, @Year, null, GETDATE()

	SELECT @ReferralActivityMasterId=Max(ReferralActivityMasterId) FROM ReferralActivityMaster

	INSERT INTO ReferralActivityList (
		  [ReferralActivityCategoryId],ReferralActivityMasterId,
		[Day1], [Day2], [Day3], 
		[Day4], [Day5], [Day6], [Day7], [Day8], [Day9], [Day10], [Day11], [Day12], [Day13], [Day14], [Day15], [Day16],
		[Day17], [Day18], [Day19], [Day20], [Day21], [Day22],[Day23], [Day24], [Day25], [Day26], [Day27], [Day28], [Day29], 
		[Day30], [Day31]
	)
	SELECT 
	[ReferralActivityCategoryId],IDENT_CURRENT('ReferralActivityMaster'),  [Day1], [Day2], [Day3], [Day4], [Day5], [Day6], [Day7], [Day8], [Day9],
	[Day10], [Day11], [Day12], [Day13], [Day14], [Day15], [Day16], [Day17], [Day18], [Day19], [Day20], [Day21]
	,[Day22], [Day23], [Day24], [Day25], [Day26], [Day27], [Day28], [Day29], [Day30], [Day31]
	FROM @Type_ReferralActivityList

END
