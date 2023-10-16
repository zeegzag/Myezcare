USE [Kundan_Homecare]
GO
/****** Object:  Table [dbo].[RAL_FacilityMapping]    Script Date: 1/28/2020 2:02:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RAL_FacilityMapping](
	[FacilityMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[FacilityID] [bigint] NOT NULL,
	[EmployeeID] [bigint] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_FacilityMappingRAL] PRIMARY KEY CLUSTERED 
(
	[FacilityMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[RAL_FacilityMapping] ON 

INSERT [dbo].[RAL_FacilityMapping] ([FacilityMappingID], [FacilityID], [EmployeeID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (1, 1, 1, CAST(N'2020-01-27T12:03:00.000' AS DateTime), 1, CAST(N'2020-01-27T12:03:00.000' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[RAL_FacilityMapping] OFF
ALTER TABLE [dbo].[RAL_FacilityMapping] ADD  CONSTRAINT [DF_FacilityMappingRAL_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RAL_FacilityMapping]  WITH CHECK ADD  CONSTRAINT [FK_RAL_FacilityMapping_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO
ALTER TABLE [dbo].[RAL_FacilityMapping] CHECK CONSTRAINT [FK_RAL_FacilityMapping_Employees]
GO
ALTER TABLE [dbo].[RAL_FacilityMapping]  WITH CHECK ADD  CONSTRAINT [FK_RAL_FacilityMapping_Facilities] FOREIGN KEY([FacilityID])
REFERENCES [dbo].[Facilities] ([FacilityID])
GO
ALTER TABLE [dbo].[RAL_FacilityMapping] CHECK CONSTRAINT [FK_RAL_FacilityMapping_Facilities]
GO
