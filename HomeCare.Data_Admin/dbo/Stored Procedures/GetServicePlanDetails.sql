-- EXEC GetServicePlanDetails @ServicePlanID = 4
CREATE PROCEDURE [dbo].[GetServicePlanDetails]
@ServicePlanID BIGINT
AS    
BEGIN        
	SELECT
		*
	FROM
		ServicePlans SP
	WHERE
		SP.ServicePlanID = @ServicePlanID
     
    -- Service Plan Modules
    SELECT
		ModuleID,
		ModuleName,
		MaximumAllowedNumber
	FROM
		ServicePlanRates SPR
	WHERE
		SPR.ServicePlanID = @ServicePlanID		
		
	-- Service Plan Components
	SELECT
		SPC.ServicePlanComponentID,
		SPC.ServicePlanID,
		DDM.DDMasterID,
		DDM.Title
	FROM
		ServicePlanComponents SPC
		INNER JOIN DDMaster DDM ON SPC.DDMasterID = DDM.DDMasterID
	WHERE
		SPC.ServicePlanID = @ServicePlanID
END