CREATE PROCEDURE [dbo].[HC_GetDeleteUpload835FilePathList]    
@ListOfIdsInCSV varchar(4000) ,         
@UnProcess int         
AS    
BEGIN    
SELECT * FROM Upload835Files WHERE Upload835FileID IN (SELECT val FROM GetCSVTable(@ListOfIdsInCSV)) AND IsProcessed=0 AND Upload835FileProcessStatus=@UnProcess  AND BatchID IS NULL    
END
