CREATE PROCEDURE [dbo].[GetEmployeeListForDDL]
AS
BEGIN
SELECT E.EmployeeID, E.LastName +', ' + E.FirstName AS EmployeeName ,IsDeleted FROM Employees E  WHERE E.IsActive = 1 order by E.LastName ASC
END
