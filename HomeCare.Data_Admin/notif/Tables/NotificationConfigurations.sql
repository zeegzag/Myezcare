CREATE TABLE [notif].[NotificationConfigurations] (
    [NotificationConfigurationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [ConfigurationName]           NVARCHAR (100) NOT NULL,
    [Description]                 NVARCHAR (500) NULL,
    [NotificationEventID]         BIGINT         NOT NULL,
    [EmailTemplateID]             BIGINT         NOT NULL,
    [SMSTemplateID]               BIGINT         NOT NULL,
    [CreatedDate]                 DATETIME       NOT NULL,
    [CreatedBy]                   BIGINT         NOT NULL,
    [UpdatedDate]                 DATETIME       NOT NULL,
    [UpdatedBy]                   BIGINT         NOT NULL,
    [SystemID]                    VARCHAR (100)  NOT NULL,
    [IsDeleted]                   BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NotificationConfigurations] PRIMARY KEY CLUSTERED ([NotificationConfigurationID] ASC),
    CONSTRAINT [FK_NotificationConfigurations_EmailTemplates_EmailTemplateID] FOREIGN KEY ([EmailTemplateID]) REFERENCES [dbo].[EmailTemplates] ([EmailTemplateID]),
    CONSTRAINT [FK_NotificationConfigurations_EmailTemplates_SMSTemplateID] FOREIGN KEY ([SMSTemplateID]) REFERENCES [dbo].[EmailTemplates] ([EmailTemplateID]),
    CONSTRAINT [FK_NotificationConfigurations_NotificationEvents_NotificationEventID] FOREIGN KEY ([NotificationEventID]) REFERENCES [notif].[NotificationEvents] ([NotificationEventID])
);

