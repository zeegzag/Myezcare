CREATE TABLE [dbo].[EVV_SmsLogs] (
    [SmsLogID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [SmsStatusId]  INT            NULL,
    [Type]         NVARCHAR (10)  NULL,
    [ScheduleID]   BIGINT         NOT NULL,
    [EmployeeID]   BIGINT         NOT NULL,
    [ErrorMessage] NVARCHAR (MAX) NULL,
    [AttemptCount] INT            CONSTRAINT [DF_EVV_SmsLogs_AttemptCount] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_EVV_SmsLogs] PRIMARY KEY CLUSTERED ([SmsLogID] ASC)
);

