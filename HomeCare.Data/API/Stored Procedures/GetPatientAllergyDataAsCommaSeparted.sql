CREATE PROCEDURE [API].[GetPatientAllergyDataAsCommaSeparted]        
    @ReferralID INT        
AS                                           
BEGIN        
           
SELECT         
  STRING_AGG ( ISNULL(A.Allergy,'N/A'), ',') AS CS_Allergy,
  STRING_AGG ( ISNULL(A.Reaction,'N/A'), ',') AS CS_Reaction       
    FROM         
        [dbo].[Allergy] A        
       
    WHERE        
        A.[Patient] = @ReferralID             
        AND A.[IsDeleted] = 0 
        
END 