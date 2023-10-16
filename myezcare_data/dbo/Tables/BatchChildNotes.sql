CREATE TABLE [dbo].[BatchChildNotes] (
    [BatchChildNoteID] BIGINT     IDENTITY (1, 1) NOT NULL,
    [ChildNoteID]      BIGINT     NOT NULL,
    [ParentNoteID]     BIGINT     NOT NULL,
    [NoteID]           BIGINT     NOT NULL,
    [CalculatedUnit]   BIGINT     NOT NULL,
    [CalculatedAmount] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_BatchChildNotes] PRIMARY KEY CLUSTERED ([BatchChildNoteID] ASC)
);

