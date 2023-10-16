USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[GetAuthorizationDetailsByReferralId]    Script Date: 12/8/2020 11:06:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAuthorizationDetailsByReferralId]
@ReferralID bigint
AS                                                            
BEGIN                                                              

	SELECT ReferralBillingAuthorizationID,ReferralID,AuthorizationCode,StartDate,EndDate,ServiceCode FROM ReferralBillingAuthorizations
	INNER JOIN ServiceCodes ON
	ReferralBillingAuthorizations.ServiceCodeID = ServiceCodes.ServiceCodeID
	WHERE ReferralID = @ReferralID and ReferralBillingAuthorizations.IsDeleted = 0

END
GO

