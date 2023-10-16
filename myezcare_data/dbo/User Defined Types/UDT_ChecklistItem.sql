CREATE TYPE [dbo].[UDT_ChecklistItem] AS TABLE (
    [ChecklistItemTypeID]        INT    NOT NULL,
    [ChecklistItemID]            BIGINT NOT NULL,
    [ChecklistItemTypePrimaryID] BIGINT NOT NULL,
    [IsChecked]                  BIT    NULL);

