CREATE TABLE [dbo].[EmailTemplates] (
    [EmailTemplateID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmailTemplateName]    VARCHAR (500)  NULL,
    [EmailTemplateSubject] VARCHAR (500)  NULL,
    [EmailTemplateBody]    VARCHAR (MAX)  NOT NULL,
    [EmailTemplateTypeID]  VARCHAR (50)   NOT NULL,
    [Token]                VARCHAR (MAX)  NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [UpdatedDate]          DATETIME       NOT NULL,
    [UpdatedBy]            BIGINT         NOT NULL,
    [SystemID]             VARCHAR (100)  NOT NULL,
    [IsDeleted]            BIT            NOT NULL,
    [OrderNumber]          INT            NULL,
    [IsEdit]               BIT            DEFAULT ((0)) NULL,
    [IsHide]               BIT            DEFAULT ((0)) NULL,
    [EmailType]            NVARCHAR (125) NULL,
    [Module]               NVARCHAR (125) NULL,
    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED ([EmailTemplateID] ASC),
    CONSTRAINT [FK_EmailTemplates_EmailTemplates] FOREIGN KEY ([EmailTemplateID]) REFERENCES [dbo].[EmailTemplates] ([EmailTemplateID])
);


GO
CREATE TRIGGER [dbo].[tr_EmailTemplates_Updated] ON [dbo].[EmailTemplates]  
FOR UPDATE AS   
  
INSERT INTO JO_EmailTemplates(   
EmailTemplateID,  
EmailTemplateName,  
EmailTemplateSubject,  
EmailTemplateBody,  
EmailTemplateTypeID,  
CreatedDate,
CreatedBy,UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
Action,ActionDate  
)  
  
SELECT    
EmailTemplateID,  
EmailTemplateName,  
EmailTemplateSubject,  
EmailTemplateBody,  
EmailTemplateTypeID,  
CreatedDate,
CreatedBy,UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
'U',GETUTCDATE() FROM deleted