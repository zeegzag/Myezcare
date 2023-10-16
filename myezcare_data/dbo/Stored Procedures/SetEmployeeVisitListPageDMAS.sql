--  EXECUTE [SetEmployeeVisitListPageDMAS]  
  
CREATE PROCEDURE [dbo].[SetEmployeeVisitListPageDMAS]      
AS        
BEGIN                          
 Select EmployeeID, EmployeeName=dbo.GetGeneralNameFormat(FirstName,LastName) From Employees Where IsDeleted=0 ORDER BY LastName ASC        
        
 Select ReferralID, ReferralName=dbo.GetGeneralNameFormat(FirstName,LastName)  From Referrals Where IsDeleted=0 ORDER BY LastName ASC  
  select DISTINCT Title as VisitTaskType,DDMasterID as CareType from DDMaster dm
 inner join VisitTasks vt on dm.DDMasterID=vt.CareType where dm.IsDeleted=0
END     
SELECT * FROM VisitTasks
