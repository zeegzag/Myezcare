CREATE TABLE [dbo].[ChecklistItems] (
    [ChecklistItemID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [ChecklistItemTypeID]  INT            NOT NULL,
    [StepName]             NVARCHAR (100) NOT NULL,
    [StepDescription]      NVARCHAR (MAX) NOT NULL,
    [SortOrder]            INT            NOT NULL,
    [ChecklistTypeControl] VARCHAR (100)  NOT NULL,
    [IsDocumentRequired]   BIT            NOT NULL,
    [IsMandatory]          BIT            NOT NULL,
    [IsAutomatic]          BIT            NOT NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [UpdatedDate]          DATETIME       NOT NULL,
    [UpdatedBy]            BIGINT         NOT NULL,
    [SystemID]             VARCHAR (100)  NOT NULL,
    [IsDeleted]            BIT            NOT NULL,
    CONSTRAINT [PK_ChecklistItems] PRIMARY KEY CLUSTERED ([ChecklistItemID] ASC)
);

