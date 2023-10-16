/*
DECLARE @IsChecked BIT
DECLARE @IsDocumentUploaded BIT
EXEC CHK_GetPatientEmergencyContactInformation 24254, 1, @IsChecked OUTPUT, @IsDocumentUploaded OUTPUT
SELECT @IsChecked IsChecked, @IsDocumentUploaded IsDocumentUploaded
*/
CREATE PROCEDURE [dbo].[CHK_GetPatientEmergencyContactInformation]
	@PatientID BIGINT,
	@DocumentTypeID INT,
	@IsChecked BIT OUTPUT,
	@IsDocumentUploaded BIT OUTPUT
AS
BEGIN
	IF EXISTS
	(
		SELECT
			1
		FROM
			ContactMappings CM
		WHERE
			ReferralID = @PatientID
			AND IsEmergencyContact = 1
	)
	BEGIN
		SET @IsChecked = 1
	END
	ELSE
	BEGIN
		SET @IsChecked = 0
	END
	
	SET @IsDocumentUploaded = 0
END