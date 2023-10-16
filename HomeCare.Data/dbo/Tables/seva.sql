CREATE TABLE [dbo].[seva] (
    [VisitTaskType]          NVARCHAR (50) NULL,
    [VisitTaskDetail]        NVARCHAR (50) NULL,
    [IsDeleted]              NVARCHAR (50) NULL,
    [CreatedDate]            DATETIME2 (7) NULL,
    [CreatedBy]              INT           NULL,
    [UpdatedDate]            DATETIME2 (7) NULL,
    [UpdatedBy]              INT           NULL,
    [SystemID]               NVARCHAR (50) NULL,
    [ServiceCodeID]          NVARCHAR (50) NULL,
    [IsRequired]             NVARCHAR (50) NULL,
    [MinimumTimeRequired]    NVARCHAR (50) NULL,
    [IsDefault]              NVARCHAR (50) NULL,
    [VisitTaskCategoryID]    INT           NULL,
    [VisitTaskSubCategoryID] NVARCHAR (50) NULL,
    [SendAlert]              NVARCHAR (50) NULL,
    [VisitType]              INT           NULL,
    [CareType]               INT           NULL,
    [Frequency]              INT           NULL,
    [TaskCode]               NVARCHAR (50) NULL
);

