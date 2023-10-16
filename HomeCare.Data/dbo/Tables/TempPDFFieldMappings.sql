CREATE TABLE [dbo].[TempPDFFieldMappings] (
    [PDFFieldMappingId] INT             IDENTITY (1, 1) NOT NULL,
    [PDFFieldName]      NVARCHAR (1000) NULL,
    [DBFieldName]       NVARCHAR (1000) NULL,
    [TableName]         NVARCHAR (1000) NULL,
    [CreatedBy]         BIGINT          NULL,
    [CreatedDate]       DATETIME        NULL,
    [UpdatedDate]       DATETIME        NULL,
    [UpdatedBy]         BIGINT          NULL,
    [SystemID]          VARCHAR (100)   NULL,
    [IsDeleted]         BIT             NULL,
    [FormId]            INT             NULL
);

