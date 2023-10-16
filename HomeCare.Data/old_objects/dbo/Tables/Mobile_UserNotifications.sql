CREATE TABLE [dbo].[Mobile_UserNotifications] (
    [UserNotificationId] BIGINT IDENTITY (1, 1) NOT NULL,
    [EmployeeID]         BIGINT NOT NULL,
    [NotificationId]     BIGINT NOT NULL,
    [NotificationStatus] INT    NOT NULL,
    [PrimaryId]          BIGINT NULL,
    [IsNotificationRead] BIT    CONSTRAINT [DF_Mobile_UserNotifications_IsNotificationRead] DEFAULT ((0)) NOT NULL,
    [IsAccepted]         BIT    CONSTRAINT [DF__Mobile_Us__IsAct__5986288B] DEFAULT ((0)) NOT NULL,
    [IsDeleted]          BIT    CONSTRAINT [DF__Mobile_Us__IsDel__13B2CA20] DEFAULT ((0)) NOT NULL,
    [IsArchieved]        BIT    CONSTRAINT [DF__Mobile_Us__IsArc__14A6EE59] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Mobile_UserNotifications] PRIMARY KEY CLUSTERED ([UserNotificationId] ASC),
    CONSTRAINT [FK_Mobile_UserNotifications_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_Mobile_UserNotifications_Notifications] FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Mobile_Notifications] ([NotificationId])
);

