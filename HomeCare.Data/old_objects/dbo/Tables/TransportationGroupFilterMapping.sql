CREATE TABLE [dbo].[TransportationGroupFilterMapping] (
    [TransportationGroupFilterMappingID] BIGINT IDENTITY (1, 1) NOT NULL,
    [TransportationGroupClientID]        BIGINT NOT NULL,
    [TransportationFilterID]             BIGINT NOT NULL,
    CONSTRAINT [PK_TransportationGroupFilterMapping] PRIMARY KEY CLUSTERED ([TransportationGroupFilterMappingID] ASC)
);

