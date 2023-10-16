CREATE FUNCTION [dbo].[GetGeoFromLatLng]
(  
    @lat decimal(10,7),   
    @lng decimal(10,7)  
)  
RETURNS geography WITH SCHEMABINDING  
AS  
BEGIN  
    DECLARE @ret geography  
  
    select @ret = geography::STPointFromText('POINT(' + convert(nvarchar(25), @lng) +' ' + convert(nvarchar(25), @lat) + ')', 4326);          
  
    RETURN @ret  
END
