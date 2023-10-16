CREATE TABLE [dbo].[WebUserNotifications] (
    [WebUserNotificationID] BIGINT IDENTITY (1, 1) NOT NULL,
    [EmployeeID]			BIGINT NOT NULL,
    [WebNotificationID]     BIGINT NOT NULL,
	[IsRead]				BIT    DEFAULT ((0)) NOT NULL,
    [IsDeleted]				BIT    DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WebUserNotifications] PRIMARY KEY CLUSTERED ([WebUserNotificationID] ASC),
    CONSTRAINT [FK_WebUserNotifications_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_WebUserNotifications_WebNotifications] FOREIGN KEY ([WebNotificationID]) REFERENCES [dbo].[WebNotifications] ([WebNotificationID])
);

