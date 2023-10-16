CREATE TABLE [dbo].[ScheduleNotificationLogs] (
    [ScheduleNotificationID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]             BIGINT        NOT NULL,
    [ScheduleID]             BIGINT        NOT NULL,
    [NotificationType]       VARCHAR (10)  NOT NULL,
    [NotificationSubType]    VARCHAR (100) NULL,
    [Source]                 VARCHAR (100) NOT NULL,
    [FromEmailAddress]       VARCHAR (100) NULL,
    [ToEmailAddress]         VARCHAR (100) NULL,
    [FromPhone]              VARCHAR (50)  NULL,
    [ToPhone]                VARCHAR (50)  NULL,
    [Subject]                VARCHAR (MAX) NULL,
    [Body]                   VARCHAR (MAX) NULL,
    [IsSent]                 BIT           CONSTRAINT [DF_ScheduleNotificationLogs_IsSent] DEFAULT ((0)) NOT NULL,
    [CreatedDate]            DATETIME      NOT NULL,
    [CreatedBy]              BIGINT        NULL,
    CONSTRAINT [PK_ScheduleNotificationLogs] PRIMARY KEY CLUSTERED ([ScheduleNotificationID] ASC),
    CONSTRAINT [FK_ScheduleNotificationLogs_Employees] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_ScheduleNotificationLogs_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_ScheduleNotificationLogs_ScheduleMasters] FOREIGN KEY ([ScheduleID]) REFERENCES [dbo].[ScheduleMasters] ([ScheduleID])
);


GO
CREATE NONCLUSTERED INDEX [missing_index_47_46_ScheduleNotificationLogs]
    ON [dbo].[ScheduleNotificationLogs]([ScheduleID] ASC);

