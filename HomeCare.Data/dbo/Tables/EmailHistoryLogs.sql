CREATE TABLE [dbo].[EmailHistoryLogs] (
    [EmailID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [ToEmail]     VARCHAR (MAX) NULL,
    [ToPhoneNo]   VARCHAR (20)  NULL,
    [Subject]     VARCHAR (500) NULL,
    [Body]        VARCHAR (MAX) NOT NULL,
    [EmailType]   VARCHAR (MAX) NULL,
    [IsSent]      BIT           NULL,
    [CreatedDate] DATETIME      NOT NULL,
    CONSTRAINT [PK_EmailHistoryLogs] PRIMARY KEY CLUSTERED ([EmailID] ASC)
);

