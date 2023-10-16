CREATE PROCEDURE [dbo].[UpdateReferralProfileImgPath]  
 @ReferralID BIGINT,  
 @ProfileImgPath VARCHAR(300)  
      
AS                              
BEGIN    
 Update Referrals Set ProfileImagePath=@ProfileImgPath WHERE ReferralID=@ReferralID  
END