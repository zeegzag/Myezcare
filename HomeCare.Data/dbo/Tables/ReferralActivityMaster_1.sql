CREATE TABLE [dbo].[ReferralActivityMaster] (
    [ReferralActivityMasterId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [ReferralId]               INT          NULL,
    [Month]                    VARCHAR (50) NULL,
    [Year]                     INT          NULL,
    [CreatedBy]                INT          NULL,
    [CreatedDate]              DATETIME     NULL
);

