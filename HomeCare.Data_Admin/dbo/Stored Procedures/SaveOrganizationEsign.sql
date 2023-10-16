-- SaveOrganizationEsign 4, '$25/Patient/30Days', '25', '30', '2001,2002,2004', '3001,3003', 1, '192.168.1.1'
CREATE PROCEDURE [dbo].[SaveOrganizationEsign]
@OrganizationEsignID BIGINT,
@OrganizationID BIGINT,
@CompanyName NVARCHAR(100),
@DisplayName NVARCHAR(100),
@Email NVARCHAR(100),
@Phone NVARCHAR(100),
@WorkPhone NVARCHAR(100),
@DefaultEsignTerms NVARCHAR(MAX),
@OrganizationTypeID BIGINT,
@IsCompleted BIT,
@IsInProcess BIT,
@SelectedServicePlanIds VARCHAR(MAX),
@OrganizationStatusFormCreated INT,
@LoggedInUserId BIGINT,
@SystemID VARCHAR(20)
AS    
BEGIN
	DECLARE @IsEditMode INT = CASE WHEN @OrganizationEsignID > 0 THEN 1 ELSE 0 END
	DECLARE @EsignSentDate DATETIME = NULL
	
	IF(@OrganizationTypeID = 0)
		SET @OrganizationTypeID = NULL
		
	IF OBJECT_ID('tempdb..#SelectedServicePlans') IS NOT NULL DROP TABLE #SelectedServicePlans
	
	-- Get Service Plans in Temp table
	SELECT 
		SP.*
	INTO
		#SelectedServicePlans
	FROM 
	(
		SELECT
			CAST(Val AS BIGINT) AS ServicePlanID
		FROM
			GetCSVTable(@SelectedServicePlanIds)
	) AS Temp
	INNER JOIN ServicePlans SP ON SP.ServicePlanID = Temp.ServicePlanID
	
	UPDATE
		Organizations
	SET
		CompanyName = @CompanyName,
		DisplayName = @DisplayName,
		OrganizationStatusID = 
			CASE WHEN 
				@IsCompleted = 1 AND OrganizationStatusID < @OrganizationStatusFormCreated
			THEN 
				@OrganizationStatusFormCreated 
			ELSE 
				OrganizationStatusID 
			END,
		Email = @Email,
		Mobile = @Phone,
		WorkPhone = @WorkPhone,
		UpdatedDate = GETDATE(),
		UpdatedBy = @LoggedInUserId
	WHERE
		OrganizationID = @OrganizationID
	
	IF(@IsEditMode = 0)
	BEGIN
		INSERT INTO OrganizationEsigns
		(
			OrganizationID,
			EsignTerms,
			EsignSentDate,
			IsCompleted,
			IsInProcess,
			OrganizationTypeID,
			CreatedBy,
			CreatedDate,
			UpdatedDate,
			UpdatedBy,
			SystemID,
			IsDeleted
		)
		VALUES
		(
			@OrganizationID,
			@DefaultEsignTerms,
			@EsignSentDate,
			@IsCompleted,
			@IsInProcess,
			@OrganizationTypeID,
			@LoggedInUserId,
			GETDATE(),
			GETDATE(),
			@LoggedInUserId,		
			@SystemID,
			0
		)
		
		SET @OrganizationEsignID = @@IDENTITY
		
		INSERT INTO OrganizationEsignPlans
		(
			OrganizationEsignID,
			OrganizationID,
			ServicePlanID,
			IsSelectedByClient,
			PerPatientPrice,
			NumberOfDaysForBilling,
			CreatedBy,
			CreatedDate,
			UpdatedDate,
			UpdatedBy,
			SystemID,
			IsDeleted
		)
		SELECT
			@OrganizationEsignID,
			@OrganizationID,
			SSP.ServicePlanID,
			0 AS IsSelectedByClient,
			SSP.PerPatientPrice,
			SSP.NumberOfDaysForBilling,
			@LoggedInUserId,
			GETDATE(),
			GETDATE(),
			@LoggedInUserId,
			@SystemID,
			0
		FROM
			#SelectedServicePlans SSP
	END
	ELSE
	BEGIN
		UPDATE 
			OrganizationEsigns
		SET
			EsignTerms = @DefaultEsignTerms,
			OrganizationTypeID = @OrganizationTypeID,
			EsignSentDate = @EsignSentDate,
			IsCompleted = @IsCompleted,
			UpdatedDate = GETDATE(),
			UpdatedBy = @LoggedInUserId
		WHERE
			OrganizationEsignID = @OrganizationEsignID
	
		-- Update OrganizationEsignPlans in table
		MERGE OrganizationEsignPlans OEP
		USING
			#SelectedServicePlans SSP
		ON 
			OEP.OrganizationEsignID = @OrganizationEsignID
			AND OEP.ServicePlanID = SSP.ServicePlanID
		WHEN NOT MATCHED BY TARGET THEN
			INSERT
			(
				OrganizationEsignID,
				OrganizationID,
				ServicePlanID,
				IsSelectedByClient,
				PerPatientPrice,
				NumberOfDaysForBilling,
				CreatedBy,
				CreatedDate,
				UpdatedDate,
				UpdatedBy,
				SystemID,
				IsDeleted
			)
			VALUES
			(
				@OrganizationEsignID,
				@OrganizationID,
				SSP.ServicePlanID,
				0,
				SSP.PerPatientPrice,
				SSP.NumberOfDaysForBilling,
				@LoggedInUserId,
				GETDATE(),
				GETDATE(),
				@LoggedInUserId,
				@SystemID,
				0
			)
		WHEN NOT MATCHED BY SOURCE
			AND OEP.OrganizationEsignID = @OrganizationEsignID THEN
			DELETE;
			
	END
	
	SELECT 
		1 AS TransactionResultId,
		@OrganizationID AS TablePrimaryId
END