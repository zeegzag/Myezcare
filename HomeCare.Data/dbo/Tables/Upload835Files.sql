CREATE TABLE [dbo].[Upload835Files] (
    [Upload835FileID]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [PayorID]                    BIGINT         NOT NULL,
    [BatchID]                    BIGINT         NULL,
    [FileName]                   VARCHAR (200)  NOT NULL,
    [FilePath]                   VARCHAR (MAX)  NOT NULL,
    [FileSize]                   VARCHAR (50)   NOT NULL,
    [Comment]                    VARCHAR (MAX)  NULL,
    [IsProcessed]                BIT            NOT NULL,
    [Upload835FileProcessStatus] INT            NULL,
    [ReadableFilePath]           VARCHAR (MAX)  NULL,
    [IsDeleted]                  BIT            NOT NULL,
    [CreatedBy]                  BIGINT         NULL,
    [CreatedDate]                DATETIME       NULL,
    [UpdatedBy]                  BIGINT         NULL,
    [UpdatedDate]                DATETIME       NULL,
    [SystemID]                   VARCHAR (100)  NULL,
    [A835TemplateType]           VARCHAR (100)  NULL,
    [EraID]                      VARCHAR (MAX)  NULL,
    [EraMappedBatches]           NVARCHAR (MAX) NULL,
    [LogFilePath]                NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Upload835Files] PRIMARY KEY CLUSTERED ([Upload835FileID] ASC),
    CONSTRAINT [FK_Upload835Files_Batches] FOREIGN KEY ([BatchID]) REFERENCES [dbo].[Batches] ([BatchID]),
    CONSTRAINT [FK_Upload835Files_Payors] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payors] ([PayorID])
);



