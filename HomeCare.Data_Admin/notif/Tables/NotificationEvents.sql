CREATE TABLE [notif].[NotificationEvents] (
    [NotificationEventID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [EventName]           NVARCHAR (100) NOT NULL,
    [Description]         NVARCHAR (500) NULL,
    [EventDefinitionSP]   NVARCHAR (200) NOT NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [CreatedBy]           BIGINT         NOT NULL,
    [UpdatedDate]         DATETIME       NOT NULL,
    [UpdatedBy]           BIGINT         NOT NULL,
    [SystemID]            VARCHAR (100)  NOT NULL,
    [IsDeleted]           BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NotificationEvents] PRIMARY KEY CLUSTERED ([NotificationEventID] ASC)
);

