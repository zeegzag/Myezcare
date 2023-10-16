CREATE TABLE [dbo].[ReferralSources] (
    [ReferralSourceID]   INT          NOT NULL,
    [ReferralSourceName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ReferralSources] PRIMARY KEY CLUSTERED ([ReferralSourceID] ASC)
);

