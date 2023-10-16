CREATE PROCEDURE [st].[UpdateEmployeeGroupId]              
 @GroupId VARCHAR(50),              
 @EmployeeIDs VARCHAR(MAX) ,        
 @IsChecked BIT=0        
AS              
BEGIN              
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()           
 --BEGIN TRANSACTION trans              
 --BEGIN TRY          
 IF(@IsChecked=1)        
 BEGIN        
  update employees set GroupIDs=@GroupId where EmployeeID in (select val from GetCSVTable(@EmployeeIds))   
    Select e.EmployeeID,E.EmployeeUniqueID,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,Email,MobileNumber,GroupIDs from Employees e      
  where e.IsDeleted=0 and e.IsActive=1 and e.GroupIDs=@GroupId  
  END        
  ELSE         
  BEGIN         
  update employees set GroupIDs=NULL where EmployeeID in (select val from GetCSVTable(@EmployeeIds))    
    Select e.EmployeeID,E.EmployeeUniqueID,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,Email,MobileNumber,GroupIDs from Employees e      
  where e.IsDeleted=0 and e.IsActive=1 and e.GroupIDs=@GroupId  
  END        
 -- SELECT 1 AS TransactionResultId;              
 -- IF @@TRANCOUNT > 0              
 --  BEGIN              
 --   COMMIT TRANSACTION trans              
 --  END              
 --END TRY              
 --BEGIN CATCH              
 -- SELECT -1 AS TransactionResultId;              
 -- IF @@TRANCOUNT > 0              
 --  BEGIN              
 --   ROLLBACK TRANSACTION trans              
 --  END                
 --END CATCH              
END   