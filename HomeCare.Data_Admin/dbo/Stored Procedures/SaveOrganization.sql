--EXEC SaveOrganizationData @OrganizationID = '5', @OrganizationType = 'DayCare', @DisplayName = 'demo', @CompanyName = 'asapcare', @DomainName = 'asapcare', @StartDate = '9/1/2018 12:00:00 AM', @EndDate = '', @DBName = 'Zarephath_Live_04', @DBPassword = 'JitYad1!', @DBProviderName = 'System.Data.SqlClient', @DBServer = '192.168.1.32\\\\express17', @DBUserName = 'jyadav'  

CREATE PROCEDURE [dbo].[SaveOrganization]
 @OrganizationTypeID BIGINT,     
 @OrganizationStatusID BIGINT,
 @DisplayName NVARCHAR(200),     
 @CompanyName NVARCHAR(200),     
 @DomainName NVARCHAR(200),     
 @Email NVARCHAR(100),     
 @Mobile NVARCHAR(20),     
 @WorkPhone NVARCHAR(20),     
 @DefaultEsignTerms NVARCHAR(MAX),
 @LoggedInID BIGINT,
 @SystemID NVARCHAR(100)    
AS                            
BEGIN                            
                         
                   
IF NOT EXISTS (SELECT TOP 1 OrganizationID FROM Organizations WHERE (CompanyName=@CompanyName))      
BEGIN    


IF(@DomainName='') SET @DomainName=NULL;
IF(@DisplayName='') SET @DisplayName=NULL;
IF(@DefaultEsignTerms='') SET @DefaultEsignTerms=NULL;


                  
  INSERT INTO Organizations                            
  (DisplayName,CompanyName,DomainName,Email,Mobile,WorkPhone,DefaultEsignTerms,OrganizationTypeID,OrganizationStatusID,
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted,IsActive)    
  VALUES                            
  (@DisplayName,@CompanyName,@DomainName,@Email,@Mobile,@WorkPhone,@DefaultEsignTerms,@OrganizationTypeID,@OrganizationStatusID,
  GETUTCDATE(),@LoggedInID,GETUTCDATE(),@LoggedInID,@SystemID,0,0);    
      
 SELECT 1 As TransactionResultId;                        
END    
ELSE    
BEGIN    
  SELECT -1 As TransactionResultId;    
  RETURN;    
END    
    
                      
                          
          
                  
END