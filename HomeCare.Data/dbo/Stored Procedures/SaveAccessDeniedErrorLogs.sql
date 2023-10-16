CREATE PROC [dbo].[SaveAccessDeniedErrorLogs]
@permission varchar(100),
@Domain varchar(100),
@LoggedInID bigint
AS
BEGIN
INSERT INTO AccessDeniedErrorLogs(PermissionID,PermissionName,Domain,EmployeeID,Date)  
    VALUES (@permission,@permission,@Domain,@LoggedInID,GETDATE()) 
SELECT 1 RETURN;	
END