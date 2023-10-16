CREATE TABLE [dbo].[ReferralPayorMappings] (
    [ReferralPayorMappingID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReferralID]                       BIGINT         NOT NULL,
    [PayorID]                          BIGINT         NOT NULL,
    [PayorEffectiveDate]               DATE           NOT NULL,
    [PayorEffectiveEndDate]            DATE           NULL,
    [IsActive]                         BIT            NOT NULL,
    [IsDeleted]                        BIT            NOT NULL,
    [CreatedDate]                      DATETIME       NOT NULL,
    [CreatedBy]                        BIGINT         NOT NULL,
    [UpdatedDate]                      DATETIME       NOT NULL,
    [UpdatedBy]                        BIGINT         NOT NULL,
    [SystemID]                         VARCHAR (100)  NOT NULL,
    [Precedence]                       INT            NULL,
    [IsPayorNotPrimaryInsured]         BIT            CONSTRAINT [DF_ReferralPayorMappings_IsPayorNotPrimaryInsured] DEFAULT ((0)) NOT NULL,
    [InsuredId]                        VARCHAR (1000) NULL,
    [InsuredFirstName]                 VARCHAR (50)   NULL,
    [InsuredMiddleName]                VARCHAR (50)   NULL,
    [InsuredLastName]                  VARCHAR (50)   NULL,
    [InsuredAddress]                   VARCHAR (1000) NULL,
    [InsuredCity]                      VARCHAR (100)  NULL,
    [InsuredState]                     VARCHAR (100)  NULL,
    [InsuredZipCode]                   VARCHAR (5)    NULL,
    [InsuredPhone]                     VARCHAR (15)   NULL,
    [InsuredPolicyGroupOrFecaNumber]   VARCHAR (100)  NULL,
    [InsuredDateOfBirth]               DATE           NULL,
    [InsuredGender]                    VARCHAR (1)    NULL,
    [InsuredEmployersNameOrSchoolName] VARCHAR (100)  NULL,
    [BeneficiaryTypeID]                BIGINT         NULL,
    [BeneficiaryNumber]                VARCHAR (100)  NULL,
    [MemberID]                         NVARCHAR (MAX) NULL,
    [MasterJurisdictionID]             BIGINT         NULL,
    [MasterTimezoneID]                 BIGINT         NULL,
    CONSTRAINT [PK_ReferralPayorMappings] PRIMARY KEY CLUSTERED ([ReferralPayorMappingID] ASC),
    CONSTRAINT [FK_ReferralPayorMappings_Payors] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payors] ([PayorID]),
    CONSTRAINT [FK_ReferralPayorMappings_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_ReferralPayorMappings_MasterJurisdictions] FOREIGN KEY ([MasterJurisdictionID]) REFERENCES [dbo].[MasterJurisdictions] ([MasterJurisdictionID]),
    CONSTRAINT [FK_ReferralPayorMappings_MasterTimezones] FOREIGN KEY ([MasterTimezoneID]) REFERENCES [dbo].[MasterTimezones] ([MasterTimezoneID])
);


GO
CREATE NONCLUSTERED INDEX [missing_index_23_22_ReferralPayorMappings]
    ON [dbo].[ReferralPayorMappings]([ReferralID] ASC, [IsActive] ASC, [IsDeleted] ASC)
    INCLUDE([PayorID]);

