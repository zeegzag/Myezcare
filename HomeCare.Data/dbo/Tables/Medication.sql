CREATE TABLE [dbo].[Medication] (
    [MedicationId]   BIGINT         IDENTITY (2000, 1) NOT NULL,
    [MedicationName] NVARCHAR (500) NULL,
    [Generic_Name]   NVARCHAR (500) NULL,
    [Brand_Name]     NVARCHAR (500) NULL,
    [Product_Type]   NVARCHAR (500) NULL,
    [Route]          NVARCHAR (500) NULL,
    [AddedDate]      DATETIME       NULL,
    [IsActive]       BIT            NULL,
    [UpdatedDate]    DATETIME       NULL,
    [Dosage_Form]    NVARCHAR (100) NULL,
    CONSTRAINT [PK_Medication] PRIMARY KEY CLUSTERED ([MedicationId] ASC)
);

