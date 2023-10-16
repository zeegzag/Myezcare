CREATE TABLE [dbo].[ReferenceMaster](
	[ReferenceID] [bigint] IDENTITY(1,1) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ReferenceName] [varchar](100) NOT NULL,
	[ReferenceCode] [varchar](2) NOT NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_ReferenceMaster] PRIMARY KEY CLUSTERED 
(
	[ReferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReferenceMaster] ADD  CONSTRAINT [DF_ReferenceMaster_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO