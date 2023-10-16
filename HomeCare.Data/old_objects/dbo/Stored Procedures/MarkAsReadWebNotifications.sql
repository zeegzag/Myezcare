CREATE PROCEDURE [dbo].[MarkAsReadWebNotifications]      
 @WebNotificationIDs NVARCHAR(MAX),
 @EmployeeID BIGINT
AS            
BEGIN          
  
	UPDATE WUN
		SET [IsRead] = 1 
	FROM
		[dbo].[WebUserNotifications] WUN
	INNER JOIN [dbo].[CSVtoTableWithIdentity](@WebNotificationIDs, ',') WN
		ON WN.[RESULT] = WUN.[WebNotificationID]
	WHERE 
		WUN.[EmployeeID] = @EmployeeID  
	
	SELECT 1;      
        
END
