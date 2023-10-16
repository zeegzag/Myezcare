-- EXEC ValidateAndInsertAdminPatient 1

CREATE PROCEDURE [dbo].[ValidateAndInsertAdminPatient]
	@UserID INT,
	@SystemID VARCHAR(50)
AS
BEGIN
	UPDATE AdminTempPatient SET	ErrorMessage = '' WHERE CreatedBy = @UserID
	UPDATE AdminTempPatientContact SET ErrorMessage = '' WHERE CreatedBy = @UserID
		
	-- Validate Partient Records

	UPDATE AdminTempPatient SET	ErrorMessage = ErrorMessage + ' FirstName is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(FirstName))) = 0 OR FirstName IS NULL OR FirstName = '')

	UPDATE AdminTempPatient SET	ErrorMessage = ErrorMessage + ' LastName is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(LastName))) = 0 OR LastName IS NULL OR LastName = '')

	UPDATE AdminTempPatient SET	ErrorMessage = ErrorMessage + ' DOB is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(DOB))) = 0 OR DOB IS NULL OR DOB = '')

	UPDATE AdminTempPatient SET	ErrorMessage = ErrorMessage + ' Gender is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(Gender))) = 0 OR Gender IS NULL OR Gender = '')

	UPDATE AdminTempPatient SET	ErrorMessage = ErrorMessage + ' AccountNumber is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(AccountNumber))) = 0 OR AccountNumber IS NULL OR AccountNumber = '')

	UPDATE AdminTempPatient SET	ErrorMessage = ErrorMessage + ' LanguagePreference is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(LanguagePreference))) = 0 OR LanguagePreference IS NULL OR LanguagePreference = '')

	UPDATE 
		ATP 
	SET 
		ATP.ErrorMessage = ATP.ErrorMessage + ' Please enter valid Gender.' 
	FROM 
		AdminTempPatient AS ATP
	WHERE 
		ATP.CreatedBy = @UserID
		AND (ATP.Gender NOT IN ('Male','Female'))
		
	UPDATE 
		ATP 
	SET 
		ATP.ErrorMessage = ATP.ErrorMessage + ' Please enter valid Language Preference.' 
	FROM 
		AdminTempPatient AS ATP
		LEFT JOIN Languages L ON ATP.LanguagePreference = L.Name
	WHERE 
		ATP.CreatedBy = @UserID
		AND (L.LanguageID IS NULL)
		
	-- Validate Partient Contact Records

	UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' ContactType is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(ContactType))) = 0 OR ContactType IS NULL OR ContactType = '')

	UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' FirstName is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(FirstName))) = 0 OR FirstName IS NULL OR FirstName = '')

	UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' LastName is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(LastName))) = 0 OR LastName IS NULL OR LastName = '')

	UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' Phone is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(Phone))) = 0 OR Phone IS NULL OR Phone = '')

	UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' LanguagePreference is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(LanguagePreference))) = 0 OR LanguagePreference IS NULL OR LanguagePreference = '')
	
	UPDATE 
		ATPC 
	SET 
		ATPC.ErrorMessage = ATPC.ErrorMessage + ' Please enter valid Contact Type.' 
	FROM 
		AdminTempPatientContact AS ATPC
		LEFT JOIN ContactTypes CT ON ATPC.ContactType = CT.ContactTypeName
	WHERE 
		ATPC.CreatedBy = @UserID
		AND (CT.IsDeleted != 0 OR CT.ContactTypeID IS NULL)
		
	UPDATE 
		ATPC 
	SET 
		ATPC.ErrorMessage = ATPC.ErrorMessage + ' Please enter valid Language Preference.' 
	FROM 
		AdminTempPatientContact AS ATPC
		LEFT JOIN Languages L ON ATPC.LanguagePreference = L.Name
	WHERE 
		ATPC.CreatedBy = @UserID
		AND (L.LanguageID IS NULL)

	UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' Please enter valid Email.' 
	WHERE CreatedBy = @UserID 
	AND Email NOT LIKE '%_@__%.__%'

	--UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' Please enter valid Phone.' 
	--WHERE CreatedBy = @UserID 
	--AND Phone NOT LIKE '[2-9][0-8][0-9][2-9][0-9][0-9][0-9][0-9][0-9][0-9]' 

	UPDATE AdminTempPatientContact SET	ErrorMessage = ErrorMessage + ' Please enter valid value for IsEmergencyContact.' 
	WHERE CreatedBy = @UserID 
	AND IsEmergencyContact NOT IN ('Yes', 'No')

	UPDATE 
		ATPC 
	SET 
		ATPC.ErrorMessage = ATPC.ErrorMessage + ' Matching Patient entry did not found.' 
	FROM 
		AdminTempPatientContact AS ATPC
		LEFT JOIN AdminTempPatient ATP ON ATPC.PatientID = ATP.PatientID
	WHERE 
		ATPC.CreatedBy = @UserID
		AND ATP.PatientID IS NULL
		
	UPDATE AdminTempPatient SET	ErrorMessage = LTRIM(RTRIM(ErrorMessage)) WHERE CreatedBy = @UserID
	UPDATE AdminTempPatientContact SET ErrorMessage = LTRIM(RTRIM(ErrorMessage)) WHERE CreatedBy = @UserID

	IF EXISTS (SELECT 1 FROM AdminTempPatient WHERE ErrorMessage != '' AND CreatedBy = @UserID)
	OR
	EXISTS(SELECT 1 FROM AdminTempPatientContact WHERE ErrorMessage != '' AND CreatedBy = @UserID)
	BEGIN
		SELECT 0 AS TransactionResultId
		
		SELECT 
			PatientID,
			FirstName,
			LastName,
			Dob,
			Gender,
			AccountNumber,
			LanguagePreference,
			ErrorMessage
		FROM 
			AdminTempPatient 
		WHERE 
			ErrorMessage != '' 
			AND CreatedBy = @UserID
			
		SELECT 
			PatientID,
			ContactType,
			FirstName,
			LastName,
			Email,
			Phone,
			[Address],
			City,
			[State],
			ZipCode,
			LanguagePreference,
			IsEmergencyContact,
			ErrorMessage
		FROM 
			AdminTempPatientContact 
		WHERE 
			ErrorMessage != '' 
			AND CreatedBy = @UserID
	END
	ELSE -- Success
	BEGIN
	
		INSERT INTO Referrals
		(
			FirstName,
			LastName,
			Dob,
			Gender,
			LanguageID,
			AHCCCSID,
			CreatedDate,
			CreatedBy,
			UpdatedDate,
			UpdatedBy,
			SystemID
		)
		SELECT
			FirstName,
			LastName,
			Dob,
			CASE WHEN Gender = 'Male' THEN 'M' ELSE 'F' END AS Gender,
			L.LanguageID,
			AccountNumber,
			GETDATE(),
			@UserID,
			GETDATE(),
			@UserID,
			@SystemID
		FROM 
			AdminTempPatient ATP
			INNER JOIN Languages L ON ATP.LanguagePreference = L.Name
		WHERE 
			ErrorMessage = ''
			AND CreatedBy = @UserID
			
		INSERT INTO Contacts
		(
			FirstName,
			LastName,
			Email,
			[Address],
			City,
			[State],
			ZipCode,
			Phone1,
			LanguageID,
			CreatedDate,
			CreatedBy,
			UpdatedDate,
			UpdatedBy,
			SystemID,
			IsDeleted
		)
		SELECT
			FirstName,
			LastName,
			Email,
			[Address],
			City,
			[State],
			ZipCode,
			Phone,
			L.LanguageID,
			GETDATE(),
			@UserID,
			GETDATE(),
			@UserID,
			@SystemID,
			0
		FROM 
			AdminTempPatientContact ATPC
			INNER JOIN Languages L ON ATPC.LanguagePreference = L.Name
		WHERE 
			ErrorMessage = '' 
			AND CreatedBy = @UserID
	
		SELECT 1 AS TransactionResultId
		
		SELECT 1 AS PatientID
		
		SELECT 1 AS PatientID		
	END	
END
