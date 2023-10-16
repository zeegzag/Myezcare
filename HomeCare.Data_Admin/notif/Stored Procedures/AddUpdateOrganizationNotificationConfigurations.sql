
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 06 Jul 2020
-- Description:	This SP is used to add/update notification configurations from admin DB to particular organization DB.
-- =============================================
CREATE PROCEDURE [notif].[AddUpdateOrganizationNotificationConfigurations]
	@OrganizationID BIGINT,
	@NotificationConfigurationIDs NVARCHAR(MAX),
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN
	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	
	DECLARE @EventTable NVARCHAR(600), @ConfigurationTable NVARCHAR(600), @IsError BIT, @ErrorMessage NVARCHAR(100);
	SET @EventTable = [notif].[GetOrganizationTableName](@OrganizationID, '[notif].[NotificationEvents]')
	SET @ConfigurationTable = [notif].[GetOrganizationTableName](@OrganizationID, '[notif].[NotificationConfigurations]')
	SET @IsError = IIF(LEFT(@ConfigurationTable, 7) = 'Error: ', 1, 0);
	IF (@IsError = 1)
		BEGIN
			SET @ErrorMessage = SUBSTRING(@ConfigurationTable, 8, LEN(@ConfigurationTable))
			RAISERROR (@ErrorMessage, 11, 1);
		END
	ELSE
		BEGIN
			DECLARE @cmd NVARCHAR(MAX)
			SET @cmd = 'MERGE 
							' + @EventTable + ' T
						USING (	SELECT
									[NotificationEventID],
									[EventName],
									[Description],
									[EventDefinitionSP],
									@CurrDateTime [CurrDateTime],
									@LoggedInUserID [LoggedInUserID],
									@SystemID [SystemID],
									[IsDeleted]
								FROM
									[notif].[NotificationEvents] NE
								WHERE
									[NotificationEventID] IN ( SELECT DISTINCT [NotificationEventID]
																FROM
																	[notif].[NotificationConfigurations] NC
																INNER JOIN [dbo].[GetCSVTable](@NotificationConfigurationIDs) CL
																	ON NC.NotificationConfigurationID = CL.val )
								 ) S
						ON 
							T.[NotificationEventID] = S.[NotificationEventID]
						WHEN MATCHED THEN
							UPDATE SET
								[EventName] = S.[EventName],
								[Description] = S.[Description],
								[EventDefinitionSP] = S.[EventDefinitionSP],
								[UpdatedDate] = S.[CurrDateTime],
								[UpdatedBy] = S.[LoggedInUserID],
								[SystemID] = S.[SystemID]
						WHEN NOT MATCHED BY TARGET THEN
							INSERT
							(
								[NotificationEventID],
								[EventName],
								[Description],
								[EventDefinitionSP],
								[CreatedDate],
								[CreatedBy],
								[UpdatedDate],
								[UpdatedBy],
								[SystemID]
							)
							VALUES
							(
								S.[NotificationEventID],
								S.[EventName],
								S.[Description],
								S.[EventDefinitionSP],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[SystemID]
							);
				

						MERGE 
							' + @ConfigurationTable + ' T
						USING (	SELECT
									[NotificationConfigurationID],
									[ConfigurationName],
									[Description],
									[NotificationEventID],
									NULL [EmailTemplateID],
									NULL [SMSTemplateID],
									@CurrDateTime [CurrDateTime],
									@LoggedInUserID [LoggedInUserID],
									@SystemID [SystemID],
									[IsDeleted]
								FROM
									[notif].[NotificationConfigurations] NC
								INNER JOIN [dbo].[GetCSVTable](@NotificationConfigurationIDs) CL
									ON NC.NotificationConfigurationID = CL.val ) S
						ON 
							T.[NotificationConfigurationID] = S.[NotificationConfigurationID]
						WHEN MATCHED THEN
							UPDATE SET
								[ConfigurationName] = S.[ConfigurationName],
								[Description] = S.[Description],
								[NotificationEventID] = S.[NotificationEventID],
								[UpdatedDate] = S.[CurrDateTime],
								[UpdatedBy] = S.[LoggedInUserID],
								[SystemID] = S.[SystemID],
								[IsDeleted] = S.[IsDeleted]
						WHEN NOT MATCHED BY TARGET THEN
							INSERT
							(
								[NotificationConfigurationID],
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
								S.[NotificationConfigurationID],
								S.[ConfigurationName],
								S.[Description],
								S.[NotificationEventID],
								S.[EmailTemplateID],
								S.[SMSTemplateID],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[SystemID]
							);';

			EXEC sp_executesql @cmd, 
							   N'@NotificationConfigurationIDs NVARCHAR(MAX),
								 @CurrDateTime DATETIME,
								 @LoggedInUserID BIGINT,
								 @SystemID VARCHAR(100)', 
							   @NotificationConfigurationIDs = @NotificationConfigurationIDs, 
							   @CurrDateTime = @CurrDateTime, 
							   @LoggedInUserID = @LoggedInUserID, 
							   @SystemID = @SystemID;
		END

END