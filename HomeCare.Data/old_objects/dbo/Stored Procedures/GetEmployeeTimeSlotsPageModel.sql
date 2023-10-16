--  SP_HELPTEXT GetEmployeeTimeSlotsPageModel



CREATE PROCEDURE [dbo].[GetEmployeeTimeSlotsPageModel]
AS
BEGIN
SELECT Value=EmployeeID, Name= dbo.GetGeneralNameFormat(FirstName,LastName)  FROM Employees  ORDER BY LastName ASC
END
