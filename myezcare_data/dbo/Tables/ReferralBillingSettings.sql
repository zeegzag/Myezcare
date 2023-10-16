CREATE TABLE [dbo].[ReferralBillingSettings] (
    [ReferralBillingSettingID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReferralID]                 BIGINT         NOT NULL,
    [AuthrizationCode_CMS1500]   NVARCHAR (20)  NULL,
    [POS_CMS1500]                INT            NULL,
    [AuthrizationCode_UB04]      NVARCHAR (20)  NULL,
    [POS_UB04]                   INT            NULL,
    [AdmissionTypeCode_UB04]     INT            NULL,
    [AdmissionSourceCode_UB04]   INT            NULL,
    [PatientStatusCode_UB04]     INT            NULL,
    [CreatedDate]                DATETIME       NULL,
    [CreatedBy]                  BIGINT         NULL,
    [UpdatedDate]                DATETIME       NULL,
    [UpdatedBy]                  BIGINT         NULL,
    [SystemID]                   NVARCHAR (MAX) NULL,
    [SpecialProgramCode_CMS1500] INT            CONSTRAINT [DF__ReferralB__Speci__1392CE8F] DEFAULT ((3)) NOT NULL,
    CONSTRAINT [PK__Referral__76AF6615997AB9C5] PRIMARY KEY CLUSTERED ([ReferralBillingSettingID] ASC)
);

