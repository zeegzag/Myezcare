
-- =============================================
-- Author:		<Author,,Balwinder>
-- Create date: <Create Date,,07-05-2020>
-- Description:	<Description,,Get all organization list>
-- =============================================
CREATE PROCEDURE GetAllOrganizationList
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	Select OrganizationID,DisplayName,CompanyName FROM dbo.Organizations  NOLOCK WHERE IsDeleted !=1
END
GO
