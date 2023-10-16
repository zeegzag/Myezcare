CREATE TABLE [dbo].[ReferralRespiteUsageLimit] (
    [ReferralRespiteUsageLimitID] BIGINT     IDENTITY (1, 1) NOT NULL,
    [StartDate]                   DATE       NOT NULL,
    [EndDate]                     DATE       NOT NULL,
    [ReferralID]                  BIGINT     NOT NULL,
    [IsActive]                    BIT        CONSTRAINT [DF_ReferralRespiteUsageLimit_IsActive] DEFAULT ((0)) NOT NULL,
    [UsedRespiteHours]            FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ReferralRespiteUsageLimit] PRIMARY KEY CLUSTERED ([ReferralRespiteUsageLimitID] ASC),
    CONSTRAINT [FK_ReferralRespiteUsageLimit_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

