CREATE PROCEDURE [dbo].[HC_GetReferralAuthorizationsByReferralID]            
 @ReferralID BIGINT,            
 @CareTypeID BIGINT        
AS              
BEGIN    
  
   
 SELECT            
  DISTINCT            
  ReferralBillingAuthorizationID,                
  RBA.ReferralID,                  
  AuthorizationCode,                  
  RBA.StartDate,                 
  RBA.EndDate,            
  --RPM.PayorEffectiveEndDate,        
  AllowedTime,                  
  RBA.UnitType,                  
  RBA.MaxUnit,                  
  RBA.DailyUnitLimit,                  
  UnitLimitFrequency,            
  ServiceCode = SC.ServiceCode + CASE WHEN M.ModifierCode IS NULL THEN '' ELSE ' : '+M.ModifierCode END,            
  CareType = DM.Title,        
  DM.DDMasterID as CareTypeID        
         
 FROM            
  ReferralBillingAuthorizations RBA            
  INNER JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID            
  INNER JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID          
  LEFT JOIN Modifiers M ON CONVERT(NVARCHAR(MAX), M.ModifierID) = SC.ModifierID         
  INNER JOIN DDMaster DM on RBA.CareType = DM.DDMasterID          
 WHERE            
  RBA.ReferralID = @ReferralID            
  AND RPM.Precedence = 1            
  AND (RBA.StartDate <= RPM.PayorEffectiveEndDate AND RBA.EndDate >= RPM.PayorEffectiveDate)            
  AND (ISNULL(@CareTypeID, 0) = 0 OR RBA.CareType = @CareTypeID)         
          
  AND RBA.IsDeleted = 0         
  
UNION  
select 0,0,null,null,null,null,null,null,null,null,null,null,0  
END 