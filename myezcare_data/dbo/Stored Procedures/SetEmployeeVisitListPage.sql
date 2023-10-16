CREATE PROCEDURE [dbo].[SetEmployeeVisitListPage]  
AS    
BEGIN                      
 Select EmployeeID, EmployeeName=dbo.GetGeneralNameFormat(FirstName,LastName) From Employees Where IsDeleted=0 ORDER BY LastName ASC    
    
 Select ReferralID, ReferralName=dbo.GetGeneralNameFormat(FirstName,LastName)  From Referrals Where IsDeleted=0 ORDER BY LastName ASC  
END 
