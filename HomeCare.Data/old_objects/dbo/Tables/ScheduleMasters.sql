CREATE TABLE [dbo].[ScheduleMasters] (
    [ScheduleID]                          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReferralID]                          BIGINT         NULL,
    [FacilityID]                          BIGINT         NULL,
    [StartDate]                           DATETIME       NULL,
    [EndDate]                             DATETIME       NULL,
    [PickUpLocation]                      BIGINT         NULL,
    [DropOffLocation]                     BIGINT         NULL,
    [ScheduleStatusID]                    BIGINT         NOT NULL,
    [Comments]                            VARCHAR (500)  NULL,
    [IsAssignedToTransportationGroupDown] BIT            NULL,
    [IsAssignedToTransportationGroupUp]   BIT            NULL,
    [CreatedBy]                           BIGINT         NULL,
    [CreatedDate]                         DATETIME       NULL,
    [UpdatedBy]                           BIGINT         NULL,
    [UpdatedDate]                         DATETIME       NULL,
    [SystemID]                            VARCHAR (100)  NULL,
    [IsDeleted]                           BIT            NULL,
    [WhoCancelled]                        VARCHAR (50)   NULL,
    [WhenCancelled]                       DATE           NULL,
    [CancelReason]                        VARCHAR (MAX)  NULL,
    [IsReschedule]                        BIT            NULL,
    [WeekEmailDate]                       DATETIME       NULL,
    [WeekSMSDate]                         DATETIME       NULL,
    [WeekMasterID]                        BIGINT         NULL,
    [EmailSent]                           BIT            CONSTRAINT [DF__ScheduleM__Email__55209ACA] DEFAULT ((0)) NULL,
    [SMSSent]                             BIT            CONSTRAINT [DF__ScheduleM__SMSSe__5614BF03] DEFAULT ((0)) NULL,
    [NoticeSent]                          BIT            CONSTRAINT [DF__ScheduleM__Notic__5708E33C] DEFAULT ((0)) NULL,
    [EmployeeID]                          BIGINT         NULL,
    [EmployeeTSDateID]                    BIGINT         NULL,
    [ReferralTSDateID]                    BIGINT         NULL,
    [PayorID]                             BIGINT         NULL,
    [ServiceCodeID]                       BIGINT         NULL,
    [IsPatientAttendedSchedule]           BIT            NULL,
    [AbsentReason]                        NVARCHAR (MAX) NULL,
    [VisitType]                           INT            NULL,
    [CareTypeTimeSlotID]                  BIGINT         NULL,
    [CareTypeId]                          VARCHAR (100)  NULL,
	[ReferralBillingAuthorizationID]	  BIGINT		 NULL,
    [StartTime] TIME NULL, 
    [EndTime] TIME NULL, 
	[AnyTimeClockIn] BIT NULL,
	[IsVirtualVisit] BIT NULL,
    CONSTRAINT [PK_ScheduleMasters_1] PRIMARY KEY CLUSTERED ([ScheduleID] ASC),
    CONSTRAINT [FK_ScheduleMasters_Referrals] FOREIGN KEY ([ReferralID]) REFERENCES [dbo].[Referrals] ([ReferralID]),
    CONSTRAINT [FK_ScheduleMasters_ScheduleStatuses] FOREIGN KEY ([ScheduleStatusID]) REFERENCES [dbo].[ScheduleStatuses] ([ScheduleStatusID]),
    CONSTRAINT [FK_ScheduleMasters_TransportLocations] FOREIGN KEY ([PickUpLocation]) REFERENCES [dbo].[TransportLocations] ([TransportLocationID]),
    CONSTRAINT [FK_ScheduleMasters_TransportLocations1] FOREIGN KEY ([DropOffLocation]) REFERENCES [dbo].[TransportLocations] ([TransportLocationID]),
    CONSTRAINT [FK_ScheduleMasters_WeekMasters] FOREIGN KEY ([WeekMasterID]) REFERENCES [dbo].[WeekMasters] ([WeekMasterID])
);


GO
CREATE NONCLUSTERED INDEX [missing_index_10_9_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([StartDate], [EndDate], [ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_137_136_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [ReferralTSDateID] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_14_13_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [EmployeeTSDateID] ASC)
    INCLUDE([StartDate], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_141_140_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_143_142_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [EmployeeID] ASC, [EmployeeTSDateID] ASC, [ReferralTSDateID] ASC)
    INCLUDE([ReferralID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_145_144_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [ReferralID] ASC, [EmployeeTSDateID] ASC, [ReferralTSDateID] ASC)
    INCLUDE([EmployeeID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_147_146_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [EmployeeTSDateID] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_152_151_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([EmployeeTSDateID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_16_15_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([StartDate], [EndDate], [EmployeeTSDateID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_19_18_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [EmployeeID] ASC)
    INCLUDE([StartDate], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_2_1_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([ReferralID] ASC, [IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [missing_index_21_20_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([StartDate], [EndDate], [EmployeeID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_27_26_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([ScheduleStatusID] ASC, [IsDeleted] ASC, [EmployeeID] ASC, [StartDate] ASC)
    INCLUDE([ReferralID], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_31_30_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([ReferralID] ASC, [IsDeleted] ASC)
    INCLUDE([ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_33_32_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([ReferralID], [ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_37_36_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [ReferralTSDateID] ASC)
    INCLUDE([StartDate], [EndDate], [ScheduleStatusID], [EmployeeID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_39_38_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([StartDate], [EndDate], [ScheduleStatusID], [EmployeeID], [ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_4_3_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([ReferralID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_59_58_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([ReferralID], [StartDate], [EndDate], [EmployeeID]);


GO
CREATE NONCLUSTERED INDEX [missing_index_6_5_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [ReferralTSDateID] ASC)
    INCLUDE([StartDate], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [missing_index_828_827_ScheduleMasters]
    ON [dbo].[ScheduleMasters]([EmployeeID] ASC)
    INCLUDE([ReferralID]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_IsDeleted_DB88C]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC)
    INCLUDE([ReferralID], [StartDate], [EndDate], [EmployeeID], [ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_IsDeleted_StartDate_EndDate_63584]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [StartDate] ASC, [EndDate] ASC)
    INCLUDE([EmployeeID]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_ReferralID_StartDate_EndDate_E3569]
    ON [dbo].[ScheduleMasters]([ReferralID] ASC, [StartDate] ASC, [EndDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_StartDate_EndDate_ReferralTSDateID_CCFDF]
    ON [dbo].[ScheduleMasters]([StartDate] ASC, [EndDate] ASC, [ReferralTSDateID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_ReferralID_StartDate_296FC]
    ON [dbo].[ScheduleMasters]([ReferralID] ASC, [StartDate] ASC)
    INCLUDE([EndDate], [ScheduleStatusID], [CreatedDate], [IsDeleted], [EmployeeID], [EmployeeTSDateID], [ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_ReferralID_StartDate_C183D]
    ON [dbo].[ScheduleMasters]([ReferralID] ASC, [StartDate] ASC)
    INCLUDE([CareTypeTimeSlotID]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_ReferralID_StartDate_742F9]
    ON [dbo].[ScheduleMasters]([ReferralID] ASC, [StartDate] ASC)
    INCLUDE([EndDate], [EmployeeID]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_EmployeeID_StartDate_AA06C]
    ON [dbo].[ScheduleMasters]([EmployeeID] ASC, [StartDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_IsDeleted_EmployeeID_2121B]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [EmployeeID] ASC)
    INCLUDE([ReferralID], [StartDate], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_ScheduleStatusID_EmployeeID_StartDate_638D3]
    ON [dbo].[ScheduleMasters]([ScheduleStatusID] ASC, [EmployeeID] ASC, [StartDate] ASC)
    INCLUDE([ReferralID], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_ScheduleStatusID_EmployeeID_StartDate_IsDeleted_8759C]
    ON [dbo].[ScheduleMasters]([ScheduleStatusID] ASC, [EmployeeID] ASC, [StartDate] ASC, [IsDeleted] ASC)
    INCLUDE([ReferralID], [EndDate], [CreatedDate], [EmployeeTSDateID], [ReferralTSDateID]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_IsDeleted_EmployeeID_StartDate_3497E]
    ON [dbo].[ScheduleMasters]([IsDeleted] ASC, [EmployeeID] ASC, [StartDate] ASC)
    INCLUDE([ReferralID], [EndDate]);


GO
CREATE NONCLUSTERED INDEX [IX_ScheduleMasters_ReferralID_ScheduleStatusID_IsDeleted_5DD79]
    ON [dbo].[ScheduleMasters]([ReferralID] ASC, [ScheduleStatusID] ASC, [IsDeleted] ASC)
    INCLUDE([StartDate]);


GO
CREATE TRIGGER [dbo].[tr_ScheduleMasters_Updated] ON dbo.ScheduleMasters
FOR UPDATE AS 

INSERT INTO JO_ScheduleMasters( 
ScheduleID					   ,
ReferralID					   ,
FacilityID					   ,
StartDate					   ,
EndDate						   ,
PickUpLocation				   ,
DropOffLocation				   ,
ScheduleStatusID			   ,
Comments					   ,
IsAssignedToTransportationGroupUp,
IsAssignedToTransportationGroupDown,
CreatedBy					   ,
CreatedDate					   ,
UpdatedBy					   ,
UpdatedDate					   ,
SystemID					   ,
IsDeleted					   ,
WhoCancelled				   ,
WhenCancelled				   ,
CancelReason				   ,
IsReschedule				   ,
Action,ActionDate
)

SELECT  
ScheduleID					   ,
ReferralID					   ,
FacilityID					   ,
StartDate					   ,
EndDate						   ,
PickUpLocation				   ,
DropOffLocation				   ,
ScheduleStatusID			   ,
Comments					   ,
IsAssignedToTransportationGroupUp,
IsAssignedToTransportationGroupDown,
CreatedBy					   ,
CreatedDate					   ,
UpdatedBy					   ,
UpdatedDate					   ,
SystemID					   ,
IsDeleted					   ,
WhoCancelled				   ,
WhenCancelled				   ,
CancelReason				   ,
IsReschedule				   ,
'U',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ScheduleMasters_Updated]
    ON [dbo].[ScheduleMasters];


GO
CREATE TRIGGER [dbo].[tr_ScheduleMasters_Deleted] ON dbo.ScheduleMasters
FOR DELETE AS 

INSERT INTO JO_ScheduleMasters( 
ScheduleID					   ,
ReferralID					   ,
FacilityID					   ,
StartDate					   ,
EndDate						   ,
PickUpLocation				   ,
DropOffLocation				   ,
ScheduleStatusID			   ,
Comments					   ,
--IsAssignedToTransportationGroup,
CreatedBy					   ,
CreatedDate					   ,
UpdatedBy					   ,
UpdatedDate					   ,
SystemID					   ,
IsDeleted					   ,
WhoCancelled				   ,
WhenCancelled				   ,
CancelReason				   ,
IsReschedule				   ,
Action,ActionDate
)

SELECT  
ScheduleID					   ,
ReferralID					   ,
FacilityID					   ,
StartDate					   ,
EndDate						   ,
PickUpLocation				   ,
DropOffLocation				   ,
ScheduleStatusID			   ,
Comments					   ,
--IsAssignedToTransportationGroupUP,
CreatedBy					   ,
CreatedDate					   ,
UpdatedBy					   ,
UpdatedDate					   ,
SystemID					   ,
IsDeleted					   ,
WhoCancelled				   ,
WhenCancelled				   ,
CancelReason				   ,
IsReschedule				   ,
'D',GETUTCDATE() FROM deleted

GO
DISABLE TRIGGER [dbo].[tr_ScheduleMasters_Deleted]
    ON [dbo].[ScheduleMasters];

