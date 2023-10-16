CREATE procedure [dbo].[ValidateTransportationGroup]
	@TransportationGroupID bigint=0,
	@StaffID varchar(500)=null,
	@TransportationDate date=null,
	@TripDirection varchar(20)
as      
begin        
	select 
		distinct TG.*
	from TransportationGroups TG
	 inner join	TransportationGroupStaffs TGS on TGS.TransportationGroupID=TG.TransportationGroupID
	where 
	TG.TransportationGroupID!=@TransportationGroupID 
	and TGS.StaffID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@StaffID )) 
	and TG.TransportationDate=@TransportationDate
	and TG.IsDeleted=0 
	and TG.TripDirection=@TripDirection
end