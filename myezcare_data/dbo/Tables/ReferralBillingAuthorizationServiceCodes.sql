CREATE TABLE [dbo].[ReferralBillingAuthorizationServiceCodes] (
    [ReferralBillingAuthorizationServiceCodeID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralBillingAuthorizationID]            BIGINT        NOT NULL,
    [ServiceCodeID]                             BIGINT        NOT NULL,
    [CreatedDate]                               DATETIME      NULL,
    [CreatedBy]                                 BIGINT        NULL,
    [UpdatedDate]                               DATETIME      NULL,
    [UpdatedBy]                                 BIGINT        NULL,
    [SystemID]                                  VARCHAR (100) NULL,
    [IsDeleted]                                 BIT           CONSTRAINT [DF_ReferralBillingAuthorizationServiceCodes_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DailyUnitLimit]                            INT           CONSTRAINT [DF__ReferralB__Daily__4F089A18] DEFAULT ((0)) NOT NULL,
    [MaxUnitLimit]                              INT           CONSTRAINT [DF__ReferralB__MaxUn__4FFCBE51] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReferralBillingAuthorizationServiceCodes] PRIMARY KEY CLUSTERED ([ReferralBillingAuthorizationServiceCodeID] ASC)
);

