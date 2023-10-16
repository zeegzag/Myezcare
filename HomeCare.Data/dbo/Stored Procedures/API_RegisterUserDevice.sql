/*  
Changed by Pallav Saxena  
Change Date 11/15/2019  
Reason: Added the parameter to capture Device OS Versiona  
*/  
CREATE PROCEDURE [dbo].[API_RegisterUserDevice]               
 -- Add the parameters for the stored procedure here              
 @EmployeeId BIGINT,              
 @DeviceUDID NVARCHAR(MAX),      
 @DeviceOSVersion NVARCHAR(500) null,      
 @DeviceType NVARCHAR(10),              
 @ServerCurrentDateTime NVARCHAR(30)               
AS              
BEGIN          
  --  declare @DeviceOSVersion nvarchar(500)    
 --set @DeviceOSVersion ='dummy';    
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
 SET NOCOUNT ON;              
               
 DECLARE @UserDeviceCount INT=(SELECT COUNT(*) FROM UserDevices WHERE EmployeeID=@EmployeeID);              
 DECLARE @CurrentDateTime DATETIME = CAST(@ServerCurrentDateTime AS DATETIME);               
 DECLARE @UserOtpId BIGINT=0;              
 BEGIN TRANSACTION trans              
 BEGIN TRY              
 UPDATE Employees SET DeviceType=@DeviceType,FcmTokenId=@DeviceUDID,IsFirstTimeLogin=1 WHERE EmployeeID=@EmployeeId        
   IF NOT EXISTS (SELECT 1 FROM UserDevices WHERE DeviceType=@DeviceType AND DeviceUDID=@DeviceUDID AND EmployeeID=@EmployeeID)              
    BEGIN                       
     DELETE FROM UserDevices WHERE DeviceUDID=@DeviceUDID              
     INSERT INTO UserDevices (EmployeeID,DeviceUDID,DeviceType,CreatedDate,DeviceOSVersion)              
     VALUES (@EmployeeID,@DeviceUDID,UPPER(@DeviceType),@ServerCurrentDateTime,@DeviceOSVersion)      
    END              
   ELSE              
    BEGIN              
     DELETE FROM UserDevices WHERE DeviceUDID=@DeviceUDID AND EmployeeID!=@EmployeeID              
     UPDATE UserDevices SET CreatedDate=@ServerCurrentDateTime,DeviceOSVersion=@DeviceOSVersion WHERE DeviceType=@DeviceType AND DeviceUDID=@DeviceUDID AND EmployeeID=@EmployeeID          
          
    END              
                   
   SELECT 1 AS TransactionResultId;              
              
   IF @@TRANCOUNT > 0              
    BEGIN               
     COMMIT TRANSACTION trans              
    END              
 END TRY                
 BEGIN CATCH              
                  
    SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;              
    IF @@TRANCOUNT > 0              
    BEGIN               
     ROLLBACK TRANSACTION trans               
    END              
 END CATCH               
END
GO

