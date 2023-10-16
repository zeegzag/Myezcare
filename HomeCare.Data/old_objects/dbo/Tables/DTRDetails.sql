CREATE TABLE [dbo].[DTRDetails] (
    [DTRDetailID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [VehicleNumber]   VARCHAR (100) NULL,
    [VehicleType]     VARCHAR (100) NULL,
    [LocationAddress] VARCHAR (500) NULL,
    [DTRDetailType]   INT           NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_DTRDetails_IsDeleted] DEFAULT ((0)) NOT NULL,
    [OrderNumber]     INT           NULL,
    CONSTRAINT [PK_DTRDetails] PRIMARY KEY CLUSTERED ([DTRDetailID] ASC)
);

