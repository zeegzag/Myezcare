--  exec GetCertificateAuthority   
  
CREATE PROC GetCertificateAuthority    
    
as    
begin    
select dm.Title AS Name, dm.DDMasterID as Value     
from ddmaster dm    
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID = dm.ItemType    
where lu.Name='CertificationAuthority'    
    
end

select * from lu_DDMasterTypes