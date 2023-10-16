CREATE PROCEDURE [dbo].[EditDeleteReferralActivityNotes]
	@ReferralActivityNoteId int,
	@Description NVARCHAR(MAX)=null,
	@Initials NVARCHAR(500)=null,
	@AddOrEdit NVARCHAR(100)
AS
BEGIN
	
	IF(@AddOrEdit='DELETE')
	BEGIN
		DELETE FROM [ReferralActivityNotes]
		WHERE ReferralActivityNoteId=@ReferralActivityNoteId
	END	

	IF(@AddOrEdit='EDIT')
	BEGIN
		UPDATE [ReferralActivityNotes]
			SET Description=@Description,Initials=@Initials
		WHERE ReferralActivityNoteId=@ReferralActivityNoteId
	END	
END