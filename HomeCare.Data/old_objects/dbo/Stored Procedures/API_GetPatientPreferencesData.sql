CREATE PROCEDURE [dbo].[API_GetPatientPreferencesData]        
    @ReferralID INT        
AS                                           
BEGIN      
       SELECT       
  RP.[ReferralID],    
        P.[PreferenceName],  
  P.[KeyType]  
    FROM       
        [dbo].[ReferralPreferences] RP      
    LEFT JOIN [dbo].[Preferences] P      
        ON P.[PreferenceID] = RP.[PreferenceID]     
    WHERE      
        RP.[ReferralID] = @ReferralID      
      
END 