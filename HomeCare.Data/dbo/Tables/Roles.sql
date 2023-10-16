CREATE TABLE [dbo].[Roles] (
    [RoleID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [RoleName]    VARCHAR (100) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [CreatedDate] DATETIME      NOT NULL,
    [CreatedBy]   BIGINT        NOT NULL,
    [UpdatedDate] DATETIME      NOT NULL,
    [UpdatedBy]   BIGINT        NOT NULL,
    [SystemID]    VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_Roles_Updated] ON [dbo].[Roles]
FOR UPDATE AS 

INSERT INTO JO_Roles( 
RoleID	,
RoleName	,
Description	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
Action,ActionDate
)

SELECT  
RoleID	,
RoleName	,
Description	,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
'U',GETUTCDATE() FROM deleted