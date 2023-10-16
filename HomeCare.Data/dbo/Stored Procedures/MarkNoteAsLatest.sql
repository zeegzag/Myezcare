CREATE PROCEDURE [dbo].[MarkNoteAsLatest]
@BatchNoteID as bigint,
@BatchID as bigint,
@NoteID as bigint
AS
BEGIN

UPDATE BatchNotes  SET MarkAsLatest=0 WHERE BatchID=@BatchID AND NoteID=@NoteID;
UPDATE BatchNotes  SET MarkAsLatest=1 WHERE BatchID=@BatchID AND NoteID=@NoteID AND BatchNoteID=@BatchNoteID;


END