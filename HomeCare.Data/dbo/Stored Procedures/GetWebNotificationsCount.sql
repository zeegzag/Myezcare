CREATE PROCEDURE [dbo].[GetWebNotificationsCount]    
 @EmployeeID BIGINT    
AS    
BEGIN    
 SET NOCOUNT ON;    
    
   SELECT   
    WebNotificationID,    
 SUM(COUNT(WebNotificationID)) OVER() AS MsgCount  
FROM  
 (SELECT TOP 20    
  WN.[WebNotificationID] AS WebNotificationID  
 FROM    
  [dbo].[WebNotifications] WN    
 INNER JOIN [dbo].[WebUserNotifications] WUN   
  ON WN.[WebNotificationID] = WUN.[WebNotificationID]    
   AND WUN.[EmployeeID] = @EmployeeID    
   AND WUN.[IsRead] = 0  
   AND WUN.[IsDeleted] = 0    ) T  
  GROUP BY WebNotificationID
END
