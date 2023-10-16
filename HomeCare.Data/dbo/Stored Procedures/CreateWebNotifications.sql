CREATE PROCEDURE [dbo].[CreateWebNotifications]
	@EmployeeIDs NVARCHAR(MAX),
	@Message NVARCHAR(MAX),
	@ServerDateTime DATETIME,
	@LoggedInId BIGINT
AS
BEGIN
	DECLARE @WebNotificationID BIGINT;
	BEGIN TRANSACTION trans
	BEGIN TRY
		
		INSERT INTO [dbo].[WebNotifications] ([Message], [CreatedDate], [CreatedBy])
			VALUES (@Message, @ServerDateTime, @LoggedInId)

		SET @WebNotificationID = SCOPE_IDENTITY();

		INSERT INTO [dbo].[WebUserNotifications] ([EmployeeID],[WebNotificationID])
			SELECT
				[RESULT], @WebNotificationID
			FROM
				[dbo].[CSVtoTableWithIdentity](@EmployeeIDs, ',')

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