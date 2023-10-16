CREATE TABLE [dbo].[GroupMessageLogs] (
    [GroupMessageLogID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeIDs]       NVARCHAR (MAX) NOT NULL,
    [Message]           NVARCHAR (MAX) NOT NULL,
    [NotificationSID]   NVARCHAR (100) NULL,
    [CreatedDate]       DATETIME       NULL,
    [CreatedBy]         BIGINT         NULL,
    [UpdatedDate]       DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [SystemID]          VARCHAR (100)  NULL,
    CONSTRAINT [PK_GroupMessageLogs] PRIMARY KEY CLUSTERED ([GroupMessageLogID] ASC)
);

