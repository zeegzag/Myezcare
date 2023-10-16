CREATE PROCEDURE HC_GetEmployeeByUserName  
@UserName NVARCHAR(100)  
AS   
  
BEGIN  
  
IF (LOWER(@UserName) = 'superadmin')  
BEGIN  
  SELECT TOP  1 @UserName = SuperAdminUserName FROM OrganizationSettings  
  
 IF( ISNULL(LTRIM(RTRIM(@UserName)),'') = '')  
  SET @UserName = 'me-admin';  
  
END  
  
  
SELECT * FROM Employees WHERE LOWER(UserName) =LOWER(@UserName)  
  
END