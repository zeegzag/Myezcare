CREATE TABLE [dbo].[EBMarkets] (
    [EBMarketID]  NVARCHAR (MAX) NOT NULL,
    [Id]          NVARCHAR (MAX) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [CreatedDate] DATETIME       NULL,
    [UpdatedDate] DATETIME       NULL,
    [IsDeleted]   BIT            CONSTRAINT [DF_EBMarkets_IsDeleted] DEFAULT ((0)) NOT NULL
);

