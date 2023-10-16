CREATE TABLE [SSO].[RefreshTokens] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [Token]           NVARCHAR (MAX) NOT NULL,
    [Expires]         DATETIME2 (7)  NOT NULL,
    [IsExpired]       BIT            NOT NULL,
    [Created]         DATETIME2 (7)  NOT NULL,
    [CreatedByIp]     NVARCHAR (MAX) NULL,
    [Revoked]         DATETIME2 (7)  NULL,
    [RevokedByIp]     NVARCHAR (MAX) NULL,
    [ReplacedByToken] NVARCHAR (MAX) NULL,
    [IsActive]        BIT            NOT NULL,
    [UserID]          BIGINT         NOT NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_RefreshToken_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [SSO].[Users] ([UserID]) ON DELETE CASCADE
);

