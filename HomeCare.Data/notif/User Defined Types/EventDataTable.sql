CREATE TYPE [notif].[EventDataTable] AS TABLE (
    [RefID]             BIGINT        NULL,
    [RefTableName]      VARCHAR (200) NULL,
    [TemplateData]      VARCHAR (MAX) NULL,
    [DefaultRecipients] VARCHAR (MAX) NULL);

