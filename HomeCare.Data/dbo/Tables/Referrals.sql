

CREATE TABLE [dbo].[Referrals](
	[ReferralID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](10) NULL,
	[FirstName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[ClientNickName] [varchar](100) NULL,
	[Dob] [date] NULL,
	[Gender] [char](1) NULL,
	[RecordRequestEmail] [varchar](max) NULL,
	[LanguageID] [bigint] NULL,
	[ClientNumber] [varchar](15) NULL,
	[AHCCCSID] [varchar](10) NULL,
	[CISNumber] [varchar](15) NULL,
	[Population] [varchar](50) NULL,
	[HealthPlan] [varchar](500) NULL,
	[RateCode] [varchar](100) NULL,
	[RateCodeStartDate] [date] NULL,
	[RateCodeEndDate] [date] NULL,
	[AHCCCSEnrollDate] [date] NULL,
	[PlacementRequirement] [varchar](max) NULL,
	[BehavioralIssue] [varchar](max) NULL,
	[OtherInformation] [varchar](max) NULL,
	[AgencyID] [bigint] NULL,
	[AgencyLocationID] [bigint] NULL,
	[CaseManagerID] [bigint] NULL,
	[FirstDOS] [date] NULL,
	[ReferralDate] [date] NULL,
	[ClosureDate] [date] NULL,
	[ClosureReason] [varchar](500) NULL,
	[CareConsent] [bit] NULL,
	[SelfAdministrationofMedication] [bit] NULL,
	[HealthInformationDisclosure] [bit] NULL,
	[AdmissionRequirements] [bit] NULL,
	[AdmissionOrientation] [bit] NULL,
	[ZarephathCrisisPlan] [varchar](2) NULL,
	[NetworkCrisisPlan] [varchar](5) NULL,
	[NCPExpirationDate] [datetime] NULL,
	[PermissionForVoiceMail] [bit] NULL,
	[PermissionForEmail] [bit] NULL,
	[PermissionForSMS] [bit] NULL,
	[PermissionForMail] [bit] NULL,
	[AROI] [varchar](5) NULL,
	[AROIAgencyID] [bigint] NULL,
	[AROIExpirationDate] [date] NULL,
	[PHI] [bit] NULL,
	[PHIAgencyID] [bigint] NULL,
	[PHIExpirationDate] [date] NULL,
	[RespiteService] [bit] NULL,
	[ZSPRespite] [bit] NULL,
	[ZSPRespiteExpirationDate] [date] NULL,
	[ZSPRespiteGuardianSignature] [bit] NULL,
	[ZSPRespiteBHPSigned] [bit] NULL,
	[LifeSkillsService] [bit] NULL,
	[ZSPLifeSkills] [bit] NULL,
	[ZSPLifeSkillsExpirationDate] [date] NULL,
	[ZSPLifeSkillsGuardianSignature] [bit] NULL,
	[ZSPLifeSkillsBHPSigned] [bit] NULL,
	[CounselingService] [bit] NULL,
	[ZSPCounselling] [bit] NULL,
	[ZSPCounsellingExpirationDate] [date] NULL,
	[ZSPCounsellingGuardianSignature] [bit] NULL,
	[ZSPCounsellingBHPSigned] [bit] NULL,
	[NetworkServicePlan] [bit] NULL,
	[NSPExpirationDate] [date] NULL,
	[NSPGuardianSignature] [bit] NULL,
	[NSPBHPSigned] [bit] NULL,
	[NSPSPidentifyService] [varchar](2) NULL,
	[BXAssessment] [bit] NULL,
	[BXAssessmentExpirationDate] [date] NULL,
	[BXAssessmentBHPSigned] [bit] NULL,
	[Demographic] [varchar](5) NULL,
	[DemographicExpirationDate] [date] NULL,
	[SNCD] [varchar](5) NULL,
	[SNCDExpirationDate] [date] NULL,
	[ACAssessment] [bit] NULL,
	[ACAssessmentExpirationDate] [date] NULL,
	[IsCheckListCompleted] [bit] NULL,
	[IsSparFormCompleted] [bit] NULL,
	[IsSaveAsDraft] [bit] NULL,
	[ReferralStatusID] [bigint] NULL,
	[Assignee] [bigint] NULL,
	[DropOffLocation] [bigint] NULL,
	[PickUpLocation] [bigint] NULL,
	[NeedPrivateRoom] [bit] NULL,
	[OrientationDate] [date] NULL,
	[FrequencyCodeID] [bigint] NULL,
	[ClientID] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[RegionID] [bigint] NULL,
	[NotifyCaseManager] [bit] NULL,
	[LastAttendedDate] [date] NULL,
	[ReferralSourceID] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NOT NULL,
	[SystemID] [varchar](100) NOT NULL,
	[NotifyCaseManagerDate] [date] NULL,
	[ScheduleRequestDates] [varchar](max) NULL,
	[ReferralTrackingComment] [varchar](500) NULL,
	[Caseload] [bigint] NULL,
	[MonthlySummaryEmail] [varchar](max) NULL,
	[ConnectingFamiliesService] [bit] NULL,
	[ZSPConnectingFamilies] [bit] NULL,
	[ZSPConnectingFamiliesExpirationDate] [date] NULL,
	[ZSPConnectingFamiliesGuardianSignature] [bit] NULL,
	[ZSPConnectingFamiliesBHPSigned] [bit] NULL,
	[PCMVoiceMail] [bit] NULL,
	[PCMEmail] [bit] NULL,
	[PCMSMS] [bit] NULL,
	[PCMMail] [bit] NULL,
	[ReferralLSTMCaseloadsComment] [varchar](500) NULL,
	[PolicyNumber] [varchar](100) NULL,
	[CASIIScore] [varchar](100) NULL,
	[ImportHours] [varchar](50) NULL,
	[MondaySchedule] [bit] NOT NULL,
	[TuesdaySchedule] [bit] NOT NULL,
	[WednesdaySchedule] [bit] NOT NULL,
	[ThursdaySchedule] [bit] NOT NULL,
	[FridaySchedule] [bit] NOT NULL,
	[SaturdaySchedule] [bit] NOT NULL,
	[SundaySchedule] [bit] NOT NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](100) NULL,
	[PasswordSalt] [varchar](max) NULL,
	[SignatureNeeded] [bit] NOT NULL,
	[PhysicianID] [bigint] NULL,
	[CareTypeIds] [varchar](500) NULL,
	[ProfileImagePath] [varchar](300) NULL,
	[TempPatientID] [bigint] NULL,
	[ProfessionalAuthrizationCode] [nvarchar](50) NULL,
	[InstitutionalAuthrizationCode] [nvarchar](50) NULL,
	[RoleID] [bigint] NULL,
	[DischargeComment] [nvarchar](max) NULL,
	[DischargeDate] [date] NULL,
	[DefaultFacilityID] [bigint] NULL,
	[BeneficiaryType] [int] NULL,
	[ServiceType] [varchar](50) NULL,
	[SocialSecurityNumber] [varchar](max) NULL,
	[GroupIDs] [nvarchar](max) NULL,
	[ReferralTrackingID] [bigint] NULL,
	[BloodGroup] [varchar](50) NULL,
	[Height] [varchar](50) NULL,
	[Weight] [varchar](50) NULL,
	[Ethnicity] [varchar](255) NULL,
	[Race] [varchar](max) NULL,
	[CaregiverStatus] [varchar](max) NULL,
	[CodeStatus] [bigint] NULL,
	[BMI] [varchar](max) NULL,
	[BMIValue] [nvarchar](max) NULL,
	[SandataLastSent] [datetime] NULL,
	[IsBillable] [bit] NULL,
 CONSTRAINT [PK_Referrals] PRIMARY KEY CLUSTERED 
(
	[ReferralID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Referrals] ADD  CONSTRAINT [DF_Referrals_PermissionForMail]  DEFAULT ((0)) FOR [PermissionForMail]
GO

ALTER TABLE [dbo].[Referrals] ADD  CONSTRAINT [DF_Referrals_IsSaveAsDraft]  DEFAULT ((0)) FOR [IsSaveAsDraft]
GO

ALTER TABLE [dbo].[Referrals] ADD  CONSTRAINT [DF_Referrals_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Referrals] ADD  CONSTRAINT [DF_Referrals_NotifyCaseManager]  DEFAULT ((0)) FOR [NotifyCaseManager]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [MondaySchedule]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [TuesdaySchedule]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [WednesdaySchedule]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [ThursdaySchedule]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [FridaySchedule]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [SaturdaySchedule]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [SundaySchedule]
GO

ALTER TABLE [dbo].[Referrals] ADD  CONSTRAINT [DF__Referrals__Signa__270FB757]  DEFAULT ((0)) FOR [SignatureNeeded]
GO

ALTER TABLE [dbo].[Referrals] ADD  DEFAULT ((0)) FOR [IsBillable]
GO

ALTER TABLE [dbo].[Referrals]  WITH CHECK ADD  CONSTRAINT [FK_ReferralCasload_Employee] FOREIGN KEY([Caseload])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO

ALTER TABLE [dbo].[Referrals] CHECK CONSTRAINT [FK_ReferralCasload_Employee]
GO


