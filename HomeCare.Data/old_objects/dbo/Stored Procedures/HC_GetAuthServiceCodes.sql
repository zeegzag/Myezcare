CREATE PROCEDURE [dbo].[HC_GetAuthServiceCodes]
@ReferralBillingAuthorizationID BIGINT
AS
BEGIN


SELECT SC.ServiceCodeID,SC.ServiceName, SC.ServiceCode,
 Modifiers= CASE WHEN SC.ModifierID is null THEN ''        
			 ELSE         
			 STUFF( (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)            
			 FROM Modifiers M            
			 WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(RBS.ModifierID))            
			 FOR XML PATH ('')), 1, 1, '')        
			END,
  DM.Title AS Taxonomy,
  RBS.Rate,
  DM2.Title AS RevenueCode,
  RBS.PerUnitQuantity AS PerUnitValue,
  RBS.RoundUpUnit AS RoundUpMinutes,
  RBS.MaxUnit,
  RBS.DailyUnitLimit AS DailyUnit

FROM ReferralBillingAuthorizations RBS
INNER JOIN ServiceCodes SC on SC.ServiceCodeID=RBS.ServiceCodeID   
LEFT JOIN DDMaster DM ON DM.DDMasterID = RBS.TaxonomyID
LEFT JOIN DDMaster DM2 ON DM2.DDMasterID = RBS.RevenueCode
WHERE RBS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RBS.IsDeleted=0

END
