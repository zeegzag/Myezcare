CREATE TABLE [dbo].[CommonNotes] (
    [CommonNoteID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmployeeID]   BIGINT         NULL,
    [ReferralID]   BIGINT         NULL,
    [Note]         NVARCHAR (MAX) NULL,
    [IsDeleted]    BIT            NOT NULL,
    [CreatedBy]    BIGINT         NOT NULL,
    [CreatedDate]  DATETIME       NOT NULL,
    [UpdatedDate]  DATETIME       NOT NULL,
    [UpdatedBy]    BIGINT         NOT NULL,
    [RoleID]       NVARCHAR (50)  NULL,
    [EmployeesID]  NVARCHAR (50)  NULL,
    [CategoryID]   BIGINT         DEFAULT ((0)) NULL,
    [isPrivate]    BIT            DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CommonNotes] PRIMARY KEY CLUSTERED ([CommonNoteID] ASC),
    CONSTRAINT [FK_CommonNote_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_CommonNote_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);









