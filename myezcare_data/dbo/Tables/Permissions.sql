CREATE TABLE [dbo].[Permissions] (
    [PermissionID]       BIGINT         NOT NULL,
    [PermissionName]     VARCHAR (100)  NOT NULL,
    [Description]        VARCHAR (MAX)  NULL,
    [ParentID]           BIGINT         NOT NULL,
    [OrderID]            INT            NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [UsedInHomeCare]     BIT            DEFAULT ((0)) NOT NULL,
    [PermissionCode]     VARCHAR (1000) NULL,
    [CompanyHasAccess]   BIT            DEFAULT ((0)) NOT NULL,
    [PermissionPlatform] VARCHAR (100)  NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([PermissionID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_UC_PermissionCode_NotNull]
    ON [dbo].[Permissions]([PermissionCode] ASC) WHERE ([PermissionCode] IS NOT NULL);


GO
CREATE TRIGGER [dbo].[tr_Permissions_Deleted] ON [dbo].[Permissions]
FOR Delete AS 

INSERT INTO JO_Permissions( 
PermissionID,
PermissionName,
Description,
ParentID,
Action,ActionDate
)

SELECT  
PermissionID,
PermissionName,
Description,
ParentID,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Permissions_Deleted]
    ON [dbo].[Permissions];


GO
CREATE TRIGGER [dbo].[tr_Permissions_Updated] ON [dbo].[Permissions]
FOR UPDATE AS 

INSERT INTO JO_Permissions( 
PermissionID,
PermissionName,
Description,
ParentID,
Action,ActionDate
)

SELECT  
PermissionID,
PermissionName,
Description,
ParentID,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Permissions_Updated]
    ON [dbo].[Permissions];

