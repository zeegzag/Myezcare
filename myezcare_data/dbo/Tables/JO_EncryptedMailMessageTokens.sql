CREATE TABLE [dbo].[JO_EncryptedMailMessageTokens] (
    [JO_EncryptedMailID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [EncryptedMailID]    BIGINT        NOT NULL,
    [EncryptedValue]     VARCHAR (MAX) NOT NULL,
    [IsUsed]             BIT           NOT NULL,
    [ExpireDateTime]     DATETIME      NOT NULL,
    [Action]             CHAR (1)      NOT NULL,
    [ActionDate]         DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_EncryptedMailMessageTokens] PRIMARY KEY CLUSTERED ([JO_EncryptedMailID] ASC)
);

