CREATE FUNCTION [dbo].[GetOrgDomain]()           
RETURNS  NVARCHAR(MAX)  
AS  
BEGIN   
    RETURN CONCAT('https://',(SELECT TOP 1 DomainName FROM [Admin_Myezcare_Live].[dbo].[Organizations] where OrganizationID = [dbo].[GetOrgId]()),'.myezcare.com')    
END