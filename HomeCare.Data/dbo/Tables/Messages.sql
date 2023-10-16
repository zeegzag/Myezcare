CREATE TABLE [dbo].[Messages] (
    [MessageID]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [ConversationID] NVARCHAR (128) NOT NULL,
    [CategoryID]     BIGINT         NOT NULL,
    [TextMessage]    VARCHAR (MAX)  NOT NULL,
    [RepliedBy]      BIGINT         NULL,
    [IsRead]         BIT            NOT NULL,
    [CreatedBy]      BIGINT         NOT NULL,
    [CreatedDate]    DATETIME       NOT NULL,
    [UpdatedDate]    DATETIME       NULL,
    [UpdatedBy]      BIGINT         NULL,
    [SystemID]       VARCHAR (100)  NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([MessageID] ASC)
);

