-- =============================================
-- Author:	 Tapendra kumar Sharma
-- Create date: 25/09/2019
-- Description:	Insert clockin out time log
-- =============================================
CREATE PROCEDURE API_AddClockInOutLog
	-- Add the parameters for the stored procedure here
	@PatientID BIGINT,
    @EmployeeID BIGINT, 
    @ScheduleID BIGINT,
    @OrganizationID BIGINT,
    @ClockInOutType  VARCHAR(50),
    @Time  Datetime,
	@PatientLat float,
	@PatientLong float,
	@EmployeeLat float,
	@EmployeeLong  float

	
AS
BEGIN
	SET NOCOUNT ON;
INSERT INTO CheckInOutLog
           (PatientID
           ,EmployeeID
           ,scheduleID
           ,OrganizationID
           ,ClockInOutType
           ,Time
		   ,PatientLat
		   ,PatientLong
		   ,EmployeeLat
		   ,EmployeeLong
			   )
     VALUES
           (@PatientID
           ,@EmployeeID
           ,@ScheduleID
           ,@OrganizationID
           ,@ClockInOutType
           ,@Time
		   ,@PatientLat
		   ,@PatientLong
		   ,@EmployeeLat
		   ,@EmployeeLong)

		   select 1;
END