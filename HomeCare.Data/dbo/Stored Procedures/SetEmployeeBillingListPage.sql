--CreatedBy:Akhilesh kamal    
--CreatedDate:06/08/2020    
--Description:For get employee list  
-- exec SetEmployeeBillingListPage  
CREATE PROCEDURE [dbo].[SetEmployeeBillingListPage]                    
AS                      
BEGIN   
   Select COUNT(EmployeeID) From Employees Where IsDeleted=0                                    
 Select EmployeeID, EmployeeName=dbo.GetGeneralNameFormat(FirstName,LastName) From Employees Where IsDeleted=0 ORDER BY LastName ASC    
  
          
END