CREATE TABLE [dbo].[CheckInOutLog] (
    [Id]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [PatientID]      BIGINT       NOT NULL,
    [EmployeeID]     BIGINT       NOT NULL,
    [scheduleID]     BIGINT       NOT NULL,
    [OrganizationID] BIGINT       NOT NULL,
    [ClockInOutType] VARCHAR (50) NOT NULL,
    [Time]           DATETIME     NOT NULL,
    [PatientLat]     FLOAT (53)   NULL,
    [PatientLong]    FLOAT (53)   NULL,
    [EmployeeLat]    FLOAT (53)   NULL,
    [EmployeeLong]   FLOAT (53)   NULL,
    CONSTRAINT [PK_CheckInOutLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CheckInOutLog_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_CheckInOutLog_Referrals] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_CheckInOutLog_ScheduleMasters] FOREIGN KEY ([scheduleID]) REFERENCES [dbo].[ScheduleMasters] ([ScheduleID])
);

