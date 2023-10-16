CREATE PROCEDURE GetLoginPageDetail    
@DomainName VARCHAR(100)    
AS    
BEGIN  
   
 DECLARE @Query VARCHAR(MAX)  
 DECLARE @AdminDatabaseName VARCHAR(200)  
 SELECT TOP 1 @AdminDatabaseName=AdminDatabaseName FROM OrganizationSettings  
  
 SET @Query =  
 'SELECT iOSAppDownloadURL,AndroidAppDownloadURL FROM '+@AdminDatabaseName+'.dbo.Organizations WHERE DomainName='''+@DomainName+''''  
  
  EXEC(@Query)
    
END