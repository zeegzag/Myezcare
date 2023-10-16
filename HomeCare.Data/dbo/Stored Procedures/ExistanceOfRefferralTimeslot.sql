      
CREATE PROCEDURE [dbo].[ExistanceOfRefferralTimeslot]       
@ReferralBillingAuthorizationID BIGINT,      
@ReferralID BIGINT       
      
      
AS          
BEGIN           
      
SELECT 1 AS TransactionResultId,          
          
      
ReferralTimeSlotMasterID as TablePrimaryId from ReferralTimeSlotMaster       
where ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID       
and ReferralID=@ReferralID       
and IsDeleted=0      
      
END      
      
--exec [dbo].[ExistanceOfRefferralTimeslot]  162,178,0