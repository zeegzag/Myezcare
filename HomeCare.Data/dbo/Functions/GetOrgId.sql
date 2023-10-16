CREATE FUNCTION [dbo].[GetOrgId]()         
RETURNS  NVARCHAR(MAX)
AS
BEGIN 
    RETURN (SELECT TOP 1 OrganizationID FROM [Admin_Myezcare_Live].[dbo].[Organizations] where DBName = DB_NAME()) 
END