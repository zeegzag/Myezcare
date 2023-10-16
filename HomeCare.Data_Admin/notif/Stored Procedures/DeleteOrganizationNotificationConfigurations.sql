
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 06 Jul 2020
-- Description:	This SP is used to delete notification configurations from particular organization DB.
-- =============================================
CREATE PROCEDURE [notif].[DeleteOrganizationNotificationConfigurations]
	@OrganizationID BIGINT,
	@NotificationConfigurationIDs NVARCHAR(MAX),
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();

	DECLARE @ConfigurationTable NVARCHAR(600), @IsError BIT, @ErrorMessage NVARCHAR(100);
	SET @ConfigurationTable = [notif].[GetOrganizationTableName](@OrganizationID, '[notif].[NotificationConfigurations]')
	SET @IsError = IIF(LEFT(@ConfigurationTable, 7) = 'Error: ', 1, 0);
	IF (@IsError = 1)
		BEGIN
			SET @ErrorMessage = SUBSTRING(@ConfigurationTable, 8, LEN(@ConfigurationTable))
			RAISERROR (@ErrorMessage, 11, 1);
		END
	ELSE
		BEGIN
			DECLARE @cmd NVARCHAR(1000)
			SET @cmd = 'UPDATE NC
						SET
				 			[IsDeleted] = 1,
				 			[UpdatedDate] = @CurrDateTime,
				 			[UpdatedBy] = @LoggedInUserID,
				 			[SystemID] = @SystemID
						FROM
				 			' + @ConfigurationTable + ' NC
						INNER JOIN [dbo].[GetCSVTable](@NotificationConfigurationIDs) CL
				 			ON NC.NotificationConfigurationID = CL.val';

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