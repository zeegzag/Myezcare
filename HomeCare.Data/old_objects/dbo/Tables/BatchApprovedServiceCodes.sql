CREATE TABLE [dbo].[BatchApprovedServiceCodes] (
    [BatchApprovedServiceCodeID] BIGINT IDENTITY (1, 1) NOT NULL,
    [ServiceCodeID]              BIGINT NOT NULL,
    [BatchID]                    BIGINT NOT NULL,
    CONSTRAINT [PK_BatchApprovedServiceCodes] PRIMARY KEY CLUSTERED ([BatchApprovedServiceCodeID] ASC),
    CONSTRAINT [FK_BatchApprovedServiceCodes_Batches] FOREIGN KEY ([BatchID]) REFERENCES [dbo].[Batches] ([BatchID]),
    CONSTRAINT [FK_BatchApprovedServiceCodes_ServiceCodes] FOREIGN KEY ([ServiceCodeID]) REFERENCES [dbo].[ServiceCodes] ([ServiceCodeID])
);

