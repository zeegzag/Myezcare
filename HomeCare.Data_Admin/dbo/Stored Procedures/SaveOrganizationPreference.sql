CREATE PROCEDURE [dbo].[SaveOrganizationPreference]    
  -- Add the parameters for the stored procedure here                                  
 @OrganizationID   BIGINT,                                  
 @DateFormat    NVARCHAR(max),                
 @Currency    NVARCHAR(max),                        
 @Region     NVARCHAR(max),                    
 @Language    NVARCHAR(max),                             
 @NameDisplayFormat  NVARCHAR(max),                
 @CssFilePath   NVARCHAR(max),                  
 @loggedInUserId   BIGINT,                            
 @SystemID    VARCHAR(max),
 @WeekStartDay VARCHAR (MAX) = NULL
AS                                  
BEGIN                                  
                               
                
 IF(NOT EXISTS(Select * from OrganizationPreference Where OrganizationID = @OrganizationID))    
  BEGIN                                  
    
   INSERT INTO OrganizationPreference                                  
   (OrganizationID, [DateFormat], Currency, Region, Language, NameDisplayFormat, CssFilePath, [WeekStartDay], CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, SystemID)    
   VALUES                                  
   (@OrganizationID, @DateFormat, @Currency, @Region, @Language, @NameDisplayFormat, @CssFilePath, @WeekStartDay, GETUTCDATE(), @loggedInUserId,GETUTCDATE(),@loggedInUserId,@SystemID);                
                      
  END                                  
 ELSE                                  
  BEGIN                                  
    UPDATE OrganizationPreference                                   
    SET                                        
     [DateFormat]  = @DateFormat,                                  
     Currency   = @Currency,                        
     Region    = @Region,                    
     [Language]   = @Language,                
     NameDisplayFormat = @NameDisplayFormat,                
     CssFilePath  = @CssFilePath,                
     [WeekStartDay] = @WeekStartDay,
     UpdatedBy   = @loggedInUserId,                                  
     UpdatedDate  = GETUTCDATE(),                                  
     SystemID   = @SystemID    
         
    WHERE OrganizationID = @OrganizationID;                                  
   END                                  
                              
                          
 SELECT 1; RETURN;                              
                              
                              
END