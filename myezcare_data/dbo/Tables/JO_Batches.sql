CREATE TABLE [dbo].[JO_Batches] (
    [JO_BatchID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [BatchID]     BIGINT        NOT NULL,
    [BatchTypeID] BIGINT        NOT NULL,
    [PayorID]     BIGINT        NOT NULL,
    [StartDate]   DATE          NOT NULL,
    [EndDate]     DATE          NOT NULL,
    [IsDeleted]   BIT           NOT NULL,
    [IsSentBy]    BIGINT        NOT NULL,
    [IsSent]      BIT           NOT NULL,
    [SentDate]    DATETIME      NOT NULL,
    [CreatedBy]   BIGINT        NOT NULL,
    [CreatedDate] DATETIME      NOT NULL,
    [UpdatedDate] DATETIME      NOT NULL,
    [UpdatedBy]   BIGINT        NOT NULL,
    [SystemID]    VARCHAR (100) NOT NULL,
    [Action]      CHAR (1)      NOT NULL,
    [ActionDate]  DATETIME      NOT NULL
);

