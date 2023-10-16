-- EXEC GetOrganizationEsignDetails @OrganizationID = '12', @OrganizationEsignID = '0'

-- EXEC GetOrganizationEsignDetails 1,8
CREATE PROCEDURE [dbo].[GetOrganizationEsignDetails]
@OrganizationID BIGINT,
@OrganizationEsignID BIGINT
AS    
BEGIN

	DECLARE @TransactionResultId INT = 1

	IF OBJECT_ID('tempdb..#OrgEsignPlans') IS NOT NULL DROP TABLE #OrgEsignPlans
	IF OBJECT_ID('tempdb..#ServicePlans') IS NOT NULL DROP TABLE #ServicePlans

	IF(@OrganizationEsignID = 0)
	BEGIN
		IF EXISTS
		(
			SELECT
				1
			FROM
				OrganizationEsigns OE
			WHERE
				OE.OrganizationID = @OrganizationID
				AND OE.IsInProcess = 1
		)
		BEGIN
			-- If there is any InProgress Esign, then return that
			SELECT
				OE.OrganizationEsignID,
				OE.OrganizationID,
				O.CompanyName,
				O.DisplayName,
				O.Email,
				O.Mobile AS Phone,
				O.WorkPhone,
				OE.EsignTerms AS DefaultEsignTerms,
				OE.OrganizationTypeID,
				OE.IsCompleted,
				OE.IsInProcess
			FROM
				OrganizationEsigns OE
				INNER JOIN Organizations O ON OE.OrganizationID = O.OrganizationID
			WHERE
				OE.OrganizationID = @OrganizationID
				AND OE.IsInProcess = 1
		END
		ELSE
		BEGIN
			-- New Esign
			SELECT
				0 AS OrganizationEsignID,
				OrganizationID,
				CompanyName,
				DisplayName,
				Email,
				Mobile AS Phone,
				WorkPhone,
				DefaultEsignTerms,
				OT.OrganizationTypeID
			FROM
				Organizations O
				LEFT JOIN OrganizationTypes OT ON O.OrganizationTypeID = OT.OrganizationTypeID
			WHERE
				O.OrganizationID = @OrganizationID
		END		
	END
	ELSE
	BEGIN
		IF EXISTS
		(
			SELECT
				1
			FROM
				OrganizationEsigns OE
			WHERE
				OE.OrganizationEsignID = @OrganizationEsignID
				AND OE.IsInProcess = 0
		)
		BEGIN
			-- Return error as the Esign is already processed
			SET @TransactionResultId = -1
		END
		ELSE
		BEGIN
			-- Return the details of the existing esign
			SELECT
				OE.OrganizationEsignID,
				OE.OrganizationID,
				O.CompanyName,
				O.DisplayName,
				O.Email,
				O.Mobile AS Phone,
				O.WorkPhone,
				OE.EsignTerms AS DefaultEsignTerms,
				OE.OrganizationTypeID,
				OE.IsCompleted,
				OE.IsInProcess
			FROM
				OrganizationEsigns OE
				INNER JOIN Organizations O ON OE.OrganizationID = O.OrganizationID
			WHERE
				OE.OrganizationEsignID = @OrganizationEsignID
		END		
	END
     
    -- Select Organization Plans If added earlier
    SELECT
		*
	INTO
		#OrgEsignPlans
	FROM
		OrganizationEsignPlans OEP
	WHERE
		OEP.OrganizationEsignID = @OrganizationEsignID
    -- Insert Service Plans in temp table
    SELECT
		*
	INTO
		#ServicePlans
    FROM
    (
		SELECT
			SP.ServicePlanID,
			SP.ServicePlanName,
			SP.PerPatientPrice,
			SP.SetupFees,
			SP.NumberOfDaysForBilling,
			SPR.ModuleName,
			SPR.MaximumAllowedNumber,
			CASE WHEN OEP.ServicePlanID IS NOT NULL THEN 1 ELSE 0 END AS IsSelected
		FROM
			ServicePlans SP
			INNER JOIN ServicePlanRates SPR ON SP.ServicePlanID = SPR.ServicePlanID
			LEFT JOIN #OrgEsignPlans OEP ON SP.ServicePlanID = OEP.ServicePlanID
		WHERE
			SP.IsDeleted = 0
	) AS Temp
	PIVOT
	(
		MIN(MaximumAllowedNumber)
		FOR ModuleName IN ([Patient], [Facility], [Task], [Employee], [Billing])
	) AS Pvt
	
	-- Select Service Plans
	SELECT
		*
	FROM
		#ServicePlans
	
	-- Select Service Plan Components
	SELECT
		SPC.ServicePlanComponentID,
		SPC.ServicePlanID,
		DDM.DDMasterID,
		DDM.[Title]
	FROM
		ServicePlanComponents SPC
		INNER JOIN DDMaster DDM ON SPC.DDMasterID = DDM.DDMasterID
		INNER JOIN #ServicePlans SP ON SP.ServicePlanID = SPC.ServicePlanID
	
	SELECT
		OrganizationTypeName AS Name,
		OrganizationTypeID AS Value
	FROM
		OrganizationTypes
	
	SELECT
		ServiceTypeName AS Name,
		ServiceTypeID AS Value
	FROM	
		ServiceTypes
	
	-- Return whether we need to show the page or not
	SELECT 
		@TransactionResultId AS TransactionResultId
END