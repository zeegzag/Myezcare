CREATE PROCEDURE [dbo].[GetMapReport]     
@RoleID BIGINT  
AS    
BEGIN        
 SELECT Distinct RoleID,ReportID  
 FROM ReportPermissionMapping             
  WHERE RoleID = @RoleID AND IsDeleted=0  
END    