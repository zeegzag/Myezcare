-- EXEC [SetReferralListPage]  
CREATE PROCEDURE [dbo].[SetReferralListPage]   
   
AS  
BEGIN  
 SELECT PayorID,PayorName from Payors order by PayorName ASC  
  
 SELECT * FROM ReferralStatuses  
  
 select EmployeeID,LastName+', '+FirstName as Name from Employees order by LastName ASC--where IsDeleted=0  
  
 select EmployeeID,LastName+', '+FirstName as EmployeeName,IsDeleted from Employees where IsDeleted=0 order by LastName ASC  
   
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY  
 SELECT 0;  
  
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY  
 SELECT 0;  
  
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY  
 SELECT 0;  
  
 SELECT CaseManagerID,LastName+', '+FirstName as Name From CaseManagers order by LastName ASC --where IsDeleted=0  
  
 SELECT  LanguageID,Name from Languages order by Name ASC  
 
 SELECT  RegionID,RegionName from  Regions ORDER BY RegionName ASC;  
  
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY  
 SELECT 0;  
  
 select AgencyID,NickName from Agencies order by NickName ASC  
  
 select AgencyLocationID,LocationName from AgencyLocations order by LocationName ASC  
  
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY  
 SELECT 0;  
  
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY  
 SELECT 0;  
  
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY  
 SELECT 0;  
END