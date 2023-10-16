USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[UpdateAppointmentToScheduleMaster]    Script Date: 12/9/2020 2:49:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: 26 Nov 2020
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAppointmentToScheduleMaster]
	@ScheduleId bigint,
	@EmployeeId bigint,
	@ReferralId bigint,
	@CareTypeId NVARCHAR(100),
	@UpdatedBy bigint,
	@UpdatedDate datetime,
	@StartDate datetime,
	@EndDate datetime,
	@FrequencyChoice int,
	@Frequency int,
	@DaysOfWeek int,
	@WeeklyInterval int,
	@MonthlyInterval int,
	@IsSundaySelected bit,
	@IsMondaySelected bit,
	@IsTuesdaySelected bit,
	@IsWednesdaySelected bit,
	@IsThursdaySelected bit,
	@IsFridaySelected bit,
	@IsSaturdaySelected bit,
	@IsFirstWeekOfMonthSelected bit,
	@IsSecondWeekOfMonthSelected bit,
	@IsThirdWeekOfMonthSelected bit,
	@IsFourthWeekOfMonthSelected bit,
	@IsLastWeekOfMonthSelected bit,
	@FrequencyTypeOptions NVARCHAR(MAX),
	@MonthlyIntervalOptions NVARCHAR(MAX),
	@ScheduleRecurrence NVARCHAR(MAX),
	@DaysOfWeekOptions  NVARCHAR(MAX),
	@PayorID bigint,
	@ReferralBillingAuthorizationID bigint,
	@IsVirtualVisit bit,
	@Notes varchar(1000),
	@ReferralTSMasterID bigint,
	@AnyTimeClockIn bit,
	@StartTime time,
	@EndTime time
AS
BEGIN
	UPDATE ScheduleMasters
	SET FrequencyChoice = @FrequencyChoice, Frequency= @Frequency, DaysOfWeek = @DaysOfWeek, WeeklyInterval = @WeeklyInterval, MonthlyInterval = @MonthlyInterval,
	IsSundaySelected = @IsSundaySelected,IsMondaySelected = @IsMondaySelected,IsTuesdaySelected = @IsTuesdaySelected, IsWednesdaySelected = @IsWednesdaySelected,
	IsThursdaySelected = @IsThursdaySelected, IsFridaySelected = @IsFridaySelected, IsSaturdaySelected = @IsSaturdaySelected,
	IsFirstWeekOfMonthSelected = @IsFirstWeekOfMonthSelected, IsSecondWeekOfMonthSelected = @IsSecondWeekOfMonthSelected,IsThirdWeekOfMonthSelected=@IsThirdWeekOfMonthSelected,
	IsFourthWeekOfMonthSelected = @IsFourthWeekOfMonthSelected,IsLastWeekOfMonthSelected=@IsLastWeekOfMonthSelected,
	FrequencyTypeOptions = @FrequencyTypeOptions,ScheduleRecurrence=@ScheduleRecurrence,MonthlyIntervalOptions=@MonthlyIntervalOptions, CareTypeId = @CareTypeId,
	StartDate = @StartDate, EndDate = @EndDate, UpdatedBy =	@UpdatedBy, UpdatedDate = @UpdatedDate,PayorID=@PayorID,
	ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID,IsVirtualVisit = @IsVirtualVisit,EmployeeId=@EmployeeId, ReferralId=@ReferralId,DaysOfWeekOptions = @DaysOfWeekOptions,
	Notes=@Notes,ReferralTSMasterID=@ReferralTSMasterID,AnyTimeClockIn=@AnyTimeClockIn,StartTime=@StartTime,EndTime = @EndTime
	WHERE ScheduleID = @ScheduleId

	 SELECT @ScheduleId;    
END
GO

