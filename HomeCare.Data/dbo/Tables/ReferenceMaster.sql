CREATE TABLE [dbo].[ReferenceMaster] (
    [ReferenceID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [IsActive]      BIT           NOT NULL,
    [ReferenceName] VARCHAR (100) NOT NULL,
    [ReferenceCode] VARCHAR (2)   NOT NULL,
    [CreatedBy]     BIGINT        NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_ReferenceMaster_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]     BIGINT        NULL,
    [UpdateDate]    DATETIME      NULL,
    CONSTRAINT [PK_ReferenceMaster] PRIMARY KEY CLUSTERED ([ReferenceID] ASC)
);

