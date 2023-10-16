CREATE TABLE [dbo].[TempPatients] (
    [First Name]       NVARCHAR (255) NULL,
    [Middle Name]      NVARCHAR (255) NULL,
    [Last Name]        NVARCHAR (255) NULL,
    [DOB]              DATETIME       NULL,
    [Gender]           NVARCHAR (255) NULL,
    [Language]         NVARCHAR (255) NULL,
    [Account #]        FLOAT (53)     NULL,
    [Medicaid #]       FLOAT (53)     NULL,
    [Status]           NVARCHAR (255) NULL,
    [Username]         NVARCHAR (255) NULL,
    [Preferences]      NVARCHAR (255) NULL,
    [Skills]           NVARCHAR (255) NULL,
    [Email]            NVARCHAR (255) NULL,
    [Phone]            NVARCHAR (255) NULL,
    [Address 1]        NVARCHAR (255) NULL,
    [Address 2]        NVARCHAR (255) NULL,
    [City]             NVARCHAR (255) NULL,
    [State]            NVARCHAR (255) NULL,
    [Zip Code]         FLOAT (53)     NULL,
    [Lang# Preference] NVARCHAR (255) NULL,
    [TempPatientID]    BIGINT         IDENTITY (1, 1) NOT NULL
);

