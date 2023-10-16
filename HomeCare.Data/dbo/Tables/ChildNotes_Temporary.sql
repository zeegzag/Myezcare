
CREATE TABLE [dbo].[ChildNotes_Temporary] (
    [ChildNoteID]      BIGINT     IDENTITY (1, 1) NOT NULL,
    [ParentNoteID]     BIGINT     NOT NULL,
    [NoteID]           BIGINT     NOT NULL,
    [CalculatedUnit]   BIGINT     NOT NULL,
    [CalculatedAmount] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ChildNoteID] PRIMARY KEY CLUSTERED ([ChildNoteID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [missing_index_210842_210841_ChildNotes_Temporary]
    ON [dbo].[ChildNotes_Temporary]([NoteID] ASC);

