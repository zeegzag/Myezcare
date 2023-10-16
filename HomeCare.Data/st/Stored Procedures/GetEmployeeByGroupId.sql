CREATE PROCEDURE [st].[GetEmployeeByGroupId]        
 @GroupId VARCHAR(50)      
AS        
BEGIN Try    
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 --Select EmployeeID from Employees where GroupIDs in (select val from GetCSVTable(@GroupId))      
 Select e.EmployeeID,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName from Employees e where GroupIDs in (select val from GetCSVTable(@GroupId))      
END TRY        
BEGIN CATCH        
 SELECT -1 AS EmployeeID;        
END CATCH   