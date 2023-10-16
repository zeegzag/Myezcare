CREATE PROCEDURE [dbo].[API_CheckForPermissionExist]
@EmployeeID BIGINT,
@IVRInstantNoSchClockInOut VARCHAR(1000) 
AS
BEGIN

DECLARE @RoleID BIGINT;  
DECLARE @PermissionID BIGINT;  
   
SELECT @RoleID=RoleID FROM Employees WHERE EmployeeID=@EmployeeID
  
SELECT @PermissionID=P.PermissionID FROM RolePermissionMapping RPM  
INNER JOIN Permissions P ON P.PermissionID=RPM.PermissionID  
WHERE RPM.RoleID=@RoleID AND P.PermissionCode=@IVRInstantNoSchClockInOut AND RPM.IsDeleted=0  


IF(@PermissionID>0)
SELECT 1;
ELSE
SELECT 0;

END