  
-- =============================================  

CREATE PROCEDURE [dbo].[DeleteTransportationGroupClientForDownDirection]  
 -- Add the parameters for the stored procedure here  
 @TransportationGroupClientID bigint
 AS  
BEGIN  

	DECLARE @Direction varchar(5)
	DECLARE @ScheduleID bigint


	select @ScheduleID = tgs.ScheduleID,@Direction=tg.TripDirection from TransportationGroupClients tgs inner join TransportationGroups TG on TG.TransportationGroupID=tgs.TransportationGroupID where tgs.TransportationGroupClientID=@TransportationGroupClientID


	
	delete from TransportationGroupFilterMapping where TransportationGroupClientID
	 in(select TransportationGroupClientID from TransportationGroupClients where (@Direction='UP' OR TransportationGroupClientID=@TransportationGroupClientID) AND ScheduleID=@ScheduleID )

	DELETE FROM  TransportationGroupClients  where (@Direction='UP' OR TransportationGroupClientID=@TransportationGroupClientID) AND ScheduleID = @ScheduleID

	
END