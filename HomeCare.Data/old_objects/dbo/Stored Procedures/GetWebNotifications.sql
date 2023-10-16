CREATE PROCEDURE [dbo].[GetWebNotifications]
	@EmployeeID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 20
		WN.[WebNotificationID],
		WN.[Message],
		WN.[CreatedDate],
		WUN.[IsRead]
	FROM
		[dbo].[WebNotifications] WN
	INNER JOIN [dbo].[WebUserNotifications] WUN 
		ON WN.[WebNotificationID] = WUN.[WebNotificationID]
			AND WUN.[EmployeeID] = @EmployeeID
			AND WUN.[IsDeleted] = 0
    ORDER BY
		CreatedDate DESC
END
