CREATE TABLE [dbo].[ReportMaster] (
    [ReportID]          INT            IDENTITY (1, 1) NOT NULL,
    [ReportName]        NVARCHAR (MAX) NULL,
    [SqlString]         NVARCHAR (MAX) NULL,
    [ReportDescription] NVARCHAR (MAX) NULL,
    [DataSet]           VARCHAR (MAX)  NULL,
    [RDL_FileName]      VARCHAR (MAX)  NULL,
    [IsDeleted]         BIT            NULL,
    [IsActive]          BIT            NULL,
    [Category]          VARCHAR (255)  NULL,
    [IsDisplay]         BIT            NULL,
    CONSTRAINT [PK_ReportMaster] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);



