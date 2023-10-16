CREATE TABLE [dbo].[DXCodes] (
    [DXCodeID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [DXCodeName]       VARCHAR (100) NOT NULL,
    [DXCodeWithoutDot] VARCHAR (50)  NULL,
    [DxCodeType]       VARCHAR (50)  CONSTRAINT [DF_DXCodes_DxCodeType] DEFAULT ('ABK') NULL,
    [Description]      VARCHAR (500) NULL,
    [IsDeleted]        BIT           NOT NULL,
    [EffectiveFrom]    DATE          NULL,
    [EffectiveTo]      DATE          NULL,
    [CreatedDate]      DATETIME      NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [UpdatedDate]      DATETIME      NOT NULL,
    [UpdatedBy]        BIGINT        NOT NULL,
    [SystemID]         VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_DXCodes_1] PRIMARY KEY CLUSTERED ([DXCodeID] ASC),
    CONSTRAINT [FK__DXCodes__DxCodeT__74F938D6] FOREIGN KEY ([DxCodeType]) REFERENCES [dbo].[DxCodeTypes] ([DxCodeTypeID])
);


GO
CREATE TRIGGER [dbo].[tr_DXCodes_Updated] ON [dbo].[DXCodes]
FOR UPDATE AS 

INSERT INTO JO_DXCodes( 
DXCodeID,
DXCodeName,
DXCodeWithoutDot,
DxCodeType,
Description,
IsDeleted,
EffectiveFrom,
EffectiveTo,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
Action,ActionDate
)

SELECT  
DXCodeID,
DXCodeName,
DXCodeWithoutDot,
DxCodeType,
Description,
IsDeleted,
EffectiveFrom,
EffectiveTo,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
'U',GETUTCDATE() FROM deleted