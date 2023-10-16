CREATE PROCEDURE [dbo].[SavePreference]     
 @PreferenceID BIGINT,        
 @PreferenceName NVARCHAR(1000),
 @KeyType   NVARCHAR(100),
 @loggedInUserId BIGINT,
 @SystemID NVARCHAR(100)
AS    
BEGIN    
 
     
 IF EXISTS (SELECT 1 FROM Preferences WHERE PreferenceName=@PreferenceName AND KeyType=@KeyType AND PreferenceID != @PreferenceID)          
 BEGIN  
 SELECT -1 RETURN;    
 END  
      
        
          
 IF(@PreferenceID=0)        
 BEGIN        
   INSERT INTO Preferences        
   SELECT @PreferenceName,@KeyType,0, GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@loggedInUserId,@SystemID   
           
 END        
 ELSE        
 BEGIN        
   UPDATE Preferences         
   SET              
      PreferenceName=@PreferenceName,        
      KeyType=@KeyType,
      UpdatedBy=@loggedInUserId,        
      UpdatedDate=GETUTCDATE()      
   WHERE PreferenceID=@PreferenceID;        
 END        
    

 SELECT 1; RETURN;    
    
 
 
   
END