-- =============================================
-- Author:		Kundan
-- Create date: 9/12/2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetWizardStatus]
	@OrganizationID bigint
AS
BEGIN

	SELECT ORG.OnboardingWizardLogID, ORG.Menu, ORG.IsCompleted 
	FROM OnboardingWizardLog ORG 
	WHERE ORG.OrganizationID = @OrganizationID AND ORG.IsDeleted = 0

END
GO