CREATE TABLE [dbo].[JO_EmailTemplates] (
    [JO_EmailTemplateID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmailTemplateID]      BIGINT        NOT NULL,
    [EmailTemplateName]    VARCHAR (500) NULL,
    [EmailTemplateSubject] VARCHAR (500) NULL,
    [EmailTemplateBody]    VARCHAR (MAX) NOT NULL,
    [EmailTemplateTypeID]  BIGINT        NOT NULL,
    [Token]                VARCHAR (MAX) NULL,
    [CreatedDate]          DATETIME      NULL,
    [CreatedBy]            BIGINT        NULL,
    [UpdatedDate]          DATETIME      NULL,
    [UpdatedBy]            BIGINT        NULL,
    [SystemID]             VARCHAR (100) NULL,
    [IsDeleted]            BIT           NULL,
    [Action]               CHAR (1)      NOT NULL,
    [ActionDate]           DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_EmailTemplates] PRIMARY KEY CLUSTERED ([JO_EmailTemplateID] ASC)
);

