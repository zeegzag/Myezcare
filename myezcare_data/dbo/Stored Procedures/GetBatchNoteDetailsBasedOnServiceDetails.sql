﻿CREATE PROCEDURE [dbo].[GetBatchNoteDetailsBasedOnServiceDetails]
@ServiceCode VARCHAR(MAX),
@BatchNoteID01 BIGINT=0,
@BatchNoteID02 BIGINT=0
AS
BEGIN

DECLARE @NoteID01 BIGINT=0;
SELECT @NoteID01=BN.NoteID FROM BatchNotes BN WHERE BN.BatchNoteID=@BatchNoteID01

DECLARE @NoteID02 BIGINT=0;
SELECT @NoteID02=BN.NoteID FROM BatchNotes BN WHERE BN.BatchNoteID=@BatchNoteID02


	IF EXISTS (SELECT N.NoteID  FROM Notes N WHERE N.ServiceCode=@ServiceCode AND N.NoteID=@NoteID01)
	BEGIN
	 SELECT @BatchNoteID01
	END
	ELSE IF EXISTS (SELECT N.NoteID  FROM Notes N WHERE N.ServiceCode=@ServiceCode AND N.NoteID=@NoteID02)
	BEGIN
	 SELECT @BatchNoteID02
	END
	ELSE 
	 SELECT 0
	
END
