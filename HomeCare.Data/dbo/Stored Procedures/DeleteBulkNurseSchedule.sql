

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteBulkNurseSchedule]
	-- Add the parameters for the stored procedure here
	@ScheduleId bigint
AS
BEGIN

	DECLARE @StartDate date
	DECLARE @EndDate date
	DECLARE @ReferralID bigint
	DECLARE @EmployeeID bigint
	DECLARE @ReferralTimeSlotMasterID bigint
	DECLARE @EmployeeTimeSlotMasterID bigint
	DECLARE @NurseScheduleId bigint

	SELECT @ReferralID = ReferralID,@EmployeeID = EmployeeID, @NurseScheduleId = NurseScheduleId 
	FROM ScheduleMasters WHERE ScheduleID = @ScheduleID

	SELECT @StartDate = CONVERT(date, StartDate), @EndDate = CONVERT(date, EndDate) FROM NurseSchedules WHERE NurseScheduleId = @NurseScheduleId

	SELECT @ReferralTimeSlotMasterID = ReferralTimeSlotMasterID FROM ReferralTimeSlotMaster
	WHERE StartDate = @StartDate AND EndDate = @EndDate AND ReferralID = @ReferralID

	SELECT @EmployeeTimeSlotMasterID = EmployeeTimeSlotMasterID FROM EmployeeTimeSlotMaster
	WHERE StartDate = @StartDate AND EndDate = @EndDate AND EmployeeID = @EmployeeID


	DELETE FROM EmployeeTimeSlotDates WHERE EmployeeTimeSlotMasterID = @EmployeeTimeSlotMasterID
	DELETE FROM EmployeeTimeSlotDetails where EmployeeTimeSlotMasterID = @EmployeeTimeSlotMasterID
	DELETE FROM EmployeeTimeSlotMaster where EmployeeTimeSlotMasterID = @EmployeeTimeSlotMasterID

	DELETE FROM ReferralTimeSlotDates where ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID
	DELETE FROM ReferralTimeSlotDetails where ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID
	DELETE FROM ReferralTimeSlotMaster where ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID

	DELETE FROM ScheduleMasters WHERE NurseScheduleID = @NurseScheduleId

	DELETE FROM NurseSchedules WHERE NurseScheduleID = @NurseScheduleId

	SELECT @ScheduleId
END
GO

