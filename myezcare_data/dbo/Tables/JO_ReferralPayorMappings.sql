CREATE TABLE [dbo].[JO_ReferralPayorMappings] (
    [JO_ReferralPayorMappingID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralPayorMappingID]    BIGINT        NOT NULL,
    [ReferralID]                BIGINT        NOT NULL,
    [PayorID]                   BIGINT        NOT NULL,
    [PayorEffectiveDate]        DATE          NOT NULL,
    [PayorEffectiveEndDate]     DATE          NULL,
    [IsActive]                  BIT           NOT NULL,
    [IsDeleted]                 BIT           NOT NULL,
    [CreatedDate]               DATETIME      NOT NULL,
    [CreatedBy]                 BIGINT        NOT NULL,
    [UpdatedDate]               DATETIME      NOT NULL,
    [UpdatedBy]                 BIGINT        NOT NULL,
    [SystemID]                  VARCHAR (100) NOT NULL,
    [Action]                    CHAR (1)      NOT NULL,
    [ActionDate]                DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ReferralPayorMappings] PRIMARY KEY CLUSTERED ([JO_ReferralPayorMappingID] ASC)
);

