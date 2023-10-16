-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [dbo].[AddBulkNurseSchedule]
	@CareTypeId NVARCHAR(100)
	,@EmployeeId BIGINT
	,@ReferralId BIGINT
	,@LoggedInUserId BIGINT
	,@StartDate DATETIME
	,@EndDate DATETIME
	,@FrequencyChoice INT
	,@Frequency INT
	,@DaysOfWeek INT
	,@DayOfMonth INT
	,@IsMonthlyDaySelection BIT
	,@DailyInterval INT
	,@WeeklyInterval INT
	,@MonthlyInterval INT
	,@IsSundaySelected BIT
	,@IsMondaySelected BIT
	,@IsTuesdaySelected BIT
	,@IsWednesdaySelected BIT
	,@IsThursdaySelected BIT
	,@IsFridaySelected BIT
	,@IsSaturdaySelected BIT
	,@IsFirstWeekOfMonthSelected BIT
	,@IsSecondWeekOfMonthSelected BIT
	,@IsThirdWeekOfMonthSelected BIT
	,@IsFourthWeekOfMonthSelected BIT
	,@IsLastWeekOfMonthSelected BIT
	,@FrequencyTypeOptions NVARCHAR(MAX)
	,@MonthlyIntervalOptions NVARCHAR(MAX)
	,@ScheduleRecurrence NVARCHAR(MAX)
	,@DaysOfWeekOptions NVARCHAR(MAX)
	,@PayorID BIGINT
	,@ReferralBillingAuthorizationID BIGINT
	,@IsVirtualVisit BIT
	,@Notes VARCHAR(1000)
	,@AnyTimeClockIn BIT
	,@StartTime TIME
	,@EndTime TIME
	,@AnniversaryDay INT
	,@AnniversaryMonth INT
	,@IsAnyDay BIT
	,@SystemID VARCHAR(100)
	,@ScheduleDayList NVARCHAR(max)
AS
BEGIN
	DECLARE @pos INT
	DECLARE @len INT
	DECLARE @scheduleDay NVARCHAR(MAX)
	DECLARE @referralTimeSlotMasterID BIGINT
	DECLARE @employeeTimeSlotMasterID BIGINT
	DECLARE @nurseScheduleID BIGINT
	DECLARE @referralTimeSlotDetailID BIGINT
	DECLARE @employeeTimeSlotDetailID BIGINT
	DECLARE @referralTSDateID BIGINT
	DECLARE @employeeTSDateID BIGINT
	DECLARE @ScheduleID BIGINT
	DECLARE @dateToday DATETIME
	DECLARE @allDay BIT
	--- for calculating unique weekdays timeslot detail tables for emp and referral    
	DECLARE @weekDay INT
	DECLARE @tableWeekDays TABLE (
		ScheduleWeekDay INT
		,ReferralTimeSlotDetailID BIGINT
		,EmployeeTimeSlotDetailID BIGINT
		)
	DECLARE @uniqueWeekDayCount INT = 0
	DECLARE @isExist BIT

	SET @dateToday = GETDATE()

	IF @AnyTimeClockIn = 1
		SET @allDay = 1
	ELSE
		SET @allDay = 0

	-- Adding patient record to referraltimeslotmaster table    
	INSERT INTO ReferralTimeSlotMaster (
		ReferralID
		,StartDate
		,EndDate
		,IsDeleted
		,CreatedDate
		,CreatedBy
		,UpdatedDate
		,UpdatedBy
		,SystemID
		,IsEndDateAvailable
		,ReferralBillingAuthorizationID
		,IsAnyDay
		)
	VALUES (
		@ReferralID
		,CONVERT(DATE, @StartDate)
		,CONVERT(DATE, @EndDate)
		,0
		,@dateToday
		,@LoggedInUserId
		,@dateToday
		,@LoggedInUserId
		,@SystemID
		,1
		,@ReferralBillingAuthorizationID
		,@IsAnyDay
		);

	SET @referralTimeSlotMasterID = @@IDENTITY

	-- Adding employee record to referraltimeslotmaster table    
	INSERT INTO EmployeeTimeSlotMaster (
		EmployeeID
		,StartDate
		,EndDate
		,IsDeleted
		,CreatedDate
		,CreatedBy
		,UpdatedDate
		,UpdatedBy
		,SystemID
		,IsEndDateAvailable
		)
	VALUES (
		@EmployeeID
		,CONVERT(DATE, @StartDate)
		,CONVERT(DATE, @EndDate)
		,0
		,@dateToday
		,@LoggedInUserId
		,@dateToday
		,@LoggedInUserId
		,@SystemID
		,1
		);

	SET @employeeTimeSlotMasterID = @@IDENTITY

	INSERT INTO NurseSchedules (
		FrequencyChoice
		,Frequency
		,DaysOfWeek
		,[DayOfMonth]
		,IsMonthlyDaySelection
		,DailyInterval
		,WeeklyInterval
		,MonthlyInterval
		,IsSundaySelected
		,IsMondaySelected
		,IsTuesdaySelected
		,IsWednesdaySelected
		,IsThursdaySelected
		,IsFridaySelected
		,IsSaturdaySelected
		,IsFirstWeekOfMonthSelected
		,IsSecondWeekOfMonthSelected
		,IsThirdWeekOfMonthSelected
		,IsFourthWeekOfMonthSelected
		,IsLastWeekOfMonthSelected
		,FrequencyTypeOptions
		,MonthlyIntervalOptions
		,ScheduleRecurrence
		,DaysOfWeekOptions
		,AnniversaryDay
		,AnniversaryMonth
		,CreatedDate
		,CreatedBy
		,UpdatedDate
		,UpdatedBy
		,Notes
		,IsAnyDay
		,StartDate
		,EndDate
		)
	VALUES (
		@FrequencyChoice
		,@Frequency
		,@DaysOfWeek
		,@DayOfMonth
		,@IsMonthlyDaySelection
		,@DailyInterval
		,@WeeklyInterval
		,@MonthlyInterval
		,@IsSundaySelected
		,@IsMondaySelected
		,@IsTuesdaySelected
		,@IsWednesdaySelected
		,@IsThursdaySelected
		,@IsFridaySelected
		,@IsSaturdaySelected
		,@IsFirstWeekOfMonthSelected
		,@IsSecondWeekOfMonthSelected
		,@IsThirdWeekOfMonthSelected
		,@IsFourthWeekOfMonthSelected
		,@IsLastWeekOfMonthSelected
		,@FrequencyTypeOptions
		,@MonthlyIntervalOptions
		,@ScheduleRecurrence
		,@DaysOfWeekOptions
		,@AnniversaryDay
		,@AnniversaryMonth
		,@dateToday
		,@LoggedInUserId
		,@dateToday
		,@LoggedInUserId
		,@Notes
		,@IsAnyDay
		,@StartDate
		,@EndDate
		);

	SET @nurseScheduleID = @@IDENTITY
	SET @pos = 0
	SET @len = 0

	WHILE CHARINDEX(',', @ScheduleDayList, @pos + 1) > 0
	BEGIN
		SET @len = CHARINDEX(',', @ScheduleDayList, @pos + 1) - @pos
		SET @scheduleDay = SUBSTRING(@ScheduleDayList, @pos, @len)

		SELECT @isExist = dbo.udf_IsNurseScheduleExist(@scheduleDay, @StartTime, @EndTime, @EmployeeId, @ReferralId, @CareTypeId, @PayorID, @ReferralBillingAuthorizationID, @AnyTimeClockIn)

		IF (@isExist = 0)
		BEGIN
			SET @weekDay = DATEPART(dw, @scheduleDay)

			SELECT @uniqueWeekDayCount = COUNT(ScheduleWeekDay)
			FROM @tableWeekDays
			WHERE ScheduleWeekDay = @weekDay

			IF (@uniqueWeekDayCount = 0) -- if unique weekday    
			BEGIN
				-- Add record to referraltimeslotdetails table    
				INSERT INTO ReferralTimeSlotDetails (
					ReferralTimeSlotMasterID
					,[Day]
					,StartTime
					,EndTime
					,IsDeleted
					,CreatedDate
					,CreatedBy
					,UpdatedDate
					,UpdatedBy
					,SystemID
					,Notes
					,UsedInScheduling
					,CareTypeId
					,AnyTimeClockIn
					)
				VALUES (
					@ReferralTimeSlotMasterID
					,DATEPART(dw, @scheduleDay)
					,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @StartTime, 108))
					,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @EndTime, 108))
					,0
					,@dateToday
					,@LoggedInUserId
					,@dateToday
					,@LoggedInUserId
					,@SystemID
					,@Notes
					,1
					,@CareTypeId
					,@AnyTimeClockIn
					);

				SET @referralTimeSlotDetailID = @@IDENTITY

				-- Add record to employeetimeslotdetails table    
				INSERT INTO EmployeeTimeSlotDetails (
					EmployeeTimeSlotMasterID
					,[Day]
					,StartTime
					,EndTime
					,IsDeleted
					,CreatedDate
					,CreatedBy
					,UpdatedDate
					,UpdatedBy
					,SystemID
					,Notes
					,AllDay
					)
				VALUES (
					@EmployeeTimeSlotMasterID
					,DATEPART(dw, @scheduleDay)
					,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @StartTime, 108))
					,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @EndTime, 108))
					,0
					,@dateToday
					,@LoggedInUserId
					,@dateToday
					,@LoggedInUserId
					,@SystemID
					,@Notes
					,@allDay
					);

				SET @employeeTimeSlotDetailID = @@IDENTITY

				INSERT INTO @tableWeekDays (
					ScheduleWeekDay
					,ReferralTimeSlotDetailID
					,EmployeeTimeSlotDetailID
					)
				SELECT @weekDay
					,@referralTimeSlotDetailID
					,@employeeTimeSlotDetailID
			END
			ELSE
			BEGIN
				SELECT @referralTimeSlotDetailID = ReferralTimeSlotDetailID
				FROM @tableWeekDays
				WHERE ScheduleWeekDay = @weekDay

				SELECT @employeeTimeSlotDetailID = EmployeeTimeSlotDetailID
				FROM @tableWeekDays
				WHERE ScheduleWeekDay = @weekDay
			END

			--Add record to referraltimeslotdates table    
			INSERT INTO ReferralTimeSlotDates (
				ReferralID
				,ReferralTimeSlotMasterID
				,ReferralTSDate
				,ReferralTSStartTime
				,ReferralTSEndTime
				,UsedInScheduling
				,Notes
				,DayNumber
				,ReferralTimeSlotDetailID
				,OnHold
				,IsDenied
				)
			VALUES (
				@ReferralID
				,@referralTimeSlotMasterID
				,CONVERT(DATE, @scheduleDay)
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @EndTime, 108))
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @EndTime, 108))
				,1
				,@Notes
				,DATEPART(dw, @scheduleDay)
				,@ReferralTimeSlotDetailID
				,0
				,0
				);

			SET @referralTSDateID = @@IDENTITY

			--Add record to employeetimeslotdates table    
			INSERT INTO EmployeeTimeSlotDates (
				EmployeeID
				,EmployeeTimeSlotMasterID
				,EmployeeTSDate
				,EmployeeTSStartTime
				,EmployeeTSEndTime
				,DayNumber
				,EmployeeTimeSlotDetailID
				)
			VALUES (
				@EmployeeID
				,@EmployeeTimeSlotMasterID
				,CONVERT(DATE, @scheduleDay)
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @EndTime, 108))
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @EndTime, 108))
				,DATEPART(dw, @scheduleDay)
				,@EmployeeTimeSlotDetailID
				);

			SET @employeeTSDateID = @@IDENTITY

			--Add record to schedule master table    
			INSERT INTO ScheduleMasters (
				EmployeeId
				,ReferralId
				,CreatedBy
				,UpdatedBy
				,CreatedDate
				,UpdatedDate
				,StartDate
				,EndDate
				,ScheduleStatusID
				,CareTypeId
				,IsDeleted
				,PayorID
				,ReferralBillingAuthorizationID
				,IsVirtualVisit
				,AnyTimeClockIn
				,StartTime
				,EndTime
				,EmployeeTSDateID
				,ReferralTSDateID
				,NurseScheduleID
				,SystemID
				)
			VALUES (
				@EmployeeId
				,@ReferralId
				,@LoggedInUserId
				,@LoggedInUserId
				,@dateToday
				,@dateToday
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @StartTime, 108))
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @EndTime, 108))
				,2
				,@CareTypeId
				,0
				,@PayorID
				,@ReferralBillingAuthorizationID
				,@IsVirtualVisit
				,@AnyTimeClockIn
				,@StartTime
				,@EndTime
				,@EmployeeTSDateID
				,@ReferralTSDateID
				,@nurseScheduleID
				,@SystemID
				);

			SET @ScheduleID = @@IDENTITY

			EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule'
				,@ScheduleID
				,''
				,''
		END

		SET @pos = CHARINDEX(',', @ScheduleDayList, @pos + @len) + 1
	END

	SELECT @nurseScheduleID
END
