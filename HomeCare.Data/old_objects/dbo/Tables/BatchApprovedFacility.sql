CREATE TABLE [dbo].[BatchApprovedFacility] (
    [FacilityApprovedBatchID] BIGINT IDENTITY (1, 1) NOT NULL,
    [BillingProviderID]       BIGINT NOT NULL,
    [PayorID]                 BIGINT NOT NULL,
    [BatchID]                 BIGINT NOT NULL,
    CONSTRAINT [PK_FacilityApprovedBatch] PRIMARY KEY CLUSTERED ([FacilityApprovedBatchID] ASC),
    CONSTRAINT [FK_BatchApprovedFacility_Batches] FOREIGN KEY ([BatchID]) REFERENCES [dbo].[Batches] ([BatchID])
);

