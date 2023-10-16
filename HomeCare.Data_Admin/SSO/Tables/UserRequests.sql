CREATE TABLE [SSO].[UserRequests] (
    [RequestID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [RequestDate] DATETIME2 (7)  NOT NULL,
    [FirstName]   NVARCHAR (MAX) NULL,
    [LastName]    NVARCHAR (MAX) NULL,
    [Email]       NVARCHAR (MAX) NOT NULL,
    [PhoneNumber] NVARCHAR (MAX) NOT NULL,
    [PortalName]  NVARCHAR (MAX) NOT NULL,
    [PortalCode]  NVARCHAR (MAX) NOT NULL,
    [PortalUrl]   NVARCHAR (MAX) NOT NULL,
    [IsCompleted] BIT            NOT NULL,
    CONSTRAINT [PK_UsersRequest] PRIMARY KEY CLUSTERED ([RequestID] ASC)
);

