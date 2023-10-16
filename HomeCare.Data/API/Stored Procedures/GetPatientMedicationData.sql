
CREATE PROCEDURE [API].[GetPatientMedicationData]          
    @ReferralID INT          
AS                                             
BEGIN        
      SELECT         
      RM.[ReferralID],      
            M.[MedicationId] [MedicationID],        
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
            RM.[IsActive],
		    DT.Title DosageTime
 	    FROM         
            [dbo].[ReferralMedication] RM        
        LEFT JOIN [dbo].[Medication] M        
            ON RM.[MedicationId] = M.[MedicationId]      
        LEFT JOIN [dbo].[Physicians] P            
            ON RM.[PhysicianID] = P.[PhysicianID]       
	    OUTER APPLY
		    (
		    SELECT Title, Val FROM  GetCSVTable(DosageTime)C INNER JOIN DDMaster DM ON  DM.DDMasterID = C.val
 		    ) DT
	    WHERE        
            RM.[ReferralID] = @ReferralID        
            AND RM.[IsDeleted] = 0         
        ORDER BY DT.Title DESC 
    
END