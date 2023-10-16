       
CREATE PROCEDURE [API].[GetPatientContactData]              
    @ReferralID INT              
AS                                                 
BEGIN                
 SELECT               
  R.[ReferralID],
  C.[FirstName],
  C.[LastName],             
  C.[FirstName] + ' ' + C.[LastName] AS [Name],      
  CT.[ContactTypeName],                
  ISNULL(C.Email, 'NA') AS Email,      
  ISNULL(C.Phone1, 'NA') AS MainPhone,  
  C.[Address],
  C.[City],
  C.[State],
  C.[ZipCode],          
  ISNULL(C.[Address] + ' ' + C.[City] + ' ' + C.[State] + ' ' + C.[ZipCode], 'NA') AS FullAddress,  
   case when CM.[IsEmergencyContact] = 0 then 'No' else 'Yes' end AS [IsEmergencyContact]  
     
    FROM               
        [dbo].[Referrals] R              
    LEFT JOIN [dbo].[ContactMappings] CM              
        ON CM.[ReferralID] = R.[ReferralID]              
    LEFT JOIN [dbo].[Contacts] C              
        ON CM.[ContactID] = C.[ContactID]        
   LEFT JOIN [dbo].[ContactTypes] CT              
        ON CT.[ContactTypeID] = CM.[ContactTypeID]             
    WHERE              
        R.[ReferralID] = @ReferralID              
        AND R.[IsDeleted] = 0             
            
END 