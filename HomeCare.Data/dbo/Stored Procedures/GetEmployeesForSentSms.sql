CREATE PROCEDURE [dbo].[GetEmployeesForSentSms]
@GroupMessageLogID BIGINT
AS
BEGIN

 DECLARE @EmployeeIDs VARCHAR(MAX);
 SELECT @EmployeeIDs=EmployeeIDs FROM GroupMessageLogs WHERE GroupMessageLogID=@GroupMessageLogID

 SELECT  E.EmployeeID, E.MobileNumber, E.FirstName, E.LastName FROM Employees E 
 WHERE E.IsDeleted=0 AND E.IsActive=1 AND E.EmployeeID IN (SELECT val FROM GetCSVTable(@EmployeeIDs))
 ORDER BY  E.LastName

END