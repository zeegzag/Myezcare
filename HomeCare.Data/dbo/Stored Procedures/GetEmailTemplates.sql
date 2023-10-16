CREATE procedure [dbo].[GetEmailTemplates]  
as  
begin  
  select e.EmailTemplateID,e.EmailTemplateName from EmailTemplates e where e.Module='Patient' and e.EmailType='Email' and e.IsDeleted=0  
end