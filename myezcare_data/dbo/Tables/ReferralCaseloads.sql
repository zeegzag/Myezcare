CREATE TABLE [dbo].[ReferralCaseloads] (
    [ReferralCaseloadID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]         BIGINT        NOT NULL,
    [EmployeeID]         BIGINT        NOT NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_ReferralCaseloads_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CaseLoadType]       VARCHAR (100) NOT NULL,
    [StartDate]          DATETIME      NOT NULL,
    [EndDate]            DATETIME      NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME      NOT NULL,
    [UpdatedDate]        DATETIME      NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [SystemID]           VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ReferralCaseloads] PRIMARY KEY CLUSTERED ([ReferralCaseloadID] ASC),
    CONSTRAINT [FK_ReferralCaseloads_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_ReferralCaseloads_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

