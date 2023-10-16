CREATE TABLE [SSO].[ResetPasswordRequests] (
    [RequestID]   UNIQUEIDENTIFIER NOT NULL,
    [RequestDate] DATETIME2 (7)    NOT NULL,
    [UserID]      BIGINT           NOT NULL,
    CONSTRAINT [PK_ResetPasswordRequests] PRIMARY KEY CLUSTERED ([RequestID] ASC),
    CONSTRAINT [FK_ResetPasswordRequests_Users] FOREIGN KEY ([UserID]) REFERENCES [SSO].[Users] ([UserID])
);

