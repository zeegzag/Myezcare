CREATE TABLE [dbo].[Edi270271Files] (
    [Edi270271FileID]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [FileType]                   VARCHAR (100) NOT NULL,
    [FileName]                   VARCHAR (200) NOT NULL,
    [FilePath]                   VARCHAR (MAX) NOT NULL,
    [FileSize]                   VARCHAR (10)  NULL,
    [Comment]                    VARCHAR (MAX) NULL,
    [ReadableFilePath]           VARCHAR (MAX) NULL,
    [Upload271FileProcessStatus] INT           NULL,
    [IsDeleted]                  BIT           CONSTRAINT [DF_Edi270271Files_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedBy]                  BIGINT        NOT NULL,
    [CreatedDate]                DATETIME      NOT NULL,
    [UpdatedBy]                  BIGINT        NOT NULL,
    [UpdatedDate]                DATETIME      NOT NULL,
    [SystemID]                   VARCHAR (100) NOT NULL,
    [PayorIDs]                   VARCHAR (MAX) NULL,
    [ServiceIDs]                 VARCHAR (MAX) NULL,
    [Name]                       VARCHAR (100) NULL,
    [EligibilityCheckDate]       DATE          NULL,
    [ReferralStatusIDs]          VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Edi270271Files] PRIMARY KEY CLUSTERED ([Edi270271FileID] ASC)
);

