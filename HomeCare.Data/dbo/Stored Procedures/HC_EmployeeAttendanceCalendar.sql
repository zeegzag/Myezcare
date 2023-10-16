CREATE PROCEDURE [dbo].[HC_EmployeeAttendanceCalendar]      
AS      
BEGIN      
      
SELECT EmployeeID=EmployeeID, FirstName, LastName FROM Employees WHERE IsDeleted=0 ORDER BY LastName ASC      
      
END 