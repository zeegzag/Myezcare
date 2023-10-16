CREATE PROCEDURE [dbo].[AddOrUpdateReferralTaskMappingsGoal] 
	@GoalID BIGINT,
	@ReferralId BIGINT,
	@Goal NVARCHAR(MAX),
	@IsActive BIT = 0,
	@IsDeleted BIT = 0,
	@CreatedBy BIGINT
AS
BEGIN
IF @GoalID > 0 
	BEGIN
		UPDATE ReferralTaskMappingsGoal SET Goal=@Goal, UpdatedBy = @CreatedBy, UpdatedDate = GETDATE() 
		WHERE GoalID = @GoalID;
		SELECT 2;
	END
ELSE
	BEGIN 
		INSERT INTO ReferralTaskMappingsGoal (ReferralID, Goal, IsActive,IsDeleted, CreatedBy, CreatedDate ) 
		VALUES( @ReferralId,@Goal, @IsActive, @IsDeleted, @CreatedBy, GETDATE());
		SELECT 1;
	END
END
