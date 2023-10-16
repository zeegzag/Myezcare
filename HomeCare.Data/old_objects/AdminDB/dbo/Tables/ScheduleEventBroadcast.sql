CREATE TABLE [dbo].[ScheduleEventBroadcast] (
  [ScheduleEventBroadcastID] bigint IDENTITY (1, 1) NOT NULL,
  [OrganizationID] bigint NOT NULL,
  [ScheduleID] bigint NOT NULL,
  [EventName] nvarchar(max) NOT NULL,
  [ReasonCode] nvarchar(max) NOT NULL,
  [ActionCode] nvarchar(max) NOT NULL,
  [CreatedDate] datetime NOT NULL,
  [IsProcessed] bit NOT NULL
);