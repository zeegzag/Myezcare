              
CREATE PROCEDURE [dbo].[API_GetPatientPersonalData]                      
  @ReferralID INT=0,             
  @OrganizationID INT=0     
AS                                                         
BEGIN                 
            
                  
 SELECT                       
  R.[ReferralID],                 
  R.[FirstName],           
  ISNULL(R.MiddleName, 'NA') AS MiddleName,                                
  R.[LastName],              
  CONVERT(date, Dob) AS DateOfBirth ,                  
  R.[Gender],                    
  D.[Title] AS [Language],                    
  ISNULL(R.SocialSecurityNumber, 'NA') AS [SSN],                   
  R.[AHCCCSID] AS [AccountNumber],                  
  S.[Status],                    
  ISNULL(C.FirstName + ' ' + C.LastName, 'NA') AS CaseManagerName,              
  CONCAT('https://',(SELECT DomainName FROM [Kundan_Admin].[dbo].[Organizations] where OrganizationID=@OrganizationID),'.myezcare.com', ISNUll(R.ProfileImagePath, '/Assets/images/DefaultProfileImage.jpg')) AS ProfileImagePath,
  --CONCAT('https://','test','.myezcare.com', ISNUll(R.ProfileImagePath, '/Assets/images/DefaultProfileImage.jpg')) AS ProfileImagePath,
  ISNULL(E.FirstName + ' ' + E.LastName, 'NA') AS Assignee       
 FROM [dbo].[Referrals] R                    
 INNER JOIN [dbo].[DDMaster] D  ON D.[DDMasterID] = R.[LanguageID]                     
 INNER JOIN [dbo].[ReferralStatuses] S  ON S.[ReferralStatusID] = R.[ReferralStatusID]                    
 LEFT JOIN [dbo].[CaseManagers] C ON C.[CaseManagerID] = R.[CaseManagerID]      
 LEFT JOIN [dbo].[Employees] E ON E.[EmployeeID] = R.[Assignee]                    
 WHERE                      
        R.[ReferralID] = @ReferralID                      
        AND R.[IsDeleted] = 0                      
                  
 select 0;                
END 