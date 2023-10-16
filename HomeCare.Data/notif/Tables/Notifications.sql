CREATE TABLE [notif].[Notifications] (
    [NotificationID]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [NotificationConfigurationID] BIGINT         NOT NULL,
    [NotificationEventID]         BIGINT         NOT NULL,
    [RefID]                       BIGINT         NOT NULL,
    [RefTableName]                VARCHAR (200)  NOT NULL,
    [TemplateData]                NVARCHAR (MAX) NOT NULL,
    [CreatedDate]                 DATETIME       NOT NULL,
    [IsEmailSent]                 BIT            DEFAULT ((0)) NOT NULL,
    [IsSMSSent]                   BIT            DEFAULT ((0)) NOT NULL,
    [IsWebNotificationSent]       BIT            DEFAULT ((0)) NOT NULL,
    [IsMobileAppNotificationSent] BIT            DEFAULT ((0)) NOT NULL,
    [IsProcessed]                 BIT            DEFAULT ((0)) NOT NULL,
    [DefaultRecipients]           VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([NotificationID] ASC),
    CONSTRAINT [FK_Notifications_NotificationConfigurations_NotificationConfigurationID] FOREIGN KEY ([NotificationConfigurationID]) REFERENCES [notif].[NotificationConfigurations] ([NotificationConfigurationID])
);




GO
CREATE NONCLUSTERED INDEX [missing_index_209415_209414_Notifications]
    ON [notif].[Notifications]([NotificationConfigurationID] ASC, [NotificationEventID] ASC, [RefTableName] ASC)
    INCLUDE([RefID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_209411_209410_Notifications]
    ON [notif].[Notifications]([NotificationConfigurationID] ASC, [NotificationEventID] ASC, [RefID] ASC, [RefTableName] ASC);

