CREATE TABLE [dbo].[Batches] (
    [BatchID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [BatchTypeID] BIGINT        NOT NULL,
    [PayorID]     BIGINT        NOT NULL,
    [StartDate]   DATE          NOT NULL,
    [EndDate]     DATE          NOT NULL,
    [IsDeleted]   BIT           NOT NULL,
    [IsSentBy]    BIGINT        NULL,
    [IsSent]      BIT           NOT NULL,
    [SentDate]    DATETIME      NULL,
    [CreatedBy]   BIGINT        NOT NULL,
    [CreatedDate] DATETIME      NOT NULL,
    [UpdatedDate] DATETIME      NOT NULL,
    [UpdatedBy]   BIGINT        NOT NULL,
    [SystemID]    VARCHAR (100) NOT NULL,
    [Comment]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Batches] PRIMARY KEY CLUSTERED ([BatchID] ASC),
    CONSTRAINT [FK_Batches_BatchTypes] FOREIGN KEY ([BatchTypeID]) REFERENCES [dbo].[BatchTypes] ([BatchTypeID]),
    CONSTRAINT [FK_Batches_Payors] FOREIGN KEY ([PayorID]) REFERENCES [dbo].[Payors] ([PayorID])
);


GO
create TRIGGER [dbo].[tr_Batches_Updated] ON [dbo].[Batches]
FOR DELETE AS 

INSERT INTO JO_Batches( 
BatchID,
BatchTypeID,
PayorID,
StartDate,
EndDate,
IsDeleted,
IsSentBy,
IsSent,
SentDate,
CreatedBy,
CreatedDate,
UpdatedDate,
UpdatedBy,
SystemID,
Action,
ActionDate
)
SELECT  
BatchID,
BatchTypeID,
PayorID,
StartDate,
EndDate,
IsDeleted,
IsSentBy,
IsSent,
SentDate,
CreatedBy,
CreatedDate,
UpdatedDate,
UpdatedBy,
SystemID, 
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Batches_Updated]
    ON [dbo].[Batches];


GO
create TRIGGER [dbo].[tr_Batches_Deleted] ON [dbo].[Batches]
FOR DELETE AS 

INSERT INTO JO_Batches( 
BatchID,
BatchTypeID,
PayorID,
StartDate,
EndDate,
IsDeleted,
IsSentBy,
IsSent,
SentDate,
CreatedBy,
CreatedDate,
UpdatedDate,
UpdatedBy,
SystemID,
Action,
ActionDate
)
SELECT  
BatchID,
BatchTypeID,
PayorID,
StartDate,
EndDate,
IsDeleted,
IsSentBy,
IsSent,
SentDate,
CreatedBy,
CreatedDate,
UpdatedDate,
UpdatedBy,
SystemID, 
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_Batches_Deleted]
    ON [dbo].[Batches];

