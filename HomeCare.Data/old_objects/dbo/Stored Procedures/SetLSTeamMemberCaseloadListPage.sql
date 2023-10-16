CREATE PROCEDURE [dbo].[SetLSTeamMemberCaseloadListPage]   
 AS  
BEGIN  
 SELECT PayorID,PayorName from Payors order by PayorName ASC  
 SELECT * FROM ReferralStatuses  
 SELECT CaseManagerID,LastName+', '+FirstName as Name From CaseManagers order by LastName ASC --where IsDeleted=0  
 select AgencyID,NickName from Agencies order by NickName ASC  
 SELECT Value=EmployeeID,Name=LastName+', '+FirstName  FROM Employees WHERE EmployeeID IN (Select Distinct EmployeeID From ReferralCaseloads) AND IsDeleted=0 AND IsActive=1 Order by LastName ASC 
END
