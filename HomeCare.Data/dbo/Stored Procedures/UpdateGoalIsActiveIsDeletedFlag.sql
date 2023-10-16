CREATE PROCEDURE [dbo].[UpdateGoalIsActiveIsDeletedFlag]
	@GoalIDs VARCHAR(MAX),
	@IsActive BIT = 0,
	@IsDeleted BIT = 0,
	@UpdatedBy BIGINT
AS
BEGIN
	SELECT VAL AS GoalID INTO #Goal FROM dbo.GetCSVTable(@GoalIDs)
	UPDATE ReferralTaskMappingsGoal SET IsActive = @IsActive, IsDeleted = @IsDeleted, UpdatedBy = @UpdatedBy, UpdatedDate = GETDATE() WHERE GoalID IN (SELECT GoalID FROM #Goal);
	DROP TABLE #Goal
	SELECT 1;
END
