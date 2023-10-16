GO

CREATE TABLE [dbo].[ReferralMedication](
	[ReferralMedicationID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralID] [bigint] NOT NULL,
	[MedicationId] [bigint] NOT NULL,
	[PhysicianID] [bigint] NOT NULL,
	[Dose] [nvarchar](100) NOT NULL,
	[Unit] [int] NOT NULL,
	[Frequency] [nvarchar](100) NOT NULL,
	[Route] [nvarchar](100) NOT NULL,
	[Quantity] [nvarchar](100) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[SystemID] [varchar](100) NOT NULL,
	[HealthDiagnostics] [nvarchar](1000) NULL,
	[PatientInstructions] [nvarchar](1000) NULL,
	[PharmacistInstructions] [nvarchar](1000) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReferralMedication]  WITH CHECK ADD  CONSTRAINT [FK_ReferralMedication_Medication] FOREIGN KEY([MedicationId])
REFERENCES [dbo].[Medication] ([MedicationId])
GO

ALTER TABLE [dbo].[ReferralMedication] CHECK CONSTRAINT [FK_ReferralMedication_Medication]
GO

ALTER TABLE [dbo].[ReferralMedication]  WITH CHECK ADD  CONSTRAINT [FK_ReferralMedication_Physicians] FOREIGN KEY([PhysicianID])
REFERENCES [dbo].[Physicians] ([PhysicianID])
GO

ALTER TABLE [dbo].[ReferralMedication] CHECK CONSTRAINT [FK_ReferralMedication_Physicians]
GO

ALTER TABLE [dbo].[ReferralMedication]  WITH CHECK ADD  CONSTRAINT [FK_ReferralMedication_ReferralMedication] FOREIGN KEY([ReferralID])
REFERENCES [dbo].[Referrals] ([ReferralID])
GO

ALTER TABLE [dbo].[ReferralMedication] CHECK CONSTRAINT [FK_ReferralMedication_ReferralMedication]
GO


