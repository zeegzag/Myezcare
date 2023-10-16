CREATE PROC UploadCerificate

@FilePath varchar(max),
@LoggedInUserID bigint
AS
BEGIN
INSERT INTO EmployeeCertificates(CertificatePath,CreatedBy) values(@FilePath,@LoggedInUserID)
END