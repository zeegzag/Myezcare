CREATE TABLE [dbo].[JO_ContactMappings] (
    [JO_ContactMappingID]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [ContactMappingID]                BIGINT        NOT NULL,
    [ContactID]                       BIGINT        NOT NULL,
    [ReferralID]                      BIGINT        NOT NULL,
    [ClientID]                        BIGINT        NOT NULL,
    [IsEmergencyContact]              BIT           CONSTRAINT [DF_JO_ContactMappings_IsEmergencyContact] DEFAULT ((0)) NOT NULL,
    [ContactTypeID]                   BIGINT        NULL,
    [IsPrimaryPlacementLegalGuardian] BIT           CONSTRAINT [DF_JO_ContactMappings_IsPrimaryPlacementLegalGuardian] DEFAULT ((0)) NOT NULL,
    [IsDCSLegalGuardian]              BIT           CONSTRAINT [DF_JO_ContactMappings_IsDCSLegalGuardian] DEFAULT ((0)) NOT NULL,
    [ROIExpireDate]                   DATE          NULL,
    [ROIType]                         NVARCHAR (20) NULL,
    [Relation]                        VARCHAR (50)  NULL,
    [IsNoticeProviderOnFile]          BIT           CONSTRAINT [DF_JO_ContactMappings_IsNoticeProviderOnFile] DEFAULT ((0)) NOT NULL,
    [CreatedDate]                     DATETIME      NOT NULL,
    [CreatedBy]                       BIGINT        NOT NULL,
    [UpdatedDate]                     DATETIME      NOT NULL,
    [UpdatedBy]                       BIGINT        NOT NULL,
    [SystemID]                        VARCHAR (100) NOT NULL,
    [Action]                          CHAR (1)      NOT NULL,
    [ActionDate]                      DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_ContactMappings] PRIMARY KEY CLUSTERED ([JO_ContactMappingID] ASC)
);

