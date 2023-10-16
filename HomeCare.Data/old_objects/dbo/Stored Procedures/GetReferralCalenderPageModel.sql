
CREATE PROCEDURE [dbo].[GetReferralCalenderPageModel]  
AS  
BEGIN  
  
  
SELECT ReferralID, FirstName, LastName FROM Referrals WHERE IsDeleted=0 ORDER BY LastName ASC
  
SELECT EmployeeID=EmployeeID, FirstName, LastName FROM Employees WHERE IsDeleted=0 ORDER BY LastName ASC  
  
END
