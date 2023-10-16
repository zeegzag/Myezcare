CREATE TABLE [dbo].[Vehicles](
	[VehicleID] [bigint] IDENTITY(1,1) NOT NULL,
	[VIN_Number] [nvarchar](max) NULL,
	[SeatingCapacity] [bigint] NULL,
	[VehicleType] [nvarchar](max) NULL,
	[BrandName] [nvarchar](max) NULL,
	[Model] [nvarchar](max) NULL,
	[Color] [nvarchar](max) NULL,
	[Attendent] [nvarchar](max) NULL,
	[ContactID] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[SystemID] [nvarchar](max) NULL,
	[note] [nvarchar](255) NULL,
	[EmployeeID] [bigint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

