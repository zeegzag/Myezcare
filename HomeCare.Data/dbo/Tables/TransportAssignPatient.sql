

CREATE TABLE [dbo].[TransportAssignPatient](
	[TransportAssignPatientID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralID] [bigint] NOT NULL,
	[TransportID] [bigint] NOT NULL,
	[Startdate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Note] [nvarchar](255) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[IsBillable] [bit] NULL,
 CONSTRAINT [PK_TransportAssignPatient] PRIMARY KEY CLUSTERED 
(
	[TransportAssignPatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TransportAssignPatient]  WITH CHECK ADD  CONSTRAINT [FK_TransportAssignPatient_Referrals] FOREIGN KEY([ReferralID])
REFERENCES [dbo].[Referrals] ([ReferralID])
GO

ALTER TABLE [dbo].[TransportAssignPatient] CHECK CONSTRAINT [FK_TransportAssignPatient_Referrals]
GO

ALTER TABLE [dbo].[TransportAssignPatient]  WITH CHECK ADD  CONSTRAINT [FK_TransportAssignPatient_Transport] FOREIGN KEY([TransportID])
REFERENCES [dbo].[Transport] ([TransportID])
GO

ALTER TABLE [dbo].[TransportAssignPatient] CHECK CONSTRAINT [FK_TransportAssignPatient_Transport]
GO


