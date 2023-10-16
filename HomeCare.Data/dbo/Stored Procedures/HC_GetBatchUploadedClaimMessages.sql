-- EXEC HC_GetBatchUploadedClaimMessages        
CREATE PROCEDURE HC_GetBatchUploadedClaimMessages        
@BatchID NVARCHAR(MAX) ='100560',          
@ClaimID NVARCHAR(MAX) ='246366718'  ,        
@Total_Charge NVARCHAR(MAX)='1295.70'          
AS          
BEGIN          
        
DECLARE @SQL NVARCHAR(MAX)='';          
DECLARE @Bill_NPI  NVARCHAR(MAX)='';          
DECLARE @Bill_TaxID NVARCHAR(MAX)='';          
        
        
SELECT TOP 1 @Bill_NPI= Submitter_NM109_IdCode, @Bill_TaxID= BillingProvider_REF02_ReferenceIdentification  FROM OrganizationSettings        
        
DECLARE @AdminDatabaseName NVARCHAR(MAX)='';        
SELECT TOP 1 @AdminDatabaseName=AdminDatabaseName FROM OrganizationSettings        
        
SET @SQL = 

'
SELECT * FROM (
SELECT row_num = DENSE_RANK() OVER (ORDER BY  L.ClaimMD_ID DESC), 
L.ClaimMD_ID,L.BatchID,L.Bill_NPI,L.Bill_TaxID,L.FDOS ,L.FileID, L.FileName,L.Sender_Name,L.Status,L.Total_Charge,L.Messages 
FROM '+@AdminDatabaseName+'.dbo.BatchUploadedClaimMessages L
WHERE BatchID ='''+@BatchID+''' AND  Bill_NPI=  '''+@Bill_NPI+''' AND  Bill_TaxID=  '''+@Bill_TaxID+''' 
--AND ( LEN(@ClaimID)=0 OR ClaimID=  '''+@ClaimID+''' )    
AND    FORMAT(CONVERT(FLOAT,Total_Charge), ''N2'')=  '''+FORMAT(CONVERT(FLOAT,@Total_Charge), 'N2')+'''
 ) AS T WHERE row_num=1'
  
PRINT @SQL         
EXECUTE sp_executesql @SQL        
        
-- EXEC HC_GetBatchUploadedClaimMessages @BatchID = '100560', @ClaimID = 'C022533290', @Total_Charge = '1295.70'        
          
END 