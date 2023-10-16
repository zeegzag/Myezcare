--exec GetTransportationGroupClientByScheduleID 522,'UP'
CREATE procedure [dbo].[GetTransportationGroupClientByScheduleID]                  
@ScheduleID bigint,                  
@TripDirection varchar(10)
as                  
BEGIN                  

select TGS.* from TransportationGroupClients TGS
inner join TransportationGroups TG on TGS.TransportationGroupID = TG.TransportationGroupID
where TGS.ScheduleID=@ScheduleID and TG.TripDirection = @TripDirection

  
END