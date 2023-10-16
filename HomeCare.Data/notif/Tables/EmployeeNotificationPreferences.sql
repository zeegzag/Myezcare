CREATE TABLE [notif].[EmployeeNotificationPreferences] (
    [EmployeeNotificationPreferenceID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmployeeID]                       BIGINT        NOT NULL,
    [SendEmail]                        BIT           DEFAULT ((1)) NOT NULL,
    [SendSMS]                          BIT           DEFAULT ((1)) NOT NULL,
    [WebNotification]                  BIT           DEFAULT ((1)) NOT NULL,
    [MobileAppNotification]            BIT           DEFAULT ((1)) NOT NULL,
    [CreatedDate]                      DATETIME      NOT NULL,
    [CreatedBy]                        BIGINT        NOT NULL,
    [UpdatedDate]                      DATETIME      NOT NULL,
    [UpdatedBy]                        BIGINT        NOT NULL,
    [SystemID]                         VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EmployeeNotificationPreferences] PRIMARY KEY CLUSTERED ([EmployeeNotificationPreferenceID] ASC),
    CONSTRAINT [FK_EmployeeNotificationPreferences_Employees_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

