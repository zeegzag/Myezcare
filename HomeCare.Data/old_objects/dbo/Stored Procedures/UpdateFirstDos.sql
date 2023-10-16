CREATE PROCEDURE [dbo].[UpdateFirstDos]  
 -- Add the parameters for the stored procedure here  
 @ReferralID bigint
 AS  
BEGIN  
	DECLARE @referralDOS date
	DECLARE @noteDOS date

	select @referralDOS = FirstDos from Referrals where ReferralID=@ReferralID

	select top 1 @noteDOS = ServiceDate from Notes where ReferralID=@ReferralID AND IsDeleted=0 order by NoteID DESC
	
	--update Referrals set FirstDos=(select top 1 ServiceDate from Notes where ReferralID=@ReferralID and IsBillable=1 AND IsDeleted=0 order by StartTime ASC)
	--where ReferralID=@ReferralID

	if(@referralDOS is null OR @noteDOS < @referralDOS)
	BEGIN
		update Referrals set FirstDos=@noteDOS where ReferralID=@ReferralID
	END
	
END
