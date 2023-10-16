CREATE PROCEDURE [dbo].[API_GetPatientMedicationData]          
    @ReferralID INT          
AS                                             
BEGIN        
          
    SELECT         
  RM.[ReferralID],      
        M.[MedicationID],        
        ISNULL(M.[MedicationName], 'NA') AS [MedicationName],     
  P.[PhysicianID],    
  ISNULL(P.[FirstName] + ' ' + P.[LastName], 'NA') AS [PhysicianName],        
        ISNULL(RM.[Dose], 'NA') AS [Strength],        
        ISNULL(RM.[Unit], 'NA') AS [Unit],        
        ISNULL(RM.[Frequency], 'NA') AS [Frequency],        
        RM.[Route],        
        ISNULL(RM.[Quantity], 'NA') AS [Quantity],        
        RM.[StartDate],        
        RM.[EndDate],        
        RM.[IsActive]        
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