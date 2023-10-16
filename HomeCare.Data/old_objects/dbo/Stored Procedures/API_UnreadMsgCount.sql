CREATE PROCEDURE [dbo].[API_UnreadMsgCount]                    
 @EmployeeID BIGINT        
AS                 
BEGIN                     
DECLARE @UnreadMsgCount BIGINT=0;        
DECLARE @RoleID BIGINT = (SELECT RoleID FROM Employees Where EmployeeID=@EmployeeID)        
SET @UnreadMsgCount=(SELECT COUNT(ReferralInternalMessageID) FROM ReferralInternalMessages WHERE Assignee=@EmployeeID AND IsDeleted=0 AND IsResolved=0)            
              
SELECT EmployeeID=@EmployeeID , UnreadMsgCount=@UnreadMsgCount            
            
SELECT p.PermissionCode FROM Permissions p              
  INNER JOIN RolePermissionMapping rpm ON p.PermissionID=rpm.PermissionID               
  INNER JOIN Roles r ON r.RoleID=rpm.RoleID               
  WHERE r.RoleID = @RoleID AND rpm.IsDeleted=0 AND p.IsDeleted=0 AND  p.CompanyHasAccess=1 AND LOWER(PermissionPlatform)='mobile'            
  ORDER BY OrderID ASC      
      
  SELECT AndroidMinimumVersion,AndroidCurrentVersion,IOSMinimumVersion,IOSCurrentVersion FROM OrganizationSettings      
      
END
