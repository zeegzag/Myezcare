CREATE TABLE [dbo].[OrganizationSettings] (
    [OrganizationSettingID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrganizationID]          BIGINT         NOT NULL,
    [IsSMTPSettingsEntered]   BIT            NULL,
    [NetworkHost]             NVARCHAR (100) NULL,
    [NetworkPort]             NVARCHAR (10)  NULL,
    [FromTitle]               NVARCHAR (100) NULL,
    [FromEmail]               NVARCHAR (100) NULL,
    [FromEmailPassword]       NVARCHAR (100) NULL,
    [EnableSSL]               BIT            NULL,
    [IsTwilioSettingsEntered] BIT            NULL,
    [CreatedDate]             DATETIME       NULL,
    [CreatedBy]               BIGINT         NULL,
    [UpdatedDate]             DATETIME       NULL,
    [UpdatedBy]               BIGINT         NULL,
    [SystemID]                NVARCHAR (MAX) NULL,
    [TwilioCountryCode]       NVARCHAR (100) NULL,
    [TwilioLocation]          NVARCHAR (100) NULL,
    CONSTRAINT [PK__Organiza__4F308E6EF359BFFC] PRIMARY KEY CLUSTERED ([OrganizationSettingID] ASC),
    CONSTRAINT [FK_OrganizationSettings_Organizations] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[Organizations] ([OrganizationID])
);

