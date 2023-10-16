
--    EXEC [API].[GetOrganizationData] 11 
 
CREATE PROCEDURE [API].[GetOrganizationData]   
@OrganizationID VARCHAR(100)     
AS                                           
BEGIN        
        
  SELECT TOP 1        
    OS.[OrganizationID],          
   CONCAT('https://',(SELECT DomainName FROM [Admin_Myezcare_Live].[dbo].[Organizations] where OrganizationID=@OrganizationID),'.myezcare.com', OS.[SiteLogo]) AS [SiteLogo],    
   --CONCAT('https://','test','.myezcare.com', '/uploads1/6ae72785-2738-41c4-84fb-65b378f1c283-202008031338133472.png') AS [SiteLogo],      
   OS.[SiteName],             
   OS.[SupportEmail],
   OS.[Submitter_EDIContact1_PER04_CommunicationNumber] AS [Phone], 
   OS.[OrganizationAddress],
   OS.[OrganizationCity],
   OS.[OrganizationState], 
   OS.[OrganizationZipcode],   
   OS.[OrganizationAddress] + ' ' + OS.[OrganizationCity] + ' ' + OS.[OrganizationState] + ', ' + OS.[OrganizationZipcode] AS [OrgAddress],
	CONCAT(OS.[SiteName],', ',OS.[OrganizationAddress],' ',OS.[OrganizationCity],' ',OS.[OrganizationState],' ',OS.[OrganizationZipcode], ', ',Submitter_EDIContact1_PER04_CommunicationNumber) AS [OrgSiteNameAndAddress],
	OS.[TermsCondition]
    FROM        
        OrganizationSettings OS     
    
        
END