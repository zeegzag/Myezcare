CREATE PROCEDURE [dbo].[SaveOrganizationFormName]
@OrganizationFormID BIGINT,
@OrganizationFriendlyFormName NVARCHAR(MAX),
@LoggedInUserId BIGINT
AS
BEGIN
	UPDATE OrganizationForms SET OrganizationFriendlyFormName=@OrganizationFriendlyFormName,UpdatedDate=GETDATE(),UpdatedBy=@LoggedInUserId
	WHERE OrganizationFormID=@OrganizationFormID
END