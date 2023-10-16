
  
CREATE PROCEDURE [API].[GetPatientPersonalData]                          
  @ReferralID INT=0,                 
  @OrganizationID INT=0         
AS                                                             
BEGIN                     
                 
 SELECT                           
  R.[ReferralID],                     
  R.[FirstName],               
  ISNULL(R.MiddleName, 'NA') AS [MiddleName],                                    
  R.[LastName],  
  CONCAT(R.FirstName,' ',R.LastName) AS [ReferralFullName],  
  CONCAT(C.ApartmentNo,' ',C.Address,' ',C.City,' ',C.State,' ',C.ZipCode) AS [ReferralAddress],    
  CONCAT(R.FirstName,' ',R.LastName,',',C.ApartmentNo,' ',C.Address,' ',C.City,' ',C.State,' ',C.ZipCode) AS [ReferralNameAndAddress],  
  ISNULL(C.[ApartmentNo], 'NA') AS [ApartmentNo],  
  C.[Address],  
  C.[City],  
  C.[State],  
  C.[ZipCode],  
  CONVERT(date, Dob) AS [DateOfBirth],  
  DATEDIFF(hour,dob,GETDATE())/8766 AS Age,                     
  Case when R.[Gender] = 'M' then 'Male' when R.[Gender] = 'F' then 'Female' else 'Other' end AS [Gender],                      
  D.[Title] AS [Language],                        
  ISNULL(R.SocialSecurityNumber, 'NA') AS [SSN],                       
  R.[AHCCCSID] AS [AccountNumber],                      
  S.[Status],                        
  ISNULL(CMG.FirstName + ' ' + CMG.LastName, 'NA') AS [CaseManagerName],                  
  CONCAT('https://',(SELECT DomainName FROM [Admin_Myezcare_Live].[dbo].[Organizations] where OrganizationID=@OrganizationID),'.myezcare.com', ISNUll(R.ProfileImagePath, '/Assets/images/DefaultProfileImage.jpg')) AS ProfileImagePath,    
 -- CONCAT('https://','test','.myezcare.com', ISNUll(R.ProfileImagePath, '/Assets/images/DefaultProfileImage.jpg')) AS [ProfileImagePath],    
  ISNULL(E.FirstName + ' ' + E.LastName, 'NA') AS [Assignee],  
  ISNULL(R.Height, 'NA') AS [Height],  
  ISNULL(R.Weight, 'NA') AS [Weight],   
  ISNULL(R.BloodGroup, 'NA') AS [BloodGroup],   
  ISNULL(DM.Title, 'NA') AS [Race],  
  ISNULL(DME.Title, 'NA') AS [Ethnicity],  
  ISNULL(R.BMI, 'NA') AS [BMI],  
  convert(varchar, R.CreatedDate, 23) AS [CreatedDate]  
 FROM [dbo].[Referrals] R                        
 INNER JOIN [dbo].[DDMaster] D  ON D.[DDMasterID] = R.[LanguageID]                         
 INNER JOIN [dbo].[ReferralStatuses] S  ON S.[ReferralStatusID] = R.[ReferralStatusID]                        
 LEFT JOIN [dbo].[CaseManagers] CMG ON CMG.[CaseManagerID] = R.[CaseManagerID]          
 LEFT JOIN [dbo].[Employees] E ON E.[EmployeeID] = R.[Assignee]  
 INNER JOIN [dbo].[ContactMappings] CM ON CM.[ReferralID] = R.[ReferralID]        
 INNER JOIN [dbo].[Contacts] C ON C.[ContactID] = CM.[ContactID]   
 LEFT JOIN [dbo].[DDMaster] DM ON DM.DDMasterID = cast(R.Race AS bigint)  
 LEFT JOIN [dbo].[DDMaster] DME ON DME.DDMasterID = cast(R.Ethnicity AS bigint)                   
 WHERE                          
        R.[ReferralID] = @ReferralID                          
        --AND R.[IsDeleted] = 0                          
         
 SELECT 0;                    
END
