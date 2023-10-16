CREATE TABLE [dbo].[EmailTemplateTokens] (
    [TokenID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Module]  NVARCHAR (128) NULL,
    [Tokens]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__EmailTem__658FEE8A17274112] PRIMARY KEY CLUSTERED ([TokenID] ASC)
);

