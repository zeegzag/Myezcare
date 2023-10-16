    
--  exec [GetCertificateList] 4    
CREATE procedure [dbo].[GetCertificateList]    
   
  @EmployeeID bigint    
     
as    
begin    
                  
    
--select ec.CertificateID,ec.CertificatePath,ec.Name,CONVERT(VARCHAR(10), CAST(ec.StartDate AS DATE), 101)as[StartDate],CONVERT(VARCHAR(10),     
--CAST(ec.EndDate AS DATE), 101)as[EndDate],CONVERT(VARCHAR(10), CAST(ec.CreatedOn AS DATE), 101)as[CreatedOn],dm.Title as CertificateAuthority,ec.CertificateAuthority as CertificateAuthorityID    
select ec.CertificateID,ec.CertificatePath,ec.Name,ec.StartDate as StartDate,     
ec.EndDate as EndDate,ec.CreatedOn as CreatedOn ,dm.Title as CertificateAuthority,ec.CertificateAuthority as CertificateAuthorityID    
  
from EmployeeCertificates ec    
left join DDMaster dm on dm.DDMasterID=ec.CertificateAuthority    
where ec.EmployeeID=@EmployeeID and ec.IsDeleted=0;    
end