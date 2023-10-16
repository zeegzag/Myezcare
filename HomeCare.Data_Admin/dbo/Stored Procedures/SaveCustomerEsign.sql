CREATE PROCEDURE [dbo].[SaveCustomerEsign]
@OrganizationEsignID BIGINT,
@OrganizationID BIGINT,
@CompanyName NVARCHAR(100),
@DisplayName NVARCHAR(100),
@DomainName NVARCHAR(100),
@Email NVARCHAR(100),
@Phone NVARCHAR(100),
@WorkPhone NVARCHAR(100),
@EsignName NVARCHAR(100),
@IsSMTPSettingsEntered BIT,
@NetworkHost NVARCHAR(200),
@NetworkPort NVARCHAR(200),
@FromTitle NVARCHAR(200),
@FromEmail NVARCHAR(200),
@FromEmailPassword NVARCHAR(200),
@EnableSSL BIT,
@IsTwilioSettingsEntered BIT,
@TwilioCountryCode NVARCHAR(3),
@TwilioLocation NVARCHAR(200),
@SelectedServicePlanIds VARCHAR(MAX),
@OrganizationStatusEsignCompleted INT,
@LoggedInUserId BIGINT,
@SystemID VARCHAR(20)
AS    
BEGIN
	IF OBJECT_ID('tempdb..#SelectedServicePlanRateDetails') IS NOT NULL DROP TABLE #SelectedServicePlanRateDetails
	DECLARE @ServicePlanID INT = CONVERT(INT, @SelectedServicePlanIds)
	DECLARE @PerPatientPrice FLOAT
	DECLARE @NumberOfDaysForBilling INT
	DECLARE @OrganizationEsignPlanID BIGINT
	
	SELECT
		@OrganizationEsignPlanID = OrganizationEsignPlanID
	FROM
		OrganizationEsignPlans OEP
	WHERE
		OEP.OrganizationEsignID = @OrganizationEsignID
		AND OEP.OrganizationID = @OrganizationID
		AND OEP.ServicePlanID = @ServicePlanID	
	
	SELECT
		@PerPatientPrice = PerPatientPrice,
		@NumberOfDaysForBilling = NumberOfDaysForBilling
	FROM
		ServicePlans SP
	WHERE
		SP.ServicePlanID = @ServicePlanID
	
	-- Get Service Plan Rates in Temp table
	SELECT 
		SPR.*
	INTO
		#SelectedServicePlanRateDetails
	FROM 
	(
		SELECT
			CAST(Val AS BIGINT) AS ServicePlanID
		FROM
			GetCSVTable(@SelectedServicePlanIds)
	) AS Temp
	INNER JOIN ServicePlans SP ON SP.ServicePlanID = Temp.ServicePlanID
	INNER JOIN ServicePlanRates SPR ON SP.ServicePlanID = SPR.ServicePlanID
	
	UPDATE
		Organizations
	SET
		CompanyName = @CompanyName,
		DisplayName = @DisplayName,
		DomainName = @DomainName,
		OrganizationStatusID = @OrganizationStatusEsignCompleted,
		Email = @Email,
		Mobile = @Phone,
		WorkPhone = @WorkPhone,
		UpdatedDate = GETDATE(),
		UpdatedBy = @LoggedInUserId
	WHERE
		OrganizationID = @OrganizationID
	
	IF EXISTS
	(
		SELECT
			1
		FROM
			OrganizationSettings
		WHERE
			OrganizationID = @OrganizationID
	)
	BEGIN
	UPDATE
		OrganizationSettings
	SET
		OrganizationID = @OrganizationID,
		IsSMTPSettingsEntered = @IsSMTPSettingsEntered,
		NetworkHost = @NetworkHost,
		NetworkPort = @NetworkPort,
		FromTitle = @FromTitle,
		FromEmail = @FromEmail,
		FromEmailPassword = @FromEmailPassword,
		EnableSSL = @EnableSSL,
		IsTwilioSettingsEntered = @IsTwilioSettingsEntered,
		TwilioCountryCode = @TwilioCountryCode,
		TwilioLocation = @TwilioLocation,		
		UpdatedDate = GETDATE(),
		UpdatedBy = @LoggedInUserId
	WHERE
		OrganizationID = @OrganizationID
	END
	ELSE
	BEGIN
		INSERT INTO OrganizationSettings
		(
			OrganizationID,
			IsSMTPSettingsEntered,
			NetworkHost,
			NetworkPort,
			FromTitle,
			FromEmail,
			FromEmailPassword,
			EnableSSL,
			IsTwilioSettingsEntered,
			TwilioCountryCode,
			TwilioLocation,
			CreatedBy,
			CreatedDate,
			UpdatedDate,
			UpdatedBy,
			SystemID
		)
		SELECT
			@OrganizationID,
			@IsSMTPSettingsEntered,
			@NetworkHost,
			@NetworkPort,
			@FromTitle,
			@FromEmail,
			@FromEmailPassword,
			@EnableSSL,
			@IsTwilioSettingsEntered,
			@TwilioCountryCode,
			@TwilioLocation,
			@LoggedInUserId,
			GETDATE(),  
			GETDATE(),  
			@LoggedInUserId,  
			@SystemID
	END
	
	UPDATE 
		OrganizationEsigns
	SET
		EsignAcceptedDate = GETDATE(),
		EsignName = @EsignName,
		IsInProcess = 0,
		UpdatedDate = GETDATE(),
		UpdatedBy = @LoggedInUserId
	WHERE
		OrganizationEsignID = @OrganizationEsignID
	
	UPDATE
		OrganizationEsignPlans
	SET
		IsSelectedByClient = 1,
		ClientSelecetdDate = GETDATE(),
		PlanStartDate = GETDATE(),
		PlanEndDate = DATEADD(DAY, @NumberOfDaysForBilling, GETDATE()),
		NextDueDate = DATEADD(DAY, @NumberOfDaysForBilling + 1, GETDATE()),
		UpdatedDate = GETDATE(),
		UpdatedBy = @LoggedInUserId
	WHERE
		OrganizationEsignPlanID = @OrganizationEsignPlanID
		
	INSERT INTO OrganizationPlanRates
	(
		OrganizationEsignPlanID,
		ServicePlanRateID,
		ServicePlanID,
		ModuleName,
		MaximumAllowedNumber,
		CreatedBy,
		CreatedDate,
		UpdatedDate,
		UpdatedBy,
		SystemID,
		IsDeleted
	)
	SELECT
		@OrganizationEsignPlanID,
		SPR.ServicePlanRateID,
		SPR.ServicePlanID,
		SPR.ModuleName,
		SPR.MaximumAllowedNumber,
		@LoggedInUserId,
		GETDATE(),  
		GETDATE(),  
		@LoggedInUserId,  
		@SystemID,  
		0  
	FROM
		#SelectedServicePlanRateDetails SPR
	
	SELECT 
		1 AS TransactionResultId,
		@OrganizationID AS TablePrimaryId
END