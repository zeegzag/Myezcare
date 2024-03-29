﻿CREATE TABLE [dbo].[Referrals] (
    [ReferralID]                             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]                                  VARCHAR (10)   NULL,
    [FirstName]                              VARCHAR (50)   NULL,
    [MiddleName]                             VARCHAR (50)   NULL,
    [LastName]                               VARCHAR (50)   NULL,
    [ClientNickName]                         VARCHAR (100)  NULL,
    [Dob]                                    DATE           NULL,
    [Gender]                                 CHAR (1)       NULL,
    [RecordRequestEmail]                     VARCHAR (MAX)  NULL,
    [LanguageID]                             BIGINT         NULL,
    [ClientNumber]                           VARCHAR (15)   NULL,
    [AHCCCSID]                               VARCHAR (10)   NULL,
    [CISNumber]                              VARCHAR (15)   NULL,
    [Population]                             VARCHAR (50)   NULL,
    [HealthPlan]                             VARCHAR (500)  NULL,
    [RateCode]                               VARCHAR (100)  NULL,
    [RateCodeStartDate]                      DATE           NULL,
    [RateCodeEndDate]                        DATE           NULL,
    [AHCCCSEnrollDate]                       DATE           NULL,
    [PlacementRequirement]                   VARCHAR (MAX)  NULL,
    [BehavioralIssue]                        VARCHAR (MAX)  NULL,
    [OtherInformation]                       VARCHAR (MAX)  NULL,
    [AgencyID]                               BIGINT         NULL,
    [AgencyLocationID]                       BIGINT         NULL,
    [CaseManagerID]                          BIGINT         NULL,
    [FirstDOS]                               DATE           NULL,
    [ReferralDate]                           DATE           NULL,
    [ClosureDate]                            DATE           NULL,
    [ClosureReason]                          VARCHAR (500)  NULL,
    [CareConsent]                            BIT            NULL,
    [SelfAdministrationofMedication]         BIT            NULL,
    [HealthInformationDisclosure]            BIT            NULL,
    [AdmissionRequirements]                  BIT            NULL,
    [AdmissionOrientation]                   BIT            NULL,
    [ZarephathCrisisPlan]                    VARCHAR (2)    NULL,
    [NetworkCrisisPlan]                      VARCHAR (5)    NULL,
    [NCPExpirationDate]                      DATETIME       NULL,
    [PermissionForVoiceMail]                 BIT            NULL,
    [PermissionForEmail]                     BIT            NULL,
    [PermissionForSMS]                       BIT            NULL,
    [PermissionForMail]                      BIT            CONSTRAINT [DF_Referrals_PermissionForMail] DEFAULT ((0)) NULL,
    [AROI]                                   VARCHAR (5)    NULL,
    [AROIAgencyID]                           BIGINT         NULL,
    [AROIExpirationDate]                     DATE           NULL,
    [PHI]                                    BIT            NULL,
    [PHIAgencyID]                            BIGINT         NULL,
    [PHIExpirationDate]                      DATE           NULL,
    [RespiteService]                         BIT            NULL,
    [ZSPRespite]                             BIT            NULL,
    [ZSPRespiteExpirationDate]               DATE           NULL,
    [ZSPRespiteGuardianSignature]            BIT            NULL,
    [ZSPRespiteBHPSigned]                    BIT            NULL,
    [LifeSkillsService]                      BIT            NULL,
    [ZSPLifeSkills]                          BIT            NULL,
    [ZSPLifeSkillsExpirationDate]            DATE           NULL,
    [ZSPLifeSkillsGuardianSignature]         BIT            NULL,
    [ZSPLifeSkillsBHPSigned]                 BIT            NULL,
    [CounselingService]                      BIT            NULL,
    [ZSPCounselling]                         BIT            NULL,
    [ZSPCounsellingExpirationDate]           DATE           NULL,
    [ZSPCounsellingGuardianSignature]        BIT            NULL,
    [ZSPCounsellingBHPSigned]                BIT            NULL,
    [NetworkServicePlan]                     BIT            NULL,
    [NSPExpirationDate]                      DATE           NULL,
    [NSPGuardianSignature]                   BIT            NULL,
    [NSPBHPSigned]                           BIT            NULL,
    [NSPSPidentifyService]                   VARCHAR (2)    NULL,
    [BXAssessment]                           BIT            NULL,
    [BXAssessmentExpirationDate]             DATE           NULL,
    [BXAssessmentBHPSigned]                  BIT            NULL,
    [Demographic]                            VARCHAR (5)    NULL,
    [DemographicExpirationDate]              DATE           NULL,
    [SNCD]                                   VARCHAR (5)    NULL,
    [SNCDExpirationDate]                     DATE           NULL,
    [ACAssessment]                           BIT            NULL,
    [ACAssessmentExpirationDate]             DATE           NULL,
    [IsCheckListCompleted]                   BIT            NULL,
    [IsSparFormCompleted]                    BIT            NULL,
    [IsSaveAsDraft]                          BIT            CONSTRAINT [DF_Referrals_IsSaveAsDraft] DEFAULT ((0)) NULL,
    [ReferralStatusID]                       BIGINT         NULL,
    [Assignee]                               BIGINT         NULL,
    [DropOffLocation]                        BIGINT         NULL,
    [PickUpLocation]                         BIGINT         NULL,
    [NeedPrivateRoom]                        BIT            NULL,
    [OrientationDate]                        DATE           NULL,
    [FrequencyCodeID]                        BIGINT         NULL,
    [ClientID]                               BIGINT         NULL,
    [IsDeleted]                              BIT            CONSTRAINT [DF_Referrals_IsDeleted] DEFAULT ((0)) NULL,
    [RegionID]                               BIGINT         NULL,
    [NotifyCaseManager]                      BIT            CONSTRAINT [DF_Referrals_NotifyCaseManager] DEFAULT ((0)) NULL,
    [LastAttendedDate]                       DATE           NULL,
    [ReferralSourceID]                       INT            NULL,
    [CreatedDate]                            DATETIME       NOT NULL,
    [CreatedBy]                              BIGINT         NOT NULL,
    [UpdatedDate]                            DATETIME       NOT NULL,
    [UpdatedBy]                              BIGINT         NOT NULL,
    [SystemID]                               VARCHAR (100)  NOT NULL,
    [NotifyCaseManagerDate]                  DATE           NULL,
    [ScheduleRequestDates]                   VARCHAR (MAX)  NULL,
    [ReferralTrackingComment]                VARCHAR (500)  NULL,
    [Caseload]                               BIGINT         NULL,
    [MonthlySummaryEmail]                    VARCHAR (MAX)  NULL,
    [ConnectingFamiliesService]              BIT            NULL,
    [ZSPConnectingFamilies]                  BIT            NULL,
    [ZSPConnectingFamiliesExpirationDate]    DATE           NULL,
    [ZSPConnectingFamiliesGuardianSignature] BIT            NULL,
    [ZSPConnectingFamiliesBHPSigned]         BIT            NULL,
    [PCMVoiceMail]                           BIT            NULL,
    [PCMEmail]                               BIT            NULL,
    [PCMSMS]                                 BIT            NULL,
    [PCMMail]                                BIT            NULL,
    [ReferralLSTMCaseloadsComment]           VARCHAR (500)  NULL,
    [PolicyNumber]                           VARCHAR (100)  NULL,
    [CASIIScore]                             VARCHAR (100)  NULL,
    [ImportHours]                            VARCHAR (50)   NULL,
    [MondaySchedule]                         BIT            DEFAULT ((0)) NOT NULL,
    [TuesdaySchedule]                        BIT            DEFAULT ((0)) NOT NULL,
    [WednesdaySchedule]                      BIT            DEFAULT ((0)) NOT NULL,
    [ThursdaySchedule]                       BIT            DEFAULT ((0)) NOT NULL,
    [FridaySchedule]                         BIT            DEFAULT ((0)) NOT NULL,
    [SaturdaySchedule]                       BIT            DEFAULT ((0)) NOT NULL,
    [SundaySchedule]                         BIT            DEFAULT ((0)) NOT NULL,
    [UserName]                               VARCHAR (50)   NULL,
    [Password]                               VARCHAR (100)  NULL,
    [PasswordSalt]                           VARCHAR (MAX)  NULL,
    [SignatureNeeded]                        BIT            CONSTRAINT [DF__Referrals__Signa__270FB757] DEFAULT ((0)) NOT NULL,
    [PhysicianID]                            BIGINT         NULL,
    [CareTypeIds]                            VARCHAR (500)  NULL,
    [ProfileImagePath]                       VARCHAR (300)  NULL,
    [TempPatientID]                          BIGINT         NULL,
    [ProfessionalAuthrizationCode]           NVARCHAR (50)  NULL,
    [InstitutionalAuthrizationCode]          NVARCHAR (50)  NULL,
    [RoleID]                                 BIGINT         NULL,
    [DischargeComment]                       NVARCHAR (MAX) NULL,
    [DischargeDate]                          DATE           NULL,
    [DefaultFacilityID]                      BIGINT         NULL,
    [BeneficiaryType]                        INT            NULL,
    [ServiceType]                            VARCHAR (50)   NULL,
	[GroupIDs]								 NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_Referrals] PRIMARY KEY CLUSTERED ([ReferralID] ASC),
    CONSTRAINT [FK_ReferralCasload_Employee] FOREIGN KEY ([Caseload]) REFERENCES [dbo].[Employees] ([EmployeeID])
);


GO
CREATE TRIGGER [dbo].[tr_Referrals_Deleted] ON dbo.Referrals
FOR DELETE AS 

INSERT INTO JO_Referrals( 
ReferralID,
Title,
FirstName,
MiddleName,
LastName,
Dob	,
Gender,
RecordRequestEmail,
LanguageID	,
ClientNumber,	
AHCCCSID	,
CISNumber	,
Population	,
HealthPlan	,
RateCode	,
RateCodeStartDate,
RateCodeEndDate,
AHCCCSEnrollDate,
PlacementRequirement,
BehavioralIssue,
OtherInformation,
AgencyID,
AgencyLocationID,
CaseManagerID,
FirstDOS,
ReferralDate,
ClosureDate,
ClosureReason,
CareConsent,
SelfAdministrationofMedication,
HealthInformationDisclosure,
AdmissionRequirements,
AdmissionOrientation,
ZarephathCrisisPlan,
NetworkCrisisPlan,
NCPExpirationDate,
PermissionForVoiceMail,
PermissionForEmail,
PermissionForSMS,
AROI,
AROIAgencyID,
AROIExpirationDate,
PHI,
PHIAgencyID,
PHIExpirationDate,
ZSPRespite,
ZSPRespiteExpirationDate,
ZSPRespiteGuardianSignature,
ZSPRespiteBHPSigned,
ZSPLifeSkills,
ZSPLifeSkillsExpirationDate,
ZSPLifeSkillsGuardianSignature,
ZSPLifeSkillsBHPSigned,
ZSPCounselling,
ZSPCounsellingExpirationDate,
ZSPCounsellingGuardianSignature,
ZSPCounsellingBHPSigned,
NetworkServicePlan,
NSPExpirationDate,
NSPGuardianSignature,
NSPBHPSigned,
NSPSPidentifyService,
BXAssessment,
BXAssessmentExpirationDate,
BXAssessmentBHPSigned,
Demographic,
DemographicExpirationDate,
SNCD,
SNCDExpirationDate,
ACAssessment,
ACAssessmentExpirationDate,
IsCheckListCompleted,
IsSparFormCompleted,
IsSaveAsDraft,
ReferralStatusID,
Assignee,
DropOffLocation,
PickUpLocation,
NeedPrivateRoom,
OrientationDate,
FrequencyCodeID,
ClientID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
RegionID,
NotifyCaseManager,
LastAttendedDate,
ReferralSourceID,
Action,ActionDate,ClientNickName,ScheduleRequestDates
)

SELECT  
ReferralID,
Title,
FirstName,
MiddleName,
LastName,
Dob	,
Gender,
RecordRequestEmail,
LanguageID	,
ClientNumber,	
AHCCCSID	,
CISNumber	,
Population	,
HealthPlan	,
RateCode	,
RateCodeStartDate,
RateCodeEndDate,
AHCCCSEnrollDate,
PlacementRequirement,
BehavioralIssue,
OtherInformation,
AgencyID,
AgencyLocationID,
CaseManagerID,
FirstDOS,
ReferralDate,
ClosureDate,
ClosureReason,
CareConsent,
SelfAdministrationofMedication,
HealthInformationDisclosure,
AdmissionRequirements,
AdmissionOrientation,
ZarephathCrisisPlan,
NetworkCrisisPlan,
NCPExpirationDate,
PermissionForVoiceMail,
PermissionForEmail,
PermissionForSMS,
AROI,
AROIAgencyID,
AROIExpirationDate,
PHI,
PHIAgencyID,
PHIExpirationDate,
ZSPRespite,
ZSPRespiteExpirationDate,
ZSPRespiteGuardianSignature,
ZSPRespiteBHPSigned,
ZSPLifeSkills,
ZSPLifeSkillsExpirationDate,
ZSPLifeSkillsGuardianSignature,
ZSPLifeSkillsBHPSigned,
ZSPCounselling,
ZSPCounsellingExpirationDate,
ZSPCounsellingGuardianSignature,
ZSPCounsellingBHPSigned,
NetworkServicePlan,
NSPExpirationDate,
NSPGuardianSignature,
NSPBHPSigned,
NSPSPidentifyService,
BXAssessment,
BXAssessmentExpirationDate,
BXAssessmentBHPSigned,
Demographic,
DemographicExpirationDate,
SNCD,
SNCDExpirationDate,
ACAssessment,
ACAssessmentExpirationDate,
IsCheckListCompleted,
IsSparFormCompleted,
IsSaveAsDraft,
ReferralStatusID,
Assignee,
DropOffLocation,
PickUpLocation,
NeedPrivateRoom,
OrientationDate,
FrequencyCodeID,
ClientID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
RegionID,
NotifyCaseManager,
LastAttendedDate,
ReferralSourceID,
'D',GETUTCDATE(),ClientNickName,ScheduleRequestDates
 FROM deleted

GO
CREATE TRIGGER [dbo].[tr_Referrals_Updated] ON [dbo].[Referrals]
FOR UPDATE AS 



IF((SELECT IsDeleted from inserted) = 1)
BEGIN

	UPDATE Referrals SET IsDeleted = 1, ReferralStatusID=3 WHERE ReferralID = (SELECT ReferralID FROM deleted) 
	DELETE FROM ScheduleMasters WHERE ReferralID IN (SELECT ReferralID FROM deleted)  AND StartDate >= GETDATE()
END

INSERT INTO JO_Referrals( 
ReferralID,
Title,
FirstName,
MiddleName,
LastName,
Dob	,
Gender,
RecordRequestEmail,
LanguageID	,
ClientNumber,	
AHCCCSID	,
CISNumber	,
Population	,
HealthPlan	,
RateCode	,
RateCodeStartDate,
RateCodeEndDate,
AHCCCSEnrollDate,
PlacementRequirement,
BehavioralIssue,
OtherInformation,
AgencyID,
AgencyLocationID,
CaseManagerID,
FirstDOS,
ReferralDate,
ClosureDate,
ClosureReason,
CareConsent,
SelfAdministrationofMedication,
HealthInformationDisclosure,
AdmissionRequirements,
AdmissionOrientation,
ZarephathCrisisPlan,
NetworkCrisisPlan,
NCPExpirationDate,
PermissionForVoiceMail,
PermissionForEmail,
PermissionForSMS,
AROI,
AROIAgencyID,
AROIExpirationDate,
PHI,
PHIAgencyID,
PHIExpirationDate,
ZSPRespite,
ZSPRespiteExpirationDate,
ZSPRespiteGuardianSignature,
ZSPRespiteBHPSigned,
ZSPLifeSkills,
ZSPLifeSkillsExpirationDate,
ZSPLifeSkillsGuardianSignature,
ZSPLifeSkillsBHPSigned,
ZSPCounselling,
ZSPCounsellingExpirationDate,
ZSPCounsellingGuardianSignature,
ZSPCounsellingBHPSigned,
NetworkServicePlan,
NSPExpirationDate,
NSPGuardianSignature,
NSPBHPSigned,
NSPSPidentifyService,
BXAssessment,
BXAssessmentExpirationDate,
BXAssessmentBHPSigned,
Demographic,
DemographicExpirationDate,
SNCD,
SNCDExpirationDate,
ACAssessment,
ACAssessmentExpirationDate,
IsCheckListCompleted,
IsSparFormCompleted,
IsSaveAsDraft,
ReferralStatusID,
Assignee,
DropOffLocation,
PickUpLocation,
NeedPrivateRoom,
OrientationDate,
FrequencyCodeID,
ClientID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
RegionID,
NotifyCaseManager,
LastAttendedDate,
ReferralSourceID,
Action,ActionDate,ClientNickName,ScheduleRequestDates
)

SELECT  
ReferralID,
Title,
FirstName,
MiddleName,
LastName,
Dob	,
Gender,
RecordRequestEmail,
LanguageID	,
ClientNumber,	
AHCCCSID	,
CISNumber	,
Population	,
HealthPlan	,
RateCode	,
RateCodeStartDate,
RateCodeEndDate,
AHCCCSEnrollDate,
PlacementRequirement,
BehavioralIssue,
OtherInformation,
AgencyID,
AgencyLocationID,
CaseManagerID,
FirstDOS,
ReferralDate,
ClosureDate,
ClosureReason,
CareConsent,
SelfAdministrationofMedication,
HealthInformationDisclosure,
AdmissionRequirements,
AdmissionOrientation,
ZarephathCrisisPlan,
NetworkCrisisPlan,
NCPExpirationDate,
PermissionForVoiceMail,
PermissionForEmail,
PermissionForSMS,
AROI,
AROIAgencyID,
AROIExpirationDate,
PHI,
PHIAgencyID,
PHIExpirationDate,
ZSPRespite,
ZSPRespiteExpirationDate,
ZSPRespiteGuardianSignature,
ZSPRespiteBHPSigned,
ZSPLifeSkills,
ZSPLifeSkillsExpirationDate,
ZSPLifeSkillsGuardianSignature,
ZSPLifeSkillsBHPSigned,
ZSPCounselling,
ZSPCounsellingExpirationDate,
ZSPCounsellingGuardianSignature,
ZSPCounsellingBHPSigned,
NetworkServicePlan,
NSPExpirationDate,
NSPGuardianSignature,
NSPBHPSigned,
NSPSPidentifyService,
BXAssessment,
BXAssessmentExpirationDate,
BXAssessmentBHPSigned,
Demographic,
DemographicExpirationDate,
SNCD,
SNCDExpirationDate,
ACAssessment,
ACAssessmentExpirationDate,
IsCheckListCompleted,
IsSparFormCompleted,
IsSaveAsDraft,
ReferralStatusID,
Assignee,
DropOffLocation,
PickUpLocation,
NeedPrivateRoom,
OrientationDate,
FrequencyCodeID,
ClientID,
CreatedDate,
CreatedBy,
UpdatedDate,
UpdatedBy,
SystemID,
IsDeleted,
RegionID,
NotifyCaseManager,
LastAttendedDate,
ReferralSourceID,
'U',GETUTCDATE(),ClientNickName,
ScheduleRequestDates
 FROM deleted