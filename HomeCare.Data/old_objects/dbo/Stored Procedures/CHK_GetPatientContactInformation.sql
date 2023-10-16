CREATE PROCEDURE [dbo].[CHK_GetPatientContactInformation]
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
			INNER JOIN ContactTypes CT ON CM.ContactTypeID = CT.ContactTypeID
		WHERE
			ReferralID = @PatientID
			AND CT.ContactTypeID = 1 -- Primary Contact
	)
	BEGIN
		SET @IsChecked = 1	
	END
	ELSE
	BEGIN
		SET @IsChecked = 0
	END
	
	SET @IsDocumentUploaded = 1
END
