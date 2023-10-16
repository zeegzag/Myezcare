CREATE PROCEDURE [dbo].[API_AcceptScheduleNotification]  
 @NotificationID BIGINT,
 @EmployeeID BIGINT
AS        
BEGIN      
    
 UPDATE Mobile_UserNotifications SET IsAccepted=1,IsNotificationRead=1 WHERE NotificationId=@NotificationID AND EmployeeID=@EmployeeID
   
 SELECT 1;  
    
END
