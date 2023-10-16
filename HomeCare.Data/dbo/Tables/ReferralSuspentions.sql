CREATE TABLE [dbo].[ReferralSuspentions] (
    [ReferralSuspentionID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [SuspentionType]       VARCHAR (20)  NOT NULL,
    [SuspentionLength]     INT           NOT NULL,
    [ReturnEligibleDate]   DATETIME      NULL,
    [ReferralID]           BIGINT        NOT NULL,
    [IsDeleted]            BIT           CONSTRAINT [DF_ReferralSuspentions_IsActive] DEFAULT ((0)) NOT NULL,
    [CreatedDate]          DATETIME      NOT NULL,
    [CreatedBy]            BIGINT        NOT NULL,
    [UpdatedDate]          DATETIME      NOT NULL,
    [UpdatedBy]            BIGINT        NOT NULL,
    [SystemID]             VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ReferralSuspentions] PRIMARY KEY CLUSTERED ([ReferralSuspentionID] ASC),
    CONSTRAINT [FK_ReferralSuspentions_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

