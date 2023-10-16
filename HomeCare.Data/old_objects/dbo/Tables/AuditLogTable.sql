CREATE TABLE [dbo].[AuditLogTable] (
    [AuditLogID]       INT            IDENTITY (1, 1) NOT NULL,
    [ParentKeyFieldID] BIGINT         NOT NULL,
    [ChildKeyFieldID]  BIGINT         NULL,
    [AuditActionType]  VARCHAR (10)   COLLATE Latin1_General_CI_AI NOT NULL,
    [DateTimeStamp]    DATETIME       NOT NULL,
    [DataModel]        NVARCHAR (100) COLLATE Latin1_General_CI_AI NOT NULL,
    [Changes]          NVARCHAR (MAX) COLLATE Latin1_General_CI_AI NOT NULL,
    [ValueBefore]      NVARCHAR (MAX) COLLATE Latin1_General_CI_AI NOT NULL,
    [ValueAfter]       NVARCHAR (MAX) COLLATE Latin1_General_CI_AI NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [CreatedBy]        BIGINT         NOT NULL,
    [UpdatedDate]      DATETIME       NOT NULL,
    [UpdatedBy]        BIGINT         NOT NULL,
    [SystemID]         VARCHAR (100)  COLLATE Latin1_General_CI_AI NOT NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([AuditLogID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLogTable_ParentKeyFieldID_FF596]
    ON [dbo].[AuditLogTable]([ParentKeyFieldID] ASC);

