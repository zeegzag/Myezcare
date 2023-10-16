/*
DECLARE @IsChecked BIT
DECLARE @IsDocumentUploaded BIT
EXEC CHK_GetPatientPhysicianInformation 24254, 1, @IsChecked OUTPUT, @IsDocumentUploaded OUTPUT
SELECT @IsChecked IsChecked, @IsDocumentUploaded IsDocumentUploaded
*/
CREATE PROCEDURE [dbo].[CHK_GetPatientPhysicianInformation]
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
			Referrals R
		WHERE
			ReferralID = @PatientID
			AND PhysicianID IS NOT NULL
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