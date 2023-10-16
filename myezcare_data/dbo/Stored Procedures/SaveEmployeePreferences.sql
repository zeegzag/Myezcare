-- EXEC SaveEmployeePreferences @EmployeeID = '1', @Preferences = '34|P_SMKOKING,35|P_CRICKET', @Skills = '30,28', @PreferenceType_Preference = 'Preference', @PreferenceType_Skill = 'Skill', @LoggedInID = '1'  
CREATE PROCEDURE [dbo].[SaveEmployeePreferences]            
  @EmployeeID bigint,        
  @Preferences nvarchar(max),  
  @Skills nvarchar(max),  
  @PreferenceType_Preference VARCHAR(100),  
  @PreferenceType_Skill VARCHAR(100),  
  @LoggedInID BIGINT,  
  @SystemID NVARCHAR(100)=NULL  
AS             
 BEGIN            
     
 DECLARE @TablePrimaryId bigint;                  
 BEGIN TRANSACTION trans            
 BEGIN TRY            
         
      
  -- ADD EMPLOYEE PREFERENCES         
  IF (@EmployeeID>0)    
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
    
  UPDATE T SET T.PreferenceID=P.PreferenceID FROM @TempTable T    
  INNER JOIN Preferences P ON P.PreferenceName=T.PreferenceName    
  WHERE T.PreferenceID=0 AND P.KeyType=@PreferenceType_Preference    
    
  --SELECT * FROM @TempTable    
    
  INSERT INTO EmployeePreferences     
  SELECT DISTINCT @EmployeeID, T.PreferenceID FROM @TempTable T    
  LEFT JOIN EmployeePreferences E ON E.EmployeeID=@EmployeeID AND E.PreferenceID=T.PreferenceID    
  WHERE E.EmployeePreferenceID IS NULL   
      
  -- SELECT * FROM EmployeePreferences  
-- EXEC SaveEmployeePreferences @EmployeeID = '1', @Preferences = '34|P_SMKOKING,35|P_CRICKET', @Skills = '30,28', @PreferenceType_Preference = 'Preference', @PreferenceType_Skill = 'Skill', @LoggedInID = '1'  
  
  
    
  End            
  -- END EMPLOYEE PREFERENCES   
  
  
  
  -- ADD EMPLOYEE SKILLS  
  
  DECLARE @SkillTempTable TABLE    
  (    
    PreferenceID BIGINT   
  )    
    
  INSERT INTO @SkillTempTable    
  SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@Skills)  
  
  
    
  
  
  -- DELETE FROM Exisitng  
   DELETE EP FROM EmployeePreferences EP  
   INNER JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill  
   LEFT JOIN @SkillTempTable ST  ON EP.PreferenceID=ST.PreferenceID AND EP.EmployeeID=@EmployeeID  
   WHERE  ST.PreferenceID IS NULL  
  
   --SELECT * FROM @SkillTempTable  
   --SELECT * FROM EmployeePreferences EP  
   --LEFT JOIN @SkillTempTable ST  ON EP.PreferenceID=ST.PreferenceID AND EP.EmployeeID=@EmployeeID  
   --LEFT JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill   
   --WHERE  ST.PreferenceID IS NULL  
  
     
     
   -- INSERT NEW  
   INSERT INTO EmployeePreferences  
   SELECT @EmployeeID,ST.PreferenceID FROM @SkillTempTable ST    
   LEFT JOIN EmployeePreferences EP ON EP.PreferenceID=ST.PreferenceID AND EP.EmployeeID=@EmployeeID  
   LEFT JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill  
   WHERE EP.EmployeeID IS NULL  
       
  
   --SELECT @EmployeeID,ST.PreferenceID FROM @SkillTempTable ST    
   --LEFT JOIN EmployeePreferences EP ON EP.PreferenceID=ST.PreferenceID AND EP.EmployeeID=1  
   --LEFT JOIN Preferences P ON P.PreferenceID=EP.PreferenceID AND P.KeyType=@PreferenceType_Skill  
   --WHERE EP.EmployeeID IS NULL  
  
  -- END EMPLOYEE SKILLS  
    
  
  
  
  
  
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
