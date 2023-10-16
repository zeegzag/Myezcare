CREATE TABLE [dbo].[JO_ReferralRespiteUsageLimit] (
    [Jo_ReferralRespiteUsageLimitID] BIGINT     NOT NULL,
    [ReferralRespiteUsageLimitID]    BIGINT     NOT NULL,
    [StartDate]                      DATE       NOT NULL,
    [EndDate]                        DATE       NOT NULL,
    [ReferralID]                     BIGINT     NOT NULL,
    [IsActive]                       BIT        NOT NULL,
    [UsedRespiteHours]               FLOAT (53) NOT NULL,
    [Action]                         CHAR (1)   NULL,
    [ActionDate]                     DATETIME   NULL,
    CONSTRAINT [PK_JO_ReferralRespiteUsageLimit] PRIMARY KEY CLUSTERED ([Jo_ReferralRespiteUsageLimitID] ASC),
    CONSTRAINT [FK_JO_ReferralRespiteUsageLimit_JO_ReferralRespiteUsageLimit] FOREIGN KEY ([Jo_ReferralRespiteUsageLimitID]) REFERENCES [dbo].[JO_ReferralRespiteUsageLimit] ([Jo_ReferralRespiteUsageLimitID])
);

