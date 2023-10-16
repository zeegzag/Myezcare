CREATE TABLE [dbo].[EmailLogs] (
    [EmailLogID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [ToEmail]       VARCHAR (100) NULL,
    [Subject]       VARCHAR (100) NULL,
    [Body]          VARCHAR (MAX) NULL,
    [SentBy]        BIGINT        NULL,
    [SentDate]      DATETIME      CONSTRAINT [DF_EmailLogs_SentDate] DEFAULT (getutcdate()) NULL,
    [IsAttachments] BIT           NULL,
    [Status]        INT           NULL,
    [EmailUniqueId] BIGINT        NULL,
    CONSTRAINT [PK_EmailLogs] PRIMARY KEY CLUSTERED ([EmailLogID] ASC)
);

