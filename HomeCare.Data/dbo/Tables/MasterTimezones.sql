CREATE TABLE [dbo].[MasterTimezones] (
    [MasterTimezoneID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Code]                 NVARCHAR (MAX) NULL,
    [CompanyName]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_MasterTimezones] PRIMARY KEY CLUSTERED ([MasterTimezoneID] ASC)
);

