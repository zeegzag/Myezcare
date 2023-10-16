CREATE PROCEDURE [dbo].[SandataUpdateEmployeeLastSent]
    @EmployeeID BIGINT
AS
BEGIN
  UPDATE dbo.Employees
  SET
    SandataLastSent = UpdatedDate
  WHERE
    EmployeeID = @EmployeeID
END