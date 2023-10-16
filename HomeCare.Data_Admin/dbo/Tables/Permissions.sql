CREATE TABLE [dbo].[Permissions] (
    [PermissionID]       BIGINT         NOT NULL,
    [PermissionName]     VARCHAR (100)  NOT NULL,
    [Description]        VARCHAR (MAX)  NULL,
    [ParentID]           BIGINT         NOT NULL,
    [OrderID]            INT            NULL,
    [IsDeleted]          BIT            CONSTRAINT [DF__Permissio__IsDel__59063A47] DEFAULT ((0)) NOT NULL,
    [PermissionCode]     VARCHAR (1000) NULL,
    [PermissionPlatform] VARCHAR (100)  NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([PermissionID] ASC)
);

