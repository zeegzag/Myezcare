USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[GetNurseScheduleMaster]    Script Date: 12/9/2020 2:50:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: 21 Nov 2020
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetNurseScheduleMaster] 
	@CareTypeIds VARCHAR(MAX),
	@EmployeeIds VARCHAR(MAX),
	@ReferralIds VARCHAR(MAX)
AS
BEGIN
	SELECT ScheduleID, ScheduleMasters.ReferralID, ScheduleMasters.EmployeeID, FrequencyChoice, Frequency, DaysOfWeek, WeeklyInterval,MonthlyInterval,
	IsSundaySelected, IsMondaySelected, IsTuesdaySelected, IsWednesdaySelected, IsThursdaySelected, IsFridaySelected, IsSaturdaySelected,
	IsFirstWeekOfMonthSelected, IsSecondWeekOfMonthSelected, IsThirdWeekOfMonthSelected,IsFourthWeekOfMonthSelected,IsLastWeekOfMonthSelected,
	FrequencyTypeOptions,ScheduleRecurrence,MonthlyIntervalOptions, ScheduleMasters.CareTypeId, Employees.FirstName + ' ' + Employees.LastName AS EmployeeFullName,
	Referrals.FirstName + ' ' + Referrals.LastName AS PatientFullName, ScheduleMasters.StartDate, ScheduleMasters.EndDate,DaysOfWeekOptions,ScheduleMasters.PayorID,
	ScheduleMasters.ReferralBillingAuthorizationID,IsVirtualVisit, PayorName, DDMaster.Title AS CareType,Notes,IsAnyDay,AnyTimeClockIn,StartTime AS ClockInStartTime,
	EndTime AS ClockInEndTime
	FROM ScheduleMasters

	INNER JOIN Employees ON
	ScheduleMasters.EmployeeId = Employees.EmployeeId

	INNER JOIN Referrals ON
	ScheduleMasters.ReferralID = Referrals.ReferralID

	INNER JOIN ReferralTimeSlotMaster ON
	ScheduleMasters.ReferralTSMasterID = ReferralTimeSlotMaster.ReferralTimeSlotMasterID

	LEFT JOIN Payors ON
	ScheduleMasters.PayorID = Payors.PayorID

	INNER JOIN DDMaster ON
	ScheduleMasters.CareTypeID = DDMaster.DDMasterID

	WHERE (CareTypeId IN (SELECT Item from dbo.SplitString(@CareTypeIds,',')) OR @CareTypeIds = '')
	AND (ScheduleMasters.EmployeeId IN (SELECT Item from dbo.SplitString(@EmployeeIds,',')) OR @EmployeeIds = '')
	AND (ScheduleMasters.ReferralId IN (SELECT Item from dbo.SplitString(@ReferralIds,',')) OR @ReferralIds ='') AND ScheduleMasters.IsDeleted = 0 AND NewCalenderType = 1
END
GO

