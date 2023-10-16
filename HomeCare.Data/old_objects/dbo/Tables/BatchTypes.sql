CREATE TABLE [dbo].[BatchTypes] (
    [BatchTypeID]    BIGINT        NOT NULL,
    [BatchTypeName]  VARCHAR (100) NOT NULL,
    [IsDeleted]      BIT           NOT NULL,
    [BatchTypeShort] VARCHAR (10)  NULL,
    CONSTRAINT [PK_BatchTypes] PRIMARY KEY CLUSTERED ([BatchTypeID] ASC)
);

