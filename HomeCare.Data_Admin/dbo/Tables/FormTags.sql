CREATE TABLE [dbo].[FormTags] (
    [FormTagID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [FormTagName] NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_FormTags] PRIMARY KEY CLUSTERED ([FormTagID] ASC)
);

