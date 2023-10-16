CREATE PROCEDURE [dbo].[GetAllOrganizationList]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	Select OrganizationID,DisplayName,CompanyName FROM dbo.Organizations  NOLOCK WHERE IsActive=1 and EndDate> getdate()	  order by DisplayName
END