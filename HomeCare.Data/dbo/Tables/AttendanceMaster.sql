CREATE TABLE [dbo].[AttendanceMaster] (
    [AttendanceMasterID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [ScheduleMasterID]   BIGINT        NOT NULL,
    [ReferralID]         BIGINT        NOT NULL,
    [StartDate]          DATE          NOT NULL,
    [EndDate]            DATE          NOT NULL,
    [Comment]            VARCHAR (MAX) NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME      NOT NULL,
    [UpdatedBy]          BIGINT        NOT NULL,
    [UpdatedDate]        DATETIME      NOT NULL,
    [SystemID]           VARCHAR (50)  NOT NULL,
    [AttendanceStatus]   INT           NULL,
    CONSTRAINT [PK_AttendanceMaster] PRIMARY KEY CLUSTERED ([AttendanceMasterID] ASC),
    CONSTRAINT [FK__Attendanc__Refer__5F891AA4] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK__Attendanc__Sched__5E94F66B] FOREIGN KEY ([ScheduleMasterID]) REFERENCES [dbo].[ScheduleMasters] ([ScheduleID]),
    CONSTRAINT [FK_AttendanceMaster_AttendanceMaster] FOREIGN KEY ([AttendanceMasterID]) REFERENCES [dbo].[AttendanceMaster] ([AttendanceMasterID])
);

