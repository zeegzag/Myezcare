CREATE PROCEDURE [dbo].[API_ActionOnNotification]      
 @NotificationID BIGINT,    
 @EmployeeID BIGINT,  
 @Action VARCHAR(100)  
AS            
BEGIN          
  
IF(@Action='Delete')  
 UPDATE Mobile_UserNotifications SET IsDeleted=1 WHERE NotificationId=@NotificationID AND EmployeeID=@EmployeeID  
  
IF(@Action='Archive')  
 UPDATE Mobile_UserNotifications SET IsArchieved=1 WHERE NotificationId=@NotificationID AND EmployeeID=@EmployeeID  
       
 SELECT 1;      
        
END
