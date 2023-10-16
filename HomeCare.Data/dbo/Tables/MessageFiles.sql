CREATE TABLE [dbo].[MessageFiles] (
    [MessageFileID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [MessageID]     BIGINT        NOT NULL,
    [FileUploaded]  VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_MessageFiles] PRIMARY KEY CLUSTERED ([MessageFileID] ASC),
    CONSTRAINT [FK_MessageFiles_Messages] FOREIGN KEY ([MessageID]) REFERENCES [dbo].[Messages] ([MessageID]) ON DELETE CASCADE ON UPDATE CASCADE
);

