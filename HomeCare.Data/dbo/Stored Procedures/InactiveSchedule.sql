
--EXEC InactiveSchedule @ReferralID = '2892', @CancelStatus = '6', @Comment = 'Inactivation', @WhoCancel = 'Office', @ChangeStatus = '1,8'
CREATE procedure [dbo].[InactiveSchedule]    
--declare
@ReferralID bigint,
@Comment varchar(500),
@ChangeStatus varchar(50),
@CancelStatus int,
@WhoCancel varchar(50)
as    
	


	---Delete Transportation Mapping Of All Schedule
	delete from TransportationGroupFilterMapping where TransportationGroupClientID
			in(
				select TransportationGroupClientID from 
					TransportationGroupClients 
				where ScheduleID in (
					select ScheduleID from scheduleMasters 
					where StartDate >= CAST(GETDATE() as DATE)
					and ReferralID=@ReferralID and ScheduleStatusID in(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ChangeStatus))
				)  
			)


	--Delete Transportation Clients of That Schedule
	DELETE FROM  TransportationGroupClients  where ScheduleID in(
				
					select ScheduleID from scheduleMasters 
					where StartDate >= CAST(GETDATE() as DATE)
					and ReferralID=@ReferralID and ScheduleStatusID in(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ChangeStatus))
				
			)
	
	--Delete Transportation Clients of That Schedule
	DELETE AttendanceMaster where ScheduleMasterID  in(
				
					select ScheduleID from scheduleMasters 
					where StartDate >= CAST(GETDATE() as DATE)
					and ReferralID=@ReferralID and ScheduleStatusID in(SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ChangeStatus))
				
			)

DELETE FROM ScheduleMasters 
	where StartDate >= CAST(GETDATE() as DATE)
	and ReferralID=@ReferralID
	and IsDeleted = 0