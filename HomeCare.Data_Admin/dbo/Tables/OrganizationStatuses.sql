CREATE TABLE [dbo].[OrganizationStatuses] (
    [OrganizationStatusID]   BIGINT          NOT NULL,
    [OrganizationStatusName] NVARCHAR (1000) NOT NULL,
    [DisplayValue]           NVARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_OrganizationStatuses] PRIMARY KEY CLUSTERED ([OrganizationStatusID] ASC)
);

