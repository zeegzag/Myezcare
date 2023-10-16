CREATE TABLE [dbo].[BatchUploadedClaimMessages](
	[BatchUploadedClaimMessageID] [bigint] IDENTITY(1,1) NOT NULL,
	[LastResponseID] [nvarchar](max) NULL,
	[BatchID] [nvarchar](max) NULL,
	[Bill_NPI] [nvarchar](max) NULL,
	[Bill_TaxID] [nvarchar](max) NULL,
	[ClaimID] [nvarchar](max) NULL,
	[ClaimMD_ID] [nvarchar](max) NULL,
	[FDOS] [nvarchar](max) NULL,
	[FileID] [nvarchar](max) NULL,
	[FileName] [nvarchar](max) NULL,
	[INS_Number] [nvarchar](max) NULL,
	[PayerID] [nvarchar](max) NULL,
	[PCN] [nvarchar](max) NULL,
	[Remote_ClaimID] [nvarchar](max) NULL,
	[Sender_ICN] [nvarchar](max) NULL,
	[Sender_Name] [nvarchar](max) NULL,
	[SenderID] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Total_Charge] [nvarchar](max) NULL,
	[Messages] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_BatchUploadedClaimMessages] PRIMARY KEY CLUSTERED 
(
	[BatchUploadedClaimMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]