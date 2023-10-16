
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to get notification event by id.
-- =============================================
CREATE PROCEDURE [notif].[GetNotificationEventById]
	@NotificationEventID BIGINT
AS
BEGIN

	SELECT 
		* 
	FROM 
		[notif].[NotificationEvents] 
	WHERE 
		[NotificationEventID] = @NotificationEventID;

END