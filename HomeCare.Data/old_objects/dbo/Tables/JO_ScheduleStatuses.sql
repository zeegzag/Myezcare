CREATE TABLE [dbo].[JO_ScheduleStatuses] (
    [JO_ScheduleStatusID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ScheduleStatusID]    BIGINT        NOT NULL,
    [ScheduleStatusName]  VARCHAR (100) NULL,
    [Action]              CHAR (1)      NOT NULL,
    [ActionDate]          DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ScheduleStatuses] PRIMARY KEY CLUSTERED ([JO_ScheduleStatusID] ASC)
);

