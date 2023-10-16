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
                NED.[DefaultRecipients],
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
                [DefaultRecipients],
				[CreatedDate]
			)
			VALUES
			(
				S.[NotificationConfigurationID],
				S.[NotificationEventID],
				S.[RefID],
				S.[RefTableName],
				S.[TemplateData],
                S.[DefaultRecipients],
				S.[CreatedDate]
			)
	OUTPUT
		inserted.[NotificationID],
		S.[NotificationEventDataID]
	INTO 
		@Notifications;

END