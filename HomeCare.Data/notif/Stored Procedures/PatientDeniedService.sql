CREATE PROCEDURE [notif].[PatientDeniedService]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @PatientDeniedService [notif].[EventDataTable];

	INSERT INTO @PatientDeniedService
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData],
            NULL [DefaultRecipients]
		FROM
			[dbo].[ReferralTimeSlotDates] RTSD
		CROSS APPLY ( SELECT RTSD.[ReferralTSDateID] [RefID], '[dbo].[ReferralTimeSlotDates]' [RefTableName] ) REF
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		CROSS APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = RTSD.[ReferralID]
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					CONVERT(VARCHAR(20), CAST(RTSD.ReferralTSStartTime AS DATE), 0) [##DropOffDay##],
					CONVERT(VARCHAR(10), CAST(RTSD.ReferralTSStartTime AS TIME), 0) [##DropOffTime##],
					PD.[PatientName] [##PatientName##],
					'Patient Denied Service' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
            AND RTSD.IsDenied = 1
            AND RTSD.ReferralTSStartTime >= @FromDateTime;

	SELECT * FROM @PatientDeniedService;

END