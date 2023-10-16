/*
DECLARE @IsChecked BIT
DECLARE @IsDocumentUploaded BIT
EXEC CHK_GetPatientDHSDocument 24254, 1, @IsChecked OUTPUT, @IsDocumentUploaded OUTPUT
SELECT @IsChecked IsChecked, @IsDocumentUploaded IsDocumentUploaded
*/
CREATE PROCEDURE [dbo].[CHK_GetPatientDHSDocument]
	@PatientID BIGINT,
	@DocumentTypeID INT,
	@IsChecked BIT OUTPUT,
	@IsDocumentUploaded BIT OUTPUT
AS
BEGIN
	SET @IsChecked = 1
	SET @IsDocumentUploaded = 1
END