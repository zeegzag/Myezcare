CREATE SCHEMA [notif]
GO

CREATE TABLE [notif].[NotificationEvents] (
    [NotificationEventID] BIGINT NOT NULL,
    [EventName] NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (500) NULL,
	[EventDefinitionSP] NVARCHAR (200) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
    [CreatedBy] BIGINT NOT NULL,
    [UpdatedDate] DATETIME NOT NULL,
    [UpdatedBy] BIGINT NOT NULL,
    [SystemID] VARCHAR (100) NOT NULL,
    [IsDeleted] BIT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NotificationEvents] PRIMARY KEY CLUSTERED ([NotificationEventID] ASC)
);
GO

CREATE TABLE [notif].[NotificationConfigurations] (
    [NotificationConfigurationID] BIGINT NOT NULL,
    [ConfigurationName] NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (500) NULL,
	[NotificationEventID] BIGINT NOT NULL,
	[EmailTemplateID] BIGINT NULL,
	[SMSTemplateID] BIGINT NULL,
	[CreatedDate] DATETIME NOT NULL,
    [CreatedBy] BIGINT NOT NULL,
    [UpdatedDate] DATETIME NOT NULL,
    [UpdatedBy] BIGINT NOT NULL,
    [SystemID] VARCHAR (100) NOT NULL,
    [IsDeleted] BIT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NotificationConfigurations] PRIMARY KEY CLUSTERED ([NotificationConfigurationID] ASC),
    CONSTRAINT [FK_NotificationConfigurations_NotificationEvents_NotificationEventID] FOREIGN KEY ([NotificationEventID]) REFERENCES [notif].[NotificationEvents] ([NotificationEventID]),
	CONSTRAINT [FK_NotificationConfigurations_EmailTemplates_EmailTemplateID] FOREIGN KEY ([EmailTemplateID]) REFERENCES [dbo].[EmailTemplates] ([EmailTemplateID]),
	CONSTRAINT [FK_NotificationConfigurations_EmailTemplates_SMSTemplateID] FOREIGN KEY ([SMSTemplateID]) REFERENCES [dbo].[EmailTemplates] ([EmailTemplateID])
);
GO

CREATE TABLE [notif].[NotificationConfigurationRoleMapping] (
    [NotificationConfigurationRoleMappingID] BIGINT IDENTITY (1, 1) NOT NULL,
	[NotificationConfigurationID] BIGINT NOT NULL,
	[RoleID] BIGINT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
    [CreatedBy] BIGINT NOT NULL,
    [UpdatedDate] DATETIME NOT NULL,
    [UpdatedBy] BIGINT NOT NULL,
    [SystemID] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_NotificationConfigurationRoleMapping] PRIMARY KEY CLUSTERED ([NotificationConfigurationRoleMappingID] ASC),
    CONSTRAINT [FK_NotificationConfigurationRoleMapping_NotificationConfigurations_NotificationConfigurationID] FOREIGN KEY ([NotificationConfigurationID]) REFERENCES [notif].[NotificationConfigurations] ([NotificationConfigurationID]),
	CONSTRAINT [FK_NotificationConfigurationRoleMapping_Roles_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID])
);
GO

CREATE TABLE [notif].[Notifications] (
    [NotificationID] BIGINT IDENTITY (1, 1) NOT NULL,
	[NotificationConfigurationID] BIGINT NOT NULL,
	[NotificationEventID] BIGINT NOT NULL,
	[RefID] BIGINT NOT NULL,
	[RefTableName] VARCHAR (200) NOT NULL,
	[TemplateData] NVARCHAR (MAX) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[IsEmailSent] BIT DEFAULT ((0)) NOT NULL,
	[IsSMSSent] BIT DEFAULT ((0)) NOT NULL,
	[IsWebNotificationSent] BIT DEFAULT ((0)) NOT NULL,
	[IsMobileAppNotificationSent] BIT DEFAULT ((0)) NOT NULL,
	[IsProcessed] BIT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([NotificationID] ASC),
    CONSTRAINT [FK_Notifications_NotificationConfigurations_NotificationConfigurationID] FOREIGN KEY ([NotificationConfigurationID]) REFERENCES [notif].[NotificationConfigurations] ([NotificationConfigurationID])
);
GO

CREATE TABLE [notif].[EmployeeNotificationPreferences] (
    [EmployeeNotificationPreferenceID] BIGINT IDENTITY (1, 1) NOT NULL,
	[EmployeeID] BIGINT NOT NULL,
	[SendEmail] BIT DEFAULT ((1)) NOT NULL,
	[SendSMS] BIT DEFAULT ((1)) NOT NULL,
	[WebNotification] BIT DEFAULT ((1)) NOT NULL,
	[MobileAppNotification] BIT DEFAULT ((1)) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
    [CreatedBy] BIGINT NOT NULL,
    [UpdatedDate] DATETIME NOT NULL,
    [UpdatedBy] BIGINT NOT NULL,
    [SystemID] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EmployeeNotificationPreferences] PRIMARY KEY CLUSTERED ([EmployeeNotificationPreferenceID] ASC),
    CONSTRAINT [FK_EmployeeNotificationPreferences_Employees_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);
GO