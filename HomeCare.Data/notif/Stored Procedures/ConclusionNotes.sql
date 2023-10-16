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
			TD.[Data] [TemplateData],
            NULL [DefaultRecipients]
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