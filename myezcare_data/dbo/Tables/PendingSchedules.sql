CREATE TABLE [dbo].[PendingSchedules] (
    [PendingScheduleID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReferralID]        BIGINT         NOT NULL,
    [EmployeeID]        BIGINT         NOT NULL,
    [ClockInTime]       DATETIME       NOT NULL,
    [ClockOutTime]      DATETIME       NULL,
    [CreatedBy]         BIGINT         NULL,
    [CreatedDate]       DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [UpdatedDate]       DATETIME       NULL,
    [SystemID]          NVARCHAR (100) NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_PendingSchedules_IsDeleted] DEFAULT ((0)) NOT NULL,
    [DayNumber]         INT            NULL,
    [ScheduleID]        BIGINT         NULL,
    CONSTRAINT [PK_PendingSchedules] PRIMARY KEY CLUSTERED ([PendingScheduleID] ASC)
);

