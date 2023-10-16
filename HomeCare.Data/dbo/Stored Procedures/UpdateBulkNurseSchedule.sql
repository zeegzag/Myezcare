-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[UpdateBulkNurseSchedule]
	@CareTypeId NVARCHAR(100)
	,@NurseScheduleId BIGINT
	,@ScheduleId BIGINT
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
	DECLARE @scheduleConflict INT;
	DECLARE @CurrScheduleID BIGINT

	-- check if there is conflict with existing employee schedule  
	SELECT @scheduleConflict = COUNT(DISTINCT CONVERT(DATE, SM.StartDate))
	FROM ScheduleMasters SM
	WHERE (
			(
				DATEADD(MINUTE, 1, @StartDate) BETWEEN SM.StartDate
					AND SM.EndDate
				)
			OR (
				DATEADD(MINUTE, - 1, @EndDate) BETWEEN SM.StartDate
					AND SM.EndDate
				) -- IS EMPLOYEE TIME CONFLICTS INSIDE WITH EXISTING SCHEDULE        
			OR (
				DATEADD(MINUTE, 1, @StartDate) <= SM.StartDate
				AND DATEADD(MINUTE, 1, @EndDate) >= SM.EndDate
				)
			)
		-- IS EMPLOYEE TIME OVERLAPPING AND CONFLICTS OUTSIDE WITH EXISTING SCHEDULE        
		AND SM.EmployeeID = @EmployeeId
		AND NurseScheduleId != @NurseScheduleId
		AND ISNULL(SM.AnyTimeClockIn, 0) = 0
		AND SM.IsDeleted = 0

	IF (@scheduleConflict > 0)
	BEGIN
		SELECT - 1

		RETURN;
	END

	DECLARE @pos INT
	DECLARE @len INT
	DECLARE @scheduleDay NVARCHAR(MAX)
	DECLARE @referralTimeSlotMasterID BIGINT
	DECLARE @employeeTimeSlotMasterID BIGINT
	DECLARE @referralTimeSlotDetailID BIGINT
	DECLARE @employeeTimeSlotDetailID BIGINT
	DECLARE @referralTSDateID BIGINT
	DECLARE @employeeTSDateID BIGINT
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
	--- local variables for deleting existing schedule  
	DECLARE @scheduleStartDate DATE
	DECLARE @scheduleEndDate DATE
	DECLARE @existingScheduleReferralID BIGINT
	DECLARE @existingScheduleEmployeeID BIGINT
	DECLARE @existingReferralTimeSlotMasterID BIGINT
	DECLARE @existingEmployeeTimeSlotMasterID BIGINT
	DECLARE @isExist BIT

	-- Deleting existing created slots for this schedule  
	SELECT @existingScheduleReferralID = ReferralID
		,@existingScheduleEmployeeID = EmployeeID
	FROM ScheduleMasters
	WHERE ScheduleID = @ScheduleID

	SELECT @scheduleStartDate = CONVERT(DATE, StartDate)
		,@scheduleEndDate = CONVERT(DATE, EndDate)
	FROM NurseSchedules
	WHERE NurseScheduleId = @NurseScheduleId

	SELECT @existingReferralTimeSlotMasterID = ReferralTimeSlotMasterID
	FROM ReferralTimeSlotMaster
	WHERE StartDate = @scheduleStartDate
		AND EndDate = @scheduleEndDate
		AND ReferralID = @existingScheduleReferralID

	SELECT @existingEmployeeTimeSlotMasterID = EmployeeTimeSlotMasterID
	FROM EmployeeTimeSlotMaster
	WHERE StartDate = @scheduleStartDate
		AND EndDate = @scheduleEndDate
		AND EmployeeID = @existingScheduleEmployeeID

	DELETE
	FROM EmployeeTimeSlotDates
	WHERE EmployeeTimeSlotMasterID = @existingEmployeeTimeSlotMasterID

	DELETE
	FROM EmployeeTimeSlotDetails
	WHERE EmployeeTimeSlotMasterID = @existingEmployeeTimeSlotMasterID

	DELETE
	FROM EmployeeTimeSlotMaster
	WHERE EmployeeTimeSlotMasterID = @existingEmployeeTimeSlotMasterID

	DELETE
	FROM ReferralTimeSlotDates
	WHERE ReferralTimeSlotMasterID = @existingReferralTimeSlotMasterID

	DELETE
	FROM ReferralTimeSlotDetails
	WHERE ReferralTimeSlotMasterID = @existingReferralTimeSlotMasterID

	DELETE
	FROM ReferralTimeSlotMaster
	WHERE ReferralTimeSlotMasterID = @existingReferralTimeSlotMasterID

	DELETE
	FROM ScheduleMasters
	WHERE NurseScheduleID = @NurseScheduleId

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
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @StartTime, 108))
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
				,CONVERT(DATETIME, CONVERT(VARCHAR(20), @scheduleDay, 101) + ' ' + CONVERT(VARCHAR(8), @StartTime, 108))
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

				SET @CurrScheduleID = @@IDENTITY

			EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule'
				,@CurrScheduleID
				,''
				,''
		END

		SET @pos = CHARINDEX(',', @ScheduleDayList, @pos + @len) + 1
	END

	UPDATE NurseSchedules
	SET FrequencyChoice = @FrequencyChoice
		,Frequency = @Frequency
		,DaysOfWeek = @DaysOfWeek
		,[DayOfMonth] = @DayOfMonth
		,IsMonthlyDaySelection = @IsMonthlyDaySelection
		,DailyInterval = @DailyInterval
		,WeeklyInterval = @WeeklyInterval
		,MonthlyInterval = @MonthlyInterval
		,IsSundaySelected = @IsSundaySelected
		,IsMondaySelected = @IsMondaySelected
		,IsTuesdaySelected = @IsTuesdaySelected
		,IsWednesdaySelected = @IsWednesdaySelected
		,IsThursdaySelected = @IsThursdaySelected
		,IsFridaySelected = @IsFridaySelected
		,IsSaturdaySelected = @IsSaturdaySelected
		,IsFirstWeekOfMonthSelected = @IsFirstWeekOfMonthSelected
		,IsSecondWeekOfMonthSelected = @IsSecondWeekOfMonthSelected
		,IsThirdWeekOfMonthSelected = @IsThirdWeekOfMonthSelected
		,IsFourthWeekOfMonthSelected = @IsFourthWeekOfMonthSelected
		,IsLastWeekOfMonthSelected = @IsLastWeekOfMonthSelected
		,FrequencyTypeOptions = @FrequencyTypeOptions
		,ScheduleRecurrence = @ScheduleRecurrence
		,MonthlyIntervalOptions = @MonthlyIntervalOptions
		,StartDate = @StartDate
		,EndDate = @EndDate
		,UpdatedBy = @LoggedInUserId
		,UpdatedDate = @dateToday
		,DaysOfWeekOptions = @DaysOfWeekOptions
		,Notes = @Notes
		,AnniversaryDay = @AnniversaryDay
		,AnniversaryMonth = @AnniversaryMonth
		,IsAnyDay = @IsAnyDay
	WHERE NurseScheduleID = @NurseScheduleId

	SELECT @NurseScheduleId;
END
