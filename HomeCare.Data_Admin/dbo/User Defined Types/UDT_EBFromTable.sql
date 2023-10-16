CREATE TYPE [dbo].[UDT_EBFromTable] AS TABLE (
    [FromUniqueID] NVARCHAR (MAX) NOT NULL,
    [Id]           NVARCHAR (MAX) NOT NULL,
    [FormId]       NVARCHAR (MAX) NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [NameForUrl]   NVARCHAR (MAX) NULL,
    [Version]      NVARCHAR (MAX) NULL,
    [FormLongName] NVARCHAR (MAX) NULL,
    [IsActive]     BIT            DEFAULT ((0)) NOT NULL,
    [HasHtml]      BIT            DEFAULT ((0)) NOT NULL,
    [NewHtmlURI]   NVARCHAR (MAX) NULL,
    [HasPDF]       BIT            DEFAULT ((0)) NOT NULL,
    [NewPdfURI]    NVARCHAR (MAX) NULL,
    [EbCategoryID] NVARCHAR (MAX) NULL,
    [EbMarketIDs]  NVARCHAR (MAX) NULL);

