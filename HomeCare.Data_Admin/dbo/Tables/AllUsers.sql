CREATE TABLE [dbo].[AllUsers] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [FirstName]            VARCHAR (300)    NULL,
    [LastName]             VARCHAR (300)    NULL,
    [Email]                VARCHAR (300)    NULL,
    [Username]             VARCHAR (300)    NOT NULL,
    [Password]             VARCHAR (MAX)    NOT NULL,
    [PasswordSalt]         VARCHAR (MAX)    NOT NULL,
    [UserExternalID]       BIGINT           NOT NULL,
    [UserType]             VARCHAR (50)     NOT NULL,
    [ClientID]             BIGINT           NOT NULL,
    [RoleID]               BIGINT           CONSTRAINT [DF_AllUsers_RoleID] DEFAULT ((0)) NOT NULL,
    [AccountAccessCount]   INT              CONSTRAINT [DF_AllUsers_FailedAccessCount] DEFAULT ((0)) NOT NULL,
    [AccountLockedOutDate] DATETIME         NULL,
    [AccountPin]           VARCHAR (50)     NULL,
    [OrganizationID]       BIGINT           CONSTRAINT [DF_AllUsers_OrganizationID] DEFAULT ((0)) NOT NULL,
    [DomainName]           VARCHAR (500)    CONSTRAINT [DF_AllUsers_Domain] DEFAULT ('') NOT NULL,
    [IsActive]             BIT              NOT NULL,
    [IsDeleted]            BIT              CONSTRAINT [DF_AllUsers_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AllUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

