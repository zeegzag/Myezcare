/****** Object:  Table [dbo].[LatestERAs]    Script Date: 3/5/2020 1:49:30 AM ******/
CREATE TABLE [dbo].[LatestERAs](
	[LatestEraID] [bigint] IDENTITY(1,1) NOT NULL,
	[CheckNumber] [nvarchar](100) NULL,
	[CheckType] [nvarchar](100) NULL,
	[ClaimProviderName] [nvarchar](200) NULL,
	[DownTime] [nvarchar](50) NULL,
	[EraID] [nvarchar](100) NULL,
	[PaidAmount] [decimal](18, 0) NULL,
	[PaidDate] [nvarchar](10) NULL,
	[PayerName] [nvarchar](100) NULL,
	[PayerID] [nvarchar](50) NULL,
	[ProviderName] [nvarchar](150) NULL,
	[ProviderNPI] [nvarchar](100) NULL,
	[ProviderTaxID] [nvarchar](100) NULL,
	[RecievedTime] [datetime] NULL,
	[Source] [nvarchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[SystemID] [varchar](100) NULL,
 CONSTRAINT [PK_LatestERAs] PRIMARY KEY CLUSTERED 
(
	[LatestEraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]