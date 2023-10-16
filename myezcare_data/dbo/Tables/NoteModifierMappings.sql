CREATE TABLE [dbo].[NoteModifierMappings] (
    [NoteModifierID] BIGINT IDENTITY (1, 1) NOT NULL,
    [NoteID]         BIGINT NOT NULL,
    [ModifierID]     BIGINT NOT NULL,
    CONSTRAINT [PK_NoteModifierMapping] PRIMARY KEY CLUSTERED ([NoteModifierID] ASC)
);

