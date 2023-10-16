-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to save notification event.
-- =============================================
CREATE PROCEDURE [notif].[SaveNotificationEvent]
	@NotificationEventID BIGINT,
	@EventName NVARCHAR(100),
	@Description NVARCHAR(500),
	@EventDefinitionSP NVARCHAR (200) NULL,
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	IF EXISTS (SELECT TOP 1 [NotificationEventID] FROM [notif].[NotificationEvents] WHERE [EventName] = @EventName AND [NotificationEventID] != @NotificationEventID)
		BEGIN
			SELECT -1 As TransactionResultId; RETURN;
		END

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	IF (@NotificationEventID = 0)
		BEGIN
			INSERT INTO [notif].[NotificationEvents]
			(
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
				@EventName,
				@Description,
				@EventDefinitionSP,
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
				[notif].[NotificationEvents]
			SET
				[EventName] = @EventName,
				[Description] = @Description,
				[EventDefinitionSP] = @EventDefinitionSP,
				[UpdatedDate] = @CurrDateTime,
				[UpdatedBy] = @LoggedInUserID,
				[SystemID] = @SystemID
			WHERE
				[NotificationEventID] = @NotificationEventID;
		END

	SELECT 1 As TransactionResultId; RETURN;

END