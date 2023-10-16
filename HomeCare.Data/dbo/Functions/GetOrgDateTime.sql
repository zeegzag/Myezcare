CREATE FUNCTION [dbo].[GetOrgDateTime](
	@DateTime datetime
)         
	RETURNS  DATETIME
AS
BEGIN 
    DECLARE @currentDateTime DATETIME;
    DECLARE @TimeZoneStr varchar(200);
    DECLARE @TimeZone varchar(10);

	SELECT TOP 1 @TimeZoneStr=TimeZone FROM OrganizationSettings 
	--SELECT @TimeZoneStr
	
	SELECT @TimeZone=RIGHT(cast(getutcdate() as datetime) AT TIME ZONE @TimeZoneStr,6)
	--SELECT @TimeZone

	select @currentDateTime=CONVERT(datetime,SWITCHOFFSET(CONVERT(datetimeoffset,@DateTime),@TimeZone))
	--SELECT @currentDateTime
  
	RETURN @currentDateTime; 
END