CREATE PROCEDURE [dbo].[API_DeleteUserToken]  
 @Token NVARCHAR(100),  
 @DeviceUDID NVARCHAR(MAX)  
AS  
BEGIN   
 SET NOCOUNT ON;  
 DELETE FROM dbo.UserTokens WHERE Token = @Token;  
 IF (@DeviceUDID IS NOT NULL AND LEN(@DeviceUDID)!=0)  
  BEGIN  
   DELETE FROM dbo.UserDevices WHERE DeviceUDID = @DeviceUDID  
  END  
END  
