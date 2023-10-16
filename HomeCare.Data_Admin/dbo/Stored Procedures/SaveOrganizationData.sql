--EXEC SaveOrganizationData @OrganizationID = '5', @OrganizationType = 'DayCare', @DisplayName = 'demo', @CompanyName = 'asapcare', @DomainName = 'asapcare', @StartDate = '9/1/2018 12:00:00 AM', @EndDate = '', @DBName = 'Zarephath_Live_04', @DBPassword = 'JitYad1!', @DBProviderName = 'System.Data.SqlClient', @DBServer = '192.168.1.32\\\\express17', @DBUserName = 'jyadav'

CREATE PROCEDURE [dbo].[SaveOrganizationData]        
 @OrganizationID BIGINT,  
 @OrganizationType NVARCHAR(200),   
 @DisplayName NVARCHAR(200),   
 @CompanyName NVARCHAR(200),   
 @DomainName NVARCHAR(200),   
 @StartDate DateTime,  
 @EndDate DateTime=null,  
 @DBName NVARCHAR(200),   
 @DBPassword NVARCHAR(200),   
 @DBProviderName NVARCHAR(200),   
 @DBServer NVARCHAR(200),   
 @DBUserName NVARCHAR(200)  
AS                          
BEGIN                          
                       
                 
IF NOT EXISTS (SELECT TOP 1 OrganizationID FROM Organizations WHERE ((DomainName=@DomainName or CompanyName=@CompanyName) AND OrganizationID != @OrganizationID))    
BEGIN                    
 IF(@OrganizationID=0)                          
 BEGIN                          
  INSERT INTO Organizations                          
  (DisplayName,CompanyName,DomainName,DBServer,DBName,DBUserName,DBPassword,DBProviderName,StartDate,EndDate,IsActive,OrganizationType)  
  VALUES                          
  (@DisplayName,@CompanyName,@DomainName,@DBServer,@DBName,@DBUserName,@DBPassword,@DBProviderName,@StartDate,@EndDate,1,@OrganizationType);  
 END     
 ELSE                          
 BEGIN                          
  UPDATE Organizations                           
  SET   
  DisplayName=@DisplayName,  
  CompanyName=@CompanyName,  
  DomainName=@DomainName,  
  DBServer=@DBServer,  
  DBName=@DBName,  
  DBUserName=@DBUserName,  
  DBPassword=@DBPassword,  
  DBProviderName=@DBProviderName,  
  StartDate=@StartDate,  
  EndDate=@EndDate,  
  OrganizationType=@OrganizationType                             
  WHERE OrganizationID=@OrganizationID;                          
 END  
    
 SELECT 1 As TransactionResultId;                      
END  
ELSE  
BEGIN  
  SELECT -1 As TransactionResultId;  
  RETURN;  
END  
  
                    
                        
        
                
END