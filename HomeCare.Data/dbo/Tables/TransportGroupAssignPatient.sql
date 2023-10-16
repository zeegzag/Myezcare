

CREATE TABLE [fleet].[TransportGroupAssignPatient](
	[TransportGroupAssignPatientID] [bigint] IDENTITY(1,1) NOT NULL,
	[TransportGroupID] [bigint] NOT NULL,
	[ReferralID] [bigint] NOT NULL,
	[Note] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[IsBillable] [bit] NULL,
 CONSTRAINT [PK_TransportGroupAssignPatient] PRIMARY KEY CLUSTERED 
(
	[TransportGroupAssignPatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [fleet].[TransportGroupAssignPatient]  WITH CHECK ADD  CONSTRAINT [FK_TransportGroupAssignPatient_TransportGroup] FOREIGN KEY([TransportGroupID])
REFERENCES [fleet].[TransportGroup] ([TransportGroupID])
GO

ALTER TABLE [fleet].[TransportGroupAssignPatient] CHECK CONSTRAINT [FK_TransportGroupAssignPatient_TransportGroup]
GO

ALTER TABLE [fleet].[TransportGroupAssignPatient]  WITH CHECK ADD  CONSTRAINT [FK_TransportGroupAssignPatient_TransportGroup2] FOREIGN KEY([TransportGroupID])
REFERENCES [fleet].[TransportGroup] ([TransportGroupID])
GO

ALTER TABLE [fleet].[TransportGroupAssignPatient] CHECK CONSTRAINT [FK_TransportGroupAssignPatient_TransportGroup2]
GO

