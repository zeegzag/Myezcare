CREATE TABLE [SSO].[Users] (
    [UserID]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserName]             NVARCHAR (MAX) NOT NULL,
    [FirstName]            NVARCHAR (MAX) NOT NULL,
    [LastName]             NVARCHAR (MAX) NOT NULL,
    [Email]                NVARCHAR (MAX) NOT NULL,
    [PhoneNumber]          NVARCHAR (MAX) NOT NULL,
    [Password]             NVARCHAR (MAX) NOT NULL,
    [PasswordSalt]         NVARCHAR (MAX) NOT NULL,
    [AccountAccessCount]   INT            NULL,
    [AccountLockedOutDate] DATETIME2 (7)  NULL,
    [IsActive]             BIT            NOT NULL,
    [IsDeleted]            BIT            NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

