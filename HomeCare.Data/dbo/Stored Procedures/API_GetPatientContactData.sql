﻿   
CREATE PROCEDURE [dbo].[API_GetPatientContactData]              
    @ReferralID INT              
AS                                                 
BEGIN             
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()          
 SELECT               
        R.[ReferralID],              
        dbo.GetGenericNameFormat(C.FirstName,'', C.LastName,@NameFormat) AS [Name],      
  CT.[ContactTypeName],                
        ISNULL(C.Email, 'NA') AS Email,      
        ISNULL(C.Phone1, 'NA') AS MainPhone,            
  ISNULL(C.[Address] + ' ' + C.[City] + ' ' + C.[State] + ' ' + C.[ZipCode], 'NA') AS FullAddress        
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