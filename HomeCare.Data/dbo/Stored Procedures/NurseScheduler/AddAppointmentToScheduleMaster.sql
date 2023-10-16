USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[AddAppointmentToScheduleMaster]    Script Date: 12/9/2020 2:48:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: 22 Nov 2020
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddAppointmentToScheduleMaster]
	@CareTypeId NVARCHAR(100),
	@EmployeeId bigint,
	@ReferralId bigint,
	@CreatedBy bigint,
	@UpdatedBy bigint,
	@CreatedDate datetime,
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

 DECLARE @ScheduleID BIGINT;  

		INSERT INTO ScheduleMasters (EmployeeId, ReferralId, CreatedBy, UpdatedBy, CreatedDate, UpdatedDate,
		StartDate,EndDate,FrequencyChoice,Frequency,DaysOfWeek,WeeklyInterval,MonthlyInterval,IsSundaySelected,
		IsMondaySelected,IsTuesdaySelected,IsWednesdaySelected,IsThursdaySelected,IsFridaySelected,IsSaturdaySelected,
		IsFirstWeekOfMonthSelected,IsSecondWeekOfMonthSelected,IsThirdWeekOfMonthSelected,IsFourthWeekOfMonthSelected,IsLastWeekOfMonthSelected,
		FrequencyTypeOptions,MonthlyIntervalOptions,ScheduleRecurrence,ScheduleStatusID,CareTypeId,DaysOfWeekOptions,IsDeleted,NewCalenderType,
		PayorID,ReferralBillingAuthorizationID,IsVirtualVisit,Notes,ReferralTSMasterID,AnyTimeClockIn,StartTime,EndTime)
		VALUES (@EmployeeId, @ReferralId, @CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,
		@StartDate,@EndDate,@FrequencyChoice,@Frequency,@DaysOfWeek,@WeeklyInterval,@MonthlyInterval,@IsSundaySelected,
		@IsMondaySelected,@IsTuesdaySelected,@IsWednesdaySelected,@IsThursdaySelected,@IsFridaySelected,@IsSaturdaySelected,
		@IsFirstWeekOfMonthSelected,@IsSecondWeekOfMonthSelected,@IsThirdWeekOfMonthSelected,@IsFourthWeekOfMonthSelected,@IsLastWeekOfMonthSelected,
		@FrequencyTypeOptions,@MonthlyIntervalOptions,@ScheduleRecurrence,2,@CareTypeId,@DaysOfWeekOptions,0,1,@PayorID,@ReferralBillingAuthorizationID,
		@IsVirtualVisit,@Notes,@ReferralTSMasterID,@AnyTimeClockIn,@StartTime,@EndTime);

		 SET @ScheduleID=@@IDENTITY 

		 SELECT @ScheduleID;    

END
GO

