
--  EXECUTE [SetEmployeeVisitListPageDMAS]    
    
CREATE PROCEDURE [dbo].[SetEmployeeVisitListPageDMAS]        
AS          
BEGIN  

                          
 Select EmployeeID, EmployeeName=dbo.GetGeneralNameFormat(FirstName,LastName) From Employees Where IsDeleted=0 ORDER BY LastName ASC          
          
 Select ReferralID, ReferralName=dbo.GetGeneralNameFormat(FirstName,LastName)  From Referrals Where IsDeleted=0 ORDER BY LastName ASC    
  select DISTINCT Title as VisitTaskType,DDMasterID as CareType from DDMaster dm  
 inner join VisitTasks vt on dm.DDMasterID=vt.CareType where dm.IsDeleted=0  
END   
  
--select 0;    
SELECT * FROM VisitTasks  
  
select dds.DDMasterID as ServiceTypeID,dds.Title as ServiceTypeName from DDMaster dds        
inner join lu_DDMasterTypes as luddm on dds.ItemType=luddm.DDMasterTypeID where dds.IsDeleted=0 and luddm.Name='Service Type';

select 0;
select 0;