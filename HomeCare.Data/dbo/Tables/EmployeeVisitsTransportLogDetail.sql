CREATE TABLE [dbo].[EmployeeVisitsTransportLogDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeVisitsTransportLogId] [bigint] NOT NULL,
	[ReferralID] [bigint] NOT NULL,
	[ClockInTime] [datetime] NULL,
	[ClockOutTime] [datetime] NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_EmployeeVisitsTransportLogDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLogDetail]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeVisitsTransportLog_EmployeeVisitsTransportLog] FOREIGN KEY([EmployeeVisitsTransportLogId])
REFERENCES [dbo].[EmployeeVisitsTransportLog] ([Id])
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLogDetail] CHECK CONSTRAINT [FK_EmployeeVisitsTransportLog_EmployeeVisitsTransportLog]
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLogDetail]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeVisitsTransportLog_Referrals] FOREIGN KEY([ReferralID])
REFERENCES [dbo].[Referrals] ([ReferralID])
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLogDetail] CHECK CONSTRAINT [FK_EmployeeVisitsTransportLog_Referrals]
GO