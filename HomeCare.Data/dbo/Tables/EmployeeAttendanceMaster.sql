CREATE TABLE [dbo].[EmployeeAttendanceMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [bigint] NOT NULL,
	[WorkMinutes] [int] NULL,
	[FacilityID] [bigint] NULL,
	[OrganizationID] [bigint] NULL,
	[Note] [nvarchar](512) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_EmployeeAttendanceMaster] PRIMARY KEY CLUSTERED 
(
	[Id] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

