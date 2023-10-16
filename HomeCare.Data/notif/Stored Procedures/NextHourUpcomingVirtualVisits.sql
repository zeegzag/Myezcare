CREATE PROCEDURE [notif].[NextHourUpcomingVirtualVisits]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @VirtualVisitsBefore1Hour [notif].[EventDataTable];

	INSERT INTO @VirtualVisitsBefore1Hour
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData],
            SM.EmployeeID [DefaultRecipients]
		FROM
			[dbo].[ScheduleMasters] SM
		CROSS APPLY ( SELECT SM.[ScheduleID] [RefID], '[dbo].[ScheduleMasters]' [RefTableName] ) REF
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
					CONVERT(VARCHAR(20), CAST(SM.StartDate AS DATE), 0) [##ScheduleDate##],
					CONVERT(VARCHAR(10), CAST(SM.StartDate AS TIME), 0) [##ScheduleTime##],
					PD.PatientName [##PatientName##],
					ED.[EmployeeName] [##EmployeeName##],
					'Upcoming Virtual Visits - Next Hour' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
        WHERE
			N.NotificationID IS NULL
            AND SM.IsVirtualVisit = 1
			AND DATEADD(HOUR, -1, SM.[StartDate]) BETWEEN @FromDateTime AND @ToDateTime;

	SELECT * FROM @VirtualVisitsBefore1Hour;

END