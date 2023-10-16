CREATE PROCEDURE [notif].[SendSurveyAlert]
  @NotificationConfigurationID bigint,
  @NotificationEventID bigint,
  @FromDateTime datetime,
  @ToDateTime datetime,
  @BaseURL nvarchar(100)
AS
BEGIN

  DECLARE @SendSurveyAlert [notif].[EventDataTable];

  INSERT INTO @SendSurveyAlert
    SELECT
      REF.[RefID],
      REF.[RefTableName],
      TD.[Data] [TemplateData],
      NULL [DefaultRecipients]
    FROM [dbo].[EmployeeVisitNotes] EVN
    INNER JOIN [dbo].[ReferralTaskMappings] RTM
      ON RTM.[ReferralTaskMappingID] = EVN.[ReferralTaskMappingID]
    INNER JOIN [dbo].[VisitTasks] VT
      ON VT.[VisitTaskID] = RTM.[VisitTaskID]
      AND VT.[SendAlert] = 1
    INNER JOIN [dbo].[EmployeeVisits] EV
      ON EV.[EmployeeVisitID] = EVN.[EmployeeVisitID]
    INNER JOIN [dbo].[ScheduleMasters] SM
      ON SM.[ScheduleID] = EV.[ScheduleID]
    CROSS APPLY
    (
      SELECT
        EVN.[EmployeeVisitNoteID] [RefID],
        '[dbo].[EmployeeVisitNotes]' [RefTableName]
    ) REF
    LEFT JOIN [notif].[Notifications] N
      ON N.RefID = REF.RefID
      AND N.RefTableName = REF.RefTableName
      AND N.NotificationEventID = @NotificationEventID
      AND N.NotificationConfigurationID = @NotificationConfigurationID
    CROSS APPLY
    (
      SELECT
        E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
      FROM [dbo].[Employees] E
      WHERE
        E.[EmployeeID] = SM.[EmployeeID]
    ) ED
    CROSS APPLY
    (
      SELECT
        R.[FirstName] + ' ' + R.[LastName] [PatientName]
      FROM [dbo].[Referrals] R
      WHERE
        R.[ReferralID] = SM.[ReferralID]
    ) PD
    CROSS APPLY
    (
      SELECT
        (
          SELECT
            PD.PatientName [##PatientName##],
            ED.[EmployeeName] [##EmployeeName##],
            EVN.[AlertComment] [##Message##],
            'Send Survey Alert' [##EventName##]
          FOR JSON PATH  
        )
        [Data]
    ) TD
    WHERE
      N.NotificationID IS NULL
      AND EVN.[AlertComment] IS NOT NULL
      AND EVN.[Description] LIKE 'yes';

  SELECT
    *
  FROM @SendSurveyAlert;

END