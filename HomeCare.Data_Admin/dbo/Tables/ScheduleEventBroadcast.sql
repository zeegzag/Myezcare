CREATE TABLE [dbo].[ScheduleEventBroadcast] (
    [ScheduleEventBroadcastID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrganizationID]           BIGINT         NOT NULL,
    [ScheduleID]               BIGINT         NOT NULL,
    [EventName]                NVARCHAR (MAX) NOT NULL,
    [ReasonCode]               NVARCHAR (MAX) NOT NULL,
    [ActionCode]               NVARCHAR (MAX) NOT NULL,
    [CreatedDate]              DATETIME       NOT NULL,
    [IsProcessed]              BIT            NOT NULL
);

