
CREATE TABLE [dbo].[Notes_Temporary](
	[NoteID] [bigint] IDENTITY(1,1) NOT NULL,
	[ReferralID] [bigint] NOT NULL,
	[AHCCCSID] [varchar](25) NULL,
	[CISNumber] [varchar](15) NULL,
	[ContinuedDX] [varchar](max) NULL,
	[ServiceDate] [date] NOT NULL,
	[ServiceCodeID] [bigint] NULL,
	[ServiceCode] [varchar](50) NULL,
	[ServiceName] [varchar](100) NULL,
	[Description] [varchar](500) NULL,
	[MaxUnit] [int] NULL,
	[DailyUnitLimit] [int] NULL,
	[UnitType] [int] NULL,
	[PerUnitQuantity] [decimal](18, 0) NULL,
	[ServiceCodeType] [int] NOT NULL,
	[ServiceCodeStartDate] [date] NULL,
	[ServiceCodeEndDate] [date] NULL,
	[CheckRespiteHours] [bit] NULL,
	[ModifierID] [varchar](500) NULL,
	[PosID] [bigint] NULL,
	[Rate] [decimal](18, 2) NULL,
	[POSStartDate] [date] NULL,
	[POSEndDate] [date] NULL,
	[ZarephathService] [varchar](50) NULL,
	[StartMile] [bigint] NULL,
	[EndMile] [bigint] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[CalculatedUnit] [float] NULL,
	[NoteDetails] [varchar](max) NULL,
	[Assessment] [varchar](max) NULL,
	[ActionPlan] [varchar](max) NULL,
	[SpokeTo] [varchar](max) NULL,
	[Relation] [varchar](50) NULL,
	[OtherNoteType] [varchar](50) NULL,
	[MarkAsComplete] [bit] NOT NULL,
	[SignatureDate] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[SystemID] [varchar](100) NULL,
	[IssueID] [bigint] NULL,
	[IssueAssignID] [bigint] NULL,
	[POSDetail] [varchar](100) NULL,
	[IsBillable] [bit] NOT NULL,
	[HasGroupOption] [bit] NOT NULL,
	[PayorServiceCodeMappingID] [bigint] NULL,
	[PayorID] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[NoOfStops] [int] NULL,
	[Source] [varchar](max) NULL,
	[RenderingProviderID] [bigint] NULL,
	[BillingProviderID] [bigint] NULL,
	[BillingProviderName] [varchar](100) NULL,
	[BillingProviderAddress] [varchar](200) NULL,
	[BillingProviderCity] [varchar](100) NULL,
	[BillingProviderState] [varchar](100) NULL,
	[BillingProviderZipcode] [varchar](50) NULL,
	[BillingProviderEIN] [varchar](10) NULL,
	[BillingProviderNPI] [varchar](20) NULL,
	[BillingProviderGSA] [int] NULL,
	[BillingProviderAHCCCSID] [varchar](20) NULL,
	[RenderingProviderName] [varchar](100) NULL,
	[RenderingProviderAddress] [varchar](200) NULL,
	[RenderingProviderCity] [varchar](100) NULL,
	[RenderingProviderState] [varchar](100) NULL,
	[RenderingProviderZipcode] [varchar](50) NULL,
	[RenderingProviderEIN] [varchar](10) NULL,
	[RenderingProviderNPI] [varchar](20) NULL,
	[RenderingProviderGSA] [int] NULL,
	[RenderingProviderAHCCCSID] [varchar](20) NULL,
	[PayorName] [varchar](50) NULL,
	[PayorShortName] [varchar](50) NULL,
	[PayorAddress] [varchar](200) NULL,
	[PayorIdentificationNumber] [varchar](50) NULL,
	[PayorCity] [varchar](50) NULL,
	[PayorState] [varchar](50) NULL,
	[PayorZipcode] [varchar](20) NULL,
	[CalculatedAmount] [float] NULL,
	[AttachmentURL] [varchar](max) NULL,
	[RandomGroupID] [varchar](100) NULL,
	[DriverID] [bigint] NULL,
	[VehicleNumber] [varchar](100) NULL,
	[VehicleType] [varchar](100) NULL,
	[PickUpAddress] [varchar](200) NULL,
	[DropOffAddress] [varchar](200) NULL,
	[RoundTrip] [bit] NOT NULL,
	[OneWay] [bit] NOT NULL,
	[MultiStops] [bit] NOT NULL,
	[EscortName] [varchar](100) NULL,
	[Relationship] [varchar](100) NULL,
	[DTRIsOnline] [bit] NOT NULL,
	[GroupIDForMileServices] [varchar](100) NULL,
	[NoteComments] [varchar](max) NULL,
	[NoteAssignee] [bigint] NULL,
	[NoteAssignedBy] [bigint] NULL,
	[NoteAssignedDate] [datetime] NULL,
	[MonthlySummaryIds] [varchar](500) NULL,
	[GroupID] [bigint] NULL,
	[ParentID] [bigint] NULL,
	[CalculatedServiceTime] [bigint] NULL,
	[RenderingProviderFirstName] [varchar](100) NULL,
	[BillingProviderFirstName] [varchar](100) NULL,
	[EmployeeVisitID] [bigint] NULL,
	[EmployeeVisitNoteIDs] [nvarchar](2000) NULL,
	[ScheduleID] [bigint] NULL,
	[ReferralTSDateID] [bigint] NULL,
	[ReferralBillingAuthorizationID] [bigint] NULL,
	[ReferralTimeSlotDetailID] [bigint] NULL,
	[RenderingProvider_TaxonomyCode] NVARCHAR(100) NULL, 
	SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName  VARCHAR(100),
SupervisingProvidername2420DLoop_NM104_NameFirst  VARCHAR(100),
SupervisingProvidername2420DLoop_REF02_ReferenceId  VARCHAR(100),

    CONSTRAINT [PK_NoteID] PRIMARY KEY CLUSTERED 
(
	[NoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  CONSTRAINT [DF_Notes_Temporary_MarkAsComplete]  DEFAULT ((0)) FOR [MarkAsComplete]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  CONSTRAINT [DF_Notes_Temporary_IsBillable]  DEFAULT ((0)) FOR [IsBillable]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  CONSTRAINT [DF_Notes_Temporary_HasGroupOption]  DEFAULT ((0)) FOR [HasGroupOption]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  CONSTRAINT [DF_Notes_Temporary_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  DEFAULT ((0)) FOR [RoundTrip]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  DEFAULT ((0)) FOR [OneWay]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  DEFAULT ((0)) FOR [MultiStops]
GO

ALTER TABLE [dbo].[Notes_Temporary] ADD  DEFAULT ((0)) FOR [DTRIsOnline]
GO


