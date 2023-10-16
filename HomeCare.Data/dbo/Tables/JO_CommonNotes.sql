CREATE TABLE [dbo].[JO_CommonNotes](
	[CommonNoteID] [bigint] NOT NULL,
	[EmployeeID] [bigint] NULL,
	[ReferralID] [bigint] NULL,
	[Note] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[RoleID] [nvarchar](50) NULL,
	[EmployeesID] [nvarchar](50) NULL,
	[CategoryID] [bigint] NULL,
	[Action] [char](1) NOT NULL,
	[ActionDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

