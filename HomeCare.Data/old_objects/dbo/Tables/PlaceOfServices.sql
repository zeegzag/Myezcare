CREATE TABLE [dbo].[PlaceOfServices] (
    [PosID]     BIGINT        NOT NULL,
    [PosName]   VARCHAR (100) NOT NULL,
    [IsDeleted] BIT           NOT NULL,
    CONSTRAINT [PK_PlaceOfServices] PRIMARY KEY CLUSTERED ([PosID] ASC)
);

