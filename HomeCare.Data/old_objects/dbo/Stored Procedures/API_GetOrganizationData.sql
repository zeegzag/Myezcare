
CREATE PROCEDURE [dbo].[API_GetOrganizationData] 
@OrganizationID INT  
AS                                         
BEGIN      
      
    SELECT TOP 1      
        OS.[OrganizationID],       
   CONCAT('https://',(SELECT DomainName FROM [Kundan_Admin].[dbo].[Organizations] where OrganizationID=@OrganizationID),'.myezcare.com', OS.[SiteLogo]) AS [SiteLogo],  
   --CONCAT('https://','test','.myezcare.com', '/uploads1/6ae72785-2738-41c4-84fb-65b378f1c283-202008031338133472.png') AS [SiteLogo],    
        OS.[SiteName],      
        OS.[SiteBaseUrl],    
        OS.[SupportEmail],      
        OS.[OrganizationAddress] + ' ' + OS.[OrganizationCity] + ' ' + OS.[OrganizationState] + ', ' + OS.[OrganizationZipcode] AS [OrgAddress]  
    FROM      
        [dbo].[OrganizationSettings] OS    
 
END 