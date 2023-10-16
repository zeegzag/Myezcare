CREATE PROCEDURE [dbo].[HC_GetUnsentBatches]                 
@ListOfIdsInCSV varchar(8000)=null    
as          
BEGIN          
 select * from Batches where BatchId in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)) and IsSent=0      
End