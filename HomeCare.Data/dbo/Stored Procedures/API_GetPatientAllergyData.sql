--   EXEC API_GetPatientAllergyData 60053       
    
CREATE PROCEDURE [dbo].[API_GetPatientAllergyData]      
    @ReferralID INT      
AS                                         
BEGIN      
         
    SELECT       
        A.[Id],      
        ISNULL(A.[Allergy], 'NA') AS [Allergy],
		ISNULL(A.[Reaction], 'NA') AS [Reaction],
		ISNULL(A.[Comment], 'NA') AS [Comment],      
        A.[Status]   
    FROM       
        [dbo].[Allergy] A      
     
    WHERE      
        A.[Patient] = @ReferralID           
        AND A.[IsDeleted] = 0      
      
END