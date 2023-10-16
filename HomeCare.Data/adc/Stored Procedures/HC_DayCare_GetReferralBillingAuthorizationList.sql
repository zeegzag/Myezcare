 
  
    
CREATE PROCEDURE [adc].[HC_DayCare_GetReferralBillingAuthorizationList]                 
@PayorID BIGINT=0 ,    
@ReferralID BIGINT=0    
AS          
BEGIN      
 --SELECT distinct RBA.ReferralBillingAuthorizationID, ReferralBillingAuthorizationName=AuthorizationCode    
 -- FROM ReferralBillingAuthorizations RBA    
 -- inner join ReferralTimeSlotDetails RTS on RBA.CareType=RTS.CareTypeId    
 -- WHERE     
 -- PayorID=@PayorID     
 -- AND ReferralID=@ReferralID    
 -- AND RBA.CareType=RTS.CareTypeId    
    
   SELECT     
  DISTINCT        
  ReferralBillingAuthorizationID,        
  RBA.ReferralID,        
  AuthorizationCode AS ReferralBillingAuthorizationName,        
  StartDate,        
  EndDate,        
  AllowedTime,        
  RBA.UnitType,        
  RBA.MaxUnit,        
  RBA.DailyUnitLimit,        
  UnitLimitFrequency,      
  SC.ServiceCode,      
  CareType = DM.Title      
 FROM        
  ReferralBillingAuthorizations RBA        
  INNER JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID        
  INNER JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID      
  INNER JOIN DDMaster DM on RBA.CareType = DM.DDMasterID      
 WHERE        
  RBA.ReferralID = @ReferralID        
  AND RPM.Precedence = 1        
  AND (RBA.StartDate <= RPM.PayorEffectiveEndDate AND RBA.EndDate >= RPM.PayorEffectiveDate)        
  AND (ISNULL(@PayorID, 0) = 0 OR RBA.PayorID = @PayorID)        
  AND RBA.IsDeleted = 0   
           
END  