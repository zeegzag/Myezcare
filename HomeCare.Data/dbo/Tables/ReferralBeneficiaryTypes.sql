CREATE TABLE [dbo].[ReferralBeneficiaryTypes] (
    [ReferralBeneficiaryTypeID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralID]                BIGINT        NOT NULL,
    [BeneficiaryTypeID]         BIGINT        NOT NULL,
    [BeneficiaryNumber]         VARCHAR (100) NOT NULL,
    [CreatedDate]               DATETIME      NOT NULL,
    [CreatedBy]                 BIGINT        NOT NULL,
    [UpdatedDate]               DATETIME      NULL,
    [UpdatedBy]                 BIGINT        NULL,
    [SystemID]                  VARCHAR (100) NOT NULL,
    [IsDeleted]                 BIT           NOT NULL,
    CONSTRAINT [PK_ReferralBeneficiaryTypes] PRIMARY KEY CLUSTERED ([ReferralBeneficiaryTypeID] ASC)
);

