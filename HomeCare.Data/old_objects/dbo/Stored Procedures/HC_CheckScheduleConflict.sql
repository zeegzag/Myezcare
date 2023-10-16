-- CheckScheduleConflict @ScheduleID=0, @ReferralID=23, @FacilityID=12, @StartDate='2016-07-03', @EndDate='2016-07-04'
-- EXEC HC_CheckScheduleConflict @ScheduleID = '101', @ReferralID = '1951', @EmployeeID = '12', @StartDate = '2018-02-19T10:30:00', @EndDate = '2018-02-19T12:30:00'
CREATE PROCEDURE [dbo].[HC_CheckScheduleConflict]
	@ScheduleID bigint=0,
	@EmployeeID bigint=0,
	@ReferralID bigint,
	@StartDate datetime,
	@EndDate datetime
AS
BEGIN
	Declare @ConflictCount bigint = 0;
	
	
	--- Check Schedule Conflicts
	select 
		@ConflictCount=COUNT(*) from ScheduleMasters SM
	where 
		(SM.IsDeleted = 0)
		and (SM.ScheduleID != @ScheduleID) 
		and (SM.ReferralID = @ReferralID)
		and ( 
				--(SM.StartDate between @StartDate and @EndDate)
				--or (SM.EndDate between @StartDate and @EndDate)
				--or (@StartDate between SM.StartDate and SM.EndDate)

				(SM.StartDate > @StartDate and SM.StartDate< @EndDate)
				or (SM.EndDate > @StartDate and SM.EndDate < @EndDate)
				or (@StartDate > SM.StartDate and @StartDate< SM.EndDate)
				or (@StartDate = SM.StartDate and @EndDate = SM.EndDate)
			)			
	----Check Schedule Conflicts
	



	--- --- Check Patient Schedule Preference
--MondaySchedule
--TuesdaySchedule
--WednesdaySchedule
--ThursdaySchedule
--FridaySchedule
--SaturdaySchedule
--SundaySchedule

    Declare @PatientPreferenceCount bigint = 0;
	Declare @StartDay VARCHAR(100);
	Declare @EndDay VARCHAR(100);

	SELECT @StartDay=DATENAME(dw,@StartDate)
	SELECT @EndDay=DATENAME(dw,@EndDate)

	
	SELECT @PatientPreferenceCount=
	CASE 
	    WHEN MondaySchedule=0 AND (@StartDay='Monday' OR @EndDay='Monday') THEN 1
		WHEN TuesdaySchedule=0 AND (@StartDay='Tuesday' OR @EndDay='Tuesday') THEN 1
		WHEN WednesdaySchedule=0 AND (@StartDay='Wednesday' OR @EndDay='Wednesday') THEN 1
		WHEN ThursdaySchedule=0 AND (@StartDay='Thursday' OR @EndDay='Thursday') THEN 1
		WHEN FridaySchedule=0 AND (@StartDay='Friday' OR @EndDay='Friday') THEN 1
		WHEN SaturdaySchedule=0 AND (@StartDay='Saturday' OR @EndDay='Saturday') THEN 1
		WHEN SundaySchedule=0 AND (@StartDay='Sunday' OR @EndDay='Sunday') THEN 1
    ELSE 0 END
	FROM Referrals WHERE ReferralID=@ReferralID


	--- --- Check Patient Schedule Preference

	select ScheduleConflictCount = @ConflictCount , PatientPreferenceCount = @PatientPreferenceCount
	
END
