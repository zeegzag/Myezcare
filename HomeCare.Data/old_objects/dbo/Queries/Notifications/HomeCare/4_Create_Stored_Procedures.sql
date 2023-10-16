-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Late clock in.
-- =============================================
CREATE PROCEDURE [notif].[LateClockIn]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @ClockInClockOut [notif].[EventDataTable];

	INSERT INTO @ClockInClockOut
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[ScheduleMasters] SM
		INNER JOIN [dbo].[EmployeeVisits] EV
			ON EV.[ScheduleID] = SM.[ScheduleID]
		CROSS APPLY ( SELECT EV.[EmployeeVisitID] [RefID], '[dbo].[EmployeeVisits]' [RefTableName] ) REF
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
					ED.[EmployeeName] [##EmployeeName##],
					PD.[PatientName] [##PatientName##],
					'Late Clock-in' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND SM.[IsDeleted] = 0
			AND EV.[IsDeleted] = 0
			AND EV.[ClockInTime] > SM.[StartDate]
			AND (EV.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR EV.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime); 

	SELECT * FROM @ClockInClockOut;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Late clock out.
-- =============================================
CREATE PROCEDURE [notif].[LateClockOut]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @ClockInClockOut [notif].[EventDataTable];

	INSERT INTO @ClockInClockOut
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[ScheduleMasters] SM
		INNER JOIN [dbo].[EmployeeVisits] EV
			ON EV.[ScheduleID] = SM.[ScheduleID]
		CROSS APPLY ( SELECT EV.[EmployeeVisitID] [RefID], '[dbo].[EmployeeVisits]' [RefTableName] ) REF
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
					ED.[EmployeeName] [##EmployeeName##],
					PD.[PatientName] [##PatientName##],
					'Late Clock-out' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND SM.[IsDeleted] = 0
			AND EV.[IsDeleted] = 0
			AND EV.[ClockOutTime] > SM.[EndDate]
			AND (EV.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR EV.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime); 

	SELECT * FROM @ClockInClockOut;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data early clock out.
-- =============================================
CREATE PROCEDURE [notif].[EarlyClockOut]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @EarlyClockOut [notif].[EventDataTable];

	INSERT INTO @EarlyClockOut
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[ScheduleMasters] SM
		INNER JOIN [dbo].[EmployeeVisits] EV
			ON EV.[ScheduleID] = SM.[ScheduleID]
		CROSS APPLY ( SELECT EV.[EmployeeVisitID] [RefID], '[dbo].[EmployeeVisits]' [RefTableName] ) REF
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
					ED.[EmployeeName] [##EmployeeName##],
					PD.[PatientName] [##PatientName##],
					'Early Clock-out' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND SM.[IsDeleted] = 0
			AND EV.[IsDeleted] = 0
			AND EV.[ClockOutTime] < SM.[EndDate]
			AND (EV.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR EV.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime); 

	SELECT * FROM @EarlyClockOut;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data conclusion notes.
-- =============================================
CREATE PROCEDURE [notif].[ConclusionNotes]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @ConclusionNotes [notif].[EventDataTable];

	INSERT INTO @ConclusionNotes
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[EmployeeVisitNotes] EVN
		CROSS APPLY ( SELECT EVN.[EmployeeVisitNoteID] [RefID], '[dbo].[EmployeeVisitNotes]' [RefTableName] ) REF
		INNER JOIN [dbo].[ReferralTaskMappings] RTM
			ON EVN.[ReferralTaskMappingID] = RTM.[ReferralTaskMappingID]
		INNER JOIN [dbo].[VisitTasks] VT
			ON RTM.[VisitTaskID] = VT.[VisitTaskID]
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		CROSS APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[EmployeeVisits] EV
			INNER JOIN [dbo].[ScheduleMasters] SM
				ON EV.ScheduleID = SM.ScheduleID
			INNER JOIN [dbo].[Employees] E
				ON E.EmployeeID = SM.EmployeeID
			WHERE
				 EV.EmployeeVisitID = EVN.EmployeeVisitID
		) ED
		CROSS APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = RTM.[ReferralID]
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					ED.[EmployeeName] [##EmployeeName##],
					PD.[PatientName] [##PatientName##],
					'Conclusion Notes' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND VT.[VisitTaskType] = 'Conclusion'
			AND EVN.[IsDeleted] = 0
			AND (EVN.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR EVN.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime); 

	SELECT * FROM @ConclusionNotes;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data expired documents.
-- =============================================
CREATE PROCEDURE [notif].[ExpiredDocuments]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @ExpiredDocuments [notif].[EventDataTable];

	INSERT INTO @ExpiredDocuments
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[ReferralDocuments] RD
		CROSS APPLY ( SELECT RD.[ReferralDocumentID] [RefID], '[dbo].[ReferralDocuments]' [RefTableName] ) REF
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		OUTER APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[Employees] E
			WHERE
				E.[EmployeeID] = RD.UserID
				AND RD.UserType = 2
		) ED
		OUTER APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = RD.UserID
				AND RD.UserType = 1
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					RD.[FileName] [##FileName##],
					ISNULL(ED.[EmployeeName], PD.[PatientName]) [##FullName##],
					'Expired Documents' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND CONVERT(DATETIME, RD.[ExpirationDate]) BETWEEN @FromDateTime AND @ToDateTime;

	SELECT * FROM @ExpiredDocuments;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data expiring documents before 15 days.
-- =============================================
CREATE PROCEDURE [notif].[NearExpiringDocumentsBefore15Days]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @NearExpiringDocumentsBefore15Days [notif].[EventDataTable];

	INSERT INTO @NearExpiringDocumentsBefore15Days
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[ReferralDocuments] RD
		CROSS APPLY ( SELECT RD.[ReferralDocumentID] [RefID], '[dbo].[ReferralDocuments]' [RefTableName] ) REF
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		OUTER APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[Employees] E
			WHERE
				E.[EmployeeID] = RD.UserID
				AND RD.UserType = 2
		) ED
		OUTER APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = RD.UserID
				AND RD.UserType = 1
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					RD.[FileName] [##FileName##],
					ISNULL(ED.[EmployeeName], PD.[PatientName]) [##FullName##],
					'Expiring Documents Before 15 Days'  [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND CONVERT(DATETIME, DATEADD(DAY, 15, RD.[ExpirationDate])) BETWEEN @FromDateTime AND @ToDateTime;

	SELECT * FROM @NearExpiringDocumentsBefore15Days;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data PTO Created by employee.
-- =============================================
CREATE PROCEDURE [notif].[PTOCreatedByEmployee]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @PTOCreatedByEmployee [notif].[EventDataTable];

	INSERT INTO @PTOCreatedByEmployee
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[EmployeeDayOffs] EDO
		CROSS APPLY ( SELECT EDO.[EmployeeDayOffID] [RefID], '[dbo].[EmployeeDayOffs]' [RefTableName] ) REF
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		CROSS APPLY (
			SELECT (	
				SELECT 
					[FirstName] [##FirstName##],
					[LastName] [##LastName##],
					CONVERT(VARCHAR, EDO.StartTime, 0) [##DayOffStartTime##],
					CONVERT(VARCHAR, EDO.EndTime, 0) [##DayOffEndTime##],
					CASE WHEN EDO.DayOffTypeID = 1 THEN 'Other' 
						WHEN EDO.DayOffTypeID = 2 THEN 'Sick'
						WHEN EDO.DayOffTypeID = 3 THEN 'Vacation'
					END [##DayOffType##],
					EDO.EmployeeComment [##Comment##],
					'PTO created' [##EventName##]
				FROM 
					[dbo].[Employees] E
				WHERE 
					E.[EmployeeID] = EDO.[EmployeeID] 
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND EDO.[IsDeleted] = 0
			AND EDO.[DayOffStatus] = 'InProgress'
			AND (EDO.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR EDO.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);

	SELECT * FROM @PTOCreatedByEmployee;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Patient Denied Service.
-- =============================================
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
			TD.[Data] [TemplateData]
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
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Patient on Hold.
-- =============================================
CREATE PROCEDURE [notif].[PatientOnHold]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @PatientOnHold [notif].[EventDataTable];

	INSERT INTO @PatientOnHold
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[ReferralOnHoldDetails] ROHD
		CROSS APPLY ( SELECT ROHD.[ReferralOnHoldDetailID] [RefID], '[dbo].[ReferralOnHoldDetails]' [RefTableName] ) REF
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
				E.[EmployeeID] = ROHD.[CreatedBy]
		) ED
		CROSS APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = ROHD.[ReferralID]
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					ED.[EmployeeName] [##EmployeeName##],
					PD.[PatientName] [##PatientName##],
					ROHD.PatientOnHoldReason [##OnHoldReason##],
					'Patient On Hold' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND ROHD.[IsDeleted] = 0
			AND (ROHD.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR ROHD.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);

	SELECT * FROM @PatientOnHold;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Schedule Cancellation.
-- =============================================
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
			TD.[Data] [TemplateData]
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
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Messaging.
-- =============================================
CREATE PROCEDURE [notif].[Messaging]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @Messaging [notif].[EventDataTable];

	INSERT INTO @Messaging
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData]
		FROM
			[dbo].[ReferralInternalMessages] RIM
		CROSS APPLY ( SELECT RIM.[ReferralInternalMessageID] [RefID], '[dbo].[ReferralInternalMessages]' [RefTableName] ) REF
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
				E.[EmployeeID] = RIM.CreatedBy
		) MB
		CROSS APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[Employees] E
			WHERE
				E.[EmployeeID] = RIM.Assignee
		) A
		CROSS APPLY (
			SELECT (	
				SELECT 
					RIM.Note [##Message##],
					MB.EmployeeName [##MessageBy##],
					A.EmployeeName [##Assignee##],
					'Internal Message' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND RIM.[IsDeleted] = 0
			AND RIM.[IsResolved] = 0
			AND (DATEADD(S, -1 * DATEDIFF(S, GETDATE(), GETUTCDATE()), RIM.[CreatedDate]) BETWEEN @FromDateTime AND @ToDateTime 
				 OR DATEADD(S, -1 * DATEDIFF(S, GETDATE(), GETUTCDATE()), RIM.[UpdatedDate]) BETWEEN @FromDateTime AND @ToDateTime);

	SELECT * FROM @Messaging;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SP is used to add notifications.
-- =============================================
CREATE PROCEDURE [notif].[AddNotifications]
	@CreationDateTime DATETIME,
	@NotificationEventData [notif].[NotificationEventDataTable] READONLY
AS
BEGIN
	DECLARE @Notifications [notif].[NotificationsTable];

	-- Insert into Notifications.
	MERGE 
		[notif].[Notifications] T
	USING (	SELECT 
				0 [NotificationID],
				NED.[NotificationEventDataID],
				NED.[NotificationConfigurationID],
				NED.[NotificationEventID],
				NED.[RefID],
				NED.[RefTableName],
				NED.[TemplateData],
				@CreationDateTime [CreatedDate]
			FROM
				@NotificationEventData NED ) S
	ON 
		T.[NotificationID] = S.[NotificationID]
	WHEN 
		NOT MATCHED THEN
			INSERT
			(
				[NotificationConfigurationID],
				[NotificationEventID],
				[RefID],
				[RefTableName],
				[TemplateData],
				[CreatedDate]
			)
			VALUES
			(
				S.[NotificationConfigurationID],
				S.[NotificationEventID],
				S.[RefID],
				S.[RefTableName],
				S.[TemplateData],
				S.[CreatedDate]
			)
	OUTPUT
		inserted.[NotificationID],
		S.[NotificationEventDataID]
	INTO 
		@Notifications;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SP is used to generate List of receivers and data to send notifications.
-- =============================================
CREATE PROCEDURE [notif].[GetReceiversAndDataToSend]
AS
BEGIN
	DECLARE @NotificationReceiversData [notif].[NotificationReceiversDataTable];

	-- Generate List of receivers and data to send notifications.
	WITH [NotificationEventData] AS
	(
		SELECT 
			N.[NotificationID],
			NC.[NotificationConfigurationID],
			NC.[EmailTemplateID],
			NC.[SMSTemplateID],
			N.[TemplateData],
			N.[IsEmailSent],
			N.[IsSMSSent],
			N.[IsWebNotificationSent],
			N.[IsMobileAppNotificationSent]
		FROM 
			[notif].[Notifications] N
		INNER JOIN [notif].[NotificationConfigurations] NC
			ON NC.[NotificationConfigurationID] = N.[NotificationConfigurationID]
		WHERE
			N.[IsProcessed] = 0
	),
	[RoleMappings] AS
	(
		SELECT 
			NCRM.NotificationConfigurationID,
			STRING_AGG(NCRM.RoleID, ',') RoleIDs
		FROM 
			[notif].[NotificationConfigurationRoleMapping] NCRM
		WHERE 
			NCRM.[NotificationConfigurationID] IN (SELECT DISTINCT [NotificationConfigurationID] FROM [NotificationEventData])
		GROUP BY
			NCRM.NotificationConfigurationID
	),
	[EmpWithPref] AS
	(
		SELECT 
			E.EmployeeID,
			E.RoleID,
			E.Email,
			E.MobileNumber,
			ENP.SendEmail,
			ENP.SendSMS,
			ENP.WebNotification,
			ENP.MobileAppNotification
		FROM 
			[dbo].[Employees] E
		INNER JOIN 
			[notif].[EmployeeNotificationPreferences] ENP
		ON 
			E.EmployeeID = ENP.EmployeeID
		WHERE 
			E.IsActive = 1
			AND E.IsDeleted = 0
			AND E.RoleID IN (SELECT DISTINCT RoleID FROM [RoleMappings])
	),
	[Receivers] AS
	(
		SELECT 
			RM.NotificationConfigurationID,
			ER.EmailRecipients,
			SR.SMSRecipients,
			WR.WebNotificationEmployeeIds,
			MR.MobileAppNotificationEmployeeIds
		FROM 
			[RoleMappings] RM
		CROSS APPLY (
			SELECT 
				STRING_AGG(EWP.Email, ';') [EmailRecipients] 
			FROM 
				[EmpWithPref] EWP 
			WHERE 
				EWP.RoleID IN (SELECT [val] FROM [dbo].[GetCSVTable](RM.RoleIDs))
				AND EWP.SendEmail = 1
				AND EWP.Email IS NOT NULL
		) ER
		CROSS APPLY (
			SELECT 
				STRING_AGG(EWP.MobileNumber, ';') [SMSRecipients] 
			FROM 
				[EmpWithPref] EWP 
			WHERE 
				EWP.RoleID IN (SELECT [val] FROM [dbo].[GetCSVTable](RM.RoleIDs))
				AND EWP.SendSMS = 1 
				AND EWP.MobileNumber IS NOT NULL
		) SR
		CROSS APPLY (
			SELECT 
				STRING_AGG(EWP.EmployeeID, ',') WebNotificationEmployeeIds
			FROM 
				[EmpWithPref] EWP 
			WHERE 
				EWP.RoleID IN (SELECT [val] FROM [dbo].[GetCSVTable](RM.RoleIDs))
				AND EWP.WebNotification = 1 
		) WR
		CROSS APPLY (
			SELECT 
				STRING_AGG(EWP.EmployeeID, ',') MobileAppNotificationEmployeeIds
			FROM 
				[EmpWithPref] EWP 
			WHERE 
				EWP.RoleID IN (SELECT [val] FROM [dbo].[GetCSVTable](RM.RoleIDs))
				AND EWP.MobileAppNotification = 1 
		) MR
	),
	[Notifications] AS
	(
		SELECT 
			NED.[NotificationConfigurationID],
			NED.[NotificationID],
			NED.[IsEmailSent],
			NED.[IsSMSSent],
			NED.[IsWebNotificationSent],
			NED.[IsMobileAppNotificationSent],
			ET.*,
			ST.*
		FROM 
			[NotificationEventData] NED
		OUTER APPLY (
			SELECT
				[Subject] [EmailSubject], 
				[Body] [EmailBody]
			FROM 
				[notif].[GetMessage](NED.[EmailTemplateID], NED.[TemplateData])
		) ET
		OUTER APPLY (
			SELECT
				[Body] [SMSText]
			FROM 
				[notif].[GetMessage](NED.[SMSTemplateID], NED.[TemplateData])
		) ST	
	)
	INSERT INTO @NotificationReceiversData
	SELECT 
		N.[NotificationConfigurationID],
		N.[NotificationID],
		CASE WHEN N.[IsEmailSent] = 0 THEN R.[EmailRecipients] ELSE NULL END,
		N.[EmailSubject],
		N.[EmailBody],
		CASE WHEN N.[IsWebNotificationSent] = 0 THEN R.[WebNotificationEmployeeIds] ELSE NULL END,
		CASE WHEN N.[IsMobileAppNotificationSent] = 0 THEN R.[MobileAppNotificationEmployeeIds] ELSE NULL END,
		CASE WHEN N.[IsSMSSent] = 0 THEN R.[SMSRecipients] ELSE NULL END,
		N.[SMSText] 
	FROM 
		[Notifications] N
	INNER JOIN 
		[Receivers] R
	ON
		N.NotificationConfigurationID = R.NotificationConfigurationID;

	SELECT * FROM @NotificationReceiversData;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SP is used to process and get data related to all triggered events in the given period of time.
-- =============================================
CREATE PROCEDURE [notif].[ProcessAndGetNotificationEventData]
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(MAX)
AS
BEGIN
	DECLARE @NotificationEventData [notif].[NotificationEventDataTable];
	
	-- Declare required fields to fetch for processing notifications.
	DECLARE 
		@NotificationConfigurationID BIGINT, 
		@NotificationEventID BIGINT,
		@EventDefinitionSP NVARCHAR(200),
		@EmailTemplateID BIGINT,
		@SMSTemplateID BIGINT;

	-- Declare cursor to fetch fields for processing notifications.
	DECLARE CursorNotifConfig CURSOR
	FOR SELECT 
			NC.[NotificationConfigurationID], 
			NC.[NotificationEventID], 
			NE.[EventDefinitionSP],
			NC.[EmailTemplateID], 
			NC.[SMSTemplateID]
		FROM 
			[notif].[NotificationConfigurations] NC
		INNER JOIN [notif].[NotificationEvents] NE
			ON NC.NotificationEventID = NE.NotificationEventID
		WHERE
			NC.[IsDeleted] = 0;

	-- Open cursor to fetch data.
	OPEN CursorNotifConfig;

	-- Fetch fields.
	FETCH NEXT FROM CursorNotifConfig INTO 
		@NotificationConfigurationID, 
		@NotificationEventID,
		@EventDefinitionSP,
		@EmailTemplateID,
		@SMSTemplateID;

	-- Create Table
	DECLARE @EventData [notif].[EventDataTable];
	-- Loop the cursor.
	WHILE @@FETCH_STATUS = 0
		BEGIN
			
			-- Get the event data.
			IF (OBJECT_ID(@EventDefinitionSP) IS NOT NULL)
				BEGIN
					DECLARE @cmd NVARCHAR(MAX) = N'EXEC ' + @EventDefinitionSP + ' @NotificationConfigurationID, @NotificationEventID, @FromDateTime, @ToDateTime, @BaseURL';
					INSERT INTO @EventData
						EXEC sp_executesql @cmd, 
										   N'@NotificationConfigurationID BIGINT, 
											 @NotificationEventID BIGINT,
											 @FromDateTime DATETIME,
											 @ToDateTime DATETIME,
											 @BaseURL NVARCHAR(MAX)', 
										   @NotificationConfigurationID = @NotificationConfigurationID,
										   @NotificationEventID = @NotificationEventID,
										   @FromDateTime = @FromDateTime, 
										   @ToDateTime = @ToDateTime,
										   @BaseURL = @BaseURL;
				END

			-- Temo insert the notifications events with data.
			INSERT INTO @NotificationEventData
			SELECT
				NEWID() [NotificationEventDataID],
				@NotificationConfigurationID [NotificationConfigurationID],
				@NotificationEventID [NotificationEventID],
				@EmailTemplateID [EmailTemplateID],
				@SMSTemplateID [SMSTemplateID],
				[RefID],
				[RefTableName],
				[TemplateData]
			FROM 
				@EventData

			-- Clear Temp Data
			DELETE FROM @EventData;

			-- Fetch fields.
			FETCH NEXT FROM CursorNotifConfig INTO 
				@NotificationConfigurationID, 
				@NotificationEventID,
				@EventDefinitionSP,
				@EmailTemplateID,
				@SMSTemplateID;
		END;
	
	-- Close cursor.
	CLOSE CursorNotifConfig;

	-- Deallocate curser.
	DEALLOCATE CursorNotifConfig;

	-- Add notifications and get IDs for refrence.
	EXEC [notif].[AddNotifications] @CreationDateTime = @ToDateTime, 
										@NotificationEventData = @NotificationEventData;

	-- Get List of receivers and data to send notifications.
	EXEC [notif].[GetReceiversAndDataToSend]

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SP is used to process notification and return details. (called by CronJob)
-- =============================================
CREATE PROCEDURE [notif].[GetConfiguredNotifications]
	@JobID UNIQUEIDENTIFIER,
	@BaseURL NVARCHAR(MAX)
AS
BEGIN
	DECLARE @CurrDateTime DATETIME = GETDATE();
	DECLARE @LastSuccessRunDateTime DATETIME = NULL;	
	SELECT TOP 1
		@LastSuccessRunDateTime = CONVERT(DATETIME, RTRIM([run_date]) + ' ' + STUFF(STUFF(REPLACE(STR(RTRIM([run_time]),6,0), ' ', '0'), 3, 0, ':'), 6, 0, ':'))
	FROM
		[msdb].[dbo].[sysjobhistory]
	WHERE
		[run_status] = 1
		AND [job_id] = @JobID
	ORDER BY 
		[instance_id] DESC;

	IF (@LastSuccessRunDateTime IS NOT NULL)
		BEGIN

			-- Get notifications event data.
			EXEC [notif].[ProcessAndGetNotificationEventData] @FromDateTime = @LastSuccessRunDateTime,
															  @ToDateTime = @CurrDateTime,
															  @BaseURL = @BaseURL;

		END;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to update processed notifications. (called by CronJob)
-- =============================================
CREATE PROCEDURE [notif].[UpdateProcessedNotifications]
	@EmailSentIDs NVARCHAR(MAX),
	@SMSSentIDs NVARCHAR(MAX),
	@WebNotificationSentIDs NVARCHAR(MAX),
	@MobileAppNotificationSentIDs NVARCHAR(MAX),
	@ProcessedIDs NVARCHAR(MAX)
AS
BEGIN

	BEGIN TRANSACTION trans
	BEGIN TRY
		UPDATE N
			SET N.[IsEmailSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@EmailSentIDs) ES
			ON ES.[val] = N.NotificationID

		UPDATE N
			SET N.[IsSMSSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@SMSSentIDs) SS
			ON SS.[val] = N.NotificationID

		UPDATE N
			SET N.[IsWebNotificationSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@WebNotificationSentIDs) WAS
			ON WAS.[val] = N.NotificationID

		UPDATE N
			SET N.[IsMobileAppNotificationSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@MobileAppNotificationSentIDs) MAS
			ON MAS.[val] = N.NotificationID

		UPDATE N
			SET N.[IsProcessed] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@ProcessedIDs) P
			ON P.[val] = N.NotificationID

		SELECT 1 AS TransactionResultId;

		IF (@@TRANCOUNT > 0)
			BEGIN
				COMMIT TRANSACTION trans
			END
	END TRY
	BEGIN CATCH
		SELECT -1 AS TransactionResultId;

		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION trans
			END
	END CATCH

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SP is used to process notification. (called by SQL Job)
-- =============================================
CREATE PROCEDURE [notif].[ProcessNotifications]
	@JobID UNIQUEIDENTIFIER,
	@BaseURL NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Endpoint NVARCHAR(MAX) = @BaseURL + N'/CronJob/SendConfiguredNotifications/' + CONVERT(NVARCHAR(50), @JobID);
	DECLARE @JsonResponse NVARCHAR(MAX) = NULL;
	DECLARE @NL NVARCHAR(2) = CHAR(13);

	SELECT @JsonResponse = CURL.XGET(NULL, @Endpoint);

	DECLARE @IsSuccess BIT, @Message NVARCHAR(MAX), @Data NVARCHAR(MAX);
	SELECT
		@IsSuccess = R.[IsSuccess],
		@Message = R.[Message],
		@Data = R.[Data]
	FROM 
		OPENJSON(@JsonResponse)
		WITH (   
			[IsSuccess] BIT '$.IsSuccess',  
			[Message] NVARCHAR(MAX) '$.Message',  
			[Data] NVARCHAR(MAX) '$.Data'
		) R

	IF (@IsSuccess = 1)
		BEGIN
			PRINT @Message + @NL + @Data;
		END
	ELSE
		BEGIN
			RAISERROR (N'Some errors occurred!%sDetails: %s.', 11, 1, @NL, @Message);
		END
	
END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 03 Jul 2020
-- Description:	This SP is used to get role mapping for notification configuration.
-- =============================================
CREATE PROCEDURE [notif].[GetNotificationConfigurationDetails]
	@NotificationConfigurationID BIGINT
AS
BEGIN

	SELECT 
		R.*,
		CASE WHEN NCRM.[NotificationConfigurationRoleMappingID] IS NULL THEN 0 ELSE 1 END [IsSelected]
	FROM 
		[dbo].[Roles] R
	LEFT JOIN [notif].[NotificationConfigurationRoleMapping] NCRM
		ON R.RoleID = NCRM.RoleID
		AND NCRM.[NotificationConfigurationID] = @NotificationConfigurationID;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 03 Jul 2020
-- Description:	This SP is used to save role mapping and update templates.
-- =============================================
CREATE PROCEDURE [notif].[SaveNotificationConfigurationDetails]
	@NotificationConfigurationID BIGINT,
	@EmailTemplateID BIGINT,
	@SMSTemplateID BIGINT,
	@RoleIDs NVARCHAR(MAX),
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();

	-- Update Template
	UPDATE
		[notif].[NotificationConfigurations]
	SET
		[EmailTemplateID] = @EmailTemplateID,
		[SMSTemplateID] = @SMSTemplateID,
		[UpdatedDate] = @CurrDateTime,
		[UpdatedBy] = @LoggedInUserID,
		[SystemID] = @SystemID
	WHERE
		[NotificationConfigurationID] = @NotificationConfigurationID

	-- Update Roles
	MERGE 
		[notif].[NotificationConfigurationRoleMapping] T
	USING (	SELECT 
				@NotificationConfigurationID [NotificationConfigurationID],
				[val] [RoleID],
				@CurrDateTime [CurrDate],
				@LoggedInUserID [ChangedBy],
				@SystemID [SystemID]
			FROM
				[dbo].[GetCSVTable](@RoleIDs) ) S
	ON 
		T.[NotificationConfigurationID] = S.[NotificationConfigurationID]
		AND T.[RoleID] = S.[RoleID]
	WHEN MATCHED THEN
		UPDATE SET
			[NotificationConfigurationID] = S.[NotificationConfigurationID],
			[RoleID] = S.[RoleID],
			[UpdatedDate] = S.[CurrDate],
			[UpdatedBy] = S.[ChangedBy],
			[SystemID] = S.[SystemID]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT
		(
			[NotificationConfigurationID],
			[RoleID],
			[CreatedDate],
			[CreatedBy],
			[UpdatedDate],
			[UpdatedBy],
			[SystemID]
		)
		VALUES
		(
			S.[NotificationConfigurationID],
			S.[RoleID],
			S.[CurrDate],
			S.[ChangedBy],
			S.[CurrDate],
			S.[ChangedBy],
			S.[SystemID]
		)
	WHEN NOT MATCHED BY SOURCE 
			AND T.[NotificationConfigurationID] = @NotificationConfigurationID THEN
		DELETE;

END
GO