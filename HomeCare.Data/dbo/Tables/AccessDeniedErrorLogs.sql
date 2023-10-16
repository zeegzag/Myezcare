CREATE TABLE [dbo].[AccessDeniedErrorLogs] (
    [AccessDeniedID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [PermissionID]   NVARCHAR (50) NULL,
    [PermissionName] NVARCHAR (50) NULL,
    [Domain]         NVARCHAR (50) NULL,
    [EmployeeID]     BIGINT        NULL,
    [Date]           DATE          NULL,
    CONSTRAINT [PK_AccessDeniedErrorLogs] PRIMARY KEY CLUSTERED ([AccessDeniedID] ASC)
);

