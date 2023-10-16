CREATE PROCEDURE [dbo].[DeleteCommonNote]
	@CommonNoteID bigint=null,
	@LoggedinUserID bigint 
AS 
BEGIN

	UPDATE [dbo].[CommonNotes]
	SET IsDeleted=1,UpdatedBy=@LoggedinUserID,UpdatedDate=GETUTCDATE()
	WHERE CommonnoteID=@CommonNoteID

END
