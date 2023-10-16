CREATE TYPE [dbo].[ApproveVisit] AS TABLE (
	[EmployeeVisitID] BIGINT NOT NULL
	,[ClockInTime] DATETIME NOT NULL
	,[ClockOutTime] DATETIME NOT NULL
	,[ApproveNote] NVARCHAR(MAX) NULL
);