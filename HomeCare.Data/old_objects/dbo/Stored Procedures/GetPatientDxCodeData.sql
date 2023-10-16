
CREATE PROCEDURE [API].[GetPatientDxCodeData]        
    @ReferralID INT        
AS                                           
BEGIN      
        
    SELECT      
  RDM.[ReferralID],    
        RDM.[DXCodeID],      
        ISNULL(D.[DXCodeName], 'NA') AS [DXCodeName],      
        DT.[DxCodeTypeID],     
        ISNULL(DT.[DxCodeTypeName], 'NA') AS [DxCodeTypeName],      
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