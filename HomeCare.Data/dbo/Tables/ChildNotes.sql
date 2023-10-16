CREATE TABLE [dbo].[ChildNotes] (
    [ChildNoteID]      BIGINT     IDENTITY (1, 1) NOT NULL,
    [ParentNoteID]     BIGINT     NOT NULL,
    [NoteID]           BIGINT     NOT NULL,
    [CalculatedUnit]   BIGINT     NOT NULL,
    [CalculatedAmount] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ChildNotes] PRIMARY KEY CLUSTERED ([ChildNoteID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Notes_ChildNotes_ParentNoteID]
    ON [dbo].[ChildNotes]([ParentNoteID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Notes_ChildNotes]
    ON [dbo].[ChildNotes]([NoteID] ASC);

