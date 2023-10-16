-- HC_GetReferralAuthorizationsByReferralID 14147
CREATE PROCEDURE [dbo].[HC_GetReferralAuthorizationsByReferralID]
	@ReferralID BIGINT
AS
BEGIN
	SELECT
		DISTINCT
		ReferralBillingAuthorizationID,
		RBA.ReferralID,
		AuthorizationCode,
		StartDate,
		EndDate,
		AllowedTime
	FROM
		ReferralBillingAuthorizations RBA
		INNER JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID
	WHERE
		RBA.ReferralID = @ReferralID
		AND RPM.Precedence = 1
		AND (RBA.StartDate <= RPM.PayorEffectiveEndDate AND RBA.EndDate >= RPM.PayorEffectiveDate)
		AND RBA.IsDeleted = 0
END
