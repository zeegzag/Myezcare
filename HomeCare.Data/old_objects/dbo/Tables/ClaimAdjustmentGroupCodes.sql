CREATE TABLE [dbo].[ClaimAdjustmentGroupCodes] (
    [ClaimAdjustmentGroupCodeID]          VARCHAR (10)  NOT NULL,
    [ClaimAdjustmentGroupCodeName]        VARCHAR (100) NOT NULL,
    [ClaimAdjustmentGroupCodeDescription] VARCHAR (MAX) NULL,
    [IsDeleted]                           BIT           NOT NULL,
    [OrderID]                             INT           NULL,
    CONSTRAINT [PK_ClaimGroupCodes] PRIMARY KEY CLUSTERED ([ClaimAdjustmentGroupCodeID] ASC)
);

