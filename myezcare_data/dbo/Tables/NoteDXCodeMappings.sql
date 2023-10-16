CREATE TABLE [dbo].[NoteDXCodeMappings] (
    [NoteDXCodeMappingID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [ReferralDXCodeMappingID] BIGINT        NOT NULL,
    [ReferralID]              BIGINT        NOT NULL,
    [NoteID]                  BIGINT        NOT NULL,
    [DXCodeID]                BIGINT        NOT NULL,
    [DXCodeName]              VARCHAR (50)  NULL,
    [DxCodeType]              VARCHAR (50)  NULL,
    [Precedence]              INT           NULL,
    [StartDate]               DATE          NULL,
    [EndDate]                 DATE          NULL,
    [Description]             VARCHAR (500) NULL,
    [DXCodeWithoutDot]        VARCHAR (50)  NULL,
    [DxCodeShortName]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_NoteDXCodeMappings] PRIMARY KEY CLUSTERED ([NoteDXCodeMappingID] ASC)
);


GO
create TRIGGER [dbo].[tr_NoteDXCodeMappings_Deleted] ON [dbo].[NoteDXCodeMappings]
FOR UPDATE AS 

INSERT INTO JO_NoteDXCodeMappings( 
NoteDXCodeMappingID,
ReferralDXCodeMappingID,
ReferralID,
NoteID,
DXCodeID,
DXCodeName,
DxCodeType,
Precedence,
StartDate,
EndDate,
Description,
DXCodeWithoutDot,
DxCodeShortName,
Action,ActionDate
)

SELECT  
NoteDXCodeMappingID,
ReferralDXCodeMappingID,
ReferralID,
NoteID,
DXCodeID,
DXCodeName,
DxCodeType,
Precedence,
StartDate,
EndDate,
Description,
DXCodeWithoutDot,
DxCodeShortName,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_NoteDXCodeMappings_Deleted]
    ON [dbo].[NoteDXCodeMappings];


GO
create TRIGGER [dbo].[tr_NoteDXCodeMappings_Updated] ON [dbo].[NoteDXCodeMappings]
FOR UPDATE AS 

INSERT INTO JO_NoteDXCodeMappings( 
NoteDXCodeMappingID,
ReferralDXCodeMappingID,
ReferralID,
NoteID,
DXCodeID,
DXCodeName,
DxCodeType,
Precedence,
StartDate,
EndDate,
Description,
DXCodeWithoutDot,
DxCodeShortName,
Action,ActionDate
)

SELECT  
NoteDXCodeMappingID,
ReferralDXCodeMappingID,
ReferralID,
NoteID,
DXCodeID,
DXCodeName,
DxCodeType,
Precedence,
StartDate,
EndDate,
Description,
DXCodeWithoutDot,
DxCodeShortName,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_NoteDXCodeMappings_Updated]
    ON [dbo].[NoteDXCodeMappings];

