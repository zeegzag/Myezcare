
CREATE TABLE [dbo].[EmployeeContactMappings](
	[ContactMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[EmployeeID] [bigint] NOT NULL,
	[ClientID] [bigint] NOT NULL,
	[IsEmergencyContact] [bit] NOT NULL,
	[ContactTypeID] [bigint] NULL,
	[IsPrimaryPlacementLegalGuardian] [bit] NOT NULL,
	[IsDCSLegalGuardian] [bit] NOT NULL,
	[ROIExpireDate] [date] NULL,
	[ROIType] [varchar](40) NULL,
	[Relation] [varchar](50) NULL,
	[IsNoticeProviderOnFile] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[SystemID] [varchar](100) NOT NULL,
 CONSTRAINT [PK_EmployeeContactMappings] PRIMARY KEY CLUSTERED 
(
	[ContactMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeeContactMappings] ADD  CONSTRAINT [DF_EmployeeContactMappings_IsEmergencyContact]  DEFAULT ((0)) FOR [IsEmergencyContact]
GO

ALTER TABLE [dbo].[EmployeeContactMappings] ADD  CONSTRAINT [DF_EmployeeContactMappings_IsPrimaryPlacementLegalGuardian]  DEFAULT ((0)) FOR [IsPrimaryPlacementLegalGuardian]
GO

ALTER TABLE [dbo].[EmployeeContactMappings] ADD  CONSTRAINT [DF_EmployeeContactMappings_IsDCSLegalGuardian]  DEFAULT ((0)) FOR [IsDCSLegalGuardian]
GO

ALTER TABLE [dbo].[EmployeeContactMappings] ADD  CONSTRAINT [DF_EmployeeContactMappings_IsNoticeProviderOnFile]  DEFAULT ((0)) FOR [IsNoticeProviderOnFile]
GO

ALTER TABLE [dbo].[EmployeeContactMappings]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeContactMappings_Contacts] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contacts] ([ContactID])
GO

ALTER TABLE [dbo].[EmployeeContactMappings] CHECK CONSTRAINT [FK_EmployeeContactMappings_Contacts]
GO

ALTER TABLE [dbo].[EmployeeContactMappings]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeContactMappings_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO

ALTER TABLE [dbo].[EmployeeContactMappings] CHECK CONSTRAINT [FK_EmployeeContactMappings_Employees]
GO