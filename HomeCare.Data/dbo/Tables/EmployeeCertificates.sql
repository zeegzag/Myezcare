CREATE TABLE [dbo].[EmployeeCertificates] (
    [CertificateID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [CertificatePath]      VARCHAR (200)  NULL,
    [Name]                 VARCHAR (64)   NULL,
    [StartDate]            DATETIME       NULL,
    [EndDate]              DATETIME       NULL,
    [EmployeeID]           BIGINT         NULL,
    [CreatedBy]            BIGINT         NULL,
    [CreatedOn]            DATETIME       CONSTRAINT [DF__EmployeeC__Creat__0ADE5854] DEFAULT (getdate()) NULL,
    [UpdatedBy]            BIGINT         NULL,
    [UpdatedOn]            DATETIME       NULL,
    [IsDeleted]            BIT            CONSTRAINT [DF__EmployeeC__IsDel__0BD27C8D] DEFAULT ((0)) NULL,
    [CertificateAuthority] NVARCHAR (MAX) NULL,
    [CertificateName]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__Employee__BBF8A7E15FEE7D0D] PRIMARY KEY CLUSTERED ([CertificateID] ASC)
);

