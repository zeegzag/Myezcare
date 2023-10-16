CREATE TABLE [dbo].[OnboardingWizardLog] (
    [OnboardingWizardLogID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [OrganizationID]        BIGINT        NOT NULL,
    [Menu]                  NVARCHAR (50) NOT NULL,
    [IsCompleted]           BIT           NOT NULL,
    [IsDeleted]             BIT           CONSTRAINT [DF_OnboardingWizardLog_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedBy]             BIGINT        NULL,
    [CreatedDate]           DATETIME      CONSTRAINT [DF_OnboardingWizardLog_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedDate]           DATETIME      CONSTRAINT [DF_OnboardingWizardLog_UpdatedDate] DEFAULT (getdate()) NULL,
    [UpdatedBy]             BIGINT        NULL,
    [SystemID]              VARCHAR (100) NULL,
    CONSTRAINT [PK_OnboardingWizardLog] PRIMARY KEY CLUSTERED ([OnboardingWizardLogID] ASC)
);

