CREATE TABLE [dbo].[JO_RolePermissionMapping] (
    [JO_RolePermissionMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [RolePermissionMappingID]    BIGINT        NOT NULL,
    [RoleID]                     BIGINT        NOT NULL,
    [PermissionID]               BIGINT        NOT NULL,
    [CreatedDate]                DATETIME      NOT NULL,
    [CreatedBy]                  BIGINT        NOT NULL,
    [UpdatedDate]                DATETIME      NOT NULL,
    [UpdatedBy]                  BIGINT        NOT NULL,
    [SystemID]                   VARCHAR (100) NOT NULL,
    [IsDeleted]                  BIT           NOT NULL,
    [Action]                     CHAR (1)      NOT NULL,
    [ActionDate]                 DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_RolePermissionMapping] PRIMARY KEY CLUSTERED ([JO_RolePermissionMappingID] ASC)
);

