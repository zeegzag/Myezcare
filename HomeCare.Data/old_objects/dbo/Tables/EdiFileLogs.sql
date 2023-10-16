CREATE TABLE [dbo].[EdiFileLogs] (
    [EdiFileLogID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [EdiFileTypeID] BIGINT        NOT NULL,
    [FileName]      VARCHAR (200) NOT NULL,
    [FilePath]      VARCHAR (MAX) NOT NULL,
    [FileSize]      VARCHAR (10)  NULL,
    [IsDeleted]     BIT           NOT NULL,
    [BatchID]       BIGINT        NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [CreatedDate]   DATETIME      NOT NULL,
    [UpdatedBy]     BIGINT        NOT NULL,
    [UpdatedDate]   DATETIME      NOT NULL,
    [SystemID]      VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EDIFilesLog] PRIMARY KEY CLUSTERED ([EdiFileLogID] ASC),
    CONSTRAINT [FK_EdiFileLogs_EdiFileTypes] FOREIGN KEY ([EdiFileTypeID]) REFERENCES [dbo].[EdiFileTypes] ([EdiFileTypeID])
);

