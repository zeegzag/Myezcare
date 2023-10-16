CREATE TABLE [dbo].[ReferralDXCodeMappings] (
    [ReferralDXCodeMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]              BIGINT        NOT NULL,
    [DXCodeID]                BIGINT        NOT NULL,
    [Precedence]              INT           NULL,
    [StartDate]               DATE          NULL,
    [EndDate]                 DATE          NULL,
    [CreatedDate]             DATETIME      NULL,
    [CreatedBy]               BIGINT        NOT NULL,
    [UpdatedDate]             DATETIME      NULL,
    [UpdatedBy]               BIGINT        NOT NULL,
    [SystemID]                VARCHAR (100) NOT NULL,
    [IsDeleted]               BIT           CONSTRAINT [DF_ReferralDXCodeMappings_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReferralDXCodeMappings] PRIMARY KEY CLUSTERED ([ReferralDXCodeMappingID] ASC),
    CONSTRAINT [FK_ReferralDXCodeMappings_DXCodes] FOREIGN KEY ([DXCodeID]) REFERENCES [dbo].[DXCodes] ([DXCodeID]),
    CONSTRAINT [FK_ReferralDXCodeMappings_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);

