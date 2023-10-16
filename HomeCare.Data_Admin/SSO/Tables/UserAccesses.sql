CREATE TABLE [SSO].[UserAccesses] (
    [AccessID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserID]         BIGINT         NOT NULL,
    [PortalName]     NVARCHAR (MAX) NOT NULL,
    [PortalCode]     NVARCHAR (MAX) NOT NULL,
    [PortalUrl]      NVARCHAR (MAX) NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [OrganizationID] BIGINT         NOT NULL,
    CONSTRAINT [PK_UsersAccess] PRIMARY KEY CLUSTERED ([AccessID] ASC),
    CONSTRAINT [FK_UserAccesses_Users] FOREIGN KEY ([UserID]) REFERENCES [SSO].[Users] ([UserID])
);

