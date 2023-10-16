CREATE PROCEDURE [dbo].[GetOrganizationPreference]
	@OrganizationID BIGINT=0          
AS              
BEGIN              
              
	SELECT * FROM OrganizationPreference Where OrganizationID = @OrganizationID
              
	SELECT * FROM Languages ORDER BY [Name] ASC       
	
	SELECT * FROM Currency ORDER BY [CurrencyName] ASC       
	
	SELECT * FROM CssConfig ORDER BY [CssDisplayName] ASC       
      
	Select 0;         

END