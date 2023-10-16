
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to update processed notifications. (called by CronJob)
-- =============================================
CREATE PROCEDURE [notif].[UpdateProcessedNotifications]
	@EmailSentIDs NVARCHAR(MAX),
	@SMSSentIDs NVARCHAR(MAX),
	@WebNotificationSentIDs NVARCHAR(MAX),
	@MobileAppNotificationSentIDs NVARCHAR(MAX),
	@ProcessedIDs NVARCHAR(MAX)
AS
BEGIN

	BEGIN TRANSACTION trans
	BEGIN TRY
		UPDATE N
			SET N.[IsEmailSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@EmailSentIDs) ES
			ON ES.[val] = N.NotificationID

		UPDATE N
			SET N.[IsSMSSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@SMSSentIDs) SS
			ON SS.[val] = N.NotificationID

		UPDATE N
			SET N.[IsWebNotificationSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@WebNotificationSentIDs) WAS
			ON WAS.[val] = N.NotificationID

		UPDATE N
			SET N.[IsMobileAppNotificationSent] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@MobileAppNotificationSentIDs) MAS
			ON MAS.[val] = N.NotificationID

		UPDATE N
			SET N.[IsProcessed] = 1
		FROM
			[notif].[Notifications] N
		INNER JOIN [dbo].[GetCSVTable](@ProcessedIDs) P
			ON P.[val] = N.NotificationID

		SELECT 1 AS TransactionResultId;

		IF (@@TRANCOUNT > 0)
			BEGIN
				COMMIT TRANSACTION trans
			END
	END TRY
	BEGIN CATCH
		SELECT -1 AS TransactionResultId;

		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION trans
			END
	END CATCH

END