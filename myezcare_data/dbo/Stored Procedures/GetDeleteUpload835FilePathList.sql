CREATE procedure [dbo].[GetDeleteUpload835FilePathList]    
@ListOfIdsInCSV varchar(4000) ,   
@UnProcess int   
as    
select *  from Upload835Files where Upload835FileID in(select val from GetCSVTable(@ListOfIdsInCSV)) AND IsProcessed=0 AND Upload835FileProcessStatus=@UnProcess  AND BatchID IS NULL 
 
