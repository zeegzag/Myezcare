-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 5 June, 2020
-- Description:	Get Prior autherization code by payor id and referral id
-- =============================================
CREATE PROCEDURE [dbo].[HC_GetPriorAutherizationCodeByPayorAndRererrals]
	@PayorID BIGINT = 0,
	@ReferralID BIGINT = 0
AS
BEGIN
	SELECT Value = RBA.ReferralBillingAuthorizationID, Name = RBA.AuthorizationCode 
	FROM ReferralBillingAuthorizations RBA
	WHERE RBA.PayorID = @PayorID AND RBA.ReferralID = @ReferralID AND RBA.IsDeleted = 0
END
