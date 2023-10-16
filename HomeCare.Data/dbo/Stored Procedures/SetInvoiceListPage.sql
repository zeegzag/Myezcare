CREATE PROCEDURE [dbo].[SetInvoiceListPage]  
AS  
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  SELECT  
    ReferralID,  
    ReferralName = dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) 
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