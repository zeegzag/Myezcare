CREATE TABLE [dbo].[FrequencyCodes] (
    [FrequencyCodeID]     BIGINT        NOT NULL,
    [Code]                VARCHAR (50)  NOT NULL,
    [Description]         VARCHAR (500) NOT NULL,
    [DefaultScheduleDays] INT           NULL,
    [UsedInRespiteCare]   BIT           DEFAULT ((0)) NOT NULL,
    [UsedInHomeCare]      BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FrequencyCodes] PRIMARY KEY CLUSTERED ([FrequencyCodeID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_FrequencyCodes_Updated] ON [dbo].[FrequencyCodes]
FOR UPDATE AS 

INSERT INTO JO_FrequencyCodes( 
FrequencyCodeID,
Code,
Description,
DefaultScheduleDays,
Action,ActionDate
)

SELECT  
FrequencyCodeID,
Code,
Description,
DefaultScheduleDays,
'U',GETUTCDATE() FROM deleted