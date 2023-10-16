CREATE PROCEDURE [dbo].[SetCompliancePage]  
@UserType INT    = 1
AS        
BEGIN      
 --SELECT Name=DocumentName,Value=ComplianceID,UserType FROM Compliances WHERE ParentID = 0 AND IsDeleted=0 AND (UserType=@UserType OR UserType=0)  
 IF @UserType = 1 BEGIN
	SELECT Name=E.FirstName+', '+E.LastName,Value=E.EmployeeID FROM Employees E WHERE E.IsActive =1 AND E.IsDeleted=0
 END
 ELSE BEGIN
	SELECT Name=R.LastName+', '+R.FirstName,Value=R.ReferralID FROM Referrals R WHERE R.IsDeleted=0
 END
END

