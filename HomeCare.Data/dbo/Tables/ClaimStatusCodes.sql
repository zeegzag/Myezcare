CREATE TABLE [dbo].[ClaimStatusCodes] (
    [ClaimStatusCodeID]          BIGINT        NOT NULL,
    [ClaimStatusName]            VARCHAR (100) NOT NULL,
    [ClaimStatusCodeDescription] VARCHAR (MAX) NULL,
    [IsDeleted]                  BIT           NOT NULL,
    CONSTRAINT [PK_ClaimStatusCodes] PRIMARY KEY CLUSTERED ([ClaimStatusCodeID] ASC)
);

