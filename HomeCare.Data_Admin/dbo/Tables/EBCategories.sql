CREATE TABLE [dbo].[EBCategories] (
    [EBCategoryID] NVARCHAR (MAX) NOT NULL,
    [Id]           NVARCHAR (MAX) NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    [CreatedDate]  DATETIME       NULL,
    [UpdatedDate]  DATETIME       NULL,
    [IsDeleted]    BIT            CONSTRAINT [DF_EBCategories_IsDeleted] DEFAULT ((0)) NOT NULL
);

