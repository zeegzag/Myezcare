CREATE TABLE [dbo].[Languages] (
    [LanguageID] BIGINT       NOT NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [Value]      VARCHAR (50) NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED ([LanguageID] ASC)
);

