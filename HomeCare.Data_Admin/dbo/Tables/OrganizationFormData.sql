CREATE TABLE [dbo].[OrganizationFormData] (
    [OrganizationFormDataId] INT            IDENTITY (1, 1) NOT NULL,
    [EBFormId]               NVARCHAR (500) NULL,
    [FormId]                 INT            NULL,
    [OrganizationId]         INT            NULL,
    [FormData]               NVARCHAR (MAX) NULL,
    [CreatedDate]            DATETIME       NULL,
    [ModifiedOn]             DATETIME       NULL,
    CONSTRAINT [PK_OrganizationFormData] PRIMARY KEY CLUSTERED ([OrganizationFormDataId] ASC)
);

