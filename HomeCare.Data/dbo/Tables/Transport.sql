
CREATE TABLE [dbo].[Transport](
	[TransportID] [bigint] IDENTITY(1,1) NOT NULL,
	[FacilityID] [bigint] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Attendent] [nvarchar](max) NULL,
	[VehicleID] [bigint] NULL,
	[OrganizationID] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NULL,
	[RouteCode] [bigint] NOT NULL,
 CONSTRAINT [PK_Transport] PRIMARY KEY CLUSTERED 
(
	[TransportID] ASC
)
)