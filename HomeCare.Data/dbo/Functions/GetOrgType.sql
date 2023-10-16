

CREATE FUNCTION [dbo].[GetOrgType]()         
RETURNS  NVARCHAR(MAX)
AS
BEGIN 
    RETURN (SELECT TOP 1 OrganizationType FROM [Admin_Myezcare_Live].[dbo].[Organizations] where DBName = DB_NAME()) 
END