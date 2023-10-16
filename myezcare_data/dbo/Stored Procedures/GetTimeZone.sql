
CREATE PROCEDURE [dbo].[GetTimeZone]  
AS  
BEGIN  
 SELECT TOP 1 TimeZone FROM OrganizationSettings  
END

