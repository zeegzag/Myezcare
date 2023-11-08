using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Referrals")]
    [PrimaryKey("ReferralID")]
    [Sort("ReferralID", "DESC")]
    public class Referral : BaseEntity
    {
        public long ReferralID { get; set; }

        #region tab_Client
       

        [Display(ResourceType = typeof(Resource), Name = "FirstName")]
        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "MiddleName")]
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        public string MiddleName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "LastName")]
        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NickName")]
        public string ClientNickName { get; set; }

        [Ignore]
        public string FullName { get { return Common.GetGenericNameFormat(FirstName,MiddleName, LastName); } }
        //public string Name { get; set; }

        [Required(ErrorMessageResourceName = "DateOfBirthRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessageResourceName = "GenderRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Gender { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ServiceType")]
        public string ServiceType { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ClientNumber")]
        public string ClientNumber { get; set; }

        // [RegularExpression(Constants.RegxSSN, ErrorMessage = "Incorrect SSN Format")]
        [RegularExpression(Constants.RegxIDCard, ErrorMessage = "Invalid ID Card Number")]
        public string SocialSecurityNumber { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AHCCCSID")]
        [Required(ErrorMessageResourceName = "AHCCCSIDRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[a-zA-Z]{1}[0-9]{1,9}$", ErrorMessageResourceName = "AHCCCSIDInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string AHCCCSID { get; set; }

        //[Ignore]
        //[Required(ErrorMessageResourceName = "AHCCCSIDRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(@"^[a-zA-Z]{1}[0-9]{1,9}$", ErrorMessageResourceName = "AHCCCSIDInvalid", ErrorMessageResourceType = typeof(Resource))]
        //public string NewAHCCCSID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AHCCCSEnrollDate")]
        [Required(ErrorMessageResourceName = "AHCCCSVerificationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? AHCCCSEnrollDate { get; set; }

        [Ignore]
        public long SelectedPayor { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CISNumberLabel")]
        //[Required(ErrorMessageResourceName = "CISNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(@"^\d{10}$", ErrorMessageResourceName = "CISIDInvalid", ErrorMessageResourceType = typeof(Resource))]
        //[RequiredIf("SelectedPayor!=Payor.PayorCode.PY && SelectedPayor!=Payor.PayorCode.NoPayor", ErrorMessageResourceName = "CISNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CISNumber { get; set; }



        [Required(ErrorMessageResourceName = "PopulationRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Population { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "HealthPlan")]
        //[Required(ErrorMessageResourceName = "HealthPlanRequired", ErrorMessageResourceType = typeof(Resource))]
        public string HealthPlan { get; set; }

        [Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Title { get; set; }

        public string PolicyNumber { get; set; }
        public string CASIIScore { get; set; }

        

        //[Required(ErrorMessageResourceName = "RecordRequestEmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [Display(ResourceType = typeof(Resource), Name = "RecordRequestEmail")]
        [RegularExpression(Constants.RegxMultipleEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string RecordRequestEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "MonthlySummaryEmail")]
        [RegularExpression(Constants.RegxMultipleEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string MonthlySummaryEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "RateCode")]
        public string RateCode { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "RateCodeStartDate")]
        //[RequiredIf("RateCode!=null", ErrorMessageResourceName = "RateCodeStartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? RateCodeStartDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "RateCodeEndDate")]
        //[RequiredIf("RateCode", "true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? RateCodeEndDate { get; set; }

        //[Required(ErrorMessageResourceName = "ProfessionalAuthrizationCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        //public string ProfessionalAuthrizationCode { get; set; }

        //[Required(ErrorMessageResourceName = "InstitutionalAuthrizationCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        //public string InstitutionalAuthrizationCode { get; set; }

        //[GetTableNameAttribute("Regions")]
        public string RegionID { get; set; }

        [GetTableNameAttribute("Languages")]
        [Display(ResourceType = typeof(Resource), Name = "Language")]
        [Required(ErrorMessageResourceName = "LanguagePreferenceRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LanguageID { get; set; }

        [GetTableNameAttribute("ReferralStatuses")]
        [Display(ResourceType = typeof(Resource), Name = "Status")]
        [Required(ErrorMessageResourceName = "StatusRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? ReferralStatusID { get; set; }

        [GetTableNameAttribute("Employees")]
        [Display(ResourceType = typeof(Resource), Name = "Assignee")]
        [Required(ErrorMessageResourceName = "AssigneeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? Assignee { get; set; }

        //[GetTableNameAttribute("BeneficiaryType")]
        [Display(ResourceType = typeof(Resource), Name = "BeneficiaryType")]
        public string BeneficiaryType { get; set; }

        [GetTableNameAttribute("Facilities")]
        [Display(ResourceType = typeof(Resource), Name = "Facility")]
        [Required(ErrorMessageResourceName = "FacilityRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? DefaultFacilityID { get; set; }

        public string GroupIDs { get; set; }

        [GetTableNameAttribute("Employees")]
        [Display(ResourceType = typeof(Resource), Name = "Caseload")]
        public long? Caseload { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "OrientationDate")]
        //[RequiredIf("ReferralStatus.ReferralStatuses.Active==ReferralStatusID", ErrorMessageResourceName = "OrientationDateRequired", ErrorMessageResourceType = typeof(Resource))] // As per discussed with jitu removed required if validation if status is active
        public DateTime? OrientationDate { get; set; }

        [GetTableNameAttribute("TransportLocations")]
        [Display(ResourceType = typeof(Resource), Name = "DropOffLocation")]
        [RequiredIf("ReferralStatus.ReferralStatuses.Active==ReferralStatusID", ErrorMessageResourceName = "DropOffLocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? DropOffLocation { get; set; }

        [GetTableNameAttribute("TransportLocations")]
        [Display(ResourceType = typeof(Resource), Name = "PickUpLocation")]
        [RequiredIf("ReferralStatus.ReferralStatuses.Active==ReferralStatusID", ErrorMessageResourceName = "PickUpLocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? PickUpLocation { get; set; }

        [GetTableNameAttribute("FrequencyCodes")]
        [Display(ResourceType = typeof(Resource), Name = "FrequencyCode")]
        //[RequiredIf("ReferralStatus.ReferralStatuses.Active==ReferralStatusID", ErrorMessageResourceName = "FrequencyCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? FrequencyCodeID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NeedPrivateRoom")]
        public bool? NeedPrivateRoom { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PlacementRequirement")]
        public string PlacementRequirement { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "BehavioralIssue")]
        public string BehavioralIssue { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OtherInformation")]
        public string OtherInformation { get; set; }

        public long ClientID { get; set; }

        [GetTableNameAttribute("Agencies")]
        [Display(ResourceType = typeof(Resource), Name = "Agency")]
        [Required(ErrorMessageResourceName = "AgencyRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? AgencyID { get; set; }

        [Ignore]
        public string EncryptedAgencyID { get { return AgencyID.HasValue ? Crypto.Encrypt(Convert.ToString(AgencyID)) : null; } }

        [Required(ErrorMessageResourceName = "AgencyLocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? AgencyLocationID { get; set; }

        [Ignore]
        public string EncryptedAgencyLocationID { get { return AgencyLocationID.HasValue ? Crypto.Encrypt(Convert.ToString(AgencyLocationID)) : null; } }

        [GetTableNameAttribute("Employees")]
        [Display(ResourceType = typeof(Resource), Name = "CaseManager")]
        //[Required(ErrorMessageResourceName = "CaseManagerRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? CaseManagerID { get; set; }

        [GetTableNameAttribute("Physicians")]
        [Display(ResourceType = typeof(Resource), Name = "Physician")]
        //[Required(ErrorMessageResourceName = "PhysicianRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? PhysicianID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CareConsent")]
        [Required(ErrorMessageResourceName = "CareConsentRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool CareConsent { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SelfAdministrationofMedication")]
        [Required(ErrorMessageResourceName = "AdministrationMedicationRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool SelfAdministrationofMedication { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "HealthInformationDisclosure")]
        [Required(ErrorMessageResourceName = "HealthInformationDisclosureRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool HealthInformationDisclosure { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AdmissionRequirements")]
        [Required(ErrorMessageResourceName = "AdmissionRequirementsRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool AdmissionRequirements { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AdmissionOrientation")]
        [Required(ErrorMessageResourceName = "AdmissionOrientationRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool AdmissionOrientation { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ZarephathCrisisPlan")]
        [Required(ErrorMessageResourceName = "ZarephathCrisisPlanRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZarephathCrisisPlan { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PermissionforVoiceMailTitle")]
        [Required(ErrorMessageResourceName = "PermissionForVoiceMailRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool PermissionForVoiceMail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PermissionForEmail")]
        [Required(ErrorMessageResourceName = "PermissionForEmailRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool PermissionForEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PermissionForSMS")]
        [Required(ErrorMessageResourceName = "PermissionForSMSRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool PermissionForSMS { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PermissionForMail")]
        [Required(ErrorMessageResourceName = "PermissionForMailRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool PermissionForMail { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "ROItoAgency")]
        //[Required(ErrorMessageResourceName = "PHIRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool PHI { get; set; }


        [GetTableNameAttribute("Agencies")]
        [Display(ResourceType = typeof(Resource), Name = "ROItoAgency")]
        //[RequiredIf(@"GoAbroad == true && ((NextCountry != 'Other' && NextCountry == Country)|| (Age > 24 && Age <= 55))",
        //    ErrorMessageResourceName = "AgencyRequired", ErrorMessageResourceType = typeof(Resource))]        
        [RequiredIf(@"PHI== true", ErrorMessageResourceName = "AgencyRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? PHIAgencyID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ROItoAgencyExpiration")]
        [RequiredIf(@"PHI== true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? PHIExpirationDate { get; set; }



        [Display(ResourceType = typeof(Resource), Name = "RespiteService")]
        // [Required(ErrorMessageResourceName = "ZSPRespiteRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool RespiteService { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Respite")]
        public bool ZSPRespite { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "RespiteExpirationDate")]
        [RequiredIf("ZSPRespite == true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ZSPRespiteExpirationDate { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "RespiteGuardianSignature")]
        [RequiredIf("ZSPRespite == true", ErrorMessageResourceName = "GuardianSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPRespiteGuardianSignature { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "RespiteBHPSigned")]
        [RequiredIf("ZSPRespite == true", ErrorMessageResourceName = "BHPSignedRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPRespiteBHPSigned { get; set; }

        // [Required(ErrorMessageResourceName = "ZSPLifeSkillsRequired", ErrorMessageResourceType = typeof(Resource))]
        [Display(ResourceType = typeof(Resource), Name = "LifeSkillsService")]
        public bool LifeSkillsService { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "LifeSkills")]
        public bool ZSPLifeSkills { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "LifeSkillsExpirationDate")]
        [RequiredIf("ZSPLifeSkills == true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ZSPLifeSkillsExpirationDate { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "LifeSkillsGuardianSignature")]
        [RequiredIf("ZSPLifeSkills == true", ErrorMessageResourceName = "GuardianSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPLifeSkillsGuardianSignature { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "LifeSkillsBHPSigned")]
        [RequiredIf("ZSPLifeSkills == true", ErrorMessageResourceName = "BHPSignedRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPLifeSkillsBHPSigned { get; set; }

        //  [Required(ErrorMessageResourceName = "ZSPCounsellingRequired", ErrorMessageResourceType = typeof(Resource))]
        [Display(ResourceType = typeof(Resource), Name = "CounselingService")]
        public bool CounselingService { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Counseling")]
        public bool ZSPCounselling { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "CounselingExpirationDate")]
        [RequiredIf("ZSPCounselling == true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ZSPCounsellingExpirationDate { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "CounselingGuardianSignature")]
        [RequiredIf("ZSPCounselling == true", ErrorMessageResourceName = "GuardianSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPCounsellingGuardianSignature { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "CounselingBHPSigned")]
        [RequiredIf("ZSPCounselling == true", ErrorMessageResourceName = "BHPSignedRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPCounsellingBHPSigned { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "ConnectingFamiliesService")]
        public bool ConnectingFamiliesService { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ConnectingFamilies")]
        public bool ZSPConnectingFamilies { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ConnectingFamiliesExpirationDate")]
        [RequiredIf("ZSPConnectingFamilies == true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ZSPConnectingFamiliesExpirationDate { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ConnectingFamiliesGuardianSignature")]
        [RequiredIf("ZSPConnectingFamilies == true", ErrorMessageResourceName = "GuardianSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPConnectingFamiliesGuardianSignature { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ConnectingFamiliesBHPSigned")]
        [RequiredIf("ZSPConnectingFamilies == true", ErrorMessageResourceName = "BHPSignedRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? ZSPConnectingFamiliesBHPSigned { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "AnsellCaseyAssessment")]
        [Required(ErrorMessageResourceName = "ACAssessmentRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool ACAssessment { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "AnsellCaseyAssessmentExpirationDate")]
        [RequiredIf("ACAssessment == true && LifeSkillsService==true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ACAssessmentExpirationDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ROIFromAgency")]
        //  [Required(ErrorMessageResourceName = "AgencyROIRequired", ErrorMessageResourceType = typeof(Resource))]
        public string AROI { get; set; }

        [GetTableNameAttribute("Agencies")]
        [Display(ResourceType = typeof(Resource), Name = "ROIFromAgency")]
        [RequiredIf("AROI == 'Y'", ErrorMessageResourceName = "AgencyRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? AROIAgencyID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ROIFromAgencyExpiration")]
        [RequiredIf("AROI == 'Y'", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? AROIExpirationDate { get; set; }



        [Display(ResourceType = typeof(Resource), Name = "NetworkCrisisPlan")]
        [Required(ErrorMessageResourceName = "NetworkCrisisPlanRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NetworkCrisisPlan { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "NetworkServicePlan")]
        [Required(ErrorMessageResourceName = "NetworkServicePlanRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool NetworkServicePlan { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "NSPExpirationDate")]
        [RequiredIf("NetworkServicePlan == true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? NSPExpirationDate { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "NSPGuardianSignature")]
        [RequiredIf("NetworkServicePlan == true", ErrorMessageResourceName = "GuardianSignatureRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? NSPGuardianSignature { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "NSPBHPSigned")]
        [RequiredIf("NetworkServicePlan == true", ErrorMessageResourceName = "BHPSignedRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? NSPBHPSigned { get; set; }
        
        [Display(ResourceType = typeof(Resource), Name = "NSPSPIdentifyService")]
        [RequiredIf("NetworkServicePlan == true", ErrorMessageResourceName = "IdentifyServiceRequired", ErrorMessageResourceType = typeof(Resource))]
        //public bool? NSPSPidentifyService { get; set; }
        public string NSPSPidentifyService { get; set; }

        [RequiredIf("NetworkCrisisPlan == 'Y'", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? NCPExpirationDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "BXAssessment")]
        [Required(ErrorMessageResourceName = "BXAssessmentRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool BXAssessment { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "BXAssessmentExpiration")]
        [RequiredIf("BXAssessment == true", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? BXAssessmentExpirationDate { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "BXAssessmentBHPSigned")]
        [RequiredIf("BXAssessment == true", ErrorMessageResourceName = "BHPSignedRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool? BXAssessmentBHPSigned { get; set; }

        [Required(ErrorMessageResourceName = "DemographicRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Demographic { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "DemographicExpirationDate")]
        [RequiredIf("Demographic == 'Y'", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? DemographicExpirationDate { get; set; }

        [Required(ErrorMessageResourceName = "SNCDRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SNCD { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "SNCDExpirationDate")]
        //[RequiredIf("SNCD == 'Y'", ErrorMessageResourceName = "ExpirationDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? SNCDExpirationDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ReferralDate")]
        [Required(ErrorMessageResourceName = "ReferralDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? ReferralDate { get; set; }

        [GetTableNameAttribute("ReferralSources")]
        [Display(ResourceType = typeof(Resource), Name = "ReferralSource")]
        [Required(ErrorMessageResourceName = "ReferralSourceRequired", ErrorMessageResourceType = typeof(Resource))]
        public int ReferralSourceID { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "FirstDOS")]
        //[Required(ErrorMessageResourceName = "FirstDOSRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? FirstDOS { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ClosureDate")]
        public DateTime? ClosureDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ClosureReason")]
        // [RequiredIf("ReferralStatus.ReferralStatuses.Inactive==ReferralStatusID", ErrorMessageResourceName = "ClosureReasonRequired", ErrorMessageResourceType = typeof(Resource))] //As per discussed with Paallav sir removed required on 17 Aug 2020// As per discussed with jitu removed required if validation if status is active
        public string ClosureReason { get; set; }

        public string ReferralTrackingComment { get; set; }

        public string ReferralLSTMCaseloadsComment { get; set; }

        public bool NotifyCaseManager { get; set; }

        public DateTime? NotifyCaseManagerDate { get; set; }

        public bool? IsSaveAsDraft { get; set; }

        public bool MondaySchedule { get; set; }
        public bool TuesdaySchedule { get; set; }
        public bool WednesdaySchedule { get; set; }
        public bool ThursdaySchedule { get; set; }
        public bool FridaySchedule { get; set; }
        public bool SaturdaySchedule { get; set; }
        public bool SundaySchedule { get; set; }

        public string CareTypeIds { get; set; }

        //[Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        

        public string PasswordSalt { get; set; }


        [Ignore]
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceName = "PasswordMaxLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPassword, ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Ignore]
        //[Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceName = "PasswordAndConfirmPasswordDoNotMatch", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "SignatureNeededRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool SignatureNeeded { get; set; }


        //[Ignore]
        //[RequiredIf("ScheduleRequestDateErrorCount==1", ErrorMessageResourceType = typeof(Resource))]
        //public int ScheduleRequestDateErrorCount { get; set; }

        public string ScheduleRequestDates { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PreferredCommunicationforVoiceMail")]
        public bool PCMVoiceMail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PreferredCommunicationforEmail")]
        public bool PCMEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PreferredCommunicationforSMS")]
        public bool PCMSMS { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PreferredCommunicationforMail")]
        public bool PCMMail { get; set; }










        [Ignore]
        public string EncryptedReferralID { get; set; }

        [Ignore]
        public bool IsEditMode { get; set; }

        [ResultColumn]
        public string RegionName { get; set; }

        [ResultColumn]
        public string AgencyName { get; set; }

        [ResultColumn]
        public string PayorName { get; set; }

        [ResultColumn]
        public string CaseManager { get; set; }


        [ResultColumn]
        public string Status { get; set; }

        [ResultColumn]
        public float UsedRespiteHours { get; set; }

        [ResultColumn]
        public string UsedRespiteHours12 { get; set; }

        [ResultColumn]
        public string Email { get; set; }

        [ResultColumn]
        public string Phone { get; set; }

        [ResultColumn]
        public string Fax { get; set; }

        [ResultColumn]
        public string NickName { get; set; }


        [ResultColumn]
        public string CalculateDob
        {
            get
            {
                if (Dob != null)
                {
                    string str = Common.ToAgeString((DateTime)Dob);
                    return str;
                }
                return null;
            }
        }


        [ResultColumn]
        public string SetSelectedReferralCaseloadIDs { get; set; }

        [Ignore]
        public List<string> ReferralCaseloadIDs { get; set; }

        public string ProfileImagePath { get; set; }


        public long RoleID { get; set; }

        public string DischargeComment { get; set; }

        public DateTime? DischargeDate { get; set; }



        [ResultColumn]
        public long ReferralCareGiver_AgencyID { get; set; }

        public string BloodGroup { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Ethnicity { get; set; }
        public string Race { get; set; }
        public string CaregiverStatus { get; set; }
        public string CodeStatus { get; set; }
        public string BMI { get; set; }
        public string BMIValue { get; set; }
        public bool IsBillable { get; set; }

        #endregion

    }

    public class RPTReferrals:Referral
    {
        public string Name { get; set; }
        public string GenDobAge { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string BeneficiaryNumber { get; set; }
    }

    public enum EnumReferralSources
    {
        Email = 1,
        Phone = 2,
        Fax = 3,
        Agency = 4,
        Other = 5,
    }

}
