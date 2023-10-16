CREATE TABLE [dbo].[OnboardingWizardLog](
	[OnboardingWizardLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [bigint] NOT NULL,
	[Menu] [nvarchar](50) NOT NULL,
	[IsCompleted] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[SystemID] [varchar](100) NULL,
 CONSTRAINT [PK_OnboardingWizardLog] PRIMARY KEY CLUSTERED 
(
	[OnboardingWizardLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OnboardingWizardLog] ADD  CONSTRAINT [DF_OnboardingWizardLog_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[OnboardingWizardLog] ADD  CONSTRAINT [DF_OnboardingWizardLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[OnboardingWizardLog] ADD  CONSTRAINT [DF_OnboardingWizardLog_UpdatedDate]  DEFAULT (getdate()) FOR [UpdatedDate]
GO