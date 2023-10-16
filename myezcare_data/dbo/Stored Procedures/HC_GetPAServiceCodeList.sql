-- EXEC  HC_GetPAServiceCodeList '39'
CREATE PROCEDURE [dbo].[HC_GetPAServiceCodeList]
@ReferralBillingAuthorizationID BIGINT                      
AS                                  
BEGIN                                      
 
 -- SELECT * FROM PayorServiceCodeMapping

 SELECT  RBS.ReferralBillingAuthorizationServiceCodeID, RBS.ReferralBillingAuthorizationID, RBA.ReferralID, S.ServiceCodeID, --S.ServiceCode,
 ServiceCode = S.ServiceCode +              
    CASE WHEN (S.ModifierID IS NULL OR S.ModifierID='' ) THEN '' ELSE ' -'+                    
    STUFF(                        
    (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                        
    FROM Modifiers M  where M.ModifierID IN (SELECT val FROM GetCSVTable(S.ModifierID)) AND M.IsDeleted=0                       
    FOR XML PATH ('')), 1, 1, '')                    
    END ,
  
 PSM.UnitType, PSM.PerUnitQuantity, RBS.DailyUnitLimit, RBS.MaxUnitLimit, RBS.IsDeleted
 FROM ReferralBillingAuthorizationServiceCodes RBS
 INNER JOIN ReferralBillingAuthorizations RBA ON RBA.ReferralBillingAuthorizationID=RBS.ReferralBillingAuthorizationID 
 INNER JOIN PayorServiceCodeMapping PSM ON PSM.ServiceCodeID=RBS.ServiceCodeID  AND PSM.PayorID = RBA.PayorID
 INNER JOIN ServiceCodes S ON S.ServiceCodeID =PSM.ServiceCodeID
 LEFT JOIN Modifiers M ON M.ModifierID=S.ModifierID AND M.IsDeleted=0 
 WHERE  RBS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID    
                             
END
