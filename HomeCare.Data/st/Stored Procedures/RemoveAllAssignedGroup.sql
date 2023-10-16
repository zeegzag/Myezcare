CREATE PROCEDURE [st].[RemoveAllAssignedGroup]  
@GroupId NVARCHAR(MAX),  
 @EmployeeIDs VARCHAR(MAX)  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
update employees set GroupIDs=NULL where EmployeeID in (select val from GetCSVTable(@EmployeeIds))  
  --SELECT 1 AS TransactionResultId;  
   Select e.EmployeeID,E.EmployeeUniqueID,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,Email,MobileNumber,GroupIDs from Employees e      
  where e.IsDeleted=0 and e.IsActive=1 and e.GroupIDs=@GroupId   
END