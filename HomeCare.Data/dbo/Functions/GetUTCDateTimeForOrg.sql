Create FUNCTION [dbo].[GetUTCDateTimeForOrg]
(
	@dateTime DATETIME
)         
RETURNS  DATETIME
AS
BEGIN 
    DECLARE @TimeZoneStr varchar(200);

	SELECT TOP 1 @TimeZoneStr=TimeZone FROM OrganizationSettings 
  
    SELECT @dateTime = (@dateTime at time zone @TimeZoneStr) AT TIME ZONE 'UTC'
  
	RETURN @dateTime; 
END