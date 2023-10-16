--EXEC GetDataForBroadcastNotification @Type = 'schedulenotification', @Id = '448728', @ScheduleNotification = 'schedulenotification'
CREATE PROCEDURE [dbo].[GetDataForBroadcastNotification]
	@Type NVARCHAR(500),
	@Id BIGINT,
	@ScheduleNotification NVARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;
	IF (@Type = @ScheduleNotification)
		BEGIN
			SELECT 1;
			SELECT 1;
			
			DECLARE @ReferralID BIGINT = (SELECT ReferralID FROM ReferralTimeSlotDates WHERE ReferralTsDateID=@Id)

			DECLARE @PreferenceNames NVARCHAR(MAX)
			select @PreferenceNames = COALESCE(@PreferenceNames + ',', '') + p.PreferenceName
			from ReferralPreferences rp
			INNER JOIN Preferences p ON p.PreferenceID=rp.PreferenceID AND p.KeyType='Preference'
			where ReferralID=@ReferralID

			SELECT r.ReferralID,rtsd.ReferralTsDateID,dbo.GetGeneralNameFormat(r.FirstName,r.LastName) AS PatientName,r.AHCCCSID,r.Dob,
			rtsd.ReferralTSStartTime,rtsd.ReferralTSEndTime,@PreferenceNames AS PreferenceNames
			FROM ReferralTimeSlotDates rtsd
			INNER JOIN dbo.Referrals r ON rtsd.ReferralID = r.ReferralID
			WHERE rtsd.ReferralTsDateID=@Id

		END
END
