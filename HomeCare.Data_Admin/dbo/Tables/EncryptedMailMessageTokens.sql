CREATE TABLE [dbo].[EncryptedMailMessageTokens] (
    [EncryptedMailID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [EncryptedValue]  VARCHAR (MAX) NOT NULL,
    [IsUsed]          BIT           NOT NULL,
    [ExpireDateTime]  DATETIME      NOT NULL,
    CONSTRAINT [PK_EncryptedMailMessageTokens] PRIMARY KEY CLUSTERED ([EncryptedMailID] ASC)
);

