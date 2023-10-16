CREATE TABLE [dbo].[Allergy] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [Allergy]    NVARCHAR (MAX) NULL,
    [Status]     NVARCHAR (MAX) NULL,
    [Reaction]   NVARCHAR (MAX) NULL,
    [Comment]    NVARCHAR (MAX) NULL,
    [Patient]    BIGINT         NULL,
    [CreatedBy]  BIGINT         NULL,
    [CreatedOn]  DATETIME       CONSTRAINT [DF__Allergy__Created__03FC509B] DEFAULT (getdate()) NULL,
    [UpdatedBy]  BIGINT         NULL,
    [UpdatedOn]  DATETIME       NULL,
    [ReportedBy] BIGINT         NULL,
    [IsDeleted]  BIT            NULL,
    CONSTRAINT [PK__Allergy__3214EC07769F22E2] PRIMARY KEY CLUSTERED ([Id] ASC)
);

