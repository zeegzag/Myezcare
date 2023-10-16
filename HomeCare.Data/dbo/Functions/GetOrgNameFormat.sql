

CREATE FUNCTION [dbo].[GetOrgNameFormat]()    
RETURNS VARCHAR(MAX)    
AS    
   
BEGIN
	Declare @OrgID bigint  
	declare @nameformat nvarchar(max)  
	SELECT TOP 1 @OrgID=OrganizationID FROM DEVAdmin.[dbo].[Organizations] where DBName = DB_NAME()  
	select @nameformat= op.NameDisplayFormat from DEVAdmin.dbo.OrganizationPreference op where op.OrganizationID=@OrgID  

	RETURN @nameformat     
END
