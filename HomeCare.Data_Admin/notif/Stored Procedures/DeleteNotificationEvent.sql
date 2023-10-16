
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to delete notification event by id.
-- =============================================
CREATE PROCEDURE [notif].[DeleteNotificationEvent]
	@NotificationEventIDs VARCHAR(4000),
	@IsDeleted SMALLINT,
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	DECLARE @UpdateCount INT;

	UPDATE
		[notif].[NotificationEvents]
	SET
		[IsDeleted] = CASE WHEN @IsDeleted = -1 THEN [IsDeleted] ^ 1 ELSE @IsDeleted END,
		[UpdatedDate] = @CurrDateTime,
		[UpdatedBy] = @LoggedInUserID,
		[SystemID] = @SystemID
	WHERE 
		[NotificationEventID] IN (SELECT CAST([val] AS BIGINT) FROM [dbo].[GetCSVTable](@NotificationEventIDs));
	
	SELECT @UpdateCount = @@ROWCOUNT;

	IF (@UpdateCount > 0)
		BEGIN
			SELECT @UpdateCount; RETURN;
		END
	ELSE
		BEGIN
			SELECT -1; RETURN;
		END

END