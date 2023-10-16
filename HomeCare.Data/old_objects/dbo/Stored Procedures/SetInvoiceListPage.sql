CREATE PROCEDURE [dbo].[SetInvoiceListPage]
AS
BEGIN

  SELECT
    ReferralID,
    ReferralName = dbo.GetGeneralNameFormat(FirstName, LastName)
  FROM Referrals
  WHERE IsDeleted = 0
  ORDER BY LastName ASC

  SELECT
    dds.DDMasterID AS CareTypeID,
    dds.Title AS CareType
  FROM DDMaster dds
  INNER JOIN lu_DDMasterTypes AS luddm
    ON dds.ItemType = luddm.DDMasterTypeID
  WHERE dds.IsDeleted = 0
  AND luddm.Name = 'Care Type'

END