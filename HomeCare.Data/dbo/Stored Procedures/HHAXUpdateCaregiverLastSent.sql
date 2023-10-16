CREATE PROCEDURE [dbo].[HHAXUpdateCaregiverLastSent]
    @EmployeeID BIGINT
AS
BEGIN
  UPDATE dbo.Employees
  SET
    HHAXLastSent = UpdatedDate
  WHERE
    EmployeeID = @EmployeeID
END