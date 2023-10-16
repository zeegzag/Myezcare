CREATE TABLE [dbo].[ReferralPreferences] (
    [ReferralPreferenceID] BIGINT IDENTITY (1, 1) NOT NULL,
    [ReferralID]           BIGINT NOT NULL,
    [PreferenceID]         BIGINT NOT NULL,
    CONSTRAINT [PK_ReferralPreferences] PRIMARY KEY CLUSTERED ([ReferralPreferenceID] ASC),
    CONSTRAINT [FK_ReferralPreferences_Preferences] FOREIGN KEY ([PreferenceID]) REFERENCES [dbo].[Preferences] ([PreferenceID]),
    CONSTRAINT [FK_ReferralPreferences_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

