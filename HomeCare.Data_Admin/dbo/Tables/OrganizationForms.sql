CREATE TABLE [dbo].[OrganizationForms] (
    [OrganizationFormID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [EBFormID]                     NVARCHAR (MAX) NOT NULL,
    [OrganizationID]               BIGINT         NOT NULL,
    [CreatedDate]                  DATETIME       NULL,
    [UpdatedDate]                  DATETIME       NULL,
    [CreatedBy]                    BIGINT         NULL,
    [UpdatedBy]                    BIGINT         NULL,
    [OrganizationFriendlyFormName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_OrganizationForms] PRIMARY KEY CLUSTERED ([OrganizationFormID] ASC)
);

