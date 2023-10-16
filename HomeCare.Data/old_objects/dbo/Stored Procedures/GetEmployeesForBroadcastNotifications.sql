--EXEC GetEmployeesForBroadcastNotifications @NotificationId = '139'
CREATE PROCEDURE [dbo].[GetEmployeesForBroadcastNotifications]    
 @NotificationId BIGINT,  
 @ScheduleNotificationAction INT=null
 AS    
BEGIN    
SELECT  E.EmployeeID, E.MobileNumber, E.FirstName, E.LastName     
 FROM Employees E     
 INNER JOIN dbo.Mobile_UserNotifications mun ON mun.NotificationId=@NotificationId AND E.EmployeeID = mun.EmployeeID    
 WHERE E.IsDeleted=0 AND E.IsActive=1  
 AND ((@ScheduleNotificationAction=-1 OR @ScheduleNotificationAction IS NULL) OR (mun.IsAccepted=@ScheduleNotificationAction))  
 ORDER BY  E.LastName    
END
