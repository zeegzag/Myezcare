CREATE TABLE [dbo].[ReportPermissionMapping](
	[ReportPermissionMappingID] [bigint] IDENTITY(1,1) NOT NULL,
	[RoleID] [bigint] NOT NULL,
	[ReportID] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[SystemID] [varchar](100) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ReportPermissionMapping] PRIMARY KEY CLUSTERED 
(
	[ReportPermissionMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

Go

ALTER TABLE [dbo].[ReportPermissionMapping] ADD  CONSTRAINT [DF_ReportPermissionMapping_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ReportPermissionMapping] ADD  CONSTRAINT [DF_ReportPermissionMapping_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[ReportPermissionMapping]  WITH CHECK ADD  CONSTRAINT [FK_ReportPermissionMapping_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO

ALTER TABLE [dbo].[ReportPermissionMapping] CHECK CONSTRAINT [FK_ReportPermissionMapping_Roles]
GO