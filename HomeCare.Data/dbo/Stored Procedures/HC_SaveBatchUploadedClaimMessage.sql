    
CREATE   PROCEDURE HC_SaveBatchUploadedClaimMessage          
@LastResponseID nvarchar(MAX)='NA',        
@BatchID nvarchar(MAX)='NA',        
@Bill_NPI nvarchar(MAX)='NA',        
@Bill_TaxID nvarchar(MAX)='NA',        
@ClaimID nvarchar(MAX)='NA',        
@ClaimMD_ID nvarchar(MAX)='NA',        
@FDOS nvarchar(MAX)='NA' ,        
@FileID nvarchar(MAX)='NA' ,        
@FileName nvarchar(MAX)='NA',        
@INS_Number nvarchar(MAX)='NA',        
@PayerID nvarchar(MAX)='NA',        
@PCN nvarchar(MAX) ='NA',        
@Remote_ClaimID nvarchar(MAX) ='NA',        
@Sender_ICN nvarchar(MAX)='NA' ,        
@Sender_Name nvarchar(MAX)='NA' ,        
@SenderID nvarchar(MAX) ='NA',        
@Status nvarchar(MAX)='NA' ,        
@Total_Charge nvarchar(MAX) ='NA',        
@Messages nvarchar(MAX)='NA'         
        
AS          
BEGIN          
          
          
DECLARE @SQL NVARCHAR(MAX)='';          
 DECLARE @AdminDatabaseName NVARCHAR(MAX)='';        
SELECT TOP 1 @AdminDatabaseName=AdminDatabaseName FROM OrganizationSettings        
        
SET @SQL = '        
IF NOT EXISTS (SELECT 1 FROM '+@AdminDatabaseName+'.dbo.BatchUploadedClaimMessages WHERE LastResponseID='''+@LastResponseID+''' AND BatchID='''+@BatchID+''' AND FileID='''+@FileID+''' AND  
ClaimMD_ID='''+@ClaimMD_ID+''')        
INSERT INTO '+@AdminDatabaseName+'.dbo.BatchUploadedClaimMessages         
(LastResponseID,BatchID,Bill_NPI,Bill_TaxID,ClaimID,ClaimMD_ID,FDOS ,FileID,FileName,INS_Number,PayerID,PCN,Remote_ClaimID,Sender_ICN,Sender_Name,        
SenderID,Status,Total_Charge,Messages,CreatedDate)        
SELECT '''+@LastResponseID+''','''+@BatchID+''','''+@Bill_NPI+''','''+@Bill_TaxID+''','''+@ClaimID+''','''+@ClaimMD_ID+''','''+@FDOS +''','''+@FileID+''',  
'''+@FileName+''','''+@INS_Number+''','''+@PayerID+''','''+@PCN+''','''+@Remote_ClaimID+'        
'','''+@Sender_ICN+''','''+@Sender_Name+''','''+@SenderID+''','''+@Status+''','''+@Total_Charge+''','''+@Messages+''',GETDATE()

ELSE 
PRINT ''NOT RECORD ADDED''

';        
        
        
--PRINT @SQL         
EXECUTE sp_executesql @SQL        
--EXEC HC_SaveBatchUploadedClaimMessage        
  
          
END          
    