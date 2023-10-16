CREATE TABLE [dbo].[ReferralBehaviorContracts] (
    [ReferralBehaviorContractID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [WarningDate]                DATE          NOT NULL,
    [WarningReason]              VARCHAR (MAX) NOT NULL,
    [CaseManagerNotifyDate]      DATE          NULL,
    [ReferralID]                 BIGINT        NOT NULL,
    [IsActive]                   BIGINT        CONSTRAINT [DF_ReferralBehaviorContracts_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDeleted]                  BIGINT        CONSTRAINT [DF_ReferralBehaviorContracts_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]                DATETIME      NOT NULL,
    [CreatedBy]                  BIGINT        NOT NULL,
    [UpdatedDate]                DATETIME      NOT NULL,
    [UpdatedBy]                  BIGINT        NOT NULL,
    [SystemID]                   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ReferralBehaviorContracts] PRIMARY KEY CLUSTERED ([ReferralBehaviorContractID] ASC),
    CONSTRAINT [FK_ReferralBehaviorContracts_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

