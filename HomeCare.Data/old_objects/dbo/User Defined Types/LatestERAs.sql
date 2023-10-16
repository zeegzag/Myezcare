GO
/****** Object:  UserDefinedTableType [dbo].[LatestERAs]    Script Date: 3/5/2020 1:49:30 AM ******/
CREATE TYPE [dbo].[LatestERAs] AS TABLE(
	[CheckNumber] [nvarchar](100) NULL,
	[CheckType] [nvarchar](50) NULL,
	[ClaimProviderName] [nvarchar](200) NULL,
	[DownTime] [nvarchar](50) NULL,
	[EraID] [nvarchar](100) NULL,
	[PaidAmount] [decimal](18, 2) NULL,
	[PaidDate] [nvarchar](10) NULL,
	[PayerName] [nvarchar](100) NULL,
	[PayerID] [nvarchar](50) NULL,
	[ProviderName] [nvarchar](150) NULL,
	[ProviderNPI] [nvarchar](100) NULL,
	[ProviderTaxID] [nvarchar](100) NULL,
	[RecievedTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL
)