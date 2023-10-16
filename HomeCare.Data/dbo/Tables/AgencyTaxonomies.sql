CREATE TABLE [dbo].[AgencyTaxonomies] (
    [TaxonomyID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [AgencyID]      BIGINT         NOT NULL,
    [Code]          NVARCHAR (50)  NULL,
    [Description]   NVARCHAR (500) NULL,
    [IsPrimary]     BIT            NULL,
    [State]         NVARCHAR (50)  NULL,
    [License]       NVARCHAR (100) NULL,
    [TaxonomyGroup] NVARCHAR (500) NULL,
    [IsDeleted]     BIT            NOT NULL,
    [CreatedDate]   DATETIME       NULL,
    [CreatedBy]     BIGINT         NULL,
    [UpdatedDate]   DATETIME       NULL,
    [UpdatedBy]     BIGINT         NULL,
    [SystemID]      VARCHAR (100)  NULL,
    CONSTRAINT [PK_AgencyTaxonomy] PRIMARY KEY CLUSTERED ([TaxonomyID] ASC)
);

