CREATE PROCEDURE [dbo].[GetScheduleBatchServices]
@ListOfIdsInCSV varchar(8000)=null  
as        
BEGIN        
 select * from ScheduleBatchServices where ScheduleBatchServiceID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)) 
End