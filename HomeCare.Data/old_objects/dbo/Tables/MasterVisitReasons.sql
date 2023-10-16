CREATE TABLE [dbo].[MasterVisitReasons](
	[MasterVisitReasonID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](12) NULL,
	[Code] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CompanyName] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


