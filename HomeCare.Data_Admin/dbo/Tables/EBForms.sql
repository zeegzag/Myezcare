﻿CREATE TABLE [dbo].[EBForms] (
    [EBFormID]         NVARCHAR (MAX)  NOT NULL,
    [FromUniqueID]     NVARCHAR (MAX)  NULL,
    [Id]               NVARCHAR (MAX)  NULL,
    [FormId]           NVARCHAR (MAX)  NULL,
    [Name]             NVARCHAR (MAX)  NULL,
    [FormLongName]     NVARCHAR (MAX)  NULL,
    [NameForUrl]       NVARCHAR (MAX)  NULL,
    [Version]          NVARCHAR (MAX)  NULL,
    [IsActive]         BIT             CONSTRAINT [DF_EBForms_IsActive] DEFAULT ((0)) NOT NULL,
    [HasHtml]          BIT             NOT NULL,
    [NewHtmlURI]       NVARCHAR (MAX)  NULL,
    [HasPDF]           BIT             CONSTRAINT [DF_EBForms_HasPDF] DEFAULT ((0)) NOT NULL,
    [NewPdfURI]        NVARCHAR (MAX)  NULL,
    [EBCategoryID]     NVARCHAR (MAX)  NULL,
    [EbMarketIDs]      NVARCHAR (MAX)  NULL,
    [FormPrice]        DECIMAL (10, 2) NULL,
    [CreatedDate]      DATETIME        NULL,
    [UpdatedDate]      DATETIME        NULL,
    [UpdatedBy]        BIGINT          NULL,
    [IsDeleted]        BIT             CONSTRAINT [DF_EBForms_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsInternalForm]   BIT             CONSTRAINT [DF__EBForms__IsInter__3FD07829] DEFAULT ((0)) NULL,
    [InternalFormPath] NVARCHAR (MAX)  NULL,
    [IsOrbeonForm]     BIT             NULL,
    [OrganizationIDs]  NVARCHAR (MAX)  NULL
);

