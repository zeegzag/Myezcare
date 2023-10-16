CREATE PROCEDURE [dbo].[GetReferralTaskMappingsGoal]
	@ReferralId BIGINT
AS
BEGIN
	SELECT GoalID,Goal,IsActive,IsDeleted FROM ReferralTaskMappingsGoal WHERE ReferralID = @ReferralId AND ISNULL(IsDeleted, 0) = 0 ORDER BY GoalID desc
END
