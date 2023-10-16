CREATE PROCEDURE [dbo].[SandataGetAccount]
AS
BEGIN
  SELECT TOP 1
    SandataBusinessEntityID [BusinessEntityID],
    SandataBusinessEntityMedicaidIdentifier [BusinessEntityMedicaidIdentifier],
    SandataUserID [UserID],
    SandataPassword [Password],
    SandataIsProduction [IsProduction]
  FROM dbo.OrganizationSettings
  ORDER BY OrganizationID DESC
END