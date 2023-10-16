--EXEC API_IVR_ValidateIvrCode @MobileNumber = N'8866884636', @IVRPin = N'7777', @IVRBypassPermission = N'Mobile_IVR_Bypass_ClockInOut'  
CREATE PROCEDURE [dbo].[API_IVR_ValidateIvrCode]      
 @MobileNumber VARCHAR(20),      
 @IVRPin VARCHAR(4),  
 @AutoApprovedIVRBypassPermission VARCHAR(50),
 @ApprovalRequiredIVRBypassPermission VARCHAR(100)
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 DECLARE @RoleID BIGINT;  
 DECLARE @PermissionID BIGINT;  
   
 SELECT @RoleID=RoleID FROM Employees WHERE MobileNumber=@MobileNumber AND IVRPin=@IVRPin  
  
 SELECT P.PermissionID FROM RolePermissionMapping RPM  
 INNER JOIN Permissions P ON P.PermissionID=RPM.PermissionID  
 WHERE RPM.RoleID=@RoleID AND (P.PermissionCode=@AutoApprovedIVRBypassPermission OR P.PermissionCode=@ApprovalRequiredIVRBypassPermission) AND RPM.IsDeleted=0  
      
    SELECT e.EmployeeID,e.FirstName,e.LastName  
 FROM dbo.Employees e       
 WHERE e.MobileNumber=@MobileNumber AND e.IVRPin=@IVRPin  
END
