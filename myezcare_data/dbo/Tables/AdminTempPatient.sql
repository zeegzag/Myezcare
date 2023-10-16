CREATE TABLE [dbo].[AdminTempPatient] (
    [AdminTempPatientID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [PatientID]          VARCHAR (100)  NULL,
    [FirstName]          NVARCHAR (100) NULL,
    [LastName]           NVARCHAR (100) NULL,
    [Dob]                DATETIME       NULL,
    [Gender]             VARCHAR (100)  NULL,
    [AccountNumber]      NVARCHAR (20)  NULL,
    [LanguagePreference] NVARCHAR (100) NULL,
    [ErrorMessage]       NVARCHAR (MAX) NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME       NOT NULL
);

