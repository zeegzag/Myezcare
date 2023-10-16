CREATE TABLE [dbo].[AdminTempPatientContact] (
    [AdminTempPatientContactID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [PatientID]                 VARCHAR (100)  NULL,
    [ContactType]               NVARCHAR (100) NULL,
    [FirstName]                 NVARCHAR (100) NULL,
    [LastName]                  NVARCHAR (100) NULL,
    [Email]                     VARCHAR (256)  NULL,
    [Phone]                     VARCHAR (30)   NULL,
    [Address]                   NVARCHAR (MAX) NULL,
    [City]                      NVARCHAR (500) NULL,
    [State]                     NVARCHAR (500) NULL,
    [ZipCode]                   NVARCHAR (500) NULL,
    [LanguagePreference]        NVARCHAR (100) NULL,
    [IsEmergencyContact]        VARCHAR (10)   NULL,
    [ErrorMessage]              NVARCHAR (MAX) NULL,
    [CreatedBy]                 BIGINT         NOT NULL,
    [CreatedDate]               DATETIME       NOT NULL
);

