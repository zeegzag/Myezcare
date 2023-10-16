CREATE PROCEDURE [dbo].[OnHoldNotificationLog]      
 @Title NVARCHAR(1000),
 @EmployeeID BIGINT,    
 @NotificationType INT,    
 @NotificationStatus INT,    
 @ServerDateTime DATETIME
AS        
BEGIN      
    
DECLARE @PrimaryID BIGINT    
    
 INSERT INTO Mobile_Notifications (Title,NotificationType,InProgress,CreatedDate,CreatedBy)     
 VALUES (@Title,@NotificationType,1,@ServerDateTime,@EmployeeID)    
    
 SET @PrimaryID=@@IDENTITY    
    
 INSERT INTO Mobile_UserNotifications (EmployeeID,NotificationID,NotificationStatus,IsNotificationRead)    
 VALUES (@EmployeeID,@PrimaryID,@NotificationStatus,0)    
    
END