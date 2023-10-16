CREATE TABLE [dbo].[EncryptedMailMessageTokens] (
    [EncryptedMailID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [EncryptedValue]  VARCHAR (MAX) NOT NULL,
    [IsUsed]          BIT           NOT NULL,
    [ExpireDateTime]  DATETIME      NOT NULL,
    CONSTRAINT [PK_EncryptedMailMessageTokens] PRIMARY KEY CLUSTERED ([EncryptedMailID] ASC)
);


GO
CREATE TRIGGER [dbo].[tr_EncryptedMailMessageTokens_Deleted] ON [dbo].[EncryptedMailMessageTokens]
FOR Delete AS 

INSERT INTO JO_EncryptedMailMessageTokens( 
EncryptedMailID,
EncryptedValue,
IsUsed,
ExpireDateTime,
Action,ActionDate
)

SELECT  
EncryptedMailID,
EncryptedValue,
IsUsed,
ExpireDateTime,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_EncryptedMailMessageTokens_Deleted]
    ON [dbo].[EncryptedMailMessageTokens];


GO
CREATE TRIGGER [dbo].[tr_EncryptedMailMessageTokens_Updated] ON [dbo].[EncryptedMailMessageTokens]
FOR UPDATE AS 

INSERT INTO JO_EncryptedMailMessageTokens( 
EncryptedMailID,
EncryptedValue,
IsUsed,
ExpireDateTime,
Action,ActionDate
)

SELECT  
EncryptedMailID,
EncryptedValue,
IsUsed,
ExpireDateTime,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_EncryptedMailMessageTokens_Updated]
    ON [dbo].[EncryptedMailMessageTokens];

