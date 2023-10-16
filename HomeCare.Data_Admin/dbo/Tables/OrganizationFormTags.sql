CREATE TABLE [dbo].[OrganizationFormTags] (
    [OrganizationFormTagID] BIGINT IDENTITY (1, 1) NOT NULL,
    [OrganizationFormID]    BIGINT NOT NULL,
    [FormTagID]             BIGINT NOT NULL,
    CONSTRAINT [PK_OrganizationFormTags] PRIMARY KEY CLUSTERED ([OrganizationFormTagID] ASC)
);

