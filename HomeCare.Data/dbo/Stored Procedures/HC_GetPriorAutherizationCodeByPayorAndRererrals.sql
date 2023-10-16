--  EXEC HC_GetPriorAutherizationCodeByPayorAndRererrals @PayorID=1, @ReferralID=60051    
    
CREATE PROCEDURE [dbo].[HC_GetPriorAutherizationCodeByPayorAndRererrals]      
 @PayorID BIGINT = 0,      
 @ReferralID BIGINT = 0      
AS      
BEGIN      
 SELECT Value = RBA.ReferralBillingAuthorizationID, Name = RBA.AuthorizationCode, D.Title AS CareType, RBA.StartDate, RBA.EndDate, SC.ServiceName, SC.ServiceCode + ISNULL(':' + M.ModifierCode, '') ServiceCode  
 FROM ReferralBillingAuthorizations RBA      
 INNER JOIN Referrals R ON R.ReferralID = RBA.ReferralID    
 LEFT JOIN ReferralBillingAuthorizationServiceCodes RBAS ON RBAS.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID    
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID 
 LEFT JOIN Modifiers M ON M.ModifierID = SC.ModifierID
 LEFT JOIN DDMaster D ON D.DDMasterID = RBA.CareType    
 WHERE RBA.PayorID = @PayorID AND RBA.ReferralID = @ReferralID AND RBA.IsDeleted = 0          
END 