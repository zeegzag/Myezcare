CREATE PROCEDURE [dbo].[GetReferralHistory]
    @ReferralID BIGINT
AS
BEGIN

	SELECT 
		RH.*,
        RS.ReferralSourceName
	FROM
        [dbo].[ReferralHistory] RH
    LEFT JOIN [dbo].[ReferralSources] RS
        ON RS.ReferralSourceID = RH.ReferralSourceID
	WHERE
        RH.ReferralID = @ReferralID
        AND RH.IsDeleted = 0

END
GO