CREATE procedure [dbo].[DeleteTemplate]
(
  @emailTemplateId bigint
)
as
begin
        update EmailTemplates set IsDeleted=1 where EmailTemplateID=@emailTemplateId;
        
end