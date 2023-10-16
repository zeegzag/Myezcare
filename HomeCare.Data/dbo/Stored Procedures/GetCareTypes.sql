

--updatedBy                     UpdatedDate                   Description  
--Akhilesh                      02-jul-2019               change from EmployeeID to EmployeesID in where condition   
-- exec [GetCareType] '4296',''  
CREATE PROCEDURE [dbo].[GetCareTypes] 
      
AS       
BEGIN      

--select distinct DDMasterID as CareTypeID, Title As CareType from DDMaster dm
--inner join VisitTasks v on v.CareType= dm.DDMasterID
-- where ItemType=1 and dm.IsDeleted=0 
 select  dm.DDMasterID as CareTypeID ,dm.Title AS CareType 
from ddmaster dm  
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID = dm.ItemType  
where lu.Name='Care Type'  and dm.IsDeleted=0 
      
END