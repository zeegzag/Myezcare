CREATE PROCEDURE [dbo].[SaveGroupMessageLogs]
@EmployeeIDs VARCHAR(MAX),
@Message VARCHAR(MAX),
@NotificationSID VARCHAR(100),
@LoggedInID BIGINT,
@SystemID VARCHAR(100)
AS    
BEGIN    
  -- SELECT * FROM GroupMessageLogs

  INSERT INTO GroupMessageLogs
  VALUES(@EmployeeIDs,@Message,@NotificationSID,GetDate(),@LoggedInID,GetDate(),@LoggedInID,@SystemID )
     
 SELECT 1 RETURN;
  
END
