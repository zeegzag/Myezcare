CREATE TABLE [dbo].[SignatureLogs] (
    [SignatureLogID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [NoteID]              BIGINT        NOT NULL,
    [Signature]           VARCHAR (MAX) NULL,
    [EmployeeSignatureID] BIGINT        CONSTRAINT [DF_SignatureLogs_EmployeeSignatureID] DEFAULT ((0)) NOT NULL,
    [SignatureBy]         BIGINT        NULL,
    [Name]                VARCHAR (100) NULL,
    [Date]                DATETIME      NOT NULL,
    [MacAddress]          VARCHAR (50)  NULL,
    [SystemID]            VARCHAR (50)  NULL,
    [IsActive]            BIT           CONSTRAINT [DF_SignatureLogs_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SignatureLogs] PRIMARY KEY CLUSTERED ([SignatureLogID] ASC),
    CONSTRAINT [fk_Signatue_Created] FOREIGN KEY ([SignatureBy]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [fk_Signatue_note] FOREIGN KEY ([NoteID]) REFERENCES [dbo].[Notes] ([NoteID])
);

