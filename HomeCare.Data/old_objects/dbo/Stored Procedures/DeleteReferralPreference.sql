CREATE PROCEDURE [dbo].[DeleteReferralPreference]  
@ReferralPreferenceID BIGINT  
AS  
BEGIN   
  
DELETE FROM ReferralPreferences WHERE ReferralPreferenceID=@ReferralPreferenceID  
  
END
