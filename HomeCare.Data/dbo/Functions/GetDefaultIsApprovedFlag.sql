CREATE FUNCTION [dbo].[GetDefaultIsApprovedFlag](@EmployeeID BIGINT)         
RETURNS BIT
AS
BEGIN 
  DECLARE @HasPermission BIT
  SELECT @HasPermission = [dbo].[IsEmployeeHasPermission](@EmployeeID, 'Mobile_Visit_Approval_Required');

  RETURN (SELECT CASE WHEN @HasPermission = 0 THEN NULL ELSE 0 END) 
END