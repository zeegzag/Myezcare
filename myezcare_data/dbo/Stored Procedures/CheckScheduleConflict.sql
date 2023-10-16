-- CheckScheduleConflict @ScheduleID=0, @ReferralID=23, @FacilityID=12, @StartDate='2016-07-03', @EndDate='2016-07-04'
--EXEC CheckScheduleConflict @ScheduleID = '101', @ReferralID = '95', @FacilityID = '12', @StartDate = '2016-07-29', @EndDate = '2016-07-30'
CREATE PROCEDURE [dbo].[CheckScheduleConflict]
	@ScheduleID bigint=0,
	@FacilityID bigint=0,
	@ReferralID bigint,
	@StartDate date,
	@EndDate date
AS
BEGIN
	Declare @ConflictCount bigint = 0 , @FacilityBadCapacity Bigint = 0 , @FacilityRoomCapacity bigint =0, @AvailableBadCapacity bigint = 0
	Declare @AvailableRoomCapacity bigint=0
	
	select @AvailableBadCapacity=BadCapacity, @AvailableRoomCapacity=PrivateRoomCount from Facilities where FacilityID=@FacilityID
	
	
	--- Check Schedule Conflicts
	select 
		@ConflictCount=COUNT(*) from ScheduleMasters SM
	where 
		(SM.IsDeleted = 0)
		and (SM.ScheduleID != @ScheduleID) 
		and (SM.ReferralID = @ReferralID)
		and ( 
				(SM.StartDate between @StartDate and @EndDate)
				or (SM.EndDate between @StartDate and @EndDate)
				or (@StartDate between SM.StartDate and SM.EndDate)
			)			
	----Check Schedule Conflicts
	
	---Check Facility Bad Capacity Capacity 
	select 
		@FacilityBadCapacity = Count(DT.Date)
	from 
		(SELECT DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) [Date]
			FROM master..spt_values
			WHERE type = 'P'
			AND DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) < DATEADD(day,1, @EndDate)) DT 
		left join ScheduleMasters SM on DT.Date between SM.StartDate and SM.EndDate and SM.FacilityID = @FacilityID 		
		where SM.IsDeleted=0 and SM.ScheduleStatusID = 2 and SM.ScheduleID!= @ScheduleID
	group by DT.Date
	having SUM(Case when SM.ScheduleStatusID = 2 then 1 else 0 end) >= @AvailableBadCapacity and (@AvailableBadCapacity>0)
		
	---Check Facility Conflicts
	
	
	---Check Facility Private Room Capacity 
	select 
		@FacilityRoomCapacity = Count(DT.Date)
	from 
		(SELECT DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) [Date]
			FROM master..spt_values
			WHERE type = 'P'
			AND DATEADD(DAY,number+1,DATEADD(day,-1, @StartDate)) < DATEADD(day,1, @EndDate)) DT 
		left join ScheduleMasters SM on DT.Date between SM.StartDate and SM.EndDate and SM.FacilityID = @FacilityID
		inner join Referrals R on R.ReferralID = SM.ReferralID	
		where SM.IsDeleted=0 and SM.ScheduleStatusID = 2 and SM.ScheduleID!= @ScheduleID
	group by DT.Date
	having SUM(Case when (SM.ScheduleStatusID = 2) and (R.NeedPrivateRoom=1) then 1 else 0 end) >= @AvailableRoomCapacity and (@AvailableRoomCapacity>0)
		
	---Check Facility Private Room  Conflicts
	
	
	select @ConflictCount ScheduleConflictCount , @FacilityBadCapacity OutOfBadCapacityCount, @FacilityRoomCapacity OutOfRoomCapacityCount
	
END

