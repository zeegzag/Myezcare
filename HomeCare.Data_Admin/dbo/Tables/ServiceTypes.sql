CREATE TABLE [dbo].[ServiceTypes] (
    [ServiceTypeID]   BIGINT         NOT NULL,
    [ServiceTypeName] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ServiceTypes] PRIMARY KEY CLUSTERED ([ServiceTypeID] ASC)
);

