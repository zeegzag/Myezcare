CREATE TABLE [dbo].[ReferralTimeSlotDates] (
    [ReferralTSDateID]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReferralID]               BIGINT          NOT NULL,
    [ReferralTimeSlotMasterID] BIGINT          NOT NULL,
    [ReferralTSDate]           DATE            NOT NULL,
    [ReferralTSStartTime]      DATETIME        NOT NULL,
    [ReferralTSEndTime]        DATETIME        NOT NULL,
    [UsedInScheduling]         BIT             DEFAULT ((1)) NOT NULL,
    [Notes]                    NVARCHAR (1000) NULL,
    [DayNumber]                INT             NULL,
    [ReferralTimeSlotDetailID] BIGINT          NULL,
    [OnHold]                   BIT             DEFAULT ((0)) NOT NULL,
    [ReferralOnHoldDetailID]   BIGINT          NULL,
    [IsDenied]                 BIT             CONSTRAINT [DF__ReferralT__IsDen__61274A53] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ReferralTimeSlotDates] PRIMARY KEY CLUSTERED ([ReferralTSDateID] ASC),
    CONSTRAINT [FK_ReferralTimeSlotDates_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_ReferralTimeSlotDates_ReferralTimeSlotMaster] FOREIGN KEY ([ReferralTimeSlotMasterID]) REFERENCES [dbo].[ReferralTimeSlotMaster] ([ReferralTimeSlotMasterID])
);






GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_ReferralTimeSlotMasterID_ReferralTSDate_9EC76]
    ON [dbo].[ReferralTimeSlotDates]([ReferralTimeSlotMasterID] ASC, [ReferralTSDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_ReferralID_ReferralTSDate_97D75]
    ON [dbo].[ReferralTimeSlotDates]([ReferralID] ASC, [ReferralTSDate] ASC)
    INCLUDE([ReferralTimeSlotMasterID], [ReferralTSStartTime], [ReferralTSEndTime], [UsedInScheduling], [Notes], [DayNumber], [ReferralTimeSlotDetailID], [OnHold], [ReferralOnHoldDetailID], [IsDenied]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_ReferralTSDate_B1D54]
    ON [dbo].[ReferralTimeSlotDates]([ReferralTSDate] ASC)
    INCLUDE([ReferralTimeSlotMasterID], [ReferralTSStartTime], [ReferralTSEndTime], [DayNumber]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_ReferralTimeSlotDetailID_ReferralTSDate_36911]
    ON [dbo].[ReferralTimeSlotDates]([ReferralTimeSlotDetailID] ASC, [ReferralTSDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_ReferralTSDate_EFA1B]
    ON [dbo].[ReferralTimeSlotDates]([ReferralTSDate] ASC)
    INCLUDE([ReferralTimeSlotDetailID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_ReferralTimeSlotDetailID_ReferralTSDate_54BB7]
    ON [dbo].[ReferralTimeSlotDates]([ReferralTimeSlotDetailID] ASC, [ReferralTSDate] ASC)
    INCLUDE([ReferralTimeSlotMasterID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_ReferralTSDate_50E3F]
    ON [dbo].[ReferralTimeSlotDates]([ReferralTSDate] ASC)
    INCLUDE([ReferralTimeSlotMasterID], [ReferralTimeSlotDetailID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates_IsDenied_2CA77]
    ON [dbo].[ReferralTimeSlotDates]([IsDenied] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_88_87_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([ReferralTSDate] ASC)
    INCLUDE([ReferralID], [ReferralTSStartTime], [ReferralTSEndTime]);


GO
CREATE NONCLUSTERED INDEX [missing_index_86_85_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([ReferralID] ASC, [ReferralTSDate] ASC)
    INCLUDE([ReferralTSStartTime], [ReferralTSEndTime]);


GO
CREATE NONCLUSTERED INDEX [missing_index_84_83_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([ReferralID] ASC)
    INCLUDE([ReferralTSDate], [ReferralTSStartTime], [ReferralTSEndTime], [UsedInScheduling], [ReferralTimeSlotDetailID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_187_186_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([ReferralID] ASC, [ReferralTSStartTime] ASC, [ReferralTSEndTime] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_184_183_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([UsedInScheduling] ASC)
    INCLUDE([ReferralID], [ReferralTSStartTime], [ReferralTSEndTime]);


GO
CREATE NONCLUSTERED INDEX [missing_index_151_150_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([ReferralID] ASC)
    INCLUDE([ReferralTSDate], [ReferralTSStartTime], [ReferralTSEndTime], [ReferralTimeSlotDetailID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_1054_1053_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([UsedInScheduling] ASC, [ReferralTimeSlotDetailID] ASC, [ReferralTSDate] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_104_103_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([UsedInScheduling] ASC, [OnHold] ASC)
    INCLUDE([ReferralID], [ReferralTSDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_102_101_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([ReferralID] ASC, [UsedInScheduling] ASC, [OnHold] ASC, [ReferralTSDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTimeSlotDates001]
    ON [dbo].[ReferralTimeSlotDates]([ReferralID] ASC, [ReferralOnHoldDetailID] ASC)
    INCLUDE([ReferralTSDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_219978_219977_ReferralTimeSlotDates]
    ON [dbo].[ReferralTimeSlotDates]([IsDenied] ASC, [ReferralTSStartTime] ASC)
    INCLUDE([ReferralID]);

