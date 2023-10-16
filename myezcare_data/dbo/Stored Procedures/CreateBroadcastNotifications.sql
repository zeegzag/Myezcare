CREATE PROCEDURE [dbo].[CreateBroadcastNotifications]
	@Message NVARCHAR(1000),
	@NotificationType INT,
	@ServerDateTime DATETIME,
	@NotificationStatus INT,
	@EmployeeIDs NVARCHAR(MAX),
	@PrimaryId BIGINT,
	@LoggedInId BIGINT
AS
BEGIN
	DECLARE @NotificationId BIGINT;
	DECLARE @SiteName NVARCHAR(MAX);
	BEGIN TRANSACTION trans
	BEGIN TRY
		
		INSERT INTO dbo.Mobile_Notifications (Title,NotificationType,CreatedDate,InProgress,CreatedBy)
		VALUES (@Message,@NotificationType,@ServerDateTime,@NotificationStatus,@LoggedInId)

		SET @NotificationId = SCOPE_IDENTITY();

		INSERT INTO dbo.Mobile_UserNotifications (EmployeeID,NotificationId,NotificationStatus,PrimaryId)
		SELECT RESULT,@NotificationId,@NotificationStatus,@PrimaryId
		FROM dbo.CSVtoTableWithIdentity(@EmployeeIDs,',')

		SET @SiteName = (SELECT TOP 1 SiteName FROM dbo.OrganizationSettings)

		SELECT 1 AS TransactionResultId;
		SELECT @SiteName AS SiteName;
		
		SELECT e.EmployeeID,e.FcmTokenId,e.DeviceType
		FROM Employees e
		INNER JOIN dbo.CSVtoTableWithIdentity(@EmployeeIDs,',') ct ON ct.RESULT=e.EmployeeID
		WHERE e.FcmTokenId IS NOT NULL

		--SELECT T.EmployeeID,T.DeviceUDID,T.DeviceType
		--FROM (
		--	SELECT ud.EmployeeID,ud.DeviceUDID,ud.DeviceType,DENSE_RANK() OVER (PARTITION BY ud.EmployeeID ORDER BY ud.CreatedDate DESC) AS DenseRank
		--	FROM dbo.UserDevices ud
		--	INNER JOIN dbo.CSVtoTableWithIdentity(@EmployeeIDs,',') ct ON ct.RESULT=ud.EmployeeID
		--	WHERE ud.DeviceUDID IS NOT NULL
		--) AS T WHERE T.DenseRank=1

		IF @@TRANCOUNT > 0
			BEGIN
				COMMIT TRANSACTION trans
			END
	END TRY
	BEGIN CATCH
		SELECT -1 AS TransactionResultId;
		SELECT NULL;
		SELECT NULL;
		IF @@TRANCOUNT > 0
			BEGIN
				ROLLBACK TRANSACTION trans
			END
	END CATCH
END

