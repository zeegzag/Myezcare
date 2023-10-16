CREATE TABLE [dbo].[ReferralBlockedEmployees] (
    [ReferralBlockedEmployeeID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [EmployeeID]                BIGINT          NOT NULL,
    [BlockingReason]            NVARCHAR (1000) NULL,
    [BlockingRequestedBy]       NVARCHAR (100)  NULL,
    [ReferralID]                BIGINT          NOT NULL,
    [IsDeleted]                 BIT             CONSTRAINT [DF_ReferralBlockedEmployees_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]               DATETIME        NULL,
    [CreatedBy]                 BIGINT          NULL,
    [UpdatedDate]               DATETIME        NULL,
    [UpdatedBy]                 BIGINT          NULL,
    [SystemID]                  VARCHAR (100)   NULL,
    CONSTRAINT [PK_ReferralBlockedEmployees] PRIMARY KEY CLUSTERED ([ReferralBlockedEmployeeID] ASC),
    CONSTRAINT [FK_ReferralBlockedEmployees_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_ReferralBlockedEmployees_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

