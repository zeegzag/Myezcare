CREATE TABLE [dbo].[AllUsersAccess] (
    [Id]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [UserID]     UNIQUEIDENTIFIER NOT NULL,
    [Portal]     VARCHAR (50)     NOT NULL,
    [PortalUrl]  VARCHAR (MAX)    NOT NULL,
    [DomainName] VARCHAR (500)    NOT NULL,
    [IsActive]   BIT              NOT NULL,
    CONSTRAINT [PK_AllUsersAccess] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AllUsersAccess_AllUsers] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AllUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

