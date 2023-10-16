CREATE PROCEDURE  [dbo].[GetSetAddTransportationLopcationPage]
@TransportLocationID BIGINT  
AS  
BEGIN  
  select *  from States;    
  select *  from  TransportLocations WHERE  TransportLocationID = CAST(@TransportLocationID AS BIGINT) ;
  SELECT * FROM Regions;
 END
