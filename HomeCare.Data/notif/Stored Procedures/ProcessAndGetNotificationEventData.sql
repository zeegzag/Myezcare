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
				[TemplateData],
                [DefaultRecipients]
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