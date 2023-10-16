CREATE TABLE [dbo].[EbriggsFormMppings] (
    [EbriggsFormMppingID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [EBriggsFormID]         NVARCHAR (MAX) NULL,
    [OriginalEBFormID]      NVARCHAR (MAX) NULL,
    [FormId]                NVARCHAR (MAX) NULL,
    [ReferralID]            BIGINT         NULL,
    [EmployeeID]            BIGINT         NULL,
    [CreatedDate]           DATETIME       NULL,
    [CreatedBy]             BIGINT         NULL,
    [UpdatedDate]           DATETIME       NULL,
    [UpdatedBy]             BIGINT         NULL,
    [SystemID]              NVARCHAR (100) NULL,
    [IsDeleted]             BIT            CONSTRAINT [DF_EbriggsFormMppings_IsDeleted] DEFAULT ((0)) NOT NULL,
    [SubSectionID]          BIGINT         NULL,
    [HTMLFormContent]       NVARCHAR (MAX) NULL,
    [TaskFormMappingID]     BIGINT         NULL,
    [EmployeeVisitNoteID]   BIGINT         NULL,
    [ReferralTaskMappingID] BIGINT         NULL,
    [FormName]              NVARCHAR (500) NULL,
    CONSTRAINT [PK_EbriggsFormMppings] PRIMARY KEY CLUSTERED ([EbriggsFormMppingID] ASC)
);

