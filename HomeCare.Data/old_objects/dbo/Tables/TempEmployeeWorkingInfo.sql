CREATE TABLE [dbo].[TempEmployeeWorkingInfo] (
    [EmployeeUniqueID] VARCHAR (100)  NULL,
    [EmployeeName]     NVARCHAR (255) NULL,
    [Day]              VARCHAR (50)   NULL,
    [StartDate]        DATE           NULL,
    [EndDate]          DATE           NULL,
    [StartTime]        TIME (7)       NULL,
    [EndTime]          TIME (7)       NULL,
    [SyncStatus]       BIT            CONSTRAINT [DF_TempEmployeeWorkingInfo_SyncStatus] DEFAULT ((0)) NULL,
    [SyncError]        NVARCHAR (500) NULL,
    [SyncOn]           DATETIME       NULL
);

