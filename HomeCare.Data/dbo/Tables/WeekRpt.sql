CREATE TABLE [dbo].[WeekRpt] (
    [VisitTaskID]         BIGINT          NULL,
    [VisitTaskType]       NVARCHAR (1000) NULL,
    [CareType]            BIGINT          NULL,
    [VisitTaskCategoryID] BIGINT          NULL,
    [VisitTaskDetail]     NVARCHAR (100)  NULL,
    [sortorder]           INT             NULL,
    [isVisible]           BIT             NULL,
    [isDeleted]           BIT             NULL
);

