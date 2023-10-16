CREATE TABLE [dbo].[EmployeeSignatures] (
    [EmployeeSignatureID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [SignaturePath]       VARCHAR (200) NULL,
    [EmployeeID]          BIGINT        NOT NULL,
    CONSTRAINT [PK_EmployeeSignatures] PRIMARY KEY CLUSTERED ([EmployeeSignatureID] ASC),
    CONSTRAINT [FK__EmployeeS__Emplo__318258D2] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
);

