  
-- =============================================  

CREATE PROCEDURE [dbo].[DeleteTransportationGroupClientFilter]  
 -- Add the parameters for the stored procedure here  
 @TransportationGroupClientID bigint
 AS  
BEGIN  
	delete from TransportationGroupFilterMapping where TransportationGroupClientID =@TransportationGroupClientID

END