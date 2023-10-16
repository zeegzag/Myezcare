CREATE PROCEDURE [dbo].[SaveReferralActivityNotes]
	@ReferralId NVARCHAR(MAX) = NULL,
	@Month NVARCHAR(200),@Year int,
	@Description NVARCHAR(MAX),
	@Initials NVARCHAR(500)
AS
BEGIN
	
	DECLARE @ReferralActivityMasterId BIGINT
	
	SELECT @ReferralActivityMasterId=ReferralActivityMasterId FROM ReferralActivityMaster
	WHERE [Month]=@Month AND [YEAR]=@Year AND ReferralId=@ReferralId
		
	IF(@ReferralActivityMasterId IS NOT NULL)
	BEGIN
	INSERT INTO [dbo].[ReferralActivityNotes]
	( ReferralActivityMasterId, Date, Description, Initials, CreatedBy)
	SELECT  @ReferralActivityMasterId, GETDATE(),@Description, @Initials, null
	END
	
END
