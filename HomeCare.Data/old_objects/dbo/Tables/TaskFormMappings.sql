CREATE TABLE [dbo].[TaskFormMappings] (
    [TaskFormMappingID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [VisitTaskID]       BIGINT         NOT NULL,
    [EBFormID]          NVARCHAR (MAX) NOT NULL,
    [IsRequired]        BIT            CONSTRAINT [DF_TaskFormMappings_IsRequired] DEFAULT ((0)) NOT NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_TaskFormMappings_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]       DATETIME       NULL,
    [CreatedBy]         BIGINT         NULL,
    [UpdatedDate]       DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [SystemID]          VARCHAR (100)  NULL,
    CONSTRAINT [PK_TaskFormMappings] PRIMARY KEY CLUSTERED ([TaskFormMappingID] ASC)
);

