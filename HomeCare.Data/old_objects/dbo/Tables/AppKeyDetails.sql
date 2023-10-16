CREATE TABLE [dbo].[AppKeyDetails] (
    [KeyId]   INT          NOT NULL,
    [AppKey]  VARCHAR (50) NOT NULL,
    [AppName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AppKeyDetails] PRIMARY KEY CLUSTERED ([KeyId] ASC)
);

