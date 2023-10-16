CREATE TABLE [dbo].[tmpEBMarkets] (
    [EBMarketID]  NVARCHAR (MAX) NOT NULL,
    [Id]          NVARCHAR (MAX) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [CreatedDate] DATETIME       NULL,
    [UpdatedDate] DATETIME       NULL,
    [IsDeleted]   BIT            NOT NULL
);

