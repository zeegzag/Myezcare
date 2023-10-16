CREATE TYPE [notif].[NotificationEventDataTable] AS TABLE (
    [NotificationEventDataID]     UNIQUEIDENTIFIER NULL,
    [NotificationConfigurationID] BIGINT           NULL,
    [NotificationEventID]         BIGINT           NULL,
    [EmailTemplateID]             BIGINT           NULL,
    [SMSTemplateID]               BIGINT           NULL,
    [RefID]                       BIGINT           NULL,
    [RefTableName]                VARCHAR (200)    NULL,
    [TemplateData]                VARCHAR (MAX)    NULL,
    [DefaultRecipients]           VARCHAR (MAX)    NULL);

