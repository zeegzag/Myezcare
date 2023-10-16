CREATE TABLE [dbo].[ReferralOnHoldDetails] (
    [ReferralOnHoldDetailID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReferralID]             BIGINT         NOT NULL,
    [StartDate]              DATE           NULL,
    [EndDate]                DATE           NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF_ReferralOnHoldDetails_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]            DATETIME       NULL,
    [CreatedBy]              BIGINT         NULL,
    [UpdatedDate]            DATETIME       NULL,
    [UpdatedBy]              BIGINT         NULL,
    [SystemID]               VARCHAR (100)  NULL,
    [PatientOnHoldReason]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ReferralOnHoldDetails] PRIMARY KEY CLUSTERED ([ReferralOnHoldDetailID] ASC),
    CONSTRAINT [FK_ReferralOnHoldDetails_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

