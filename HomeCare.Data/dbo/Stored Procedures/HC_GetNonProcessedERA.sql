  
CREATE PROCEDURE HC_GetNonProcessedERA           
AS           
BEGIN           
  
  
DECLARE @TaxID NVARCHAR(MAX)='';  
DECLARE @NPI NVARCHAR(MAX)='';  
SELECT @TaxID = BillingProvider_REF02_ReferenceIdentification, @NPI = Submitter_NM109_IdCode from OrganizationSettings    
  
    
SELECT LE.EraID FROM LatestERAs LE     
--INNER JOIN OrganizationSettings OS ON OS.Submitter_NM109_IdCode = LE.ProviderNPI    
LEFT JOIN Upload835Files UF ON UF.EraID = LE.EraID    
WHERE UF.Upload835FileID IS NULL  AND  LE.ProviderNPI= @NPI AND LE.ProviderTaxID =  @TaxID        
ORDER BY LE.CreatedDate ASC          
    
END   