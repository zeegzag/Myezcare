-- EXEC GetCustomerEsignDetails 1
CREATE PROCEDURE [dbo].[GetCustomerEsignDetails]
@OrganizationEsignID BIGINT
AS    
BEGIN
	IF OBJECT_ID('tempdb..#ServicePlans') IS NOT NULL DROP TABLE #ServicePlans

	DECLARE @TransactionResultId INT = 1
	DECLARE @OrganizationID BIGINT = 1

	SELECT
		OE.OrganizationEsignID,
		OE.OrganizationID,
		O.CompanyName,
		O.DisplayName,
		O.Email,
		O.Mobile AS Phone,
		O.WorkPhone,
		OE.EsignTerms AS DefaultEsignTerms,
		OE.OrganizationTypeID
	FROM
		OrganizationEsigns OE
		INNER JOIN Organizations O ON OE.OrganizationID = O.OrganizationID
	WHERE
		OE.OrganizationEsignID = @OrganizationEsignID
     
    SELECT
		*
	FROM
		OrganizationSettings OS
		INNER JOIN Organizations O ON OS.OrganizationID = O.OrganizationID
	WHERE
		OS.OrganizationID = @OrganizationID
     
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
			SPR.MaximumAllowedNumber
		FROM
			ServicePlans SP
			INNER JOIN ServicePlanRates SPR ON SP.ServicePlanID = SPR.ServicePlanID
			INNER JOIN OrganizationEsignPlans OEP ON SP.ServicePlanID = OEP.ServicePlanID
		WHERE			
			OEP.OrganizationEsignID = @OrganizationEsignID
			AND SP.IsDeleted = 0			
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
	
	-- Return whether we need to show the page or not
	SELECT 
		@TransactionResultId AS TransactionResultId
END