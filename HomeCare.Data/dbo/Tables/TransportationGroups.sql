CREATE TABLE [dbo].[TransportationGroups] (
    [TransportationGroupID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TransportationDate]      DATE          NOT NULL,
    [GroupName]               VARCHAR (100) NULL,
    [FacilityID]              BIGINT        NOT NULL,
    [LocationID]              BIGINT        NOT NULL,
    [TripDirection]           VARCHAR (5)   NOT NULL,
    [Capacity]                INT           NOT NULL,
    [TransportationUpGroupID] BIGINT        NULL,
    [CreatedBy]               BIGINT        NOT NULL,
    [CreatedDate]             DATETIME      NOT NULL,
    [UpdatedBy]               BIGINT        NOT NULL,
    [UpdatedDate]             DATETIME      NOT NULL,
    [SystemID]                VARCHAR (100) NOT NULL,
    [IsDeleted]               BIT           NOT NULL,
    CONSTRAINT [PK_TransportationGroups] PRIMARY KEY CLUSTERED ([TransportationGroupID] ASC),
    CONSTRAINT [FK_TransportationGroups_TransportationLocations] FOREIGN KEY ([LocationID]) REFERENCES [dbo].[TransportLocations] ([TransportLocationID])
);

