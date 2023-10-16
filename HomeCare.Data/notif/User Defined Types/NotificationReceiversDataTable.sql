CREATE TYPE [notif].[NotificationReceiversDataTable] AS TABLE (
    [NotificationConfigurationID]      BIGINT        NULL,
    [NotificationID]                   BIGINT        NULL,
    [EmailRecipients]                  VARCHAR (MAX) NULL,
    [EmailSubject]                     VARCHAR (500) NULL,
    [EmailBody]                        VARCHAR (MAX) NULL,
    [WebNotificationEmployeeIds]       VARCHAR (MAX) NULL,
    [MobileAppNotificationEmployeeIds] VARCHAR (MAX) NULL,
    [SMSRecipients]                    VARCHAR (MAX) NULL,
    [SMSText]                          VARCHAR (MAX) NULL);

