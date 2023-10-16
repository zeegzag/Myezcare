CREATE TABLE [dbo].[tmpEBFormMarkets] (
    [EBFormMarketID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [EBFormID]       NVARCHAR (MAX) NOT NULL,
    [EBMarketID]     NVARCHAR (MAX) NOT NULL
);

