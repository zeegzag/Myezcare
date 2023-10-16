CREATE TABLE [dbo].[BatchNoteStatus] (
    [BatchNoteStatusID]   BIGINT        NOT NULL,
    [BatchNoteStatusName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_BatchNoteStatus] PRIMARY KEY CLUSTERED ([BatchNoteStatusID] ASC)
);

