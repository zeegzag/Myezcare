CREATE TABLE [dbo].[AuditLogTable] (
    [AuditLogID]       INT            IDENTITY (1, 1) NOT NULL,
    [ParentKeyFieldID] BIGINT         NOT NULL,
    [ChildKeyFieldID]  BIGINT         NULL,
    [AuditActionType]  VARCHAR (10)   NOT NULL,
    [DateTimeStamp]    DATETIME       NOT NULL,
    [DataModel]        NVARCHAR (100) NOT NULL,
    [Changes]          NVARCHAR (MAX) NOT NULL,
    [ValueBefore]      NVARCHAR (MAX) NOT NULL,
    [ValueAfter]       NVARCHAR (MAX) NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [CreatedBy]        BIGINT         NOT NULL,
    [UpdatedDate]      DATETIME       NOT NULL,
    [UpdatedBy]        BIGINT         NOT NULL,
    [SystemID]         VARCHAR (100)  NOT NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([AuditLogID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLogTable_ParentKeyFieldID_FF596]
    ON [dbo].[AuditLogTable]([ParentKeyFieldID] ASC);

