--  EXEC SaveCertificates @CertificatePath = '', @Name = 'wew', @StartDate = '10/7/2020 12:00:00 AM', @EndDate = '10/30/2020 12:00:00 AM', @EmployeeID = '40067', @CreatedBy = '1', @CertificateAuthority = '31'

CREATE procedure SaveCertificates    
 
@CertificatePath varchar(200),    
@Name varchar(64),    
@StartDate datetime,    
@EndDate datetime,    
@EmployeeID bigint,    
@CreatedBy bigint,  
@CertificateAuthority  varchar(max)   
   
    
as    
    
begin    
DECLARE @LastID INT;
set @LastID=  (SELECT  TOP(1) CertificateID FROM EmployeeCertificates ORDER BY CertificateID DESC)


UPDATE EmployeeCertificates
SET
Name=@Name,
StartDate= @StartDate,
EndDate= @EndDate,
EmployeeID=@EmployeeID,
CreatedBy=@CreatedBy,
CreatedOn=GETDATE(),
IsDeleted=0,
CertificateAuthority=@CertificateAuthority
WHERE CertificateID=@LastID
   
    
end