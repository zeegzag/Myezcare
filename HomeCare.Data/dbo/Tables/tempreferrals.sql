﻿CREATE TABLE [dbo].[tempreferrals] (
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
    [PermissionForMail]                      BIT            NULL,
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
    [IsSaveAsDraft]                          BIT            NULL,
    [ReferralStatusID]                       BIGINT         NULL,
    [Assignee]                               BIGINT         NULL,
    [DropOffLocation]                        BIGINT         NULL,
    [PickUpLocation]                         BIGINT         NULL,
    [NeedPrivateRoom]                        BIT            NULL,
    [OrientationDate]                        DATE           NULL,
    [FrequencyCodeID]                        BIGINT         NULL,
    [ClientID]                               BIGINT         NULL,
    [IsDeleted]                              BIT            NULL,
    [RegionID]                               BIGINT         NULL,
    [NotifyCaseManager]                      BIT            NULL,
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
    [MondaySchedule]                         BIT            NOT NULL,
    [TuesdaySchedule]                        BIT            NOT NULL,
    [WednesdaySchedule]                      BIT            NOT NULL,
    [ThursdaySchedule]                       BIT            NOT NULL,
    [FridaySchedule]                         BIT            NOT NULL,
    [SaturdaySchedule]                       BIT            NOT NULL,
    [SundaySchedule]                         BIT            NOT NULL,
    [UserName]                               VARCHAR (50)   NULL,
    [Password]                               VARCHAR (100)  NULL,
    [PasswordSalt]                           VARCHAR (MAX)  NULL,
    [SignatureNeeded]                        BIT            NOT NULL,
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
    [SocialSecurityNumber]                   VARCHAR (MAX)  NULL,
    [GroupIDs]                               NVARCHAR (MAX) NULL,
    [ReferralTrackingID]                     BIGINT         NULL,
    [BloodGroup]                             VARCHAR (50)   NULL,
    [Height]                                 VARCHAR (50)   NULL,
    [Weight]                                 VARCHAR (50)   NULL,
    [Ethnicity]                              VARCHAR (255)  NULL,
    [Race]                                   VARCHAR (MAX)  NULL,
    [CaregiverStatus]                        VARCHAR (MAX)  NULL,
    [CodeStatus]                             BIGINT         NULL,
    [BMI]                                    VARCHAR (MAX)  NULL
);

