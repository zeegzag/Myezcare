CREATE PROCEDURE [dbo].[SetScheduleAggregatorLogsListPage]    
AS    
BEGIN   
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  SELECT EmployeeID,    
    FirstName,    
    LastName,  
	dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS EmployeeName,
    IsDeleted    
  FROM Employees    
  ORDER BY LastName ASC    
   SELECT 0  
END 