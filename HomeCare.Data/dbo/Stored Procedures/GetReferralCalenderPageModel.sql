  
CREATE PROCEDURE [dbo].[GetReferralCalenderPageModel]    
AS    
BEGIN    
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()    
    
SELECT ReferralID, FirstName, LastName,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS ReferralName FROM Referrals WHERE IsDeleted=0 ORDER BY LastName ASC  
    
SELECT EmployeeID=EmployeeID, FirstName, LastName,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS EmployeeName FROM Employees WHERE IsDeleted=0 ORDER BY LastName ASC    
    
END  