CREATE TABLE [dbo].[PatientHealthPlan] (
    [BenificiaryId]         INT            IDENTITY (1, 1) NOT NULL,
    [BenificiaryType]       NVARCHAR (MAX) NULL,
    [BenificiaryTypeNumber] NVARCHAR (MAX) NULL,
    [CreatedDate]           DATETIME       NOT NULL,
    [CreatedBy]             NVARCHAR (MAX) NOT NULL,
    [UpdateDate]            DATETIME       NOT NULL,
    [UpdateBy]              BIGINT         NOT NULL,
    [IsDeleted]             BIT            NOT NULL,
    [IsActive]              BIT            NULL,
    CONSTRAINT [PK_PatientHealthPlan] PRIMARY KEY CLUSTERED ([BenificiaryId] ASC)
);

