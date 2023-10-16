CREATE TABLE [dbo].[MasterVisitReasons] (
    [MasterVisitReasonID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Type]                VARCHAR (12)   NULL,
    [Code]                NVARCHAR (MAX) NULL,
    [Description]         NVARCHAR (MAX) NULL,
    [CompanyName]         NVARCHAR (MAX) NULL
);

