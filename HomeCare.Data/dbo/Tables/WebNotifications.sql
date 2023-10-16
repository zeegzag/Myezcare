CREATE TABLE [dbo].[WebNotifications] (
    [WebNotificationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Message]           NVARCHAR (MAX) NOT NULL,
    [CreatedDate]       DATETIME       NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    CONSTRAINT [PK_WebNotifications] PRIMARY KEY CLUSTERED ([WebNotificationID] ASC)
);

