--   EXEC [API].[GetPatientPhysicianData] 60053        

CREATE PROCEDURE [API].[GetPatientPhysicianData]            
    @ReferralID INT            
AS                                               
BEGIN          
            
SELECT         
  RM.[ReferralID],        
  P.[PhysicianID], 
  P.[FirstName],
  ISNULL(P.[MiddleName], 'NA') AS [MiddleName],
  P.[LastName],       
  ISNULL(P.[FirstName] + ' ' + P.[LastName], 'NA') AS [PhysicianName],
  ISNULL(P.[Email], 'NA') AS [Email],
  ISNULL(P.[Mobile], 'NA') AS [Mobile],
  ISNULL(P.[Phone], 'NA') AS [Phone],
  P.[Address],
  P.[City],
  P.[StateCode],
  P.[ZipCode],
  ISNULL(P.[Address] + ' ' + P.[City] + ' ' + P.[StateCode] + ', ' + P.[ZipCode], 'NA') AS [PhysicianAddress],
  P.[PhysicianTypeID]
    FROM           
        [dbo].[ReferralPhysicians] RM          
    LEFT JOIN [dbo].[Physicians] P          
        ON RM.[PhysicianID] = P.[PhysicianID]         
    WHERE          
        RM.[ReferralID] = @ReferralID
        AND RM.[IsDeleted] = 0          
          
END