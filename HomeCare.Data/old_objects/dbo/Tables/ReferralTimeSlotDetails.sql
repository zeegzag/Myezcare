CREATE TABLE [dbo].[ReferralTimeSlotDetails] (
    [ReferralTimeSlotDetailID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReferralTimeSlotMasterID] BIGINT          NOT NULL,
    [Day]                      INT             NOT NULL,
    [StartTime]                TIME (7)        NOT NULL,
    [EndTime]                  TIME (7)        NOT NULL,
    [IsDeleted]                BIT             CONSTRAINT [DF_ReferralTimeSlotDetails_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]              DATETIME        NULL,
    [CreatedBy]                BIGINT          NULL,
    [UpdatedDate]              DATETIME        NULL,
    [UpdatedBy]                BIGINT          NULL,
    [SystemID]                 VARCHAR (100)   NULL,
    [Notes]                    NVARCHAR (1000) NULL,
    [UsedInScheduling]         BIT             DEFAULT ((1)) NOT NULL,
    [CareTypeId]               BIGINT          NULL,
    CONSTRAINT [PK_ReferralTimeSlotDetails] PRIMARY KEY CLUSTERED ([ReferralTimeSlotDetailID] ASC),
    CONSTRAINT [FK_ReferralTimeSlotDetails_ReferralTimeSlotMaster] FOREIGN KEY ([ReferralTimeSlotMasterID]) REFERENCES [dbo].[ReferralTimeSlotMaster] ([ReferralTimeSlotMasterID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_ReferralTimeSlotMasterID_IsDeleted_F7EC0]
    ON [dbo].[ReferralTimeSlotDetails]([ReferralTimeSlotMasterID] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_IsDeleted_A847A]
    ON [dbo].[ReferralTimeSlotDetails]([IsDeleted] ASC)
    INCLUDE([ReferralTimeSlotMasterID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_ReferralTimeSlotMasterID_IsDeleted_9A03D]
    ON [dbo].[ReferralTimeSlotDetails]([ReferralTimeSlotMasterID] ASC, [IsDeleted] ASC)
    INCLUDE([Day], [StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_IsDeleted_E1F79]
    ON [dbo].[ReferralTimeSlotDetails]([IsDeleted] ASC)
    INCLUDE([ReferralTimeSlotMasterID], [Day], [StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_ReferralTimeSlotMasterID_Day_IsDeleted_1425B]
    ON [dbo].[ReferralTimeSlotDetails]([ReferralTimeSlotMasterID] ASC, [Day] ASC, [IsDeleted] ASC)
    INCLUDE([StartTime], [EndTime], [Notes], [UsedInScheduling]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_ReferralTimeSlotMasterID_IsDeleted_BD509]
    ON [dbo].[ReferralTimeSlotDetails]([ReferralTimeSlotMasterID] ASC, [IsDeleted] ASC)
    INCLUDE([Day], [StartTime], [EndTime], [Notes], [UsedInScheduling]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_IsDeleted_ReferralTimeSlotDetailID_F1F49]
    ON [dbo].[ReferralTimeSlotDetails]([IsDeleted] ASC, [ReferralTimeSlotDetailID] ASC)
    INCLUDE([ReferralTimeSlotMasterID], [Day], [StartTime], [EndTime]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDetails_Day_IsDeleted_DAA7E]
    ON [dbo].[ReferralTimeSlotDetails]([Day] ASC, [IsDeleted] ASC)
    INCLUDE([ReferralTimeSlotMasterID], [StartTime], [EndTime]);

