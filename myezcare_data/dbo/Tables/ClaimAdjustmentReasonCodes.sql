CREATE TABLE [dbo].[ClaimAdjustmentReasonCodes] (
    [ClaimAdjustmentReasonCodeID]      VARCHAR (50)  NOT NULL,
    [ClaimAdjustmentReasonDescription] VARCHAR (MAX) NOT NULL,
    [ClaimType]                        VARCHAR (10)  NOT NULL,
    [IsDeleted]                        BIT           NOT NULL,
    [OrderID]                          INT           NOT NULL,
    CONSTRAINT [PK_ClaimReasonCodes] PRIMARY KEY CLUSTERED ([ClaimAdjustmentReasonCodeID] ASC)
);

