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


GO
CREATE TRIGGER [dbo].[tr_ReferralDXCodeMappings_Updated] ON [dbo].[ReferralDXCodeMappings]
FOR UPDATE AS 

INSERT INTO JO_ReferralDXCodeMappings( 
ReferralDXCodeMappingID,
ReferralID,
DXCodeID,
Precedence,
StartDate,
EndDate,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
Action,ActionDate
)

SELECT  
ReferralDXCodeMappingID,
ReferralID,
DXCodeID,
Precedence,
StartDate,
EndDate,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
'U',GETUTCDATE() FROM deleted

GO
CREATE TRIGGER [dbo].[tr_ReferralDXCodeMappings_Deleted] ON [dbo].[ReferralDXCodeMappings]
FOR DELETE AS 

INSERT INTO JO_ReferralDXCodeMappings( 
ReferralDXCodeMappingID,
ReferralID,
DXCodeID,
Precedence,
StartDate,
EndDate,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
Action,ActionDate
)

SELECT  
ReferralDXCodeMappingID,
ReferralID,
DXCodeID,
Precedence,
StartDate,
EndDate,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
'D',GETUTCDATE() FROM deleted
