CREATE PROCEDURE [notif].[ScheduleCancellation]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @ScheduleCancellation [notif].[EventDataTable];

	INSERT INTO @ScheduleCancellation
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData],
            NULL [DefaultRecipients]
		FROM
			[dbo].[ScheduleMasters] SM
		CROSS APPLY ( SELECT SM.[ScheduleID] [RefID], '[dbo].[ScheduleMasters]' [RefTableName] ) REF
		INNER JOIN [dbo].[ReferralTimeSlotDates] RTDS
			ON RTDS.[ReferralTSDateID] = SM.[ReferralTSDateID]
			   AND SM.[IsDeleted] = 1
			   AND RTDS.[IsDenied] = 1
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		CROSS APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[Employees] E
			WHERE
				E.[EmployeeID] = SM.[EmployeeID]
		) ED
		CROSS APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = SM.[ReferralID]
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					CONVERT(VARCHAR(20), CAST(SM.StartDate AS DATE), 0) [##DropOffDay##],
					CONVERT(VARCHAR(10), CAST(SM.StartDate AS TIME), 0) [##DropOffTime##],
					PD.PatientName [##PatientName##],
					ED.[EmployeeName] [##EmployeeName##],
					'Schedule Cancellation' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND DATEADD(S, -1 * DATEDIFF(S, GETDATE(), GETUTCDATE()), SM.[UpdatedDate]) BETWEEN @FromDateTime AND @ToDateTime;

	SELECT * FROM @ScheduleCancellation;

END