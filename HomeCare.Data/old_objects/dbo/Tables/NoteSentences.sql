CREATE TABLE [dbo].[NoteSentences] (
    [NoteSentenceID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [NoteSentenceTitle]   VARCHAR (100) NOT NULL,
    [NoteSentenceDetails] VARCHAR (MAX) NOT NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF_NoteSentences_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]         DATETIME      NOT NULL,
    [CreatedBy]           BIGINT        NOT NULL,
    [UpdatedDate]         DATETIME      NOT NULL,
    [UpdatedBy]           BIGINT        NOT NULL,
    [SystemID]            VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_NoteSentences] PRIMARY KEY CLUSTERED ([NoteSentenceID] ASC)
);

