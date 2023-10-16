CREATE PROCEDURE [dbo].[GetEmpClockInOutList_Stats]
AS
BEGIN
  DECLARE @StartDate DATE = dbo.GetOrgCurrentDateTime()
  DECLARE @temp TABLE (
    [ROW] INT NOT NULL,
    ReferralID BIGINT NOT NULL,
    EmployeeID BIGINT NOT NULL,
    EmpFirstName VARCHAR(300) NOT NULL,
    EmpLastName VARCHAR(300) NOT NULL,
    FirstName VARCHAR(300) NOT NULL,
    LastName VARCHAR(300) NOT NULL,
    ScheduleStartTime DATETIME,
    ScheduleEndTime DATETIME,
    ClockIn INT NOT NULL,
    ClockOut INT NOT NULL,
    [Count] INT NOT NULL
    )
  DECLARE @OrgTimeZone VARCHAR(50) = ''

  SELECT TOP 1 @OrgTimeZone = ISNULL(TimeZone, 'UTC')
  FROM dbo.OrganizationSettings

  -- update this with client timezone
  --DECLARE @TimeZoneTime DATETIME = GETUTCDATE()
  DECLARE @TimeZoneTime DATETIME = GETUTCDATE() AT TIME ZONE @OrgTimeZone 
    --'Eastern Standard Time'

  INSERT INTO @temp
  EXEC GetEmpClockInOutList @StartDate = @StartDate,
    @EndDate = @StartDate,
    @EmployeeName = '',
    @SortExpression = 'ScheduleStartTime',
    @SortType = 'DESC',
    @FromIndex = '1',
    @PageSize = '1000'

  SELECT
    --*,
    'Current TimeZone Date' = @StartDate,
    TotalSchedule = MAX([Count]),
    TotalInprogress = SUM(CASE 
        WHEN ClockIn = 1
          AND ClockOut = 0
          AND ScheduleStartTime < @TimeZoneTime
          THEN 1
        ELSE 0
        END),
    TotalMissed = SUM(CASE 
        WHEN ClockIn = 0
          AND ScheduleEndTime > @TimeZoneTime
          THEN 1
        ELSE 0
        END),
    TotalCompleted = SUM(CASE 
        WHEN ClockIn = 1
          AND ClockOut = 1
          THEN 1
        ELSE 0
        END)
  FROM @temp
END
