CREATE TABLE [dbo].[ReferralMedication] (
    [ReferralMedicationID]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReferralID]             BIGINT          NOT NULL,
    [MedicationId]           BIGINT          NOT NULL,
    [PhysicianID]            BIGINT          NOT NULL,
    [Dose]                   NVARCHAR (100)  NOT NULL,
    [Unit]                   NVARCHAR (100)  NULL,
    [Frequency]              NVARCHAR (100)  NOT NULL,
    [Route]                  NVARCHAR (100)  NOT NULL,
    [Quantity]               NVARCHAR (100)  NOT NULL,
    [StartDate]              DATETIME        NOT NULL,
    [EndDate]                DATETIME        NOT NULL,
    [CreatedDate]            DATETIME        NOT NULL,
    [ModifiedDate]           DATETIME        NOT NULL,
    [IsActive]               BIT             NOT NULL,
    [SystemID]               VARCHAR (100)   NOT NULL,
    [HealthDiagnostics]      NVARCHAR (1000) NULL,
    [PatientInstructions]    NVARCHAR (1000) NULL,
    [PharmacistInstructions] NVARCHAR (1000) NULL,
    [IsDeleted]              BIT             NULL,
    [UpdatedBy]              BIT             NULL,
    [DosageTime]             NVARCHAR (2000) NULL,
    
    CONSTRAINT [FK_ReferralMedication_Medication] FOREIGN KEY ([MedicationId]) REFERENCES [dbo].[Medication] ([MedicationId]),
    CONSTRAINT [FK_ReferralMedication_Physicians] FOREIGN KEY ([PhysicianID]) REFERENCES [dbo].[Physicians] ([PhysicianID]),
    CONSTRAINT [FK_ReferralMedication_ReferralMedication] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

