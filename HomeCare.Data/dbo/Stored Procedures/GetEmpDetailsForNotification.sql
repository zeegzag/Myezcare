CREATE PROCEDURE [dbo].[GetEmpDetailsForNotification]    
 @EmployeeVisitID BIGINT,  
 @NotificationType INT,  
 @ServerDateTime DATETIME,  
 @NotificationStatus INT,  
 @LoggedInId BIGINT  
AS      
BEGIN    
  
DECLARE @PrimaryID BIGINT  
  
 SELECT E.EmployeeID,E.FcmTokenId,E.DeviceType,EV.ScheduleID,SM.ReferralID,
 Editable=1--CASE WHEN CONVERT(DATE,SM.StartDate)>=CONVERT(DATE,dateadd(day,-7,GETDATE())) THEN 1 ELSE 0 END
 FROM EmployeeVisits EV     
 INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID    
 INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID    
 WHERE EmployeeVisitID=@EmployeeVisitID  
  
 INSERT INTO Mobile_Notifications (Title,NotificationType,InProgress,CreatedDate,CreatedBy)   
 VALUES ('Test',@NotificationType,1,@ServerDateTime,@LoggedInId)  
  
 SET @PrimaryID=@@IDENTITY  
  
 INSERT INTO Mobile_UserNotifications (EmployeeID,NotificationID,NotificationStatus,IsNotificationRead)  
 VALUES (@LoggedInId,@PrimaryID,@NotificationStatus,0)  
  
END