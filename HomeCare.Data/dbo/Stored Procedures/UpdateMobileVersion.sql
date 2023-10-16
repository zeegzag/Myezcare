-- EXEC UpdateMobileVersion @AndroidMinimumVersion='1.0.9',@AndroidCurrentVersion='1.1.0',@IOSMinimumVersion='1.0.9',@IOSCurrentVersion='1.1.9', @DBOName='Zarephath_Live_04'   
-- EXEC UpdateMobileVersion @AndroidMinimumVersion='1.1.0',@AndroidCurrentVersion='1.1.0',@IOSMinimumVersion='1.1.0',@IOSCurrentVersion='1.1.0', @DBOName='Zarephath_Live_05'   
  
CREATE PROCEDURE [dbo].[UpdateMobileVersion]    
@AndroidMinimumVersion VARCHAR(100),    
@AndroidCurrentVersion VARCHAR(100),    
@IOSMinimumVersion VARCHAR(100),    
@IOSCurrentVersion VARCHAR(100) ,  
@DBOName VARCHAR(1000)  
   
AS    
BEGIN    
  
  
DECLARE @SQLQuery NVARCHAR(MAX)=N'';  
  
SET @SQLQuery= @SQLQuery + 'UPDATE '+@DBOName+'.dbo.OrganizationSettings SET   
AndroidMinimumVersion='''+@AndroidMinimumVersion+''',  
AndroidCurrentVersion='''+@AndroidCurrentVersion+''',    
IOSMinimumVersion='''+ @IOSMinimumVersion+''',  
IOSCurrentVersion='''+@IOSCurrentVersion +'''  ';  
  
  
PRINT @SQLQuery;  
  
EXEC (@SQLQuery);  
  
--UPDATE Zarephath_Live_05.dbo.OrganizationSettings SET AndroidMinimumVersion=@AndroidMinimumVersion,AndroidCurrentVersion=@AndroidCurrentVersion,    
--IOSMinimumVersion=@IOSMinimumVersion,IOSCurrentVersion=@IOSCurrentVersion  
    
--UPDATE Zarephath_Live_04.dbo.OrganizationSettings SET AndroidMinimumVersion=@AndroidMinimumVersion,AndroidCurrentVersion=@AndroidCurrentVersion,    
--IOSMinimumVersion=@IOSMinimumVersion,IOSCurrentVersion=@IOSCurrentVersion  
END