CREATE TABLE [dbo].[ReferralActivityNotes](
	[ReferralActivityNoteId] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralActivityMasterId] [int] NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Initials] [nvarchar](500) NULL,
	[CreatedBy] [int] NULL) 


