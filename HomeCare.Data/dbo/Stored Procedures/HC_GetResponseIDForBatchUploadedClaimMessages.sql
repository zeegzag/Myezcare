CREATE PROCEDURE HC_GetResponseIDForBatchUploadedClaimMessages
--@BatchID BIGINT,      
--@ClaimID NVARCHAR(MAX)      
AS      
BEGIN      
    
DECLARE @SQL NVARCHAR(MAX)='';      
    
DECLARE @AdminDatabaseName NVARCHAR(MAX)='';    
SELECT TOP 1 @AdminDatabaseName=AdminDatabaseName FROM OrganizationSettings    
    
SET @SQL =     
'DECLARE @LastResponseID NVARCHAR(MAX)='''';     
SELECT @LastResponseID=MAX(CONVERT(BIGINT,ISNULL(LastResponseID,0))) FROM '+@AdminDatabaseName+'.dbo.BatchUploadedClaimMessages    
    
DECLARE @CreatedDate DATETIME='''';      
SELECT @CreatedDate=CreatedDate FROM '+@AdminDatabaseName+'.dbo.BatchUploadedClaimMessages    
      
  IF ( @CreatedDate=''1900-01-01 00:00:00.000''  OR DATEDIFF(hh, @CreatedDate,GETDATE() ) >= 12 )    
  SELECT ServiceCallAllowed = 1,LastResponseID=ISNULL(@LastResponseID,0)    
  ELSE     
  SELECT ServiceCallAllowed= 0,LastResponseID=ISNULL(@LastResponseID,0)    
  '    
--PRINT @SQL     
EXECUTE sp_executesql @SQL    
    
-- EXEC HC_GetBatchUploadedClaim    
      
END 