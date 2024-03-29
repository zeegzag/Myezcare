
CREATE TABLE [dbo].[BillingNote](
	[BillingNoteID] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchID] [bigint] NULL,
	[BillingNote] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_dbo.BillingNote] PRIMARY KEY CLUSTERED 
(
	[BillingNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
