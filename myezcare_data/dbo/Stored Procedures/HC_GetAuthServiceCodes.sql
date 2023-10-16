CREATE PROCEDURE [dbo].[HC_GetAuthServiceCodes]
@ReferralBillingAuthorizationID BIGINT
AS
BEGIN


SELECT SC.ServiceCodeID,SC.ServiceName, SC.ServiceCode,
 Modifiers= CASE WHEN SC.ModifierID is null THEN ''        
			 ELSE         
			 STUFF( (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)            
			 FROM Modifiers M            
			 WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))            
			 FOR XML PATH ('')), 1, 1, '')        
			END


FROM ReferralBillingAuthorizationServiceCodes RBS
INNER JOIN ServiceCodes SC on SC.ServiceCodeID=RBS.ServiceCodeID           
WHERE RBS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RBS.IsDeleted=0

END
