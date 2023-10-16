
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAuthorizationDetailsByReferralId]
@ReferralID bigint
AS                                                            
BEGIN                                                              

	SELECT ReferralBillingAuthorizationID,ReferralID,CONCAT(AuthorizationCode,'-',ServiceCode) AS AuthorizationCode,StartDate,EndDate,ServiceCode,PayorID,RBA.CareType CareTypeID FROM ReferralBillingAuthorizations RBA
	INNER JOIN ServiceCodes ON
	RBA.ServiceCodeID = ServiceCodes.ServiceCodeID
	WHERE ReferralID = @ReferralID and RBA.IsDeleted = 0

END
GO

