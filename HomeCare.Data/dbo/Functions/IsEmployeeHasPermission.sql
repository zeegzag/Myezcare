CREATE FUNCTION [dbo].[IsEmployeeHasPermission](@EmployeeID BIGINT, @PermissionCode NVARCHAR(MAX))
RETURNS BIT
AS
BEGIN 
    RETURN ISNULL(
        (
          SELECT    
            CASE WHEN P.PermissionID IS NOT NULL THEN 1 ELSE 0 END   
          FROM [dbo].[Employees] E    
          INNER JOIN [dbo].[RolePermissionMapping] RPM ON E.RoleID = RPM.RoleID AND RPM.IsDeleted = 0
          INNER JOIN [dbo].[Permissions] P ON RPM.PermissionID = P.PermissionID AND P.PermissionCode = @PermissionCode 
          WHERE
            E.EmployeeID = @EmployeeID    
        ), 0) 
END