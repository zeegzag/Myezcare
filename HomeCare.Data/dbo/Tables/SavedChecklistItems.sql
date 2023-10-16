CREATE TABLE [dbo].[SavedChecklistItems] (
    [SavedChecklistItemID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [ChecklistItemTypeID]        INT           NOT NULL,
    [ChecklistItemID]            BIGINT        NOT NULL,
    [ChecklistItemTypePrimaryID] BIGINT        NOT NULL,
    [IsChecked]                  BIT           NOT NULL,
    [CreatedBy]                  BIGINT        NOT NULL,
    [CreatedDate]                DATETIME      NOT NULL,
    [UpdatedDate]                DATETIME      NOT NULL,
    [UpdatedBy]                  BIGINT        NOT NULL,
    [SystemID]                   VARCHAR (100) NOT NULL,
    [IsDeleted]                  BIT           NOT NULL,
    CONSTRAINT [PK_SavedChecklistItems] PRIMARY KEY CLUSTERED ([SavedChecklistItemID] ASC),
    CONSTRAINT [FK_SavedChecklistItems_ChecklistItems] FOREIGN KEY ([ChecklistItemID]) REFERENCES [dbo].[ChecklistItems] ([ChecklistItemID])
);

