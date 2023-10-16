CREATE TABLE [dbo].[VisitTaskCategories] (
    [VisitTaskCategoryID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [VisitTaskCategoryName] NVARCHAR (200) NOT NULL,
    [VisitTaskCategoryType] NVARCHAR (20)  NULL,
    [ParentCategoryLevel]   BIGINT         NULL,
    CONSTRAINT [PK_VisitTaskCategories] PRIMARY KEY CLUSTERED ([VisitTaskCategoryID] ASC)
);

