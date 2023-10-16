CREATE TABLE [dbo].[EmployeeEmailSignature] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (250) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [UpdatedBy]   BIGINT         NULL,
    [UpdatedOn]   DATETIME       DEFAULT (getdate()) NULL,
    [EmployeeID]  BIGINT         NULL,
    CONSTRAINT [PK__Employee__3214EC0785537647] PRIMARY KEY CLUSTERED ([Id] ASC)
);

