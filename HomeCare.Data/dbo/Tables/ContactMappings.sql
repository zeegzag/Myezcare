CREATE TABLE [dbo].[ContactMappings] (
    [ContactMappingID]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [ContactID]                       BIGINT        NOT NULL,
    [ReferralID]                      BIGINT        NOT NULL,
    [ClientID]                        BIGINT        NOT NULL,
    [IsEmergencyContact]              BIT           CONSTRAINT [DF_ContactMappings_IsEmergencyContact] DEFAULT ((0)) NOT NULL,
    [ContactTypeID]                   BIGINT        NULL,
    [IsPrimaryPlacementLegalGuardian] BIT           CONSTRAINT [DF_ContactMappings_IsPrimaryPlacementLegalGuardian] DEFAULT ((0)) NOT NULL,
    [IsDCSLegalGuardian]              BIT           CONSTRAINT [DF_ContactMappings_IsDCSLegalGuardian] DEFAULT ((0)) NOT NULL,
    [ROIExpireDate]                   DATE          NULL,
    [ROIType]                         VARCHAR (40)  NULL,
    [Relation]                        VARCHAR (50)  NULL,
    [IsNoticeProviderOnFile]          BIT           CONSTRAINT [DF_ContactMappings_IsNoticeProviderOnFile] DEFAULT ((0)) NOT NULL,
    [CreatedDate]                     DATETIME      NOT NULL,
    [CreatedBy]                       BIGINT        NOT NULL,
    [UpdatedDate]                     DATETIME      NOT NULL,
    [UpdatedBy]                       BIGINT        NOT NULL,
    [SystemID]                        VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ContactMappings] PRIMARY KEY CLUSTERED ([ContactMappingID] ASC),
    CONSTRAINT [FK_ContactMappings_Contacts] FOREIGN KEY ([ContactID]) REFERENCES [dbo].[Contacts] ([ContactID]),
    CONSTRAINT [FK_ContactMappings_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID])
);


GO
CREATE NONCLUSTERED INDEX [missing_index_56_55_ContactMappings]
    ON [dbo].[ContactMappings]([ReferralID] ASC, [ContactTypeID] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_36_35_ContactMappings]
    ON [dbo].[ContactMappings]([ContactTypeID] ASC)
    INCLUDE([ContactID], [ReferralID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_34_33_ContactMappings]
    ON [dbo].[ContactMappings]([ReferralID] ASC, [ContactTypeID] ASC)
    INCLUDE([ContactID]);


GO
CREATE TRIGGER [dbo].[tr_ContactMappings_Updated] ON [dbo].[ContactMappings]
FOR UPDATE AS 

INSERT INTO JO_ContactMappings( 
ContactMappingID,
ContactID,
ReferralID,
ClientID,
IsEmergencyContact,
ContactTypeID,
IsPrimaryPlacementLegalGuardian,
IsDCSLegalGuardian,
ROIExpireDate,
ROIType,
Relation,
IsNoticeProviderOnFile,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
ContactMappingID,
ContactID,
ReferralID,
ClientID,
IsEmergencyContact,
ContactTypeID,
IsPrimaryPlacementLegalGuardian,
IsDCSLegalGuardian,
ROIExpireDate,
ROIType,
Relation,
IsNoticeProviderOnFile,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'U',GETUTCDATE() FROM deleted