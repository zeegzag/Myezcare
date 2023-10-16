CREATE PROCEDURE [dbo].[HC_SaveBatchUploadedClaimMessageTable]   
@claimObject [BatchUploadedClaimMessages] ReadOnly
AS            
BEGIN            
            
DECLARE @SQL NVARCHAR(MAX)='';            
DECLARE @AdminDatabaseName NVARCHAR(MAX)='';          
SELECT TOP 1 @AdminDatabaseName=AdminDatabaseName FROM OrganizationSettings          
          
SET @SQL = '          
INSERT INTO '+@AdminDatabaseName+'.dbo.BatchUploadedClaimMessages           
(LastResponseID,BatchID,Bill_NPI,Bill_TaxID,ClaimID,ClaimMD_ID,FDOS ,FileID,FileName,INS_Number,PayerID,PCN,Remote_ClaimID,Sender_ICN,Sender_Name,          
SenderID,Status,Total_Charge,Messages,CreatedDate) 

SELECT L.LastResponseID,L.BatchID,L.Bill_NPI,L.Bill_TaxID,L.ClaimID,L.ClaimMD_ID,L.FDOS ,L.FileID,L.FileName,L.INS_Number,L.PayerID,L.PCN,L.Remote_ClaimID,
L.Sender_ICN,L.Sender_Name,L.SenderID,L.Status,L.Total_Charge,L.Messages,L.CreatedDate

FROM @LIST L
LEFT JOIN  '+@AdminDatabaseName+'.dbo.BatchUploadedClaimMessages B ON B.LastResponseID=L.LastResponseID AND B.BatchID= L.BatchID AND B.FileID = L.FileID 
AND B.ClaimMD_ID= L.ClaimMD_ID AND B.Sender_Name=L.Sender_Name
WHERE B.BatchUploadedClaimMessageID IS NULL
    
  
';          
          
          
PRINT @SQL           
EXECUTE sp_executesql @SQL,N'@LIST [BatchUploadedClaimMessages] ReadOnly', @claimObject         
-- EXEC [HC_SaveBatchUploadedClaimMessageTable]    BatchUploadedClaimMessages
    
            
END            