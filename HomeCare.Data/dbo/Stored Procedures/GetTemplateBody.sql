CREATE procedure [dbo].[GetTemplateBody]  
(  
  
@templateid bigint  
  
)  
as  
begin  
      select e.EmailTemplateBody from EmailTemplates e where e.EmailTemplateID=@templateid;  
end