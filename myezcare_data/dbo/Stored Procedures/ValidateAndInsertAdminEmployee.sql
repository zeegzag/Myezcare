-- EXEC ValidateAndInsertAdminEmployee 1

CREATE PROCEDURE [dbo].[ValidateAndInsertAdminEmployee]
	@UserID INT,
	@SystemID VARCHAR(50)
AS
BEGIN
	UPDATE AdminTempEmployee SET ErrorMessage = '' WHERE CreatedBy = @UserID
		
	-- Validate Employee Records
	
	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' Username is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(UserName))) = 0 OR UserName IS NULL OR UserName = '')
	
	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' FirstName is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(FirstName))) = 0 OR FirstName IS NULL OR FirstName = '')

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' LastName is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(LastName))) = 0 OR LastName IS NULL OR LastName = '')

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' Email is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(Email))) = 0 OR Email IS NULL OR Email = '')

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' Role is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM([Role]))) = 0 OR [Role] IS NULL OR [Role] = '')

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' Address is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM([Address]))) = 0 OR [Address] IS NULL OR [Address] = '')

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' City is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(City))) = 0 OR City IS NULL OR City = '')

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' State is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM([State]))) = 0 OR [State] IS NULL OR [State] = '')

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' ZipCode is required.' 
	WHERE CreatedBy = @UserID 
	AND (LEN(LTRIM(RTRIM(ZipCode))) = 0 OR ZipCode IS NULL OR ZipCode = '')

	-- Username
	UPDATE 
		ATE 
	SET 
		ATE.ErrorMessage = ATE.ErrorMessage + ' Username already exists.' 
	FROM 
		AdminTempEmployee AS ATE
		LEFT JOIN Employees E ON ATE.Username = E.Username
	WHERE 
		ATE.CreatedBy = @UserID
		AND (E.IsDeleted != 1 OR E.IsActive != 0 OR E.EmployeeID IS NOT NULL)

	UPDATE AdminTempEmployee SET ErrorMessage = ErrorMessage + ' Please enter valid Email.' 
	WHERE CreatedBy = @UserID 
	AND Email NOT LIKE '%_@__%.__%'

	-- Role
	UPDATE 
		ATE 
	SET 
		ATE.ErrorMessage = ATE.ErrorMessage + ' Please enter valid Role.' 
	FROM 
		AdminTempEmployee AS ATE
		LEFT JOIN Roles R ON ATE.[Role] = R.RoleName
	WHERE 
		ATE.CreatedBy = @UserID
		AND (R.RoleID IS NULL)
		
	UPDATE AdminTempEmployee SET ErrorMessage = LTRIM(RTRIM(ErrorMessage)) WHERE CreatedBy = @UserID

	IF EXISTS (SELECT 1 FROM AdminTempEmployee WHERE ErrorMessage != '' AND CreatedBy = @UserID)
	BEGIN
		SELECT 0 AS TransactionResultId
		
		SELECT 
			EmployeeID,
			UserName,
			FirstName,
			LastName,
			Email,
			[Role],
			[Address],
			City,
			[State],
			ZipCode,
			ErrorMessage
		FROM 
			AdminTempEmployee 
		WHERE 
			ErrorMessage != '' 
			AND CreatedBy = @UserID
	END
	ELSE -- Success
	BEGIN
	
		INSERT INTO Employees
		(
			EmployeeUniqueID,
			FirstName,
			LastName,
			Email,
			UserName,
			[Password],
			[PasswordSalt],
			IsDepartmentSupervisor,
			IsActive,
			IsVerify,
			RoleID,
			IsSecurityQuestionSubmitted,
			[Address],
			City,
			StateCode,
			ZipCode,
			CreatedDate,
			CreatedBy,
			UpdatedDate,
			UpdatedBy,
			SystemID,
			IsDeleted,
			EmployeeSignatureID
		)
		SELECT
			EmployeeID,
			FirstName,
			LastName,
			Email,
			UserName,
			NULL AS [Password],
			NULL AS [PasswordSalt],
			0 AS IsDepartmentSupervisor,
			1 AS IsActive,
			0 AS IsVerify,
			R.RoleID,
			0 AS IsSecurityQuestionSubmitted,
			[Address],
			City,
			[State],
			ZipCode,
			GETDATE(),
			@UserID,
			GETDATE(),
			@UserID,
			@SystemID,
			0 AS IsDeleted,
			0 AS EmployeeSignatureID
		FROM 
			AdminTempEmployee ATE
			INNER JOIN Roles R ON ATE.Role = R.RoleName
		WHERE 
			ErrorMessage = ''
			AND ATE.CreatedBy = @UserID
	
		SELECT 1 AS TransactionResultId
		
		SELECT 1 AS EmployeeID		
	END	
END
