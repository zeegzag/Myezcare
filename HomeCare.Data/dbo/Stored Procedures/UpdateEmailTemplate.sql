CREATE procedure [dbo].[UpdateEmailTemplate]
(
 @EmailTemplateID bigint,
 @EmailTemplateName varchar(500),
 @EmailTemplateSubject varchar(500),
 @EmailTemplateBody varchar(max),
 @UpdatedBy bigint,
 @SystemID varchar(100),
 @EmailType nvarchar(150),
 @Module nvarchar(150)

)
as
begin

update EmailTemplates set EmailTemplateName=@EmailTemplateName,EmailTemplateSubject=@EmailTemplateSubject,
EmailTemplateBody=@EmailTemplateBody,UpdatedDate=getdate(),UpdatedBy=@UpdatedBy,SystemID=@SystemID,
EmailType=@EmailType,Module=@Module where EmailTemplateID=@EmailTemplateID

end