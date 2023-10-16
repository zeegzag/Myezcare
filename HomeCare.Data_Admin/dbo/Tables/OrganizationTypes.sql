CREATE TABLE [dbo].[OrganizationTypes] (
    [OrganizationTypeID]   BIGINT         NOT NULL,
    [OrganizationTypeName] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_OrganizationTypes] PRIMARY KEY CLUSTERED ([OrganizationTypeID] ASC)
);

