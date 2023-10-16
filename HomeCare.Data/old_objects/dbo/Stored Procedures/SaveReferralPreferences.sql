-- EXEC SaveReferralPreferences @ReferralID = '1',@Preferences='',@Skills = '28,29', @PreferenceType_Preference = 'Preference', @PreferenceType_Skill = 'Skill', @LoggedInID = '1'  
  
CREATE PROCEDURE [dbo].[SaveReferralPreferences]              
  @ReferralID bigint,          
  @Preferences nvarchar(max),  
  @Skills nvarchar(max),  
  @PreferenceType_Preference VARCHAR(100),  
  @PreferenceType_Skill VARCHAR(100),  
  @LoggedInID BIGINT,  
  @SystemID NVARCHAR(100)=NULL       
AS               
 BEGIN              
       
 -- ADD Referral PREFERENCES  
 DECLARE @TablePrimaryId bigint;                    
 BEGIN TRANSACTION trans              
 BEGIN TRY              
              
  IF (@ReferralID>0)              
  BEGIN      
      
   DECLARE @TempTable TABLE      
  (      
    PreferenceID BIGINT,       
    PreferenceName NVARCHAR(MAX)      
  )      
      
  INSERT INTO @TempTable      
  SELECT       
  (SELECT Result FROM dbo.CSVtoTableWithIdentity(Result,'|') WHERE ReturnId = 1),      
  (SELECT Result FROM dbo.CSVtoTableWithIdentity(Result,'|') WHERE ReturnId = 2)      
  FROM dbo.CSVtoTableWithIdentity(@Preferences,',')              
       
  --SELECT * FROM @TempTable      
      
  INSERT INTO Preferences      
  SELECT T.PreferenceName,@PreferenceType_Preference,0,GETUTCDATE(),@LoggedInID,GETUTCDATE(),@LoggedInID,@SystemID FROM @TempTable T      
  LEFT JOIN Preferences P ON P.PreferenceName=T.PreferenceName      
  WHERE T.PreferenceID=0 AND P.PreferenceName IS NULL      
      
  UPDATE P SET P.IsDeleted=0 FROM Preferences P
  INNER JOIN @TempTable T ON T.PreferenceName=P.PreferenceName    
  WHERE T.PreferenceID=0 AND P.KeyType='Preference'

  UPDATE T SET T.PreferenceID= P.PreferenceID FROM @TempTable T      
  INNER JOIN Preferences P ON P.PreferenceName=T.PreferenceName      
  WHERE T.PreferenceID=0  AND P.KeyType=@PreferenceType_Preference       
      
  --SELECT * FROM @TempTable      
      
      
  INSERT INTO ReferralPreferences       
  SELECT DISTINCT @ReferralID, T.PreferenceID FROM @TempTable T      
  LEFT JOIN ReferralPreferences R ON R.ReferralID=@ReferralID AND R.PreferenceID=T.PreferenceID      
  WHERE R.ReferralPreferenceID IS NULL    
  End              
       
 -- END Referral PREFERENCES  
  
  
  
  
  
  
  
  
  
    
  -- ADD Referral SKILLS  
  
  DECLARE @SkillTempTable TABLE    
  (    
    PreferenceID BIGINT   
  )    
    
  INSERT INTO @SkillTempTable    
  SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@Skills)  
  
  
  
  -- DELETE FROM Exisitng  
   DELETE EP FROM ReferralPreferences EP  
   INNER JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill  
   LEFT JOIN @SkillTempTable ST  ON EP.PreferenceID=ST.PreferenceID AND EP.ReferralID=@ReferralID  
   WHERE  ST.PreferenceID IS NULL  
  
   --SELECT * FROM @SkillTempTable  
   --SELECT * FROM EmployeePreferences EP  
   --LEFT JOIN @SkillTempTable ST  ON EP.PreferenceID=ST.PreferenceID AND EP.EmployeeID=@EmployeeID  
   --LEFT JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill   
   --WHERE  ST.PreferenceID IS NULL  
  
     
     
   -- INSERT NEW  
   INSERT INTO ReferralPreferences  
   SELECT @ReferralID,ST.PreferenceID FROM @SkillTempTable ST    
   LEFT JOIN ReferralPreferences EP ON EP.PreferenceID=ST.PreferenceID AND EP.ReferralID=@ReferralID  
   LEFT JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill  
   WHERE EP.ReferralID IS NULL  
       
  
   --SELECT @EmployeeID,ST.PreferenceID FROM @SkillTempTable ST    
   --LEFT JOIN EmployeePreferences EP ON EP.PreferenceID=ST.PreferenceID AND EP.EmployeeID=1  
   --LEFT JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill  
   --WHERE EP.EmployeeID IS NULL  
  
  -- END Referral SKILLS  
    
  
  
  
  
  
  
  
      
SELECT 1 AS TransactionResultId,@TablePrimaryId AS TablePrimaryId;              
   IF @@TRANCOUNT > 0              
    BEGIN               
     COMMIT TRANSACTION trans               
    END              
              
 END TRY              
 BEGIN CATCH              
  SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;              
  IF @@TRANCOUNT > 0      
   BEGIN               
    ROLLBACK TRANSACTION trans               
   END              
 END CATCH              
END
