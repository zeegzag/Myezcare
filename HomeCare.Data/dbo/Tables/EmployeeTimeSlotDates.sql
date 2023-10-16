CREATE TABLE [dbo].[EmployeeTimeSlotDates] (
    [EmployeeTSDateID]         BIGINT   IDENTITY (1, 1) NOT NULL,
    [EmployeeID]               BIGINT   NOT NULL,
    [EmployeeTimeSlotMasterID] BIGINT   NOT NULL,
    [EmployeeTSDate]           DATE     NOT NULL,
    [EmployeeTSStartTime]      DATETIME NOT NULL,
    [EmployeeTSEndTime]        DATETIME NOT NULL,
    [DayNumber]                INT      NULL,
    [EmployeeTimeSlotDetailID] BIGINT   NULL,
    CONSTRAINT [PK_EmployeeTimeSlotDates] PRIMARY KEY CLUSTERED ([EmployeeTSDateID] ASC),
    CONSTRAINT [FK_EmployeeTimeSlotDates_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeTimeSlotDates_EmployeeTimeSlotMaster] FOREIGN KEY ([EmployeeTimeSlotMasterID]) REFERENCES [dbo].[EmployeeTimeSlotMaster] ([EmployeeTimeSlotMasterID])
);
GO

CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDates_EmployeeTimeSlotDetailID_EmployeeTSDate_6A423]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTimeSlotDetailID] ASC, [EmployeeTSDate] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDates_EmployeeTimeSlotDetailID_EmployeeTSDate_9147D]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTimeSlotDetailID] ASC, [EmployeeTSDate] ASC)
    INCLUDE([EmployeeTimeSlotMasterID]);
GO

CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDates_EmployeeTimeSlotMasterID_EmployeeTSDate_269E3]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTimeSlotMasterID] ASC, [EmployeeTSDate] ASC)
    INCLUDE([EmployeeTimeSlotDetailID]);
GO

CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDates_EmployeeTimeSlotMasterID_EmployeeTSDate_417C5]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTimeSlotMasterID] ASC, [EmployeeTSDate] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDates_EmployeeTSDate_34DD7]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTSDate] ASC)
    INCLUDE([EmployeeTimeSlotMasterID], [EmployeeTSStartTime], [EmployeeTSEndTime], [DayNumber]);
GO

CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDates_EmployeeTSDate_885B8]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTSDate] ASC)
    INCLUDE([EmployeeTimeSlotMasterID], [EmployeeTimeSlotDetailID]);
GO

CREATE NONCLUSTERED INDEX [IX_EmployeeTimeSlotDates_EmployeeTSDate_EBC01]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTSDate] ASC)
    INCLUDE([EmployeeTimeSlotDetailID]);
GO

CREATE NONCLUSTERED INDEX [IX_ETS_001]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTSStartTime] ASC, [EmployeeTSEndTime] ASC)
    INCLUDE([EmployeeTimeSlotDetailID]);
GO

CREATE NONCLUSTERED INDEX [missing_index_41_40_EmployeeTimeSlotDates]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeID] ASC, [EmployeeTSDate] ASC)
    INCLUDE([EmployeeTSStartTime], [EmployeeTSEndTime]);
GO

CREATE NONCLUSTERED INDEX [missing_index_43_42_EmployeeTimeSlotDates]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeTSDate] ASC)
    INCLUDE([EmployeeID], [EmployeeTSStartTime], [EmployeeTSEndTime]);
GO

CREATE NONCLUSTERED INDEX [missing_index_49_48_EmployeeTimeSlotDates]
    ON [dbo].[EmployeeTimeSlotDates]([EmployeeID] ASC, [EmployeeTSStartTime] ASC, [EmployeeTSEndTime] ASC);
GO

