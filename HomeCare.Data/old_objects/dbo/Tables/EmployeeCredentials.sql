CREATE TABLE [dbo].[EmployeeCredentials] (
    [CredentialID]   VARCHAR (50) NOT NULL,
    [CredentialName] VARCHAR (50) NULL,
    CONSTRAINT [PK_EmployeeCredentials] PRIMARY KEY CLUSTERED ([CredentialID] ASC)
);

