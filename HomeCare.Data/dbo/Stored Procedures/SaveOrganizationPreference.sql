CREATE PROCEDURE [dbo].[SaveOrganizationPreference]
	 -- Add the parameters for the stored procedure here                              
	@OrganizationID			BIGINT,                              
	@DateFormat				NVARCHAR(50),            
	@Currency				NVARCHAR(50),                    
	@Region					NVARCHAR(100),                
	@Language				NVARCHAR(100),                         
	@NameDisplayFormat		NVARCHAR(50),            
	@CssFilePath			NVARCHAR(10),              
	@loggedInUserId			BIGINT,                        
	@SystemID				VARCHAR(100)                            
AS                              
BEGIN                              
                           
            
	IF(NOT EXISTS(Select * from OrganizationPreference Where OrganizationID = @OrganizationID))
		BEGIN                              

			INSERT INTO OrganizationPreference                              
			(OrganizationID, DateFormat, Currency, Region, Language, NameDisplayFormat, CssFilePath, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, SystemID)
			VALUES                              
			(@OrganizationID, @DateFormat, @Currency, @Region, @Language, @NameDisplayFormat, @CssFilePath,GETUTCDATE(), @loggedInUserId,GETUTCDATE(),@loggedInUserId,@SystemID);            
                  
		END                              
	ELSE                              
		BEGIN                              
			 UPDATE OrganizationPreference                               
			 SET                                    
				 [DateFormat]		= @DateFormat,                              
				 Currency			= @Currency,                    
				 Region				= @Region,                
				 [Language]			= @Language,            
				 NameDisplayFormat	= @NameDisplayFormat,            
				 CssFilePath		= @CssFilePath,            
				 
				 UpdatedBy			= @loggedInUserId,                              
				 UpdatedDate		= GETUTCDATE(),                              
				 SystemID			= @SystemID
				 
			 WHERE OrganizationID	= @OrganizationID;                              
		 END                              
                          
                      
 SELECT 1; RETURN;                          
                          
                          
END