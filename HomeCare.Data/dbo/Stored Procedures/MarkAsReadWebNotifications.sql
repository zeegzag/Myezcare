CREATE PROCEDURE [dbo].[MarkAsReadWebNotifications]      
 @WebNotificationIDs NVARCHAR(MAX),
 @EmployeeID BIGINT,
 @IsRead BIT= 1
AS            
BEGIN          
  
	UPDATE WUN
		SET [IsRead] = @IsRead
	FROM
		[dbo].[WebUserNotifications] WUN
	INNER JOIN [dbo].[CSVtoTableWithIdentity](@WebNotificationIDs, ',') WN
		ON WN.[RESULT] = WUN.[WebNotificationID]
	WHERE 
		WUN.[EmployeeID] = @EmployeeID  
	
	SELECT 1;      
        
END