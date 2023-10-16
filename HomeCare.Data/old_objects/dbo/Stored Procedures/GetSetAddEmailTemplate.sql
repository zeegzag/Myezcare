CREATE PROCEDURE [dbo].[GetSetAddEmailTemplate]      
@EmailTemplateID BIGINT        
AS        
BEGIN        
  select *  from  EmailTemplates where EmailTemplateID=@EmailTemplateID    
 END
