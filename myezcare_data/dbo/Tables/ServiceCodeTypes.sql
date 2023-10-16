CREATE TABLE [dbo].[ServiceCodeTypes] (
    [ServiceCodeTypeID]   BIGINT        NOT NULL,
    [ServiceCodeTypeName] VARCHAR (100) NOT NULL,
    [IsDeleted]           BIT           NOT NULL,
    CONSTRAINT [PK_ServiceCodeTypes] PRIMARY KEY CLUSTERED ([ServiceCodeTypeID] ASC)
);

