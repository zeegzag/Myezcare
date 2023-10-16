CREATE TABLE [dbo].[VisitReasons](
	[VisitReasonID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](6) NULL,
	[ReasonCode] [nvarchar](max) NULL,
	[ActionCode] [nvarchar](max) NULL,
	[ScheduleID] [bigint] NULL,
	[CompanyName] [varchar](11) NOT NULL,
	[IsDeleted] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


