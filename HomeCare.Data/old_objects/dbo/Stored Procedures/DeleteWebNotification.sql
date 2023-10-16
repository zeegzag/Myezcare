CREATE PROCEDURE [dbo].[DeleteWebNotification]      
 @WebNotificationID BIGINT,    
 @EmployeeID BIGINT
AS            
BEGIN          
  
	UPDATE [dbo].[WebUserNotifications]
		SET [IsDeleted] = 1 
	WHERE 
		[WebNotificationID] = @WebNotificationID 
		AND [EmployeeID] = @EmployeeID  

	SELECT 1;      
        
END
