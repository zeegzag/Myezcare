CREATE TABLE [dbo].[ChecklistItemDocuments] (
    [ChecklistItemDocumentID] BIGINT IDENTITY (1, 1) NOT NULL,
    [ChecklistItemID]         BIGINT NOT NULL,
    [ComplianceID]            BIGINT NOT NULL,
    CONSTRAINT [PK_ChecklistItemDocuments] PRIMARY KEY CLUSTERED ([ChecklistItemDocumentID] ASC),
    CONSTRAINT [FK_ChecklistItemDocuments_ChecklistItems] FOREIGN KEY ([ChecklistItemID]) REFERENCES [dbo].[ChecklistItems] ([ChecklistItemID])
);

