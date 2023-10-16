CREATE PROCEDURE HC_ValdiateAndUpdateERA835ProcessStatus  
@ERA_ID NVARCHAR(MAX)  
AS  
BEGIN  
  
IF EXISTS (SELECT 1 FROM Upload835Files WHERE  EraID = @ERA_ID)  
BEGIN  
SELECT -1;  
END  
ELSE  
BEGIN  
SELECT 1;  
END  
  
  
UPDATE Upload835Files SET IsProcessed = 1, Upload835FileProcessStatus = 2 WHERE IsProcessed = 0 AND Upload835FileProcessStatus IN (1,2)   
  
  
END  