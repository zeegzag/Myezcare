CREATE TABLE [dbo].[ReferralActivityMaster](
	[ReferralActivityMasterId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralId] [int] NULL,
	[Month] [varchar](50) NULL,
	[Year] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL) 



