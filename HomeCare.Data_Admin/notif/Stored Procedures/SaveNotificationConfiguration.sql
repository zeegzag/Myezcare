
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to save notification configuration.
-- =============================================
CREATE PROCEDURE [notif].[SaveNotificationConfiguration]
	@NotificationConfigurationID BIGINT,
	@ConfigurationName NVARCHAR(100),
	@Description NVARCHAR(500),
	@NotificationEventID BIGINT,
	@EmailTemplateID BIGINT,
	@SMSTemplateID BIGINT,
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	IF EXISTS (SELECT TOP 1 [NotificationConfigurationID] FROM [notif].[NotificationConfigurations] WHERE [ConfigurationName] = @ConfigurationName AND [NotificationConfigurationID] != @NotificationConfigurationID)
		BEGIN
			SELECT -1 As TransactionResultId; RETURN;
		END

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	IF (@NotificationConfigurationID = 0)
		BEGIN
			INSERT INTO [notif].[NotificationConfigurations]
			(
				[ConfigurationName],
				[Description],
				[NotificationEventID],
				[EmailTemplateID],
				[SMSTemplateID],
				[CreatedDate],
				[CreatedBy],
				[UpdatedDate],
				[UpdatedBy],
				[SystemID]
			)
			VALUES
			(
				@ConfigurationName,
				@Description,
				@NotificationEventID,
				@EmailTemplateID,
				@SMSTemplateID,
				@CurrDateTime,
				@LoggedInUserID,
				@CurrDateTime,
				@LoggedInUserID,
				@SystemID
			);
		END
	ELSE
		BEGIN
			UPDATE
				[notif].[NotificationConfigurations]
			SET
				[ConfigurationName] = @ConfigurationName,
				[Description] = @Description,
				[NotificationEventID] = @NotificationEventID,
				[EmailTemplateID] = @EmailTemplateID,
				[SMSTemplateID] = @SMSTemplateID,
				[UpdatedDate] = @CurrDateTime,
				[UpdatedBy] = @LoggedInUserID,
				[SystemID] = @SystemID
			WHERE
				[NotificationConfigurationID] = @NotificationConfigurationID;
		END

	SELECT 1 As TransactionResultId; RETURN;

END