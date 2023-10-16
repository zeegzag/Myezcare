CREATE TABLE [dbo].[ReferralHistory] (
    [ReferralHistoryID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]        BIGINT        NOT NULL,
    [ReferralDate]      DATE          NULL,
    [ReferralSourceID]  INT           NULL,
    [ClosureDate]       DATE          NULL,
    [ClosureReason]     VARCHAR (500) NULL,
    [CreatedDate]       DATETIME      NOT NULL,
    [CreatedBy]         BIGINT        NOT NULL,
    [UpdatedDate]       DATETIME      NOT NULL,
    [UpdatedBy]         BIGINT        NOT NULL,
    [SystemID]          VARCHAR (100) NOT NULL,
    [IsDeleted]         BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReferralHistory] PRIMARY KEY CLUSTERED ([ReferralHistoryID] ASC),
    CONSTRAINT [FK_ReferralHistory_ReferralID] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

