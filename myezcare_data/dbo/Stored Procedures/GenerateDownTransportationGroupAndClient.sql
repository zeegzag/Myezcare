--   
-- EXEC GetReferralListForScheduling  @MaxAge = '50', @MinAge = '0',@ContactTypeID=null, @SortExpression = 'LastAttendedDate', @SortType = 'DESC', @FromIndex = '1', @PageSize = '10'  
CREATE PROCEDURE [dbo].[GenerateDownTransportationGroupAndClient]    
 @TransportationGroupClientID bigint,
 @LoggedInUserID bigint
AS  
BEGIN      
	declare @ScheduleID bigint
	declare @TransportationGroupID bigint
	declare @TransportationDownGroupID bigint=null
	declare @ScheduleEndDate date
	declare @ReferralID bigint
	select @ScheduleID= TGC.ScheduleID,@ScheduleEndDate=SM.EndDate,@TransportationGroupID=TGC.TransportationGroupID,@ReferralID=SM.ReferralID from TransportationGroupClients TGC
		inner join ScheduleMasters SM on SM.ScheduleID=TGC.ScheduleID
	 where TGC.TransportationGroupClientID=@TransportationGroupClientID  

	--Get down group related to up group
	select @TransportationDownGroupID=TransportationGroupID  from TransportationGroups where TransportationDate=@ScheduleEndDate and TransportationUpGroupID=@TransportationGroupID and IsDeleted=0


	if(@TransportationDownGroupID is null)
	BEGIN
		-- Create transportation group for down
		insert into TransportationGroups
				 (TransportationDate,GroupName,FacilityID,LocationID,TripDirection,Capacity,
				  TransportationUpGroupID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted)
								
		select 
				@ScheduleEndDate,GroupName,FacilityID,LocationID,'DOWN',Capacity,
				 @TransportationGroupID,@LoggedInUserID,GETUTCDATE(),@LoggedInUserID,GETUTCDATE(),SystemID,0
		from TransportationGroups where TransportationGroupID=@TransportationGroupID

		set @TransportationDownGroupID = Scope_Identity() 


		-- Insert Staff
		insert into TransportationGroupStaffs 
					(TransportationGroupID,StaffID)
		select 
					@TransportationDownGroupID,StaffID from TransportationGroupStaffs where TransportationGroupID=@TransportationGroupID

	END

	-- Create transportation group clinet 
	if((select count(*) from TransportationGroupClients TGS inner join TransportationGroups TG ON TG.TransportationGroupID=TGS.TransportationGroupID where TGS.ScheduleID=@ScheduleID AND TG.TripDirection='DOWN') = 0)
	BEGIN
	insert into TransportationGroupClients 
						(ScheduleID,TransportationGroupID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted)
				  values(@ScheduleID,@TransportationDownGroupID,@LoggedInUserID,GETUTCDATE(),@LoggedInUserID,GETUTCDATE(),'0',0)
	declare @ID bigint=SCOPE_IDENTITY()
	EXEC SetTransportationGroupClientFilter @ReferralID,@ID
	END
	
END  
  
  
