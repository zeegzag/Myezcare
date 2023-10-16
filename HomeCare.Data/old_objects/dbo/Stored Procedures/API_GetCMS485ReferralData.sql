--   EXEC API_GetCMS485ReferralData 11, 11                 
                      
CREATE PROCEDURE [dbo].[API_GetCMS485ReferralData]                        
  @ReferralID INT=0 ,
  @OrganizationID INT=0   
AS                                                           
BEGIN                   
                                 
Select R.ReferralID,CONCAT(R.FirstName,' ',R.LastName,',',C.ApartmentNo,' ',C.Address,' ',C.City,' ',C.State,' ',C.ZipCode) AS ReferralNameAndAddress, R.Dob, R.Gender from Referrals R         
INNER JOIN ContactMappings CM ON CM.ReferralID = R.ReferralID      
INNER JOIN Contacts C ON C.ContactID = CM.ContactID      
 WHERE R.ReferralID = @ReferralID 
 
  SELECT TOP 1        
        OS.[OrganizationID],         
   --CONCAT('https://',(SELECT DomainName FROM [Kundan_Admin].[dbo].[Organizations] where OrganizationID=@OrganizationID),'.myezcare.com', OS.[SiteLogo]) AS [SiteLogo],    
   CONCAT('https://','test','.myezcare.com', '/uploads1/6ae72785-2738-41c4-84fb-65b378f1c283-202008031338133472.png') AS [SiteLogo],      
        OS.[SiteName],             
        OS.[SupportEmail],        
        OS.[OrganizationAddress] + ' ' + OS.[OrganizationCity] + ' ' + OS.[OrganizationState] + ', ' + OS.[OrganizationZipcode] AS [OrgAddress],
		CONCAT(OS.[SiteName],', ',OS.[OrganizationAddress],' ',OS.[OrganizationCity],' ',OS.[OrganizationState],' ',OS.[OrganizationZipcode], ', ',Submitter_EDIContact1_PER04_CommunicationNumber) AS [OrgDetails]   
    FROM        
        [dbo].[OrganizationSettings] OS                       
                    
 Select 0;                  
END 