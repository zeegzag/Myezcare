
CREATE PROCEDURE [dbo].[API_GetPatientDxCodeData]        
    @ReferralID INT        
AS                                           
BEGIN      
        
    SELECT      
  RDM.[ReferralID],    
        RDM.[DXCodeID],      
        ISNULL(D.[DXCodeName], 'NA') AS [DXCodeName],      
        DT.[DxCodeTypeID],     
        ISNULL(DT.[DxCodeTypeName], 'NA') AS [DxCodeTypeName],      
        ISNULL(DT.[DxCodeShortName], 'NA') AS [DxCodeShortName],      
        ISNULL(D.[Description], 'NA') AS [Description],      
        RDM.[StartDate],      
        RDM.[EndDate]      
    FROM      
        [dbo].[ReferralDXCodeMappings] RDM      
    LEFT JOIN [dbo].[DXCodes] D      
        ON D.[DXCodeID] = RDM.[DXCodeID]      
    LEFT JOIN [dbo].[DxCodeTypes] DT      
        ON D.[DxCodeType] = DT.[DxCodeTypeID]      
    WHERE      
        RDM.[ReferralID] = @ReferralID      
        AND RDM.[IsDeleted] = 0        
      
END