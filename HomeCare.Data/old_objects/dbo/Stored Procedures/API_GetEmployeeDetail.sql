--EXEC API_GetEmployeeDetail @UserName='sparmar'            
CREATE PROCEDURE [dbo].[API_GetEmployeeDetail]      
 @UserName nvarchar(50)                    
AS                    
BEGIN

 DECLARE @OrganizationTwilioNumber NVARCHAR(30);
 SELECT @OrganizationTwilioNumber=TwilioFromNo FROM OrganizationSettings

 SELECT e.EmployeeID,EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),e.FirstName,e.LastName, e.Email, e.UserName, e.Password, e.PasswordSalt, e.PhoneWork,    
 e.PhoneHome, e.IsActive, e.IsVerify, e.RoleID,e.IsDeleted, e.Roles, e.MobileNumber , e.LoginFailedCount, e.IsFingerPrintAuth,ProfileUrl=e.ProfileImagePath,    
 e.IVRPin,e.AssociateWith,@OrganizationTwilioNumber AS OrganizationTwilioNumber
 FROM dbo.Employees e WHERE e.UserName=@UserName        
        
 SELECT AndroidMinimumVersion,AndroidCurrentVersion,IOSMinimumVersion,IOSCurrentVersion FROM OrganizationSettings        
END
