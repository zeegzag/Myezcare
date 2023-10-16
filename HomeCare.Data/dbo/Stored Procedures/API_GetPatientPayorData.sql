
CREATE PROCEDURE [dbo].[API_GetPatientPayorData]      
    @ReferralID INT      
AS                                         
BEGIN      
         
    SELECT       
        P.[PayorID],      
        ISNULL(P.[PayorName], 'NA') AS [PayorName],      
        RPM.[PayorEffectiveDate],     
        RPM.[PayorEffectiveEndDate],      
        RPM.[BeneficiaryTypeID],      
        ISNULL(M.[Title], 'NA') AS [BeneficiaryType],      
        ISNULL(RPM.[BeneficiaryNumber], 'NA') AS [BeneficiaryNumber]      
    FROM       
        [dbo].[ReferralPayorMappings] RPM      
    LEFT JOIN [dbo].[Payors] P      
        ON P.[PayorID] = RPM.[PayorID]      
    LEFT JOIN [dbo].[DDMaster] M      
        ON M.[DDMasterID] = RPM.[BeneficiaryTypeID]      
    WHERE      
        RPM.[ReferralID] = @ReferralID      
        AND RPM.[IsActive] = 1      
        AND RPM.[IsDeleted] = 0      
      
END