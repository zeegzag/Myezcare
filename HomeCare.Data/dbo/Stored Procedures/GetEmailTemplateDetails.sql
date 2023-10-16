CREATE procedure [dbo].[GetEmailTemplateDetails]  
(  
  @EmailTemplateTypeID bigint  
)  
as  
begin  
  select e.EmailTemplateSubject,e.EmailTemplateBody from EmailTemplates e where e.EmailTemplateID=@EmailTemplateTypeID  
end