-- EXEC GetCaseManagerListForAutoCompleter @SearchText = '', @PazeSize = '10'
CREATE PROCEDURE [dbo].[HC_GetCaseManagerListForAutoCompleter]
 @SearchText VARCHAR(MAX),
 @PazeSize int
AS  
BEGIN  
 SET NOCOUNT ON;  
 	select TOP (@PazeSize) e.EmployeeID AS CaseManagerID,e.LastName+', '+e.FirstName as Name,MobileNumber AS Cell,e.Email
	from dbo.Employees e
	where e.IsDeleted=0
	AND (
		e.FirstName LIKE '%'+@SearchText+'%' OR
		e.LastName  LIKE '%'+@SearchText+'%' OR
		e.FirstName +' '+e.LastName like '%'+@SearchText+'%' OR
		e.LastName +' '+e.FirstName like '%'+@SearchText+'%' OR
		e.FirstName +', '+e.LastName like '%'+@SearchText+'%' OR
		e.LastName +', '+e.FirstName like '%'+@SearchText+'%'
	)
END
