CREATE PROCEDURE [dbo].[GetSetAddEmailTemplate]        
@EmailTemplateID BIGINT          
AS          
BEGIN          
  select e.EmailTemplateID,e.EmailTemplateName,e.EmailTemplateTypeID,e.EmailTemplateSubject,e.EmailTemplateBody,e.Token,e.CreatedDate,e.CreatedBy,e.UpdatedDate,e.UpdatedBy,e.SystemID,e.IsDeleted,e.OrderNumber,e.IsEdit as 'IsEditUser',e.IsHide,e.EmailType 
as 'Email',e.Module   from  EmailTemplates e where  EmailTemplateID=@EmailTemplateID      
 END