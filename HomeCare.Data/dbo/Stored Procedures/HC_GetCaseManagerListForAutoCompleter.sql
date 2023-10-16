-- EXEC GetCaseManagerListForAutoCompleter @SearchText = '', @PazeSize = '10'    
CREATE PROCEDURE [dbo].[HC_GetCaseManagerListForAutoCompleter]    
 @SearchText VARCHAR(MAX),    
 @PazeSize int    
AS      
BEGIN      
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 SET NOCOUNT ON;      
 -- select TOP (@PazeSize) e.EmployeeID AS CaseManagerID,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) as Name,MobileNumber AS Cell,e.Email    
 select TOP (@PazeSize) e.CaseManagerID AS CaseManagerID,dbo.GetGenericNameFormat(e.FirstName,'', e.LastName,@NameFormat) as Name,Phone AS Cell,e.Email    
 --from dbo.Employees e    
 from dbo.CaseManagers e ----update employee table to caseManager table 'Balwinder'----Date'--29-03-2020'   
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
  