Create FUNCTION [dbo].[GetOrgCurrentDateTime]()         
RETURNS  DATETIME
AS
BEGIN 
	RETURN [dbo].[GetOrgDateTime](GetUTCDate()) ; 
END