CREATE TYPE [notif].[EventDataTable] AS TABLE
(
	[RefID] BIGINT,
	[RefTableName] VARCHAR(200),
	[TemplateData] VARCHAR(MAX)
)
GO

CREATE TYPE [notif].[NotificationEventDataTable] AS TABLE
(
	[NotificationEventDataID] UNIQUEIDENTIFIER,
	[NotificationConfigurationID] BIGINT,
	[NotificationEventID] BIGINT, 
	[EmailTemplateID] BIGINT, 
	[SMSTemplateID] BIGINT,
	[RefID] BIGINT,
	[RefTableName] VARCHAR(200),
	[TemplateData] VARCHAR(MAX)
)
GO

CREATE TYPE [notif].[NotificationsTable] AS TABLE
(
	[NotificationID] BIGINT,
	[NotificationEventDataID] UNIQUEIDENTIFIER
)
GO

CREATE TYPE [notif].[NotificationReceiversDataTable] AS TABLE
(
	[NotificationConfigurationID] BIGINT,
	[NotificationID]  BIGINT,
	[EmailRecipients] VARCHAR(MAX),
	[EmailSubject] VARCHAR(500),
	[EmailBody]	VARCHAR(MAX),
	[WebNotificationEmployeeIds] VARCHAR(MAX),
	[MobileAppNotificationEmployeeIds] VARCHAR(MAX),
	[SMSRecipients] VARCHAR(MAX),
	[SMSText] VARCHAR(MAX)
)
GO