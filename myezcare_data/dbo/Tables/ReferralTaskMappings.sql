CREATE TABLE [dbo].[ReferralTaskMappings] (
    [ReferralTaskMappingID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [VisitTaskID]           BIGINT         NOT NULL,
    [IsRequired]            BIT            CONSTRAINT [DF_ReferralTaskMappings_IsRequired] DEFAULT ((0)) NOT NULL,
    [IsDeleted]             BIT            CONSTRAINT [DF_ReferralTaskMappings_IsDeleted] DEFAULT ((0)) NOT NULL,
    [ReferralID]            BIGINT         NOT NULL,
    [CreatedDate]           DATETIME       NULL,
    [CreatedBy]             BIGINT         NULL,
    [UpdatedDate]           DATETIME       NULL,
    [UpdatedBy]             BIGINT         NULL,
    [SystemID]              VARCHAR (100)  NULL,
    [Frequency]             NVARCHAR (100) NULL,
    [Comment]               NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ReferralTaskMappings] PRIMARY KEY CLUSTERED ([ReferralTaskMappingID] ASC),
    CONSTRAINT [FK_ReferralTaskMappings_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_ReferralTaskMappings_VisitTasks] FOREIGN KEY ([VisitTaskID]) REFERENCES [dbo].[VisitTasks] ([VisitTaskID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_IsDeleted_ReferralID_B0AF0]
    ON [dbo].[ReferralTaskMappings]([IsDeleted] ASC, [ReferralID] ASC)
    INCLUDE([VisitTaskID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_IsDeleted_ReferralID_E583A]
    ON [dbo].[ReferralTaskMappings]([IsDeleted] ASC, [ReferralID] ASC)
    INCLUDE([VisitTaskID], [IsRequired], [CreatedDate], [CreatedBy], [Frequency], [Comment]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_VisitTaskID_IsDeleted_ReferralID_CAB97]
    ON [dbo].[ReferralTaskMappings]([VisitTaskID] ASC, [IsDeleted] ASC, [ReferralID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_IsDeleted_ReferralID_1FD7A]
    ON [dbo].[ReferralTaskMappings]([IsDeleted] ASC, [ReferralID] ASC)
    INCLUDE([VisitTaskID], [IsRequired]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_ReferralID_16D2B]
    ON [dbo].[ReferralTaskMappings]([ReferralID] ASC)
    INCLUDE([VisitTaskID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_IsRequired_IsDeleted_ReferralID_18422]
    ON [dbo].[ReferralTaskMappings]([IsRequired] ASC, [IsDeleted] ASC, [ReferralID] ASC)
    INCLUDE([VisitTaskID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_IsDeleted_FD9D9]
    ON [dbo].[ReferralTaskMappings]([IsDeleted] ASC)
    INCLUDE([VisitTaskID], [ReferralID]);


GO
CREATE NONCLUSTERED INDEX [IX_ReferralTaskMappings_VisitTaskID_ReferralID_CAEE5]
    ON [dbo].[ReferralTaskMappings]([VisitTaskID] ASC, [ReferralID] ASC);

