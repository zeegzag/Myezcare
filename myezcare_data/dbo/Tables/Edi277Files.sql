CREATE TABLE [dbo].[Edi277Files] (
    [Edi277FileID]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [FileType]                   VARCHAR (100) NOT NULL,
    [FileName]                   VARCHAR (200) NOT NULL,
    [FilePath]                   VARCHAR (MAX) NOT NULL,
    [FileSize]                   VARCHAR (10)  NULL,
    [ReadableFilePath]           VARCHAR (MAX) NULL,
    [PayorID]                    BIGINT        NULL,
    [Comment]                    VARCHAR (MAX) NULL,
    [Upload277FileProcessStatus] INT           NULL,
    [IsDeleted]                  BIT           CONSTRAINT [DF_Edi277Files_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedBy]                  BIGINT        NOT NULL,
    [CreatedDate]                DATETIME      NOT NULL,
    [UpdatedBy]                  BIGINT        NOT NULL,
    [UpdatedDate]                DATETIME      NOT NULL,
    [SystemID]                   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Edi277Files] PRIMARY KEY CLUSTERED ([Edi277FileID] ASC)
);

