CREATE TABLE [dbo].[OrganizationPermission] (
    [OrgPermissionId] INT      IDENTITY (1, 1) NOT NULL,
    [OrganizationId]  BIGINT   NULL,
    [PermissionId]    BIGINT   NULL,
    [CreatedBy]       INT      NULL,
    [CreatedDate]     DATETIME NULL,
    [UpdatedBy]       INT      NULL,
    [UpdatedDate]     DATETIME NULL,
    [IsDeleted]       BIT      CONSTRAINT [DF_OrganizationPermission_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__Organiza__53C5598E1264DDEA] PRIMARY KEY CLUSTERED ([OrgPermissionId] ASC)
);

