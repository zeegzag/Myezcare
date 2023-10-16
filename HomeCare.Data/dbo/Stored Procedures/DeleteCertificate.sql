
CREATE procedure [dbo].[DeleteCertificate]
(
@CertificateID bigint
)
as
begin
         update EmployeeCertificates set IsDeleted=1 where CertificateID=@CertificateID
end