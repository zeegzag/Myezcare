CREATE PROC [dbo].[GetReferralNotesById]
@ReferralId INT,@Month nvarchar(200), @Year int
AS BEGIN

	select 
	 ReferralActivityNoteId,Date,Description,Initials
	from ReferralActivityNotes a
	join ReferralActivityMaster b on a.ReferralActivityMasterId=b.ReferralActivityMasterId
	where [Month]=@Month and [Year]= @Year and ReferralId=@ReferralId
END