CREATE PROCEDURE [dbo].[GetPatientEmpInfo]    
AS    
BEGIN    
   DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()  
SELECT Name=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat), Value=R.ReferralID FROM Referrals R WHERE R.IsDeleted=0    
SELECT Name=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat), Value=E.EmployeeID FROM Employees E WHERE E.IsDeleted=0    
    
END  