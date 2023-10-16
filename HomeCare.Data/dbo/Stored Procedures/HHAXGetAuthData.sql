CREATE PROCEDURE [dbo].[HHAXGetAuthData]
AS
BEGIN
  SELECT TOP 1
    HHAXClientId [ClientId],
    HHAXClientSecret [ClientSecret]
  FROM dbo.OrganizationSettings
  ORDER BY OrganizationID DESC
END