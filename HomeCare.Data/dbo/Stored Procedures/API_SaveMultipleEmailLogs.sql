CREATE PROCEDURE [dbo].[API_SaveMultipleEmailLogs]
 -- Add the parameters for the stored procedure here  
 @EmailBody VARCHAR(MAX),  
 @Subject VARCHAR(250),  
 @SentBy BIGINT,  
 @ToEmailList VARCHAR(MAX),  
 @IsAttachments BIT,  
 @AttachmentList VARCHAR(MAX),  
 @AttachmentBasePath VARCHAR(200),  
 @ServerCurrentDateTime VARCHAR(30)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 DECLARE @currentdate DATETIME = CAST(@ServerCurrentDateTime AS DATETIME);  
  
    -- Insert statements for procedure here  
 IF(LEN(@ToEmailList) > 0)  
 BEGIN  
  
   DECLARE @EmailUniqueId BIGINT = (SELECT TOP 1 EmailUniqueId FROM EmailLogs ORDER BY EmailLogID DESC)  
   IF(@EmailUniqueId IS NULL)  
   BEGIN  
    SET @EmailUniqueId = 1  
   END  
   ELSE  
   BEGIN  
    SET @EmailUniqueId = @EmailUniqueId + 1  
   END  
  
   INSERT INTO   
   EmailLogs   
   (Body,Subject,SentBy,ToEmail,IsAttachments,SentDate,Status,EmailUniqueId)     
   SELECT @EmailBody,@Subject,@SentBy,  
     Result,@IsAttachments,@currentdate,0,@EmailUniqueId-- AS CURRENT status of mail is unsent  
   FROM [dbo].[CSVtoTableWithIdentity](@ToEmailList,',');  
  
   IF(@IsAttachments=1 AND (LEN(@AttachmentList) > 0 ))  
   BEGIN  
     INSERT INTO   
     Attachments (EmailUniqueId,FileName,BasePath)  
     SELECT @EmailUniqueId,Result,@AttachmentBasePath  
     FROM [dbo].[CSVtoTableWithIdentity](@AttachmentList,',');  
   END  
 END  
  
END