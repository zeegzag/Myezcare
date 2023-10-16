CREATE PROCEDURE [dbo].[API_ReadUserNotification]  
 @NotificationID BIGINT,
 @EmployeeID BIGINT
AS        
BEGIN      
    
 UPDATE Mobile_UserNotifications SET IsNotificationRead=1 WHERE NotificationId=@NotificationID AND EmployeeID=@EmployeeID
   
 SELECT 1;  
    
END
