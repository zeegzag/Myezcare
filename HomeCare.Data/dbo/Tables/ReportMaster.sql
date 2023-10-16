CREATE TABLE [dbo].[ReportMaster] (
    [ReportID]          INT            IDENTITY (1, 1) NOT NULL,
    [ReportName]        NVARCHAR (MAX) NULL,
    [SqlString]         NVARCHAR (MAX) NULL,
    [ReportDescription] NVARCHAR (MAX) NULL,
    [DataSet]           NVARCHAR (MAX) NULL,
    [RDL_FileName]      NVARCHAR (MAX) NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF__ReportMas__IsDel__4E498009] DEFAULT ((0)) NULL,
    [IsActive]          BIT            CONSTRAINT [DF__ReportMas__IsAct__4F3DA442] DEFAULT ((1)) NULL,
    [Category]          VARCHAR (255)  NULL,
    [IsDisplay]         BIT            NULL,
    CONSTRAINT [PK_ReportMaster] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);



