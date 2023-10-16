
CREATE PROCEDURE [dbo].[SetDepartmentListPage]	
AS
BEGIN   

	-- THIS WILL RETURN THE DEPARTMENTS
	SELECT D.DepartmentID,D.DepartmentName
	FROM Departments D order by DepartmentName ASC --WHERE D.IsDeleted = 0 

	
	-- THIS WILL RETURN THE MANAGER
	SELECT E.EmployeeID, E.LastName +', '+ E.FirstName as Manager 
	FROM Employees E
	INNER JOIN Departments D ON D.DepartmentID = E.DepartmentID
	WHERE E.IsDepartmentSupervisor = 1 order by E.LastName ASC-- AND E.IsDeleted = 0 AND D.IsDeleted = 0

	SELECT 0;

	SELECT 0;
END

