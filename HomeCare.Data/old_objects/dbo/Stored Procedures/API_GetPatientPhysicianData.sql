CREATE PROCEDURE [dbo].[API_GetPatientPhysicianData]            
    @ReferralID INT            
AS                                               
BEGIN          
            
       SELECT DISTINCT         
  RM.[ReferralID],        
        P.[PhysicianID],        
  ISNULL(P.[FirstName] + ' ' + P.[LastName], 'NA') AS PhysicianName,          
        ISNULL(P.[Email], 'NA') AS Email,             
        ISNULL(P.[Mobile], 'NA') AS Mobile,        
  ISNULL(P.[Address] + ' ' + P.[City] + ' ' + P.[StateCode] + ' ' + P.[ZipCode], 'NA') AS PhysicianAddress  
    FROM           
        [dbo].[ReferralMedication] RM          
    LEFT JOIN [dbo].[Medication] M          
        ON RM.[MedicationId] = M.[MedicationId]         
    LEFT JOIN [dbo].[Physicians] P          
        ON RM.[PhysicianID] = P.[PhysicianID]         
    WHERE          
        RM.[ReferralID] = @ReferralID          
        AND RM.[IsDeleted] = 0           
          
END 