  
 -- exec   [rpt].[EmployeeStatusReport]   41,-1  
  
CREATE PROCEDURE [rpt].[EmployeeStatusReport]          
@caretype nvarchar(max)=null,      
@Status bigint=-1        
           
AS  
BEGIN      
      DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 select e.EmployeeUniqueID,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) as Name,e.MobileNumber,e.Email,dm.Title as CareTyoe,case when e.IsActive=1 then 'Active' else 'InActive' end as Status,cast(e.UpdatedDate as date) as UpdatedDate      
from  employees e      
inner join DDMaster dm on dm.DDMasterID in (select val from GetCSVTable(e.CareTypeIds))      
where e.CareTypeIds in (select val from GetCSVTable(@caretype))      
--and (e.IsActive=@Status)   
  
and ((CAST(@Status AS bigint) = -1)                    
 OR e.IsActive = @Status)    
END 