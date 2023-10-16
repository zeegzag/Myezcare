CREATE TABLE [dbo].[EmployeeVisitsTransportLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [bigint] NOT NULL,
	[TransportGroupID] [bigint] NULL,
	[TransportAssignPatientID] [bigint] NULL,
	[VisitDate] [datetime] NULL,
	[Starttime] [datetime] NULL,
	[Endtime] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_EmployeeVisitsTransportLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLog]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeVisitsTransportLog_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLog] CHECK CONSTRAINT [FK_EmployeeVisitsTransportLog_Employees]
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLog]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeVisitsTransportLog_TransportAssignPatient] FOREIGN KEY([TransportAssignPatientID])
REFERENCES [dbo].[TransportAssignPatient] ([TransportAssignPatientID])
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLog] CHECK CONSTRAINT [FK_EmployeeVisitsTransportLog_TransportAssignPatient]
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLog]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeVisitsTransportLog_TransportGroup] FOREIGN KEY([TransportGroupID])
REFERENCES [fleet].[TransportGroup] ([TransportGroupID])
GO

ALTER TABLE [dbo].[EmployeeVisitsTransportLog] CHECK CONSTRAINT [FK_EmployeeVisitsTransportLog_TransportGroup]
GO