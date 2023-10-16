﻿CREATE TABLE [dbo].[tmpebforms] (
    [EBFormID]         NVARCHAR (MAX)  NOT NULL,
    [FromUniqueID]     NVARCHAR (MAX)  NULL,
    [Id]               NVARCHAR (MAX)  NULL,
    [FormId]           NVARCHAR (MAX)  NULL,
    [Name]             NVARCHAR (MAX)  NULL,
    [FormLongName]     NVARCHAR (MAX)  NULL,
    [NameForUrl]       NVARCHAR (MAX)  NULL,
    [Version]          NVARCHAR (MAX)  NULL,
    [IsActive]         BIT             NOT NULL,
    [HasHtml]          BIT             NOT NULL,
    [NewHtmlURI]       NVARCHAR (MAX)  NULL,
    [HasPDF]           BIT             NOT NULL,
    [NewPdfURI]        NVARCHAR (MAX)  NULL,
    [EBCategoryID]     NVARCHAR (MAX)  NULL,
    [EbMarketIDs]      NVARCHAR (MAX)  NULL,
    [FormPrice]        DECIMAL (10, 2) NULL,
    [CreatedDate]      DATETIME        NULL,
    [UpdatedDate]      DATETIME        NULL,
    [UpdatedBy]        BIGINT          NULL,
    [IsDeleted]        BIT             NOT NULL,
    [IsInternalForm]   BIT             NULL,
    [InternalFormPath] NVARCHAR (MAX)  NULL
);
