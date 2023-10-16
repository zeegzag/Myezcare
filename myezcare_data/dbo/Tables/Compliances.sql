CREATE TABLE [dbo].[Compliances] (
    [ComplianceID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserType]          INT            NULL,
    [DocumentationType] INT            NULL,
    [DocumentName]      NVARCHAR (MAX) NULL,
    [IsTimeBased]       BIT            NULL,
    [IsDeleted]         BIT            NULL,
    [CreatedBy]         BIGINT         NULL,
    [CreatedDate]       DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [UpdatedDate]       DATETIME       NULL,
    [SystemID]          NVARCHAR (MAX) NULL,
    [EBFormID]          NVARCHAR (MAX) NULL,
    [ParentID]          BIGINT         NULL,
    [Type]              NVARCHAR (50)  NULL,
    [Value]             NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Compliances] PRIMARY KEY CLUSTERED ([ComplianceID] ASC)
);

