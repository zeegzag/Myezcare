CREATE PROCEDURE [dbo].[DeleteWebNotification]      
@WebNotificationID nvarchar(max),      
@EmployeeID BIGINT 
AS
--select  @WebNotificationID='1,3,4',   @EmployeeID=1          
BEGIN            
UPDATE [dbo].[WebUserNotifications]  
  SET [IsDeleted] = 1   
WHERE   
  [WebNotificationID] in (select val from GetCSVTable(@WebNotificationID))   
  AND [EmployeeID] = @EmployeeID    
SELECT 1;        
END  