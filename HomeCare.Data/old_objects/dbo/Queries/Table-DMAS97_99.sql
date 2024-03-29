
CREATE TABLE [dbo].[DMAS97AB](
	[Dmas97ID] [bigint] IDENTITY(1,1) NOT NULL,
	[JsonData] [nvarchar](max) NULL,
	[EmployeeID] [bigint] NULL,
	[ReferralID] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_DMAS97AB] PRIMARY KEY CLUSTERED 
(
	[Dmas97ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[DMAS99](
	[Dmas99ID] [bigint] IDENTITY(1,1) NOT NULL,
	[JsonData] [nvarchar](max) NULL,
	[EmployeeID] [bigint] NULL,
	[ReferralID] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
 CONSTRAINT [PK_DMAS99] PRIMARY KEY CLUSTERED 
(
	[Dmas99ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[DMAS97AB] ADD  CONSTRAINT [DF_DMAS97AB_EmployeeID]  DEFAULT ((0)) FOR [EmployeeID]
GO
ALTER TABLE [dbo].[DMAS97AB] ADD  CONSTRAINT [DF_DMAS97AB_ReferralID]  DEFAULT ((0)) FOR [ReferralID]
GO
ALTER TABLE [dbo].[DMAS97AB] ADD  CONSTRAINT [DF_DMAS97AB_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[DMAS99] ADD  CONSTRAINT [DF_DMAS99_EmployeeID]  DEFAULT ((0)) FOR [EmployeeID]
GO
ALTER TABLE [dbo].[DMAS99] ADD  CONSTRAINT [DF_DMAS99_ReferralID]  DEFAULT ((0)) FOR [ReferralID]
GO
ALTER TABLE [dbo].[DMAS99] ADD  CONSTRAINT [DF_DMAS99_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
