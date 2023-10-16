CREATE TABLE [dbo].[TransportationGroupClients] (
    [TransportationGroupClientID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ScheduleID]                  BIGINT        NOT NULL,
    [TransportationGroupID]       BIGINT        NOT NULL,
    [CreatedBy]                   BIGINT        NOT NULL,
    [CreatedDate]                 DATETIME      NOT NULL,
    [UpdatedDate]                 DATETIME      NOT NULL,
    [UpdatedBy]                   BIGINT        NOT NULL,
    [SystemID]                    VARCHAR (100) NOT NULL,
    [IsDeleted]                   BIT           NOT NULL,
    CONSTRAINT [PK_TransportationGroupClients] PRIMARY KEY CLUSTERED ([TransportationGroupClientID] ASC),
    CONSTRAINT [FK_TransportationGroupClients_ScheduleMasters] FOREIGN KEY ([ScheduleID]) REFERENCES [dbo].[ScheduleMasters] ([ScheduleID]),
    CONSTRAINT [FK_TransportationGroupClients_TransportationGroups] FOREIGN KEY ([TransportationGroupID]) REFERENCES [dbo].[TransportationGroups] ([TransportationGroupID])
);

