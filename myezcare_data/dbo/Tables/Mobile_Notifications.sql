CREATE TABLE [dbo].[Mobile_Notifications] (
    [NotificationId]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (1000) NOT NULL,
    [FileName]         NVARCHAR (500)  NULL,
    [NotificationType] INT             NOT NULL,
    [InProgress]       INT             NOT NULL,
    [CreatedDate]      DATETIME        NOT NULL,
    [CreatedBy]        BIGINT          NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([NotificationId] ASC)
);

