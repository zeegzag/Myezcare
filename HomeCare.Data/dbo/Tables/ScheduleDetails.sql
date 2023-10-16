CREATE TABLE [dbo].[ScheduleDetails] (
    [ScheduleID]    BIGINT         NOT NULL,
    [HHAXEvvmsID]   NVARCHAR (MAX) NULL,
    [SandataID]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ScheduleDetails] PRIMARY KEY CLUSTERED ([ScheduleID] ASC),
    CONSTRAINT [FK_ScheduleDetails_ScheduleMasters] FOREIGN KEY ([ScheduleID]) REFERENCES [dbo].[ScheduleMasters] ([ScheduleID])
);

