CREATE TABLE [dbo].[ReferralComplianceMappings] (
    [ReferralComplianceID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]           BIGINT        NOT NULL,
    [ComplianceID]         BIGINT        NOT NULL,
    [Value]                BIT           CONSTRAINT [DF_ReferralComplianceMappings_Value] DEFAULT ((0)) NOT NULL,
    [ExpirationDate]       DATE          NULL,
    [CreatedDate]          DATETIME      NULL,
    [CreatedBy]            BIGINT        NULL,
    [UpdatedDate]          DATETIME      NULL,
    [UpdatedBy]            BIGINT        NULL,
    [SystemID]             VARCHAR (100) NULL,
    CONSTRAINT [PK_ReferralComplianceMappings] PRIMARY KEY CLUSTERED ([ReferralComplianceID] ASC),
    CONSTRAINT [FK_ReferralComplianceMappings_Compliances] FOREIGN KEY ([ComplianceID]) REFERENCES [dbo].[Compliances] ([ComplianceID]),
    CONSTRAINT [FK_ReferralComplianceMappings_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralComplianceMappings_ReferralID_ComplianceID_0A93D]
    ON [dbo].[ReferralComplianceMappings]([ReferralID] ASC, [ComplianceID] ASC)
    INCLUDE([Value], [ExpirationDate]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralComplianceMappings_ReferralID_BCEEF]
    ON [dbo].[ReferralComplianceMappings]([ReferralID] ASC)
    INCLUDE([ComplianceID], [Value], [ExpirationDate]);

