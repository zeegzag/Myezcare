USE [Kundan_Admin]
GO

/****** Object:  Table [dbo].[OrganizationPermission]    Script Date: 2/19/2020 10:47:44 PM ******/
--Created by Satya
--Purpose : For mapping between Organisation and Permission

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrganizationPermission](
	[OrgPermissionId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [bigint] NULL,
	[PermissionId] [bigint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK__Organiza__53C5598E1264DDEA] PRIMARY KEY CLUSTERED 
(
	[OrgPermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrganizationPermission] ADD  CONSTRAINT [DF_OrganizationPermission_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


