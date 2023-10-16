CREATE TABLE [SSO].[AccessLogs] (
    [LogID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [AccessID] BIGINT        NOT NULL,
    [Created]  DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_AccessLogs] PRIMARY KEY CLUSTERED ([LogID] ASC),
    CONSTRAINT [FK_AccessLogs_UserAccesses] FOREIGN KEY ([AccessID]) REFERENCES [SSO].[UserAccesses] ([AccessID])
);

