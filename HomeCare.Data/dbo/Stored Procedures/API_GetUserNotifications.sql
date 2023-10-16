CREATE PROCEDURE [dbo].[API_GetUserNotifications]          
 @FromIndex INT,          
 @ToIndex INT,          
 @SortExpression VARCHAR(100),          
 @SortType VARCHAR(10),        
 @NotificationType INT,        
 @EmployeeId NVARCHAR(MAX)        
AS          
BEGIN          
 SET NOCOUNT ON;          
          
 IF (@SortExpression IS NULL OR LEN(@SortExpression) = 0)          
  BEGIN          
   SET @SortExpression = 'NotificationId'          
  END          
 IF (@SortType IS NULL OR LEN(@SortType) = 0)          
  BEGIN          
   SET @SortType = 'DESC'          
  END          
          
 ;WITH CTEUserNotificationList AS              
 (                   
  SELECT ROW_NUMBER() OVER          
      (ORDER BY          
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NotificationId' THEN T.NotificationId END END ASC,          
       CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NotificationId' THEN T.NotificationId END END DESC          
      ) AS Row,*,COUNT(T.NotificationId) OVER() AS Count          
  FROM          
  (          
   SELECT DISTINCT mn.NotificationId,mn.Title,mn.NotificationType,mn.CreatedDate,mun.PrimaryId,mun.IsNotificationRead,mun.IsAccepted,mun.IsArchieved,mun.IsDeleted 
   FROM dbo.Mobile_Notifications mn          
   INNER JOIN dbo.Mobile_UserNotifications mun ON mn.NotificationId = mun.NotificationId          
   WHERE mun.EmployeeID=@EmployeeId AND mun.IsDeleted=0    
   AND (((@NotificationType=0 OR @NotificationType IS NULL) AND IsArchieved=0) OR ((NotificationType=@NotificationType AND  @NotificationType!=4 AND IsArchieved=0) OR (@NotificationType=4 AND IsArchieved=1)))
  ) AS T          
 )          
 SELECT * FROM CTEUserNotificationList WHERE ROW BETWEEN @FromIndex AND @ToIndex          
END