CREATE TABLE [dbo].[JO_Permissions] (
    [JO_PermissionID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [PermissionID]    BIGINT        NOT NULL,
    [PermissionName]  VARCHAR (100) NOT NULL,
    [Description]     VARCHAR (MAX) NULL,
    [ParentID]        BIGINT        NOT NULL,
    [Action]          CHAR (1)      NOT NULL,
    [ActionDate]      DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_Permissions] PRIMARY KEY CLUSTERED ([JO_PermissionID] ASC)
);

