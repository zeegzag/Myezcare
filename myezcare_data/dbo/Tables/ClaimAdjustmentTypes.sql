CREATE TABLE [dbo].[ClaimAdjustmentTypes] (
    [ClaimAdjustmentTypeID]   VARCHAR (50) NOT NULL,
    [ClaimAdjustmentTypeName] VARCHAR (50) NOT NULL,
    [IsDeleted]               BIT          CONSTRAINT [DF_ClaimAdjustmentType_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ClaimAdjustmentType] PRIMARY KEY CLUSTERED ([ClaimAdjustmentTypeID] ASC)
);

