﻿CREATE TABLE [dbo].[VisitTasks] (
    [VisitTaskID]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [VisitTaskType]          NVARCHAR (1000) NOT NULL,
    [VisitTaskDetail]        NVARCHAR (100)  NOT NULL,
    [IsDeleted]              BIT             CONSTRAINT [DF_VisitQuestions_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]            DATETIME        NOT NULL,
    [CreatedBy]              BIGINT          NOT NULL,
    [UpdatedDate]            DATETIME        NOT NULL,
    [UpdatedBy]              BIGINT          NOT NULL,
    [SystemID]               VARCHAR (100)   NOT NULL,
    [ServiceCodeID]          BIGINT          NULL,
    [IsRequired]             BIT             DEFAULT ((0)) NOT NULL,
    [MinimumTimeRequired]    BIGINT          NOT NULL,
    [IsDefault]              BIT             CONSTRAINT [DF__VisitTask__IsDef__2803DB90] DEFAULT ((0)) NOT NULL,
    [VisitTaskCategoryID]    BIGINT          NULL,
    [VisitTaskSubCategoryID] BIGINT          NULL,
    [SendAlert]              BIT             CONSTRAINT [DF__VisitTask__SendA__094A4A46] DEFAULT ((0)) NOT NULL,
    [VisitType]              BIGINT          NULL,
    [CareType]               BIGINT          NULL,
    [Frequency]              NVARCHAR (100)  NULL,
    [TaskCode]               NVARCHAR (MAX)  NULL,
    [TaskOption] NVARCHAR(MAX) NULL, 
    [DefaultTaskOption] BIT NULL, 
    CONSTRAINT [PK_VisitQuestions] PRIMARY KEY CLUSTERED ([VisitTaskID] ASC),
    CONSTRAINT [FK_VisitTasks_VisitTaskCategories] FOREIGN KEY ([VisitTaskCategoryID]) REFERENCES [dbo].[VisitTaskCategories] ([VisitTaskCategoryID])
);

