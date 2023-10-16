CREATE TABLE [dbo].[EmployeeAttendanceDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AttendanceMasterId] [int] NOT NULL,
	[Type] [int] NULL,
	[TypeDetail] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 [Note] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_EmployeeAttendanceDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeeAttendanceDetail]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAttendanceDetail_EmployeeAttendanceMaster] FOREIGN KEY([AttendanceMasterId])
REFERENCES [dbo].[EmployeeAttendanceMaster] ([Id])
GO

ALTER TABLE [dbo].[EmployeeAttendanceDetail] CHECK CONSTRAINT [FK_EmployeeAttendanceDetail_EmployeeAttendanceMaster]
GO