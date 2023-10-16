      
CREATE PROCEDURE [dbo].[HC_SetEmployeeVisitsPage]          
AS                                    
BEGIN                           
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()          
 Select EmployeeID, EmployeeName=dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat)           
 From dbo.Employees (nolock)           
 Where IsDeleted=0 ORDER BY LastName ASC                            
        
 SELECT 0        
       
 SELECT 0        
                         
END 