CREATE PROCEDURE [dbo].[HC_SetPendingSchedulesPage]
AS
BEGIN
SELECT EmployeeID, Name=dbo.GetGeneralNameFormat(FirstName,LastName)  FROM Employees
END
