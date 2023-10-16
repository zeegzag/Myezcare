
CREATE TABLE [dbo].[NurseSchedules](
	[NurseScheduleID] [bigint] IDENTITY(1,1) NOT NULL,
	[FrequencyChoice] [int] NULL,
	[Frequency] [int] NULL,
	[DaysOfWeek] [int] NULL,
	[DayOfMonth] [int] NULL,
	[IsMonthlyDaySelection] [bit] NULL,
	[DailyInterval] [int] NULL,
	[WeeklyInterval] [int] NULL,
	[MonthlyInterval] [int] NULL,
	[IsSundaySelected] [bit] NULL,
	[IsMondaySelected] [bit] NULL,
	[IsTuesdaySelected] [bit] NULL,
	[IsWednesdaySelected] [bit] NULL,
	[IsThursdaySelected] [bit] NULL,
	[IsFridaySelected] [bit] NULL,
	[IsSaturdaySelected] [bit] NULL,
	[IsFirstWeekOfMonthSelected] [bit] NULL,
	[IsSecondWeekOfMonthSelected] [bit] NULL,
	[IsThirdWeekOfMonthSelected] [bit] NULL,
	[IsFourthWeekOfMonthSelected] [bit] NULL,
	[IsLastWeekOfMonthSelected] [bit] NULL,
	[FrequencyTypeOptions] [nvarchar](max) NULL,
	[MonthlyIntervalOptions] [nvarchar](max) NULL,
	[ScheduleRecurrence] [nvarchar](max) NULL,
	[DaysOfWeekOptions] [nvarchar](max) NULL,
	[AnniversaryDay] [int] NULL,
	[AnniversaryMonth] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[Notes] [nvarchar](max) NULL,
	[IsAnyDay] [bit] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_NurseSchedules] PRIMARY KEY CLUSTERED 
(
	[NurseScheduleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[NurseSchedules] ADD  CONSTRAINT [DF_NurseSchedules_IsAnyDay]  DEFAULT ((0)) FOR [IsAnyDay]
GO

