CREATE PROC [dbo].[HC_GetOrgConnectionString]
(
	@OrgId INT
)
AS
begin
	declare @connectionStr nvarchar(2000) = ''

	SELECT 
		@connectionStr = 'Server='+DBServer+';Database='+DBName+';User ID='+DBUserName+';Password='+DBPassword+''
	FROM dbo.Organizations
	WHERE OrganizationID = @orgId

	SELECT @connectionStr as ConnectionString 

end