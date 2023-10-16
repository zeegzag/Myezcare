CREATE TABLE [dbo].[Currency] (
    [CurrencyID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [CurrencyName] VARCHAR (100) NOT NULL,
    [Country]      VARCHAR (100) NOT NULL,
    [Symbol]       VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([CurrencyID] ASC)
);

