CREATE TABLE [dbo].[EmployeeVisitNotes] (
    [EmployeeVisitNoteID]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [EmployeeVisitID]       BIGINT          NOT NULL,
    [Description]           NVARCHAR (1000) NULL,
    [ServiceTime]           BIGINT          CONSTRAINT [DF_EmployeeVisitNotes_ServiceTime] DEFAULT ((0)) NOT NULL,
    [IsDeleted]             BIT             CONSTRAINT [DF_EmployeeVisitNotes_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]           DATETIME        NULL,
    [CreatedBy]             BIGINT          NULL,
    [UpdatedDate]           DATETIME        NULL,
    [UpdatedBy]             BIGINT          NULL,
    [SystemID]              VARCHAR (100)   NULL,
    [ReferralTaskMappingID] BIGINT          NULL,
    [AlertComment]          NVARCHAR (1000) NULL,
    [ServiceCodeID]         BIGINT          NULL,
    [NoteID]                BIGINT          NULL,
    [CareTypeID]            BIGINT          NULL,
    [IsInvoiceGenerated]    BIT             CONSTRAINT [DF__EmployeeV__IsInv__6033261A] DEFAULT ((0)) NOT NULL,
    [VisitType]             INT             CONSTRAINT [DF_EmployeeVisitNotes_VisitType] DEFAULT ((1)) NOT NULL,
    [IsSimpleTask]          INT             NULL,
    [MultipleTask]          NVARCHAR (MAX)  NULL,
    [SimpleTaskType] BIT NULL, 
    CONSTRAINT [PK_EmployeeVisitNotes] PRIMARY KEY CLUSTERED ([EmployeeVisitNoteID] ASC),
    CONSTRAINT [FK_EmployeeVisitNotes_EmployeeVisits] FOREIGN KEY ([EmployeeVisitID]) REFERENCES [dbo].[EmployeeVisits] ([EmployeeVisitID]),
    CONSTRAINT [FK_EmployeeVisitNotes_ReferralTaskMappings] FOREIGN KEY ([ReferralTaskMappingID]) REFERENCES [dbo].[ReferralTaskMappings] ([ReferralTaskMappingID])
);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_816B4]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC)
    INCLUDE([Description], [ServiceTime], [IsDeleted], [ReferralTaskMappingID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_4B364]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC)
    INCLUDE([Description], [ServiceTime], [ReferralTaskMappingID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_ReferralTaskMappingID_BEA63]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [ReferralTaskMappingID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_141B1]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC)
    INCLUDE([NoteID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_ReferralTaskMappingID_17E5B]
    ON [dbo].[EmployeeVisitNotes]([ReferralTaskMappingID] ASC)
    INCLUDE([ServiceTime], [CreatedDate], [AlertComment]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_ReferralTaskMappingID_76765]
    ON [dbo].[EmployeeVisitNotes]([ReferralTaskMappingID] ASC)
    INCLUDE([EmployeeVisitID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_6926B]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_ReferralTaskMappingID_ServiceTime_9F585]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [ReferralTaskMappingID] ASC, [ServiceTime] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_IsDeleted_57551]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_3848F]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC)
    INCLUDE([Description], [ReferralTaskMappingID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_ReferralTaskMappingID_ServiceTime_8B7C9]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [ReferralTaskMappingID] ASC, [ServiceTime] ASC)
    INCLUDE([Description]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_IsDeleted_951BC]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [IsDeleted] ASC)
    INCLUDE([ServiceTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_IsDeleted_705D1]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [IsDeleted] ASC)
    INCLUDE([ServiceTime], [ReferralTaskMappingID], [ServiceCodeID], [NoteID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_2F349]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC)
    INCLUDE([ServiceTime], [ReferralTaskMappingID], [AlertComment]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_IsDeleted_4A11C]
    ON [dbo].[EmployeeVisitNotes]([IsDeleted] ASC)
    INCLUDE([EmployeeVisitID], [ServiceTime]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_IsDeleted_NoteID_245DF]
    ON [dbo].[EmployeeVisitNotes]([IsDeleted] ASC, [NoteID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_ReferralTaskMappingID_83CBE]
    ON [dbo].[EmployeeVisitNotes]([ReferralTaskMappingID] ASC)
    INCLUDE([EmployeeVisitID], [ServiceTime], [AlertComment]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_IsDeleted_18BBA]
    ON [dbo].[EmployeeVisitNotes]([IsDeleted] ASC)
    INCLUDE([NoteID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_ReferralTaskMappingID_9F72A]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [ReferralTaskMappingID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_ReferralTaskMappingID_EmployeeVisitNoteID_BDFC8]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [ReferralTaskMappingID] ASC, [EmployeeVisitNoteID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_69220]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC)
    INCLUDE([ReferralTaskMappingID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_657C7]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC)
    INCLUDE([Description], [ServiceTime], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [ReferralTaskMappingID], [AlertComment], [ServiceCodeID], [NoteID], [CareTypeID], [IsInvoiceGenerated], [VisitType], [IsSimpleTask], [MultipleTask]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_EmployeeVisitID_ReferralTaskMappingID_340BA]
    ON [dbo].[EmployeeVisitNotes]([EmployeeVisitID] ASC, [ReferralTaskMappingID] ASC)
    INCLUDE([Description], [ServiceTime], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [AlertComment], [ServiceCodeID], [NoteID], [CareTypeID], [IsInvoiceGenerated], [VisitType], [IsSimpleTask], [MultipleTask]);


GO
CREATE NONCLUSTERED INDEX [IX_EmployeeVisitNotes_ReferralTaskMappingID_07B7B]
    ON [dbo].[EmployeeVisitNotes]([ReferralTaskMappingID] ASC);

