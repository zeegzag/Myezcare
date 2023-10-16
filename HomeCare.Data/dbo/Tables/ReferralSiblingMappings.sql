CREATE TABLE [dbo].[ReferralSiblingMappings] (
    [ReferralSiblingMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID1]              BIGINT        NOT NULL,
    [ReferralID2]              BIGINT        NOT NULL,
    [CreatedBy]                BIGINT        NOT NULL,
    [CreatedDate]              DATETIME      NOT NULL,
    [UpdatedBy]                BIGINT        NOT NULL,
    [UpdatedDate]              DATETIME      NOT NULL,
    [SystemID]                 VARCHAR (100) NULL,
    CONSTRAINT [PK_ReferralSiblingMappings] PRIMARY KEY CLUSTERED ([ReferralSiblingMappingID] ASC),
    CONSTRAINT [FK_ReferralSiblingMappings_Referrals] FOREIGN KEY ([ReferralID2]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_ReferralSiblingMappings_ReferralSiblingMappings] FOREIGN KEY ([ReferralID1]) REFERENCES [dbo].[Referrals] ([ReferralID])
);


GO
CREATE NONCLUSTERED INDEX [ReferralSiblingMapping_Index]
    ON [dbo].[ReferralSiblingMappings]([ReferralID1] ASC, [ReferralID2] ASC);

