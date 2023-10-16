CREATE TABLE [dbo].[JO_Roles] (
    [JO_RoleID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [RoleID]      BIGINT        NOT NULL,
    [RoleName]    VARCHAR (100) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [CreatedDate] DATETIME      NOT NULL,
    [CreatedBy]   BIGINT        NOT NULL,
    [UpdatedDate] DATETIME      NOT NULL,
    [UpdatedBy]   BIGINT        NOT NULL,
    [SystemID]    VARCHAR (100) NOT NULL,
    [Action]      CHAR (1)      NOT NULL,
    [ActionDate]  DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_Roles] PRIMARY KEY CLUSTERED ([JO_RoleID] ASC)
);

