CREATE PROCEDURE [dbo].[GetReferralTaskMappingsGoal]
	@ReferralId BIGINT
AS
BEGIN
	SELECT Goal FROM ReferralTaskMappingsGoal WHERE ReferralID = @ReferralId
END

--exec [dbo].[GetReferralTaskMappingsGoal] 10163