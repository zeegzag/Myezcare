CREATE TABLE [dbo].[EBFormMarkets] (
    [EBFormMarketID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [EBFormID]       NVARCHAR (MAX) NOT NULL,
    [EBMarketID]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_EBFormMarkets] PRIMARY KEY CLUSTERED ([EBFormMarketID] ASC)
);

