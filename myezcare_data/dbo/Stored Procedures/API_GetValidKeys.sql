  
CREATE PROCEDURE [dbo].[API_GetValidKeys]  
 -- Add the parameters for the stored procedure here  
 @KeyExpirationTimeInCache INT,  
 @ServerCurrentDateTime NVARCHAR(30)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 DECLARE @CurrentDateTime DATETIME = CAST(@ServerCurrentDateTime AS DATETIME);   
    -- Insert statements for procedure here  
 SELECT   
   SUBSTRING(  
   (SELECT ',' + AppKey  
   FROM dbo.AppKeyDetails akd  
   FOR XML PATH('')),2,200000) AS KeyValues,  
   DATEADD(mi,@KeyExpirationTimeInCache,@CurrentDateTime) AS ExpireLogin;  
END  
