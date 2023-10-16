CREATE TABLE [dbo].[ChildNotes] (
    [ChildNoteID]      BIGINT     IDENTITY (1, 1) NOT NULL,
    [ParentNoteID]     BIGINT     NOT NULL,
    [NoteID]           BIGINT     NOT NULL,
    [CalculatedUnit]   BIGINT     NOT NULL,
    [CalculatedAmount] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ChildNotes] PRIMARY KEY CLUSTERED ([ChildNoteID] ASC)
);

