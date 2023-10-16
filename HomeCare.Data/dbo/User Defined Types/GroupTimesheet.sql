CREATE TYPE [dbo].[GroupTimesheet] AS TABLE (
	[ScheduleID] BIGINT NULL
	,[ClockInDateTime] DATETIME NULL
	,[ClockOutDateTime] DATETIME NULL
);
