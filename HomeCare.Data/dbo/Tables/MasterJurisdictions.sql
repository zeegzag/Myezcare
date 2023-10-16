CREATE TABLE [dbo].[MasterJurisdictions] (
    [MasterJurisdictionID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Code]                 NVARCHAR (MAX) NULL,
    [Description]          NVARCHAR (MAX) NULL,
    [PayerCode]            NVARCHAR (MAX) NULL,
    [CompanyName]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_MasterJurisdictions] PRIMARY KEY CLUSTERED ([MasterJurisdictionID] ASC)
);

