CREATE PROCEDURE [API].[GetPatientDxCodeDataAsCommaSeparted]        
    @ReferralID INT        
AS                                           
BEGIN      
        
    SELECT      
	STRING_AGG ( ISNULL(D.DXCodeName,'N/A'), ',') AS CS_DXCodeName  
    FROM      
        [dbo].[ReferralDXCodeMappings] RDM      
    LEFT JOIN [dbo].[DXCodes] D      
        ON D.[DXCodeID] = RDM.[DXCodeID]          
    WHERE      
        RDM.[ReferralID] = @ReferralID      
        AND RDM.[IsDeleted] = 0        
      
END
