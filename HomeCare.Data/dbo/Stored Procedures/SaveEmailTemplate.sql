CREATE procedure [dbo].[SaveEmailTemplate]
(
 @EmailTemplateName varchar(500),
 @EmailTemplateSubject varchar(500),
 @EmailTemplateBody varchar(max),
 @CreatedBy bigint,
 @SystemID varchar(100),
 @EmailType nvarchar(150),
 @Module nvarchar(150)
)
as
begin
      INSERT INTO [dbo].[EmailTemplates]
           ([EmailTemplateName]
           ,[EmailTemplateSubject]
           ,[EmailTemplateBody]
           ,[EmailTemplateTypeID]
           ,[Token]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[SystemID]
           ,[IsDeleted]
           ,[OrderNumber]
           ,[IsEdit]
           ,[IsHide]
           ,[EmailType]
		    ,[Module])
     VALUES
           (@EmailTemplateName,
            @EmailTemplateSubject,
           @EmailTemplateBody,
           120,
           1,
           getdate(),
           @CreatedBy,
            getdate(),
           @CreatedBy,
           @SystemID,
           0,
           0,
           1,
           1,
           @EmailType,
		   @Module)
end