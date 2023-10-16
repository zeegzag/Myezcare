CREATE TABLE [dbo].[TransportationFilters] (
    [TransportationFilterID]   BIGINT        NOT NULL,
    [TransportationFilterName] VARCHAR (100) NOT NULL,
    [ShortName]                VARCHAR (50)  NOT NULL,
    [IsDeleted]                BIT           NOT NULL,
    CONSTRAINT [PK_TransportationFilters] PRIMARY KEY CLUSTERED ([TransportationFilterID] ASC)
);

