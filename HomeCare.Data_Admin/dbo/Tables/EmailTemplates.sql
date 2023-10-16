CREATE TABLE [dbo].[EmailTemplates] (
    [EmailTemplateID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmailTemplateName]    VARCHAR (500) NULL,
    [EmailTemplateSubject] VARCHAR (500) NULL,
    [EmailTemplateBody]    VARCHAR (MAX) NOT NULL,
    [EmailTemplateTypeID]  VARCHAR (50)  NOT NULL,
    [Token]                VARCHAR (MAX) NULL,
    [CreatedDate]          DATETIME      NOT NULL,
    [CreatedBy]            BIGINT        NOT NULL,
    [UpdatedDate]          DATETIME      NOT NULL,
    [UpdatedBy]            BIGINT        NOT NULL,
    [SystemID]             VARCHAR (100) NOT NULL,
    [IsDeleted]            BIT           NOT NULL,
    [OrderNumber]          INT           NULL,
    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED ([EmailTemplateID] ASC)
);

