CREATE TABLE [dbo].[SectionPermissions] (
    [SectionPermissionID] BIGINT IDENTITY (1, 1) NOT NULL,
    [ComplianceID]        BIGINT NOT NULL,
    [RoleID]              BIGINT NOT NULL,
    CONSTRAINT [PK_SectionPermissions] PRIMARY KEY CLUSTERED ([SectionPermissionID] ASC)
);

