CREATE TABLE [dbo].[EdiFileTypes] (
    [EdiFileTypeID]   BIGINT        NOT NULL,
    [EdiFileTypeName] VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_EdiFileTypes] PRIMARY KEY CLUSTERED ([EdiFileTypeID] ASC)
);

