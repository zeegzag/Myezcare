CREATE TABLE [dbo].[RolePermissionMapping] (
    [RolePermissionMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [RoleID]                  BIGINT        NOT NULL,
    [PermissionID]            BIGINT        NOT NULL,
    [CreatedDate]             DATETIME      CONSTRAINT [DF_RolePermissionMapping_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               BIGINT        NOT NULL,
    [UpdatedDate]             DATETIME      NOT NULL,
    [UpdatedBy]               BIGINT        NOT NULL,
    [SystemID]                VARCHAR (100) NOT NULL,
    [IsDeleted]               BIT           CONSTRAINT [DF_RolePermissionMapping_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RolePermissionMapping] PRIMARY KEY CLUSTERED ([RolePermissionMappingID] ASC),
    CONSTRAINT [FK_RolePermissionMapping_Permissions] FOREIGN KEY ([PermissionID]) REFERENCES [dbo].[Permissions] ([PermissionID]),
    CONSTRAINT [FK_RolePermissionMapping_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID])
);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermissionMapping_PermissionID_532B7]
    ON [dbo].[RolePermissionMapping]([PermissionID] ASC)
    INCLUDE([RoleID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [IsDeleted]);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermissionMapping_RoleID_B573F]
    ON [dbo].[RolePermissionMapping]([RoleID] ASC)
    INCLUDE([PermissionID]);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermissionMapping_RoleID_IsDeleted_PermissionID_EF33A]
    ON [dbo].[RolePermissionMapping]([RoleID] ASC, [IsDeleted] ASC, [PermissionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermissionMapping_RoleID_PermissionID_FAFB8]
    ON [dbo].[RolePermissionMapping]([RoleID] ASC, [PermissionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermissionMapping_RoleID_PermissionID_IsDeleted_EC46A]
    ON [dbo].[RolePermissionMapping]([RoleID] ASC, [PermissionID] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RolePermissionMapping_RoleID_IsDeleted_DE14C]
    ON [dbo].[RolePermissionMapping]([RoleID] ASC, [IsDeleted] ASC)
    INCLUDE([PermissionID]);


GO
CREATE TRIGGER [dbo].[tr_RolePermissionMapping_Updated] ON [dbo].[RolePermissionMapping]
FOR UPDATE AS 

INSERT INTO JO_RolePermissionMapping( 
RolePermissionMappingID,
RoleID,
PermissionID,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
IsDeleted	,
Action,ActionDate
)

SELECT  
RolePermissionMappingID,
RoleID,
PermissionID,
CreatedDate	,
CreatedBy	,
UpdatedDate	,
UpdatedBy	,
SystemID	,
IsDeleted	,
'U',GETUTCDATE() FROM deleted