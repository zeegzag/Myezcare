CREATE PROCEDURE [dbo].[GetOrganizationData]   
@OrganizationName NVARCHAR(1000)=''
AS  
BEGIN  
  

SELECT TOP 1 * FROM Organizations  WHERE  (@OrganizationName IS NULL OR LEN(@OrganizationName)=0 OR @OrganizationName=DomainName) 
  
END