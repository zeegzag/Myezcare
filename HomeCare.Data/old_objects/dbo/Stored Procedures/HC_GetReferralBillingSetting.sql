
-- exec [HC_GetReferralBillingSetting] 14232    
CREATE PROCEDURE [dbo].[HC_GetReferralBillingSetting]                
@ReferralID BIGINT     
AS                 
BEGIN                
 select * from ReferralBillingSettings where ReferralID=@ReferralID       
END 

