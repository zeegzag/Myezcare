CREATE TABLE [notif].[NotificationConfigurationRoleMapping] (
    [NotificationConfigurationRoleMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [NotificationConfigurationID]            BIGINT        NOT NULL,
    [RoleID]                                 BIGINT        NOT NULL,
    [CreatedDate]                            DATETIME      NOT NULL,
    [CreatedBy]                              BIGINT        NOT NULL,
    [UpdatedDate]                            DATETIME      NOT NULL,
    [UpdatedBy]                              BIGINT        NOT NULL,
    [SystemID]                               VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_NotificationConfigurationRoleMapping] PRIMARY KEY CLUSTERED ([NotificationConfigurationRoleMappingID] ASC),
    CONSTRAINT [FK_NotificationConfigurationRoleMapping_NotificationConfigurations_NotificationConfigurationID] FOREIGN KEY ([NotificationConfigurationID]) REFERENCES [notif].[NotificationConfigurations] ([NotificationConfigurationID]),
    CONSTRAINT [FK_NotificationConfigurationRoleMapping_Roles_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID])
);

