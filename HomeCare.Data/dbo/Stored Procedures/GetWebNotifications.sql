-- EXEC GetWebNotifications 1  
  
CREATE PROCEDURE [dbo].[GetWebNotifications]    
 @EmployeeID BIGINT    
AS    
BEGIN    
 SET NOCOUNT ON;    
    
   SELECT   
    WebNotificationID,  
    Message,  
 CreatedDate,  
 IsRead,  
 SUM(COUNT(IsRead)) OVER() AS MsgCount  
FROM  
 (SELECT TOP 20    
  WN.[WebNotificationID] AS WebNotificationID,    
  WN.[Message],    
  WN.[CreatedDate],    
  WUN.[IsRead]  
 FROM    
  [dbo].[WebNotifications] WN    
 INNER JOIN [dbo].[WebUserNotifications] WUN   
  ON WN.[WebNotificationID] = WUN.[WebNotificationID]    
   AND WUN.[EmployeeID] = @EmployeeID    
   AND WUN.[IsRead] = 0  
   AND WUN.[IsDeleted] = 0    
    ORDER BY    
  CreatedDate DESC) T  
  GROUP BY WebNotificationID, Message, CreatedDate, IsRead  
END 