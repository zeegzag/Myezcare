
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to get notification configuration by id.
-- =============================================
CREATE PROCEDURE [notif].[GetNotificationConfigurationById]
	@NotificationConfigurationID BIGINT
AS
BEGIN

	SELECT 
		* 
	FROM 
		[notif].[NotificationConfigurations] 
	WHERE 
		[NotificationConfigurationID] = @NotificationConfigurationID;

END