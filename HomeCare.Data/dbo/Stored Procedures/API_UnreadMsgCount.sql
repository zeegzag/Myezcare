-- exec [API_UnreadMsgCount] 52

CREATE PROCEDURE [dbo].[API_UnreadMsgCount] 
--DECLARE 
 @EmployeeID BIGINT        
AS                 
BEGIN                     
DECLARE @UnreadMsgCount BIGINT=0;        
DECLARE @RoleID BIGINT ,@CanUpdateCoordinate BIT
SELECT @RoleID=RoleID,@CanUpdateCoordinate=CanUpdateCoordinate FROM Employees Where EmployeeID=@EmployeeID     
SET @UnreadMsgCount=(SELECT COUNT(ReferralInternalMessageID) FROM ReferralInternalMessages WHERE Assignee=@EmployeeID AND IsDeleted=0 AND IsResolved=0)            
              
SELECT EmployeeID=@EmployeeID , UnreadMsgCount=@UnreadMsgCount            
           
SELECT PermissionCode FROM (
SELECT DISTINCT p.PermissionCode,OrderID FROM Permissions p              
  INNER JOIN RolePermissionMapping rpm ON p.PermissionID=rpm.PermissionID               
  INNER JOIN Roles r ON r.RoleID=rpm.RoleID               
  WHERE (r.RoleID = @RoleID AND rpm.IsDeleted=0 AND p.IsDeleted=0 AND  p.CompanyHasAccess=1 AND LOWER(PermissionPlatform)='mobile')
  OR (@CanUpdateCoordinate = 1 AND PermissionCode = 'Mobile_Send_Location')
  ) X ORDER BY OrderID ASC      
      
  SELECT AndroidMinimumVersion,AndroidCurrentVersion,IOSMinimumVersion,IOSCurrentVersion FROM OrganizationSettings      
      
END
GO

