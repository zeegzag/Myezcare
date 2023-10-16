CREATE TABLE [dbo].[LatestERAs] (
    [LatestEraID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [CheckNumber]       NVARCHAR (100) NULL,
    [CheckType]         NVARCHAR (100) NULL,
    [ClaimProviderName] NVARCHAR (200) NULL,
    [DownTime]          NVARCHAR (50)  NULL,
    [EraID]             NVARCHAR (100) NULL,
    [PaidAmount]        DECIMAL (18)   NULL,
    [PaidDate]          NVARCHAR (10)  NULL,
    [PayerName]         NVARCHAR (100) NULL,
    [PayerID]           NVARCHAR (50)  NULL,
    [ProviderName]      NVARCHAR (150) NULL,
    [ProviderNPI]       NVARCHAR (100) NULL,
    [ProviderTaxID]     NVARCHAR (100) NULL,
    [RecievedTime]      DATETIME       NULL,
    [Source]            NVARCHAR (50)  NULL,
    [IsDeleted]         BIT            NOT NULL,
    [CreatedDate]       DATETIME       NOT NULL,
    [CreatedBy]         BIGINT         NULL,
    [UpdatedDate]       DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [SystemID]          VARCHAR (100)  NULL,
    ValidationMessage NVARCHAR(MAX)   
    CONSTRAINT [PK_LatestERAs] PRIMARY KEY CLUSTERED ([LatestEraID] ASC)
);

