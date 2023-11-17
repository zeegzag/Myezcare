using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;
using System.Linq;
using System.Web;

namespace Zarephath.Core.Models.ViewModel
{


    #region Add Referral

    public class ReferralModel
    {
        public AddReferralModel AddReferralModel { get; set; }
    }

    public class AddReferralModel
    {
        public AddReferralModel()
        {
            Referral = new Referral();
            ReferralStatusList = new List<ReferralStatus>();
            EmployeeList = new List<EmployeeDropDownModel>();
            FrequencyCodeList = new List<FrequencyCode>();
            TransportLocationList = new List<TransportLocation>();
            GenderList = new List<NameValueData>();
            RegionList = new List<Region>();
            LanguageList = new List<Language>();
            PrivateRoomList = new List<NameValueDataBoolean>();
            Contact = new Contact();
            ContactMapping = new ContactMapping();
            ContactTypeList = new List<ContactType>();
            ROITypes = new List<NameValueData>();
            PrimaryContactGuardianList = new List<NameValueDataBoolean>();
            DCSGuardianList = new List<NameValueDataBoolean>();
            EmergencyContactList = new List<NameValueDataBoolean>();
            NoticeProviderOnFileList = new List<NameValueDataBoolean>();
            ContactInformationList = new List<AddAndListContactInformation>();
            AddAndListContactInformation = new AddAndListContactInformation();
            AddContactModel = new AddAndListContactInformation();
            ContactSearchModel = new ContactSearchModel();
            AgencyList = new List<AgencyModel>();
            AgencyLocationList = new List<AgencyLocationModel>();
            CaseManagerList = new List<CaseManager>();
            CaseManager = new CaseManager();
            SetReferralInternalMessagePageLoad = new SetReferralInternalMessagePageLoad();
            CareConsentList = new List<NameValueDataBoolean>();
            SelfAdministrationList = new List<NameValueDataBoolean>();
            HealthInformationList = new List<NameValueDataBoolean>();
            AdmissionRequirementList = new List<NameValueDataBoolean>();
            AdmissionOrientationList = new List<NameValueDataBoolean>();
            ZarephathCrisisPlanList = new List<NameValueDataInString>();
            NetworkCrisisPlanList = new List<NameValueDataInString>();
            VoiceMailList = new List<NameValueDataBoolean>();
            PermissionEmailList = new List<NameValueDataBoolean>();
            PermissionSMSList = new List<NameValueDataBoolean>();
            PHIList = new List<NameValueDataBoolean>();
            ROIList = new List<NameValueDataInString>();
            ZSPRespiteList = new List<NameValueDataBoolean>();
            ZSPLifeSkillsList = new List<NameValueDataBoolean>();
            ZSPCounsellingList = new List<NameValueDataBoolean>();
            ZSPRespiteGuardianSignatureList = new List<NameValueDataBoolean>();
            ZSPRespiteBHPSignedList = new List<NameValueDataBoolean>();
            ZSPLifeSkillsGuardianSignatureList = new List<NameValueDataBoolean>();
            ZSPLifeSkillsBHPSignedList = new List<NameValueDataBoolean>();
            ZSPCounsellingGuardianSignatureList = new List<NameValueDataBoolean>();
            ZSPCounsellingBHPSignedList = new List<NameValueDataBoolean>();
            NetworkServicePlanList = new List<NameValueDataBoolean>();
            NetworkServiceGuardianSignatureList = new List<NameValueDataBoolean>();
            NetworkServiceBHPSignedList = new List<NameValueDataBoolean>();
            NSPSPidentifyServiceList = new List<NameValueDataInString>();
            BXAssessmentList = new List<NameValueDataBoolean>();
            BXAssessmentBHPSignedList = new List<NameValueDataBoolean>();
            DemographicList = new List<NameValueDataInString>();
            SNCDList = new List<NameValueDataInString>();
            ACAssessmentList = new List<NameValueDataBoolean>();
            //DXCodeList = new List<DXCode>();
            //DXCode = new DXCode();
            DxCodeMappingList = new List<DXCodeMappingList>();
            LU_OutcomeMeasurementOption = new List<LU_OutcomeMeasurementOption>();
            ReferralSiblingMappings = new ReferralSiblingMappingsList();
            ReferralSiblingMappingList = new List<ReferralSiblingMappingsList>();

            InternalDocuments = new List<DocumentType>();
            ExternalDocuments = new List<DocumentType>();
            DocumentKind = new List<NameValueDataBoolean>();
            PayorList = new List<Payor>();
            ReferralPayorMapping = new ReferralPayorMapping();
            ReferralDocument = new ReferralDocument();
            ReferralPayorMappingList = new List<ListReferralPayorMapping>();
            ReferralSources = new List<ReferralSource>();
            ReferralDXCodeMapping = new DXCodeMappingList();

            PermissionMailList = new List<NameValueDataBoolean>();
            ReferralOutcomeMeasurement = new ReferralOutcomeMeasurement();
            ReferralAssessmentReview = new ReferralAssessmentReview();
            SearchReferralOutcomeMeasurement = new SearchReferralOutcomeMeasurement();
            SearchReferralAssessmentReview = new SearchReferralAssessmentReview();

            SearchReferralMonthlySummary = new SearchReferralMonthlySummary();
            ReferralMonthlySummary = new ReferralMonthlySummary();
            BindMealsandSummaryofFood = new List<NameValueDataInString>();
            EnumCoordinationofCare = new List<NameValueDataInString>();
            SummaryofFood = new List<NameValueDataInString>();
            Coordinationofcare = new List<NameValueDataInString>();
            DeleteFilter = new List<NameValueData>();
            ReferralAhcccsDetails = new ReferralAhcccsDetails();

            UpdateReferralPayorMapping = new ReferralPayorMapping();

        }

        public Referral Referral { get; set; }
        public List<ReferralStatus> ReferralStatusList { get; set; }
        public List<EmployeeDropDownModel> EmployeeList { get; set; }
        public List<FrequencyCode> FrequencyCodeList { get; set; }
        public List<TransportLocation> TransportLocationList { get; set; }
        public List<NameValueData> GenderList { get; set; }
        public List<Region> RegionList { get; set; }
        public List<Language> LanguageList { get; set; }
        public List<NameValueDataBoolean> PrivateRoomList { get; set; }
        public Contact Contact { get; set; }
        public ContactMapping ContactMapping { get; set; }
        public List<ContactType> ContactTypeList { get; set; }
        public List<NameValueData> ROITypes { get; set; }
        public List<NameValueDataBoolean> PrimaryContactGuardianList { get; set; }
        public List<NameValueDataBoolean> DCSGuardianList { get; set; }
        public List<NameValueDataBoolean> EmergencyContactList { get; set; }
        public List<NameValueDataBoolean> NoticeProviderOnFileList { get; set; }
        public List<AddAndListContactInformation> ContactInformationList { get; set; }
        public AddAndListContactInformation AddAndListContactInformation { get; set; }
        public AddAndListContactInformation AddContactModel { get; set; }
        public ContactSearchModel ContactSearchModel { get; set; }
        public List<AgencyModel> AgencyList { get; set; }
        public List<AgencyLocationModel> AgencyLocationList { get; set; }
        public List<CaseManager> CaseManagerList { get; set; }
        public CaseManager CaseManager { get; set; }
        public SetReferralInternalMessagePageLoad SetReferralInternalMessagePageLoad { get; set; }
        public List<NameValueDataBoolean> CareConsentList { get; set; }
        public List<NameValueDataBoolean> SelfAdministrationList { get; set; }
        public List<NameValueDataBoolean> HealthInformationList { get; set; }
        public List<NameValueDataBoolean> AdmissionRequirementList { get; set; }
        public List<NameValueDataBoolean> AdmissionOrientationList { get; set; }
        public List<NameValueDataInString> ZarephathCrisisPlanList { get; set; }
        public List<NameValueDataInString> NetworkCrisisPlanList { get; set; }
        public List<NameValueDataBoolean> VoiceMailList { get; set; }
        public List<NameValueDataBoolean> PermissionEmailList { get; set; }
        public List<NameValueDataBoolean> PermissionSMSList { get; set; }
        public List<NameValueDataBoolean> PHIList { get; set; }
        public List<NameValueDataInString> ROIList { get; set; }
        public List<NameValueDataBoolean> ZSPRespiteList { get; set; }
        public List<NameValueDataBoolean> ZSPLifeSkillsList { get; set; }
        public List<NameValueDataBoolean> ZSPCounsellingList { get; set; }
        public List<NameValueDataBoolean> ZSPRespiteGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> ZSPRespiteBHPSignedList { get; set; }
        public List<NameValueDataBoolean> ZSPLifeSkillsGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> ZSPLifeSkillsBHPSignedList { get; set; }
        public List<NameValueDataBoolean> ZSPCounsellingGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> ZSPCounsellingBHPSignedList { get; set; }
        public List<NameValueDataBoolean> NetworkServicePlanList { get; set; }
        public List<NameValueDataBoolean> NetworkServiceGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> NetworkServiceBHPSignedList { get; set; }
        public List<NameValueDataInString> NSPSPidentifyServiceList { get; set; }
        public List<NameValueDataBoolean> BXAssessmentList { get; set; }
        public List<NameValueDataBoolean> BXAssessmentBHPSignedList { get; set; }
        public List<NameValueDataInString> DemographicList { get; set; }
        public List<NameValueDataInString> SNCDList { get; set; }
        public List<NameValueDataBoolean> ACAssessmentList { get; set; }
        public List<DXCodeMappingList> DxCodeMappingList { get; set; }
        public List<LU_OutcomeMeasurementOption> LU_OutcomeMeasurementOption { get; set; }

        public ReferralSiblingMappingsList ReferralSiblingMappings { get; set; }
        public List<ReferralSiblingMappingsList> ReferralSiblingMappingList { get; set; }

        public List<DocumentType> InternalDocuments { get; set; }
        public List<DocumentType> ExternalDocuments { get; set; }
        public List<NameValueDataBoolean> DocumentKind { get; set; }
        public List<Payor> PayorList { get; set; }
        public ReferralPayorMapping ReferralPayorMapping { get; set; }
        public ReferralDocument ReferralDocument { get; set; }
        public List<ListReferralPayorMapping> ReferralPayorMappingList { get; set; }
        public int ScheduleHistoryCount { get; set; }
        public List<ReferralSource> ReferralSources { get; set; }
        public DateTime ServiceDate { get; set; }
        public DXCodeMappingList ReferralDXCodeMapping { get; set; }



        [Range(1, int.MaxValue, ErrorMessageResourceName = "DxCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int DXCodeCount { get { return DxCodeMappingList.Count(m => m.IsDeleted == false); } }

        public List<NameValueDataBoolean> PermissionMailList { get; set; }
        [Ignore]
        public AmazonSettingModel AmazonSettingModel { get; set; }

        [Ignore]
        public ReferralOutcomeMeasurement ReferralOutcomeMeasurement { get; set; }

        [Ignore]
        public SearchReferralOutcomeMeasurement SearchReferralOutcomeMeasurement { get; set; }

        [Ignore]
        public ReferralAssessmentReview ReferralAssessmentReview { get; set; }

        public SearchReferralAssessmentReview SearchReferralAssessmentReview { get; set; }

        [Ignore]
        public string DefaultTab { get; set; }

        [Ignore]
        public ReferralMonthlySummary ReferralMonthlySummary { get; set; }

        [Ignore]
        public SearchReferralMonthlySummary SearchReferralMonthlySummary { get; set; }

        [Ignore]
        public List<NameValueDataInString> BindMealsandSummaryofFood { get; set; }

        [Ignore]
        public List<NameValueDataInString> EnumCoordinationofCare { get; set; }
        [Ignore]
        public List<NameValueDataInString> SummaryofFood { get; set; }

        [Ignore]
        public List<NameValueDataInString> Coordinationofcare { get; set; }

        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public ReferralAhcccsDetails ReferralAhcccsDetails { get; set; }

        [Ignore]
        public bool NotifyCmForInactiveStatus { get; set; }


        [Ignore]
        public ReferralPayorMapping UpdateReferralPayorMapping { get; set; }



    }



    public class HC_ReferralModel
    {
        public HC_AddReferralModel AddReferralModel { get; set; }
    }

    public class HC_AddReferralModel
    {
        public HC_AddReferralModel()
        {
            FacilityList = new List<FacilityDDL>();
            Referral = new Referral();
            ReferralStatusList = new List<ReferralStatus>();
            EmployeeList = new List<EmployeeDropDownModel>();
            FrequencyCodeList = new List<FrequencyCode>();
            TransportLocationList = new List<TransportLocation>();
            GenderList = new List<NameValueDataInString>();
            RegionList = new List<Region>();
            LanguageList = new List<Language>();
            PrivateRoomList = new List<NameValueDataBoolean>();
            Contact = new Contact();
            ContactMapping = new ContactMapping();
            ContactTypeList = new List<ContactType>();
            ROITypes = new List<NameValueData>();
            PrimaryContactGuardianList = new List<NameValueDataBoolean>();
            DCSGuardianList = new List<NameValueDataBoolean>();
            EmergencyContactList = new List<NameValueDataBoolean>();
            NoticeProviderOnFileList = new List<NameValueDataBoolean>();
            SignatureNeededList = new List<NameValueDataBoolean>();
            ContactInformationList = new List<AddAndListContactInformation>();
            AddAndListContactInformation = new AddAndListContactInformation();
            AddContactModel = new AddAndListContactInformation();
            ContactSearchModel = new ContactSearchModel();
            AgencyList = new List<AgencyModel>();
            AgencyLocationList = new List<AgencyLocationModel>();
            CaseManagerList = new List<CaseManager>();
            CaseManager = new CaseManager();
            SetReferralInternalMessagePageLoad = new SetReferralInternalMessagePageLoad();
            CareConsentList = new List<NameValueDataBoolean>();
            SelfAdministrationList = new List<NameValueDataBoolean>();
            HealthInformationList = new List<NameValueDataBoolean>();
            AdmissionRequirementList = new List<NameValueDataBoolean>();
            AdmissionOrientationList = new List<NameValueDataBoolean>();
            ZarephathCrisisPlanList = new List<NameValueDataInString>();
            NetworkCrisisPlanList = new List<NameValueDataInString>();
            VoiceMailList = new List<NameValueDataBoolean>();
            PermissionEmailList = new List<NameValueDataBoolean>();
            PermissionSMSList = new List<NameValueDataBoolean>();
            PHIList = new List<NameValueDataBoolean>();
            ROIList = new List<NameValueDataInString>();
            ZSPRespiteList = new List<NameValueDataBoolean>();
            ZSPLifeSkillsList = new List<NameValueDataBoolean>();
            ZSPCounsellingList = new List<NameValueDataBoolean>();
            ZSPRespiteGuardianSignatureList = new List<NameValueDataBoolean>();
            ZSPRespiteBHPSignedList = new List<NameValueDataBoolean>();
            ZSPLifeSkillsGuardianSignatureList = new List<NameValueDataBoolean>();
            ZSPLifeSkillsBHPSignedList = new List<NameValueDataBoolean>();
            ZSPCounsellingGuardianSignatureList = new List<NameValueDataBoolean>();
            ZSPCounsellingBHPSignedList = new List<NameValueDataBoolean>();
            NetworkServicePlanList = new List<NameValueDataBoolean>();
            NetworkServiceGuardianSignatureList = new List<NameValueDataBoolean>();
            NetworkServiceBHPSignedList = new List<NameValueDataBoolean>();
            NSPSPidentifyServiceList = new List<NameValueDataInString>();
            BXAssessmentList = new List<NameValueDataBoolean>();
            BXAssessmentBHPSignedList = new List<NameValueDataBoolean>();
            DemographicList = new List<NameValueDataInString>();
            SNCDList = new List<NameValueDataInString>();
            ACAssessmentList = new List<NameValueDataBoolean>();

            //DXCodeList = new List<DXCode>();
            //DXCode = new DXCode();
            DxCodeMappingList = new List<DXCodeMappingList>();
            ReferralBeneficiaryTypes = new List<ReferralBeneficiaryDetail>();
            ReferralPhysicians = new List<ReferralPhysicianDetail>();
            ReferralBeneficiaryTypeDetail = new ReferralBeneficiaryDetail();
            LU_OutcomeMeasurementOption = new List<LU_OutcomeMeasurementOption>();
            ReferralSiblingMappings = new ReferralSiblingMappingsList();
            ReferralSiblingMappingList = new List<ReferralSiblingMappingsList>();

            DocumentTypeList = new List<DocumentTypeModel>();
            //InternalDocuments = new List<DocumentType>();
            //ExternalDocuments = new List<DocumentType>();
            DocumentKind = new List<NameValueDataBoolean>();
            PayorList = new List<Payor>();
            ReferralPayorMapping = new ReferralPayorMapping();
            ReferralDocument = new ReferralDocument();
            ReferralPayorMappingList = new List<ListReferralPayorMapping>();
            ReferralSources = new List<ReferralSource>();
            ReferralDXCodeMapping = new DXCodeMappingList();

            PermissionMailList = new List<NameValueDataBoolean>();
            ReferralOutcomeMeasurement = new ReferralOutcomeMeasurement();
            ReferralAssessmentReview = new ReferralAssessmentReview();
            SearchReferralOutcomeMeasurement = new SearchReferralOutcomeMeasurement();
            SearchReferralAssessmentReview = new SearchReferralAssessmentReview();

            SearchReferralMonthlySummary = new SearchReferralMonthlySummary();
            ReferralMonthlySummary = new ReferralMonthlySummary();
            BindMealsandSummaryofFood = new List<NameValueDataInString>();
            EnumCoordinationofCare = new List<NameValueDataInString>();
            SummaryofFood = new List<NameValueDataInString>();
            Coordinationofcare = new List<NameValueDataInString>();
            DeleteFilter = new List<NameValueData>();
            ReferralAhcccsDetails = new ReferralAhcccsDetails();

            UpdateReferralPayorMapping = new ReferralPayorMapping();
            PreferenceList = new List<ReferralPreferenceModel>();
            SkillList = new List<Preference>();
            ReferralSkillList = new List<string>();
            StateList = new List<State>();
            CareTypeList = new List<CareType>();
            PhysicianModel = new Physician();
            SearchReferralPayorMapping = new SearchReferralPayorMapping();
            SearchReferralNoteSentenceList = new SearchReferralNoteSentenceList();


            PrecedenceList = new List<NameValueData>();
            ReferralBillingSetting = new ReferralBillingSetting();
            ReferralBillingAuthorization = new ReferralBillingAuthorization();
            ReferralBillingAuthorizationServiceCode = new ReferralBillingAuthorizationServiceCode();
            SearchReferralBillingAuthorization = new SearchReferralBillingAuthorization();
            AdmissionTypeList = new List<NameValueData>();
            AdmissionSourceList = new List<NameValueData>();
            PatientDischargeStatusList = new List<NameValueData>();
            FacilityCodeList = new List<NameValueData>();
            VisitTypeList = new List<NameValueData>();
            CareForm = new CareForm();
            InternalDocumentList = new List<ReferralComplianceDetails>();
            InternalDocumentSectionList = new List<DocumentSection>();
            ExternalDocumentList = new List<ReferralComplianceDetails>();
            ExternalDocumentSectionList = new List<DocumentSection>();
            PatientFrequencyCode = new List<NameValueData>();
            BeneficiaryTypes = new List<NameValueData>();
            ServiceType = new List<ServiceType>();

            //PriorAuthorizationFrequencyList = new List<NameValueData>();
            RevenueCodeList = new List<RevenueCode>();

            ServiceCodeList = new List<ServiceCode>();
            TaxonomyCodes = new List<NameValueStringData>();
            RaceGroupList = new List<NameValueStringData>();
            EthnicityList = new List<NameValueStringData>();
            CodeStatusList = new List<NameValueStringData>();
            DosageTimes = new List<DosageTimeDetail>();
        }

        public List<FacilityDDL> FacilityList { get; set; }
        public Referral Referral { get; set; }
        //public List<NameValueData> ReferralStatusList { get; set; }
        public List<ReferralStatus> ReferralStatusList { get; set; }
        public List<EmployeeDropDownModel> EmployeeList { get; set; }
        public List<FrequencyCode> FrequencyCodeList { get; set; }
        public List<TransportLocation> TransportLocationList { get; set; }
        public List<NameValueDataInString> GenderList { get; set; }
        public List<Region> RegionList { get; set; }
        public List<Language> LanguageList { get; set; }
        public List<NameValueDataBoolean> PrivateRoomList { get; set; }
        public Contact Contact { get; set; }
        public ContactMapping ContactMapping { get; set; }
        public List<ContactType> ContactTypeList { get; set; }
        public List<NameValueData> ROITypes { get; set; }
        public List<NameValueDataBoolean> PrimaryContactGuardianList { get; set; }
        public List<NameValueDataBoolean> DCSGuardianList { get; set; }
        public List<NameValueDataBoolean> EmergencyContactList { get; set; }
        public List<NameValueDataBoolean> NoticeProviderOnFileList { get; set; }
        [Ignore]
        public List<NameValueDataBoolean> SignatureNeededList { get; set; }
        public List<AddAndListContactInformation> ContactInformationList { get; set; }
        public AddAndListContactInformation AddAndListContactInformation { get; set; }
        public AddAndListContactInformation AddContactModel { get; set; }
        public ContactSearchModel ContactSearchModel { get; set; }
        public List<AgencyModel> AgencyList { get; set; }
        [Ignore]
        public List<AgencyModel> CareGiverList
        {
            get
            {
                List<AgencyModel> data = AgencyList == null
                    ? AgencyList
                    : AgencyList.Where(c => c.AgencyType == Common.AgencyTypeEnum.CareGiver.ToString()).ToList();
                return data;
            }
        }
        public List<AgencyLocationModel> AgencyLocationList { get; set; }
        public List<CaseManager> CaseManagerList { get; set; }
        public CaseManager CaseManager { get; set; }
        public SetReferralInternalMessagePageLoad SetReferralInternalMessagePageLoad { get; set; }
        public List<NameValueDataBoolean> CareConsentList { get; set; }
        public List<NameValueDataBoolean> SelfAdministrationList { get; set; }
        public List<NameValueDataBoolean> HealthInformationList { get; set; }
        public List<NameValueDataBoolean> AdmissionRequirementList { get; set; }
        public List<NameValueDataBoolean> AdmissionOrientationList { get; set; }
        public List<NameValueDataInString> ZarephathCrisisPlanList { get; set; }
        public List<NameValueDataInString> NetworkCrisisPlanList { get; set; }
        public List<NameValueDataBoolean> VoiceMailList { get; set; }
        public List<NameValueDataBoolean> PermissionEmailList { get; set; }
        public List<NameValueDataBoolean> PermissionSMSList { get; set; }
        public List<NameValueDataBoolean> PHIList { get; set; }
        public List<NameValueDataInString> ROIList { get; set; }
        public List<NameValueDataBoolean> ZSPRespiteList { get; set; }
        public List<NameValueDataBoolean> ZSPLifeSkillsList { get; set; }
        public List<NameValueDataBoolean> ZSPCounsellingList { get; set; }
        public List<NameValueDataBoolean> ZSPRespiteGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> ZSPRespiteBHPSignedList { get; set; }
        public List<NameValueDataBoolean> ZSPLifeSkillsGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> ZSPLifeSkillsBHPSignedList { get; set; }
        public List<NameValueDataBoolean> ZSPCounsellingGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> ZSPCounsellingBHPSignedList { get; set; }
        public List<NameValueDataBoolean> NetworkServicePlanList { get; set; }
        public List<NameValueDataBoolean> NetworkServiceGuardianSignatureList { get; set; }
        public List<NameValueDataBoolean> NetworkServiceBHPSignedList { get; set; }
        public List<NameValueDataInString> NSPSPidentifyServiceList { get; set; }
        public List<NameValueDataBoolean> BXAssessmentList { get; set; }
        public List<NameValueDataBoolean> BXAssessmentBHPSignedList { get; set; }
        public List<NameValueDataInString> DemographicList { get; set; }
        public List<NameValueDataInString> SNCDList { get; set; }
        public List<NameValueDataBoolean> ACAssessmentList { get; set; }
        public List<DXCodeMappingList> DxCodeMappingList { get; set; }
        [Ignore]
        public ReferralBeneficiaryDetail ReferralBeneficiaryTypeDetail { get; set; }
        public List<LU_OutcomeMeasurementOption> LU_OutcomeMeasurementOption { get; set; }

        public ReferralSiblingMappingsList ReferralSiblingMappings { get; set; }
        public List<ReferralSiblingMappingsList> ReferralSiblingMappingList { get; set; }

        public List<DocumentTypeModel> DocumentTypeList { get; set; }
        //public List<DocumentType> InternalDocuments { get; set; }
        //public List<DocumentType> ExternalDocuments { get; set; }
        public List<NameValueDataBoolean> DocumentKind { get; set; }
        public List<Payor> PayorList { get; set; }
        public ReferralPayorMapping ReferralPayorMapping { get; set; }
        public ReferralDocument ReferralDocument { get; set; }
        public List<ListReferralPayorMapping> ReferralPayorMappingList { get; set; }
        public int ScheduleHistoryCount { get; set; }
        public List<ReferralSource> ReferralSources { get; set; }
        public DateTime ServiceDate { get; set; }
        public DXCodeMappingList ReferralDXCodeMapping { get; set; }


        //[Range(1, int.MaxValue, ErrorMessageResourceName = "DxCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int DXCodeCount { get { return DxCodeMappingList.Count(m => m.IsDeleted == false); } }

        public List<NameValueDataBoolean> PermissionMailList { get; set; }
        [Ignore]
        public AmazonSettingModel AmazonSettingModel { get; set; }

        [Ignore]
        public ReferralOutcomeMeasurement ReferralOutcomeMeasurement { get; set; }

        [Ignore]
        public SearchReferralOutcomeMeasurement SearchReferralOutcomeMeasurement { get; set; }

        [Ignore]
        public ReferralAssessmentReview ReferralAssessmentReview { get; set; }

        public SearchReferralAssessmentReview SearchReferralAssessmentReview { get; set; }

        [Ignore]
        public string DefaultTab { get; set; }

        [Ignore]
        public ReferralMonthlySummary ReferralMonthlySummary { get; set; }

        [Ignore]
        public SearchReferralMonthlySummary SearchReferralMonthlySummary { get; set; }

        [Ignore]
        public List<NameValueDataInString> BindMealsandSummaryofFood { get; set; }

        [Ignore]
        public List<NameValueDataInString> EnumCoordinationofCare { get; set; }
        [Ignore]
        public List<NameValueDataInString> SummaryofFood { get; set; }

        [Ignore]
        public List<NameValueDataInString> Coordinationofcare { get; set; }

        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public ReferralAhcccsDetails ReferralAhcccsDetails { get; set; }

        [Ignore]
        public bool NotifyCmForInactiveStatus { get; set; }

        [Ignore]
        public bool IsEditMode { get; set; }

        [Ignore]
        public ReferralPayorMapping UpdateReferralPayorMapping { get; set; }

        public List<ReferralPreferenceModel> PreferenceList { get; set; }


        public List<Preference> SkillList { get; set; }
        public List<string> ReferralSkillList { get; set; }
        [Ignore]
        public string StrReferralSkillList { get; set; }


        public List<State> StateList { get; set; }

        public List<CareType> CareTypeList { get; set; }

        public Physician PhysicianModel { get; set; }
        public List<NameValueData> AdmissionTypeList { get; set; }
        public List<NameValueData> PriorAuthorizationFrequencyList { get; set; }
        public List<NameValueData> AdmissionSourceList { get; set; }
        public List<NameValueData> PatientDischargeStatusList { get; set; }
        public List<NameValueData> FacilityCodeList { get; set; }
        public List<NameValueData> VisitTypeList { get; set; }
        public List<ReferralComplianceDetails> InternalDocumentList { get; set; }
        [Ignore]
        public List<DocumentSection> InternalDocumentSectionList { get; set; }
        public List<ReferralComplianceDetails> ExternalDocumentList { get; set; }
        [Ignore]
        public List<DocumentSection> ExternalDocumentSectionList { get; set; }

        [Ignore]
        public SearchReferralPayorMapping SearchReferralPayorMapping { get; set; }

        [Ignore]
        public SearchReferralNoteSentenceList SearchReferralNoteSentenceList { get; set; }

        [Ignore]
        public List<NameValueData> PrecedenceList { get; set; }
        [Ignore]
        public ReferralBillingSetting ReferralBillingSetting { get; set; }
        [Ignore]
        public ReferralBillingAuthorization ReferralBillingAuthorization { get; set; }
        [Ignore]
        public ReferralBillingAuthorizationServiceCode ReferralBillingAuthorizationServiceCode { get; set; }
        [Ignore]
        public SearchReferralBillingAuthorization SearchReferralBillingAuthorization { get; set; }
        [Ignore]
        public CareForm CareForm { get; set; }
        [Ignore]
        public List<ReferralComplianceDetails> DocumentList { get; set; }

        public List<NameValueData> PatientFrequencyCode { get; set; }
        public List<NameValueData> EmployeeGroupList { get; set; }
        public List<NameValueData> BeneficiaryTypes { get; set; }
        public List<ReferralBeneficiaryDetail> ReferralBeneficiaryTypes { get; set; }
        public List<ReferralPhysicianDetail> ReferralPhysicians { get; set; }


        public List<ServiceType> ServiceType { get; set; }

        [Ignore]
        public bool GoogleDriveValidated { get; set; }
        // Added by Kundan for new claim change request
        public List<RevenueCode> RevenueCodeList { get; set; }
        public List<ServiceCode> ServiceCodeList { get; set; }
        public List<NameValueStringData> TaxonomyCodes { get; set; }
        public List<NameValueStringData> RaceGroupList { get; set; }
        public List<NameValueStringData> EthnicityList { get; set; }
        public List<NameValueStringData> CodeStatusList { get; set; }
        public List<DosageTimeDetail> DosageTimes { get; set; }
    }



    public class DocumentTypeModel
    {
        public long DocumentTypeID { get; set; }
        public int DocumentKind { get; set; }
        public string DocumentTypeName { get; set; }
    }

    public class ReferralComplianceModel
    {
        public long ReferralID { get; set; }
        public List<ReferralComplianceDetails> DocumentList { get; set; }
    }

    public class ReferralComplianceDetails
    {
        public long ComplianceID { get; set; }
        public long ReferralID { get; set; }
        public long ReferralComplianceID { get; set; }
        public string DocumentName { get; set; }
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
        public bool Value { get; set; }
        public bool IsTimeBased { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class SearchCareFormDetails
    {
        public long CareFormID { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get; set; }
    }
    public class ContactSearchModel
    {
        public string SearchText { get; set; }
        public List<string> Contacts { get; set; }
    }

    // This model is used for filling the employee list i.e Assignee
    public class EmployeeDropDownModel
    {
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ReferralPreferenceModel
    {
        public long ReferralPreferenceID { get; set; }
        public long ReferralID { get; set; }
        public long PreferenceID { get; set; }
        public string PreferenceName { get; set; }
    }

    public class AddAndListContactInformation
    {
        [Required(ErrorMessageResourceName = "ContactTypeIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ContactTypeID { get; set; }

        public string ContactTypeName { get; set; }
        public int ROIType { get; set; }

        public DateTime? ROIExpireDate { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please Enter Alphabets Only")]
        public string LastName { get; set; }
        public string FullName { get; set; }
        //public string FullName
        //{
        //    get { return Common.GetGeneralNameFormat(FirstName, LastName); }
        //}

        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        public string ApartmentNo { get; set; }
        //[Required(ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        [RequiredIf("ContactType.ContactTypes.Primary_Placement==ContactTypeID || ContactType.ContactTypes.Legal_Guardian==ContactTypeID", ErrorMessageResourceName = "AddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        //[Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        [RequiredIf("ContactType.ContactTypes.Primary_Placement==ContactTypeID || ContactType.ContactTypes.Legal_Guardian==ContactTypeID", ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(Resource))]
        public string City { get; set; }

        //[Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        [RequiredIf("ContactType.ContactTypes.Primary_Placement==ContactTypeID || ContactType.ContactTypes.Legal_Guardian==ContactTypeID", ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(Resource))]
        public string State { get; set; }

        // [Required(ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        [RequiredIf("ContactType.ContactTypes.Primary_Placement==ContactTypeID || ContactType.ContactTypes.Legal_Guardian==ContactTypeID", ErrorMessageResourceName = "ZipCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        [Required(ErrorMessageResourceName = "PhoneRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Phone1 { get; set; }

        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Phone2 { get; set; }

        [Required(ErrorMessageResourceName = "LanguagePreferenceRequired", ErrorMessageResourceType = typeof(Resource))]
        public long LanguageID { get; set; }

        [Required(ErrorMessageResourceName = "PrimaryContactRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool IsPrimaryPlacementLegalGuardian { get; set; }

        [Required(ErrorMessageResourceName = "DCSLegalGuardianRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool IsDCSLegalGuardian { get; set; }

        [Required(ErrorMessageResourceName = "NoticeProviderOnFileRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool IsNoticeProviderOnFile { get; set; }

        [Required(ErrorMessageResourceName = "EmergencyContactRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool IsEmergencyContact { get; set; }


        //[RequiredIf("ContactType.ContactTypes.Primary_Placement==ContactTypeID || ContactType.ContactTypes.Legal_Guardian==ContactTypeID", ErrorMessageResourceName = "LatitudeRequired", ErrorMessageResourceType = typeof(Resource))]
        public double? Latitude { get; set; }
        //[RequiredIf("ContactType.ContactTypes.Primary_Placement==ContactTypeID || ContactType.ContactTypes.Legal_Guardian==ContactTypeID", ErrorMessageResourceName = "LongitudeRequired", ErrorMessageResourceType = typeof(Resource))]
        public double? Longitude { get; set; }



        public long ContactID { get; set; }

        public long ReferralID { get; set; }

        public long ClientID { get; set; }

        public long ContactMappingID { get; set; }

        public long TempContactMappingID { get; set; }

        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmpFullName { get; set; }
        // public string EmpFullName { get { return Common.GetGeneralNameFormat(EmpFirstName, EmpLastName); } }
        public string Relation { get; set; }

        public string EncryptedContactMappingID { get { return Crypto.Encrypt(Convert.ToString(ContactMappingID)); } }


        public bool MasterContactUpdated { get; set; }
        //ublic bool Duplicate { get; set; }

        public bool AddNewContactDetails { get; set; }
        public string ReferenceMasterID { get; set; }
        public long EmployeeID { get; set; }

    }

    public class ContactInformationModal
    {
        public ContactInformationModal()
        {
            AddAndListContactInformation = new List<AddAndListContactInformation>();
            Contact = new Contact();
        }

        public List<AddAndListContactInformation> AddAndListContactInformation { get; set; }
        public Contact Contact { get; set; }
    }

    public class ListContactInformation
    {
        public string Name { get; set; }
        public long ContactTypeID { get; set; }
        public string ContactTypeName { get; set; }
        public string Phone1 { get; set; }
        public string Address { get; set; }
        public string ROIExpireDate { get; set; }
        public string ROIType { get; set; }
        public string AddedBy { get; set; }
    }

    public class DeleteListReferralPayorMapping
    {
        public DeleteListReferralPayorMapping()
        {
            listReferralPayorMappings = new List<ListReferralPayorMapping>();
            listModel = new Page<ListReferralPayorMapping>();
        }
        public List<ListReferralPayorMapping> listReferralPayorMappings { get; set; }
        public int DuplicateRecords { get; set; }
        [Ignore]
        public Page<ListReferralPayorMapping> listModel { get; set; }
    }


    public class ListReferralPayorMapping
    {
        public long ReferralPayorMappingID { get; set; }
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime PayorEffectiveDate { get; set; }
        public DateTime? PayorEffectiveEndDate { get; set; }
        public int Precedence { get; set; }
        public string EncryptedReferralPayorMappingID { get { return Crypto.Encrypt(Convert.ToString(ReferralPayorMappingID)); } }
        public int Count { get; set; }

        public bool IsPayorNotPrimaryInsured { get; set; }
        public string InsuredId { get; set; }
        public string InsuredFirstName { get; set; }
        public string InsuredMiddleName { get; set; }
        public string InsuredLastName { get; set; }
        public string InsuredAddress { get; set; }
        public string InsuredCity { get; set; }
        public string InsuredState { get; set; }
        public string InsuredZipCode { get; set; }
        public string InsuredPhone { get; set; }
        public string InsuredPolicyGroupOrFecaNumber { get; set; }
        public DateTime? InsuredDateOfBirth { get; set; }
        public string InsuredGender { get; set; }
        public string InsuredEmployersNameOrSchoolName { get; set; }

        [Ignore]
        public string InsuredShowToolTip
        {
            get
            {
                return "<div>" +
                                "<b>" + Resource.InsuredSNumber + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredId) ? InsuredId : Resource.NA) + "<br />" +
                                "<b>" + Resource.FirstName + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredFirstName) ? InsuredFirstName : Resource.NA) + "<br />" +
                                "<b>" + Resource.MiddleName + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredMiddleName) ? InsuredMiddleName : Resource.NA) + "<br />" +
                                "<b>" + Resource.LastName + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredLastName) ? InsuredLastName : Resource.NA) + "<br />" +
                                "<b>" + Resource.InsuredSPolicyGroupFECANumber + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredPolicyGroupOrFecaNumber) ? InsuredPolicyGroupOrFecaNumber : Resource.NA) + "<br />" +
                                "<b>" + Resource.Address + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredAddress) ? InsuredAddress : Resource.NA) + "<br />" +
                                "<b>" + Resource.City + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredCity) ? InsuredCity : Resource.NA) + "<br />" +
                                "<b>" + Resource.State + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredState) ? InsuredState : Resource.NA) + "<br />" +
                                "<b>" + Resource.ZipCode + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredZipCode) ? InsuredZipCode : Resource.NA) + "<br />" +
                                "<b>" + Resource.Phone + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredPhone) ? InsuredPhone : Resource.NA) + "<br />" +
                                "<b>" + Resource.DateOfBirth + "<b/>: " + (InsuredDateOfBirth.HasValue ? InsuredDateOfBirth.Value.ToString(Constants.GlobalDateFormat) : Resource.NA) + "<br />" +
                                "<b>" + Resource.Gender + "<b/>: " + (InsuredGender == "1" ? Constants.Male : Constants.Female) + "<br />" +
                                "<b>" + Resource.EmployersNameSchoolName + "<b/>: " + (!string.IsNullOrWhiteSpace(InsuredEmployersNameOrSchoolName) ? InsuredEmployersNameOrSchoolName : Resource.NA) + "<br />" +
                       "</div>";
            }
        }

        public bool IsActive { get; set; }
        public string AddedByName { get; set; }
        public string UpdatedByName { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        public DateTime CreatedDate { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        public DateTime UpdatedDate { get; set; }

        public string BeneficiaryType { get; set; }
        public string BeneficiaryTypeID { get; set; }
        public string BeneficiaryNumber { get; set; }
        public string MemberID { get; set; }
        public int MasterJurisdictionID { get; set; }
        public int MasterTimezoneID { get; set; }
    }

    public class SearchReferralPayorMapping
    {
        public long ReferralID { get; set; }
        public string PayorName { get; set; }
        public int IsDeleted { get; set; }
        public int? Precedence { get; set; }
        public DateTime? PayorEffectiveDate { get; set; }
        public DateTime? PayorEffectiveEndDate { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public string BeneficiaryType { get; set; }
        public string BeneficiaryNumber { get; set; }
        public string BeneficiaryTypeID { get; set; }
    }

    public class SearchReferralNoteSentenceList
    {
        public string NoteSentenceTitle { get; set; }
        public string NoteSentenceDetails { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }

    }

    public class SearchReferralBillingAuthorization
    {
        public long PayorID { get; set; }
        public long ReferralID { get; set; }
        public string AuthorizationCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IsDeleted { get; set; }
        public int Type { get; set; }
        public string AuthType { get; set; }
        public string ListOfIdsInCSV { get; set; }
    }

    public class SearchReferralBillingAuthorizationCode
    {
        public long ReferralBillingAuthorizationID { get; set; }
    }


    public class AuthServiceCodesModel
    {
        public long ServiceCodeID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string Modifiers { get; set; }

        public string Taxonomy { get; set; }
        public int Rate { get; set; }
        public string RevenueCode { get; set; }
        public int PerUnitValue { get; set; }
        public int RoundUpMinutes { get; set; }
        public int MaxUnit { get; set; }
        public int DailyUnit { get; set; }
    }

    public class SearchAuthServiceCodesModel
    {
        public long ReferralBillingAuthorizationID { get; set; }
    }

    public class ReferralBillingAuthorizationList
    {
        public long ReferralID { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }

        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public long AllowedTime { get; set; }
        public string AllowedTimeType { get; set; }
        public string Type { get; set; }
        public long PriorAuthorizationFrequencyType { get; set; }
        public string PriorAuthorizationFrequencyTitle { get; set; }
        public string StrServiceCodeIDs { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFilePath { get; set; }
        public int UnitLimitFrequency { get; set; }
        //public List<string> ServiceCodeID
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(StrServiceCodeIDs) ? new List<string>() :
        //            StrServiceCodeIDs.Split(',').Select(c => c.Trim()).ToList();
        //    }

        //}

        public bool IsDeleted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EncryptedReferralBillingAuthorizationID { get { return Crypto.Encrypt(Convert.ToString(ReferralBillingAuthorizationID)); } }

        #region New Change In Billing - Kundan>Kumar>Rai
        public long ServiceCodeID { get; set; }

        public float Rate { get; set; }

        public long? RevenueCode { get; set; }

        public int UnitType { get; set; }

        public long CareType { get; set; }
        public string CareTypeName { get; set; }

        public float PerUnitQuantity { get; set; }

        public int RoundUpUnit { get; set; }

        public int MaxUnit { get; set; }

        public int DailyUnitLimit { get; set; }
        public string ServiceCodeName { get; set; }
        public string TaxonomyID { get; set; }
        public long ModifierID { get; set; }
        public string DxCode { get; set; }
        public string DxCodeID { get; set; }
        public string PayorInvoiceType { get; set; }
        #endregion

        public float PayRate { get; set; }

        public string FacilityCode { get; set; }

        public int Count { get; set; }
    }




    public class ReferralBillingAuthorizationServiceCodeModel
    {
        public long ReferralBillingAuthorizationServiceCodeID { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }
        public long ReferralID { get; set; }
        public long PayorID { get; set; }
        public long CareTypeID { get; set; }
        public long ServiceCodeID { get; set; }

        public string ServiceCode { get; set; }

        public int UnitType { get; set; }
        public long PerUnitQuantity { get; set; }

        public long DailyUnitLimit { get; set; }
        public long MaxUnitLimit { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class AuthorizationLinkup
    {
        public string Employee { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }
        public string PayorName { get; set; }
        public string CareType { get; set; }
        public string AuthorizationCode { get; set; }
        public string ScheduleIDs { get; set; }
        public int Cnt { get; set; }
    }

    public class SearchAuthorizationScheduleLinkList
    {
        public long ReferralBillingAuthorizationID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class UpdateAuthorizationLinkupModel
    {
        public long ReferralBillingAuthorizationID { get; set; }
        public string ScheduleIDs { get; set; }
    }








    public class GetAuthorizationLoadModel
    {
        public List<PatientPayorList> PatientPayorList { get; set; }
        //public List<NameValueData> ServiceCodeList { get; set; }
    }
    public class PatientPayorList
    {
        public string Name { get; set; }
        public long Value { get; set; }
        public string PayorIdentificationNumber { get; set; }
    }


    public class GetReferralPayorDetails
    {

        public GetReferralPayorDetails()
        {
            ReferralPayorMapping = new ReferralPayorMapping();
            ListReferralPayorMapping = new List<ListReferralPayorMapping>();
        }

        public ReferralPayorMapping ReferralPayorMapping { get; set; }
        public List<ListReferralPayorMapping> ListReferralPayorMapping { get; set; }

    }


    #region Referral Detail

    public class ReferralDetailModel
    {
        public string ReferralName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string ReferringAgency { get; set; }
        public string Payor { get; set; }
        public string FacilatorInformation { get; set; }
        public DateTime ReferralReceived { get; set; }
        public bool RespiteService { get; set; }
        public bool LifeSkillsService { get; set; }
        public bool CounselingService { get; set; }
        public bool ConnectingFamiliesService { get; set; }
        public DateTime? AHCCCSEnrollDate { get; set; }
        public bool NetworkServicePlan { get; set; }
        public DateTime? NSPExpirationDate { get; set; }
        public bool NSPBHPSigned { get; set; }
        public bool NSPGuardianSignature { get; set; }
        public DateTime? BXAssessmentExpirationDate { get; set; }

        public bool BXAssessment { get; set; }
        public bool BXAssessmentBHPSigned { get; set; }
        public string SNCD { get; set; }
        public string NSPSPIdentifyService { get; set; }
        public bool ZSPRespite { get; set; }

        public DateTime? SNCDExpirationDate { get; set; }
        public string Demographic { get; set; }
        public DateTime? DemographicExpirationDate { get; set; }
        public string AROI { get; set; }
        public long? AROIAgencyID { get; set; }
        public string ROIFromAgencyName { get; set; }
        public DateTime? AROIExpirationDate { get; set; }


        public string NickName { get; set; }
        public bool NotifyCaseManager { get; set; }
        public string DXCodes { get; set; }
        public bool IsCazDoc { get; set; }
        public DateTime? ReferralDate { get; set; }
        public bool ACAssessment { get; set; }

        public long AgencyID { get; set; }
        public long CaseManagerID { get; set; }

        public string CASIIScore { get; set; }
    }

    #endregion Referral Detail

    #region ReferralCheckList
    public class SetReferralCheckListModel
    {
        public SetReferralCheckListModel()
        {
            ReferralCheckList = new ReferralCheckList();
            ReferralDetailModel = new ReferralDetailModel();
            DxCodeMappingList = new List<DXCodeMappingList>();
            YesNoList = new List<NameValueDataBoolean>();
            AgencyList = new List<AgencyModel>();
            AgencyLocationList = new List<AgencyLocationModel>();
            CaseManagerList = new List<CaseManager>();
        }
        public ReferralCheckList ReferralCheckList { get; set; }
        public ReferralDetailModel ReferralDetailModel { get; set; }
        public List<DXCodeMappingList> DxCodeMappingList { get; set; }


        [Ignore]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "DxCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int DXCodeCount { get { return DxCodeMappingList.Count(m => m.IsDeleted == false); } }
        [Ignore]
        public DXCodeMappingList ReferralDXCodeMapping { get; set; }


        [Ignore]
        public List<NameValueDataBoolean> YesNoList { get; set; }
        [Ignore]
        public List<AgencyModel> AgencyList { get; set; }
        [Ignore]
        public List<AgencyLocationModel> AgencyLocationList { get; set; }
        [Ignore]
        public List<CaseManager> CaseManagerList { get; set; }




    }

    #endregion ReferralCheckList

    #region ReferralSparform
    public class SetReferralSparFormModel
    {
        public SetReferralSparFormModel()
        {
            ReferralSparForm = new ReferralSparForm();
            ReferralForReferralSparFormModel = new ReferralDetailModel();
        }
        public ReferralSparForm ReferralSparForm { get; set; }
        public ReferralDetailModel ReferralForReferralSparFormModel { get; set; }

    }

    #endregion ReferralSparform

    #region Referral internal message

    public class ReferralInternalMessageModel
    {
        public long ReferralInternalMessageID { get; set; }
        public string Note { get; set; }
        public long ReferralID { get; set; }
        public long Assignee { get; set; }
        public string AssigneeName { get; set; }
        public bool IsResolved { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [SetValueOnUpdate((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime UpdatedDate { get; set; }

        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public int Count { get; set; }
        public bool CanResolve { get; set; }
        public string EncryptedReferralInternalMessageID { get { return Crypto.Encrypt(Convert.ToString(ReferralInternalMessageID)); } }
    }

    public class SetReferralInternalMessagePageLoad
    {
        public SetReferralInternalMessagePageLoad()
        {
            ReferralInternalMessage = new ReferralInternalMessage();
            SearchReferralInternalMessage = new SearchReferralInternalMessage();
            AssigneeList = new List<NameValueData>();
        }
        public ReferralInternalMessage ReferralInternalMessage { get; set; }
        public SearchReferralInternalMessage SearchReferralInternalMessage { get; set; }
        public List<NameValueData> AssigneeList { get; set; }
    }

    public class SetReferralInternalMessage
    {
        public SetReferralInternalMessage()
        {
            ReferralInternalMessageList = new List<ReferralInternalMessage>();
            ReferralInternalMessage = new ReferralInternalMessage();
        }
        public List<ReferralInternalMessage> ReferralInternalMessageList { get; set; }
        public ReferralInternalMessage ReferralInternalMessage { get; set; }
        public SearchReferralInternalMessage SearchReferralInternalMessage { get; set; }
    }

    public class SearchReferralInternalMessage
    {
        public int Assignee { get; set; }
    }

    #endregion Referral internal message

    public class SearchReferralNotesModel
    {
        public long ReferralID { get; set; }
    }
    public class ReferralNotesModel
    {
        public long CommonNoteID { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public string Note { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public string RoleID { get; set; }
        public string EmployeesID { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public bool isPrivate { get; set; }

    }


    #region Missing Document

    public class ReferralMissingDocumentModel
    {
        public ReferralMissingDocumentModel()
        {
            EmailTemplate = new EmailTemplate();
            MissingDocumentList = new List<MissingDocument>();
            SetMissingDocumentTokenModel = new SetMissingDocumentTokenModel();
        }
        public EmailTemplate EmailTemplate { get; set; }
        public List<MissingDocument> MissingDocumentList { get; set; }
        public SetMissingDocumentTokenModel SetMissingDocumentTokenModel { get; set; }
    }

    public class SetMissingDocumentTokenModel
    {
        CacheHelper _cacheHelper = new CacheHelper();

        public string ToEmail { get; set; }
        public string ClientName { get; set; }
        public string MissingItems { get; set; }
        public string Email { get; set; }
        public string CaseManagerFirstName { get; set; }
        public string CaseManagerLastName { get; set; }
        public string ClientNickName { get; set; }
        public string AHCCCSID { get; set; }
        public string DateofBirth { get; set; }
        public string SiteName { get; set; }

        [Ignore]
        public string TagZerpathLogoImage
        {
            get
            {
                return "<img src='" + _cacheHelper.SiteBaseURL + _cacheHelper.TemplateLogo + "' width='300' style='float:center;'/>";
            }
        }
        [Ignore]
        public string ClientList { get; set; }
        [Ignore]
        public string CaseManager { get; set; }
    }
    public class MissingDocumentModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxMultipleEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        //[StringLength(100, ErrorMessageResourceName = "EmailLength", ErrorMessageResourceType = typeof(Resource))]
        public string ToEmail { get; set; }

        [Required(ErrorMessageResourceName = "SubjectRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceName = "EmailDescriptionRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Body { get; set; }

    }

    public class MissingDocument
    {
        public string MissingDocumentName { get; set; }
        public string DocumentationType { get; set; }
        public string MissingDocumentType { get; set; }
    }

    #endregion Missing Document

    #endregion Add Referral

    #region Referral List

    public class SendReceiptNotificationEmail
    {
        public SendReceiptNotificationEmail()
        {
            EmailTemplate = new EmailTemplate();
            ReceiptNotificationModel = new ReceiptNotificationModel();
        }

        public EmailTemplate EmailTemplate { get; set; }
        public ReceiptNotificationModel ReceiptNotificationModel { get; set; }
    }

    public class ReceiptNotificationModel
    {
        public string CaseManager { get; set; }
        public string ClientName { get; set; }
        public string ToEmail { get; set; }
        public string ClientNickName { get; set; }
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }

        [Ignore]
        public string ZerpathLogoImage { get; set; }

    }


    public class SendNotificationToCMForInactiveStatus
    {
        public SendNotificationToCMForInactiveStatus()
        {
            EmailTemplate = new EmailTemplate();
            NoticeModel = new NotificationToCMForInactiveModel();
        }

        public EmailTemplate EmailTemplate { get; set; }
        public NotificationToCMForInactiveModel NoticeModel { get; set; }
    }


    public class NotificationToCMForInactiveModel
    {
        CacheHelper _cacheHelper = new CacheHelper();
        public string CaseManager { get; set; }
        public string ClientName { get; set; }
        public string ToEmail { get; set; }
        public string AHCCCSID { get; set; }
        public string ClosureDate { get; set; }
        public string ClosureReason { get; set; }
        public string RecordRequestEmail { get; set; }

        public string ZerpathLogoImage
        {
            get { return "<img src='" + _cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage + "' width='300'/>"; }
        }

    }



    public class SendNotificationToCMForReferralAcceptedStatus
    {
        public SendNotificationToCMForReferralAcceptedStatus()
        {
            EmailTemplate = new EmailTemplate();
            NoticeModel = new NotificationToCMForReferralAcceptedModel();
        }

        public EmailTemplate EmailTemplate { get; set; }
        public NotificationToCMForReferralAcceptedModel NoticeModel { get; set; }
    }


    public class NotificationToCMForReferralAcceptedModel
    {
        CacheHelper _cacheHelper = new CacheHelper();
        public string CaseManager { get; set; }
        public string ClientName { get; set; }
        public string ToEmail { get; set; }
        public string AHCCCSID { get; set; }
        public string ZerpathLogoImage
        {
            get { return "<img src='" + _cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage + "' width='300'/>"; }
        }

    }

    public class SetVirtualVisitsListModel
    {
        public SetVirtualVisitsListModel()
        {
            SearchVirtualVisitsListModel = new SearchVirtualVisitsListModel();
            EmployeeList = new List<Employee>();
        }
        public SearchVirtualVisitsListModel SearchVirtualVisitsListModel { get; set; }
        public List<Employee> EmployeeList { get; set; }
        public string Token { get; set; }
    }

    public class VirtualVisitsListModel
    {
        public VirtualVisitsListModel()
        {
            SVVLModel = new SetVirtualVisitsListModel();
        }
        public SetVirtualVisitsListModel SVVLModel { get; set; }
        public string VisitType { get; set; }
    }

    public class SearchVirtualVisitsListModel
    {
        public long EmployeeID { get; set; }
        public string ReferralName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string VisitType { get; set; }
    }

    public class VirtualVisitsList
    {
        public long ReferralID { get; set; }

        [Ignore]
        public string ReferralIDEncrypted
        {
            get
            {
                return Crypto.Encrypt(ReferralID.ToString());
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralName { get; set; }

        //[Ignore]
        //public string ReferralName
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string ScheduleStatusName { get; set; }
        public long ReferralTSDateID { get; set; }
        public bool UsedInScheduling { get; set; }
        public bool OnHold { get; set; }
        public string ScheduleComment { get; set; }
        public long ScheduleID { get; set; }

        [Ignore]
        public bool UnAllocated
        {
            get { return ScheduleID == 0; }
        }

        public long ScheduleStatusID { get; set; }
        public bool IsDenied { get; set; }
        public bool IsPendingSchProcessed { get; set; }
        public DateTime StartDate { get; set; }

        [Ignore]
        public string StrStartTime
        {
            get
            {
                return String.Format("{0:MM/dd/yyyy hh:mm tt}", StartDate);
            }
        }

        public DateTime EndDate { get; set; }

        [Ignore]
        public string StrEndTime
        {
            get
            {
                return String.Format("{0:MM/dd/yyyy hh:mm tt}", EndDate);
            }
        }

        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmployeeName { get; set; }

        //[Ignore]
        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}

        public string EmpEmail { get; set; }
        public string EmpMobile { get; set; }
        public string EmployeeUniqueID { get; set; }
        public long EmployeeID { get; set; }
        public string strClockInTime { get; set; }
        public string strClockOutTime { get; set; }
        public bool IVRClockIn { get; set; }
        public bool IVRClockOut { get; set; }
        public bool? IsPCACompleted { get; set; }
        public string Payor { get; set; }
        public string CareType { get; set; }
        public int CareTypeId { get; set; }
        public bool IsApprovalRequired { get; set; }
        public bool IsVirtualVisit { get; set; }
        public bool IsBetween { get; set; }
        public bool IsPastVisit { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class SetReferralListModel
    {
        public SetReferralListModel()
        {
            Payors = new List<PayorModel>();
            ReferralStatuses = new List<ReferralStatus>();
            //ReferralStatuses = new List<NameValueData>();
            Assignee = new List<EmployeeModel>();
            AssigneeList = new List<EmployeeDropDownModel>();
            NotifyCaseManager = new List<NameValueData>();
            Checklist = new List<NameValueData>();
            ClinicalReview = new List<NameValueData>();
            Facillator = new List<CaseManagerModel>();
            LanguageModel = new List<LanguageModel>();
            RegionModel = new List<RegionModel>();


            Services = new List<NameValueData>();
            Agencies = new List<AgencyModel>();
            AgencyLocations = new List<AgencyLocationModel>();
            SearchReferralListModel = new SearchReferralListModel();
            Draft = new List<NameValueData>();
            DeleteFilter = new List<NameValueData>();
            ServiceTypeList = new List<ServiceTypeModel>();
        }
        public List<PayorModel> Payors { get; set; }
        public List<ReferralStatus> ReferralStatuses { get; set; }
        //public List<NameValueData> ReferralStatuses { get; set; }
        public List<EmployeeModel> Assignee { get; set; }
        public List<EmployeeDropDownModel> AssigneeList { get; set; }
        public List<NameValueData> NotifyCaseManager { get; set; }
        public List<NameValueData> Checklist { get; set; }
        public List<NameValueData> ClinicalReview { get; set; }
        public List<CaseManagerModel> Facillator { get; set; }
        public List<LanguageModel> LanguageModel { get; set; }
        public List<RegionModel> RegionModel { get; set; }
        public List<NameValueData> Services { get; set; }
        public List<AgencyModel> Agencies { get; set; }
        public List<AgencyLocationModel> AgencyLocations { get; set; }
        public SearchReferralListModel SearchReferralListModel { get; set; }
        public List<NameValueData> Draft { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public List<ServiceTypeModel> ServiceTypeList { get; set; }
    }

    public class SearchReferralListModel
    {
        public long PayorID { get; set; }
        public long ReferralStatusID { get; set; }
        public long AssigneeID { get; set; }
        public string ClientName { get; set; }
        public int NotifyCaseManagerID { get; set; }
        public int ChecklistID { get; set; }
        public int ClinicalReviewID { get; set; }
        public long CaseManagerID { get; set; }
        public int ServiceID { get; set; }
        public long AgencyID { get; set; }
        public long AgencyLocationID { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsSaveAsDraft { get; set; }
        public int IsDeleted { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
        public string CaseManagerPhone { get; set; }
        public long LanguageID { get; set; }
        public long RegionID { get; set; }
        public string[] ServiceTypeID { get; set; }
        public string CommaSeparatedServiceTypeIDs { get; set; }
        public string[] GroupIds { get; set; }
        public string CommaSeparatedIds { get; set; }
        public string PayorName { get; set; }
        public string CareTypeID { get; set; }


    }

    public class PayorModel
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string ShortName { get; set; }
    }

    public class CaseManagerModel
    {
        public long CaseManagerID { get; set; }

        public string Name { get; set; }
    }

    public class LanguageModel
    {
        public long LanguageID { get; set; }
        public string Name { get; set; }
    }

    public class AgencyModel
    {
        public long AgencyID { get; set; }
        public string NickName { get; set; }
        public string AgencyType { get; set; }
    }

    public class RegionModel
    {
        public long RegionID { get; set; }
        public string RegionName { get; set; }
    }

    public class AgencyLocationModel
    {
        public long AgencyLocationID { get; set; }
        public string LocationName { get; set; }
        public long AgencyID { get; set; }
    }

    public class ReferralStatusModel
    {
        public long ReferralID { get; set; }
        public int ReferralStatusID { get; set; }
        public long AssigneeID { get; set; }
        public bool NotifyCmForInactiveStatus { get; set; }
    }
    public class ReferralBulkUpdateModel
    {
        public string BulkUpdateType { get; set; }
        public string ReferralIDs { get; set; }
        public string AssignedValues { get; set; }
    }

    public class ReferralCommentModel
    {
        public long ReferralID { get; set; }
        public string Comment { get; set; }
        public long AssigneeID { get; set; }
    }

    public class DXCodeMappingList
    {
        public long ReferralDXCodeMappingID { get; set; }

        //[Required(ErrorMessageResourceName = "DxCodeMendatory", ErrorMessageResourceType = typeof(Resource))]
        public string DXCodeID { get; set; }

        public string DXCodeName { get; set; }
        public string Description { get; set; }
        //[Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }

        //[Required(ErrorMessageResourceName = "PrecedenceRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? Precedence { get; set; }

        public string DxCodeType { get; set; }
        public string DxCodeShortName { get; set; }
        public string DXCodeWithoutDot { get; set; }
        public bool IsChecked { get; set; }

        //[SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        //[SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long CreatedBy { get; set; }

        [Ignore]
        public string RandomID
        {
            get { return Common.GenerateRandomNumber(); }
        }
    }

    public class DosageTimeDetail
    {
        public string Name { get; set; }
        public long Value { get; set; }
    }

    public class ReferralPhysicianDetail
    {
        public long ReferralPhysicianID { get; set; }
        public long ReferralID { get; set; }
        public long PhysicianID { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PhysicianType { get; set; }
        public string PhysicianTypeID { get; set; }
        public bool IsDeleted { get; set; }

        //[SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        //[SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long CreatedBy { get; set; }

        [Ignore]
        public string EncryptedReferralID
        {
            get { return Crypto.Encrypt(Convert.ToString(ReferralID)); }
        }

        [Ignore]
        public string RandomID
        {
            get { return Common.GenerateRandomNumber(); }
        }
        public string PhysicianName { get; set; }
    }

    public class ReferralBeneficiaryDetail
    {
        public long ReferralBeneficiaryTypeID { get; set; }
        public long ReferralID { get; set; }

        public long BeneficiaryTypeID { get; set; }
        public string BeneficiaryTypeName { get; set; }
        public string BeneficiaryNumber { get; set; }
        public bool IsDeleted { get; set; }

        //[SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        //[SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.LoggedInUserId)]
        public long CreatedBy { get; set; }

        [Ignore]
        public string EncryptedReferralID
        {
            get { return Crypto.Encrypt(Convert.ToString(ReferralID)); }
        }

        [Ignore]
        public string RandomID
        {
            get { return Common.GenerateRandomNumber(); }
        }
    }

    public class ReferralSiblingMappingsList
    {
        public long ReferralSiblingMappingID { get; set; }
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Phone1 { get; set; }
        public string Address { get; set; }
        public string ParentName { get; set; }
        public string Email { get; set; }
        public long ReferralID1 { get; set; }
        public long ReferralID2 { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string ParentPhone { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        public long CreatedBy { get; set; }
    }

    public class GetReferralInfoList
    {
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Phone1 { get; set; }
        public string Address { get; set; }
        public string ParentName { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public int ReferralStatusID { get; set; }
        public string ParentPhone { get; set; }
        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
    }

    #endregion Referral List

    #region Referral Tracking
    public class SetReferralTrackingListModel
    {
        public SetReferralTrackingListModel()
        {
            Payors = new List<PayorModel>();
            ReferralStatuses = new List<ReferralStatus>();
            Facillator = new List<CaseManagerModel>();
            Agencies = new List<AgencyModel>();
            SearchReferralTrackingListModel = new SearchReferralTrackingListModel();
            DeleteFilter = new List<NameValueData>();
        }
        public List<PayorModel> Payors { get; set; }
        public List<ReferralStatus> ReferralStatuses { get; set; }
        public List<CaseManagerModel> Facillator { get; set; }
        public List<AgencyModel> Agencies { get; set; }
        public SearchReferralTrackingListModel SearchReferralTrackingListModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchReferralTrackingListModel
    {


        public long ReferralStatusID { get; set; }
        public string ClientName { get; set; }
        public string AHCCCSID { get; set; }
        public long PayorID { get; set; }
        public long AgencyID { get; set; }
        public long CaseManagerID { get; set; }


        public DateTime? CMNotifiedDate { get; set; }
        public DateTime? ReferralDate { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? CreatedDate { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? ChecklistDate { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SparDate { get; set; }



        public DateTime? CMNotifiedToDate { get; set; }
        public DateTime? ReferralToDate { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? CreatedToDate { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? ChecklistToDate { get; set; }


        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SparToDate { get; set; }


        public string ReferralTrackingComment { get; set; }

        public string ListOfIdsInCsv { get; set; }

    }


    public class ReferralTrackingList
    {
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public long ClientID { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }

        public int ReferralStatusID { get; set; }
        public string Status { get; set; }
        public long Assignee { get; set; }
        public bool IsSaveAsDraft { get; set; }
        public string AssigneeName { get; set; }
        public long PayorID { get; set; }
        public string ContractName { get; set; }
        public long CaseManagerID { get; set; }
        public string FaciliatorName { get; set; }
        public bool ZSPLifeSkills { get; set; }
        public bool ZSPRespite { get; set; }
        public bool ZSPCounselling { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        public DateTime UpdatedDate { get; set; }

        public string UpdatedName { get; set; }
        public string CreatedName { get; set; }
        public string CompanyName { get; set; }
        public string LocationName { get; set; }
        public bool IsChecklistCompleted { get; set; }
        public long ChecklistCompletedBy { get; set; }
        public string ChecklistName { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? ChecklistCompletedDate { get; set; }
        public bool IsSparFormCompleted { get; set; }

        public long SparFormCompletedBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SparFormCompletedDate { get; set; }


        public DateTime? ReferralDate { get; set; }
        public DateTime? NotifyCaseManagerDate { get; set; }


        public string ClinicalReviewName { get; set; }
        public string PlacementRequirement { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public bool NotifyCaseManager { get; set; }
        public long AgencyID { get; set; }
        public long AgencyLocationID { get; set; }
        public bool CounselingService { get; set; }
        public bool LifeSkillsService { get; set; }
        public bool RespiteService { get; set; }

        public string Services
        {
            get
            {
                List<string> str = new List<string>();
                if (RespiteService)
                {
                    str.Add(Constants.Respite);
                }
                if (LifeSkillsService)
                {
                    str.Add(Constants.Life_Skills);
                }
                if (CounselingService)
                {
                    str.Add(Constants.Counselling);
                }
                if (str.Count > 0)
                {
                    return string.Join(",", str.ToArray());
                }
                return string.Empty;
            }
        }

        public string ReferralTrackingComment { get; set; }

        public bool IsDeleted { get; set; }

        public string Age { get; set; }
        public string Gender { get; set; }


        public int LastReferralStatusID { get { return ReferralStatusID; } }

        public long Row { get; set; }
        public int Count { get; set; }


    }
    #endregion

    #region AUDIT LOG MODEL
    public class RefAuditLogPageModel
    {
        public RefAuditLogPageModel()
        {
            SearchRefAuditLogListModel = new SearchRefAuditLogListModel();
        }

        public SearchRefAuditLogListModel SearchRefAuditLogListModel { get; set; }
    }


    public class SearchRefAuditLogListModel
    {
        public string EncryptedReferralID { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedFromDate { get; set; }
        public DateTime? UpdatedToDate { get; set; }
        public string Table { get; set; }
        public string ActionName { get; set; }

    }
    public class AuditChangeModel
    {

        public long AuditLogID { get; set; }
        public long ParentKeyFieldID { get; set; }
        public long ChildKeyFieldID { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string DataModel { get; set; }
        //public string ValueBefore { get; set; }
        //public string ValueAfter { get; set; }
        public string Changes { get; set; }
        public string AuditActionType { get; set; }

        public string CreatedByName { get; set; }
        public string UserName { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime UpdatedDate { get; set; }

        public long UpdatedBy { get; set; }

        public string SystemID { get; set; }


        public int Count { get; set; }



        public List<AuditDelta> ChangesList
        {
            get
            {
                List<AuditDelta> list = new List<AuditDelta>();
                if (!string.IsNullOrEmpty(Changes))
                {
                    try
                    {
                        list = JsonConvert.DeserializeObject<List<AuditDelta>>(Changes);
                    }
                    catch
                    { }
                }
                return list;
            }
        }

    }

    #endregion

    #region BX CONTRACT MODEL

    public class RefBXContractPageModel
    {
        public RefBXContractPageModel()
        {
            SearchRefBXContractListModel = new SearchRefBXContractListModel();
            ReferralBehaviorContract = new ReferralBehaviorContract();
            ReferralSuspention = new ReferralSuspention();
        }

        public SearchRefBXContractListModel SearchRefBXContractListModel { get; set; }
        public ReferralBehaviorContract ReferralBehaviorContract { get; set; }
        public ReferralSuspention ReferralSuspention { get; set; }
    }
    public class SearchRefBXContractListModel
    {
        public string EncryptedReferralID { get; set; }
    }
    public class BXContractModel
    {
        public long ReferralBehaviorContractID { get; set; }
        public DateTime WarningDate { get; set; }
        public string WarningReason { get; set; }
        public DateTime? CaseManagerNotifyDate { get; set; }
        public long ReferralID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public string CreatedByName { get; set; }
        public string UserName { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }


        public int Count { get; set; }

    }

    #endregion

    #region Block Employee

    #region BX CONTRACT MODEL

    public class RefBlockEmpPageModel
    {
        public RefBlockEmpPageModel()
        {
            SearchRefBlockEmpListModel = new SearchRefBlockEmpListModel();
            ReferralBlockedEmployee = new ReferralBlockedEmployee();
            EmployeeList = new List<EmployeeDropDownModel>();
        }

        public SearchRefBlockEmpListModel SearchRefBlockEmpListModel { get; set; }
        public ReferralBlockedEmployee ReferralBlockedEmployee { get; set; }
        public List<EmployeeDropDownModel> EmployeeList { get; set; }
    }
    public class SearchRefBlockEmpListModel
    {
        public string EncryptedReferralID { get; set; }
        public long ReferralID { get; set; }
    }


    public class BlockEmployeeList : ReferralBlockedEmployee
    {
        public string Employee { get; set; }
        public int Count { get; set; }
    }



    #endregion


    #endregion

    #region Referral Assessment Review
    public class SearchReferralAssessmentReview
    {
        public long ReferralID { get; set; }
        public long ReferralAssessmentID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class ReferralAssessmentReviewListModel : ReferralAssessmentReview
    {

        public float OverAllAverage
        {
            get
            {
                return (Permanency + DailyLiving + SelfCare + RelationshipsAndCommunication + HousingAndMoneyManagement +
                        WorkAndStudyLife + CareerAndEducationPlanning + LookingForward) / 8;
            }
        }
    }

    public class ReferralAssessmentReviewGraphModel
    {
        public string title { get; set; }
        public string type { get { return "bar"; } }
        public int interval { get { return 1; } }
        public List<List<string>> data { get; set; }
    }

    public class ReferralAssessmentReviewDetail
    {
        public List<ReferralAssessmentReviewListModel> ReferralAssessmentList { get; set; }
        public List<ReferralAssessmentReviewGraphModel> GraphSeriesList { get; set; }
    }

    #endregion


    #region Referral Outcome Measurement
    public class SearchReferralOutcomeMeasurement
    {
        public long ReferralID { get; set; }
        public long ReferralOutcomeMeasurementID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ReferralOutcomeMeasurementGraphModel
    {
        public string title { get; set; }
        public string type { get { return "bar"; } }
        public int interval { get { return 1; } }
        public List<List<string>> data { get; set; }
    }

    public class ReferralOutcomeMeasurementDetail
    {
        public List<ReferralOutcomeMeasurement> ReferralOutcomeMeasurementList { get; set; }
        public List<ReferralOutcomeMeasurementGraphModel> GraphSeriesOutcomeMeasurementList { get; set; }
    }



    #endregion


    #region Referral Monthly Summery

    public class SearchReferralMonthlySummary
    {
        public long ReferralID { get; set; }
        public long FacilityID { get; set; }
        public long RegionID { get; set; }
        public string ClientName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class ReferralMonthlySummaryListModel
    {
        public long ReferralMonthlySummariesID { get; set; }
        public long ReferralID { get; set; }
        public DateTime MonthlySummaryStartDate { get; set; }
        public DateTime MonthlySummaryEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedName { get; set; }
        public string FacilityName { get; set; }
        public string RegionName { get; set; }
        public string ClientName { get; set; }
        public string TreatmentPlan { get; set; }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
    }

    #endregion

    #region Medication Model
    public class MedicationModel
    {
        public MedicationModel()
        {
            MedicationId = string.Empty;
            MedicationName = string.Empty;
            Generic_Name = string.Empty;
            Brand_Name = string.Empty;
            Product_Type = string.Empty;
            Route = string.Empty;
            IsActive = string.Empty;
            Dosage_Form = string.Empty;
            Dose = string.Empty;
        }
        public String MedicationId { get; set; }
        public String MedicationName { get; set; }
        public String Generic_Name { get; set; }
        public String Brand_Name { get; set; }
        public String Product_Type { get; set; }
        public String Route { get; set; }
        public String IsActive { get; set; }
        public String DosageTime { get; set; }
        public String Dosage_Form { get; set; }
        public string Dose { get; set; }
    }
    #endregion

    #region Referral Allergy Model

    public class ReferralAllergy
    {
        public ReferralAllergy()
        {
            ReferralAllergyList = new List<ReferralAllergyModel>();
            //Id = string.Empty;
            //Allergy = string.Empty;
            //Status = string.Empty;
            //Reaction = string.Empty;
            //Comment = string.Empty;
            //Patient = string.Empty;
            //CreatedBy = string.Empty;
            //CreatedOn = string.Empty;
            //UpdatedBy = string.Empty;
            //UpdatedOn = string.Empty;
            //ReportedBy = string.Empty;
        }
        public List<ReferralAllergyModel> ReferralAllergyList { get; set; }
        //public string Id { get; set; }
        //public string Allergy { get; set; }
        //public string Status { get; set; }
        //public string Reaction { get; set; }
        //public string Comment { get; set; }
        //public string Patient { get; set; }
        //public string ReportedBy { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedOn { get; set; }
        //public string UpdatedBy { get; set; }
        //public string UpdatedOn { get; set; }
    }
    public class ReferralAllergyModel
    {
        public long Id { get; set; }
        public string Allergy { get; set; }
        public long Status { get; set; }
        public string Reaction { get; set; }
        public string Comment { get; set; }
        public string Patient { get; set; }
        public string ReportedBy { get; set; }
        public string ReportedByName { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }

    }
    public class AllergyDropDown
    {
        public AllergyDropDown()
        {
            Title = string.Empty;
            Value = string.Empty;
            Name = string.Empty;
        }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
    public class SearchAllergyModel
    {
        public string ReferralId { get; set; }
        public string Allergy { get; set; }
        public string Status { get; set; }
    }

    public class FaxSettings
    {
        public string Fax { get; set; }
        public string FaxTwilioAccountSID { get; set; }
        public string FaxAuthToken { get; set; }
    }


    #endregion


    public class ReferralAhcccsDetails
    {
        public long ReferralID { get; set; }

        [Ignore]
        //[Required(ErrorMessageResourceName = "AHCCCSIDRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(@"^[a-zA-Z]{1}[0-9]{1,9}$", ErrorMessageResourceName = "AHCCCSIDInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string NewAHCCCSID { get; set; }

        public string AHCCCSID { get; set; }
    }
    public class ReferralAhcccsUpdateModel
    {
        public ReferralAhcccsUpdateModel()
        {
            UpdatedReferral = new Referral();
        }


        public int ReturnValue { get; set; }
        [Ignore]
        public Referral UpdatedReferral { get; set; }
    }

    public class DayCareTimeSlotModal
    {
        public long ReferralID { get; set; }
        public long ReferralTimeSlotMasterID { get; set; }
        public bool GeneratePatientSchedules { get; set; }
    }
    public class CareTypeModel
    {
        public long CareTypeID { get; set; }
        public string CareType { get; set; }
    }
    public class ServiceTypeModel
    {
        public long ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
    }
    public class ReferralMedication
    {
        public ReferralMedication()
        {
            AddMedicationModel = new MedicationModel();
        }
        public long ReferralMedicationID { get; set; }
        public long ReferralID { get; set; }
        public long MedicationId { get; set; }
        public long PhysicianID { get; set; }
        public string Dose { get; set; }
        public string Unit { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Quantity { get; set; }
        public string IsActive { get; set; }
        public string SystemID { get; set; }
        public string DosageTime { get; set; }
        public string DosageTimeIds { get; set; }
        public string HealthDiagnostics { get; set; }
        public string PatientInstructions { get; set; }
        public string PharmacistInstructions { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public MedicationModel AddMedicationModel { get; set; }
    }

    //public class ReferralAllergy
    //{
    //    public ReferralAllergy()
    //    {
    //        AddAllergyModel = new ReferralAllergyModel();
    //    }
    //    public long Id { get; set; }
    //    public string Allergy { get; set; }
    //    public bool Status { get; set; }
    //    public string Reaction { get; set; }
    //    public string Comment { get; set; }
    //    public long Patient { get; set; }
    //    public long CreatedBy { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //    public long UpdatedBy { get; set; }
    //    public DateTime UpdatedOn { get; set; }
    //    public ReferralAllergyModel AddAllergyModel { get; set; }

    //}




    public class ReferralMedicationDetails
    {
        public long ReferralMedicationID { get; set; }
        public long ReferralID { get; set; }
        public string MedicationName { get; set; }
        public string PhysicianName { get; set; }
        public string Dose { get; set; }
        public string Unit { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Quantity { get; set; }
        public bool IsActive { get; set; }
        public string SystemID { get; set; }
        public string HealthDiagnostics { get; set; }
        public string PatientInstructions { get; set; }
        public string PharmacistInstructions { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MedicationId { get; set; }
        public string PhysicianID { get; set; }
        public string DosageTime { get; set; }
        public string DosageTimeIds { get; set; }
    }

    public class DXCodeModels
    {
        public long DXCodeID { get; set; }
        public string DXCodeName { get; set; }
        public string DXCodeWithoutDot { get; set; }
        public string DxCodeType { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool IsDeleted { get; set; }
        public string DxCodeShortName { get; set; }
    }

    public class SearchMedication
    {
        public string generic_name { get; set; }
        public string dosage_form { get; set; }
        public string labeler_name { get; set; }
        public string route { get; set; }
    }


    public class ReferralCertificate
    {
        public long CertificateID { get; set; }
        public string CertificatePath { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long EmployeeID { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public HttpPostedFileBase[] CFile { get; set; }
        public string CertificateAuthority { get; set; }
        //public long CertificateAuthorityID { get; set; }
        public string CertificateAuthorityID { get; set; }
    }

    public class MailModel
    {
        public string ReferralDocumentID { get; set; }
        public string Template { get; set; }
        public List<string> Attachment { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public HttpPostedFile ResourceFile { get; set; }
    }

    public class AddRecepient
    {
        public List<PhysicanList> physicanList { get; set; }
        public DisplayCaseManager casemanager { get; set; }
        public DisplayAssignee assignee { get; set; }
        public List<DisplayRelative> relative { get; set; }
        public List<Receipents> Receipent { get; set; }
    }

    public class Receipents
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class DisplayCaseManager
    {
        public string CaseManager { get; set; }
        public string Email { get; set; }
    }

    public class DisplayRelative
    {
        public string Relative { get; set; }
        public string Email { get; set; }
    }

    public class DisplayAssignee
    {
        public string Assignee { get; set; }
        public string Email { get; set; }
    }

    public class PhysicanList
    {
        public string PhysicianName { get; set; }
        public string Email { get; set; }
    }

    public class EmailTemplateList
    {
        public long EmailTemplateID { get; set; }
        public string EmailTemplateName { get; set; }
    }
    public class OrganizationSettings
    {
        public long OrganizationID { get; set; }
        public string FromEmail { get; set; }
        public bool ScheduleType { get; set; }
    }

    public class EmpSignature
    {
        public string SignaturePath { get; set; }
    }

    public class DisplayEmailTemplate
    {
        public string EmailTemplateSubject { get; set; }
        public string EmailTemplateBody { get; set; }
    }
    public class ReferralNurseSchModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }
        //[Ignore]
        //public string FullName { get { return Common.GetReferralNameFormat(FirstName, LastName); } }

    }


    public class FaxModel
    {
        public string To { get; set; }
        public string ReferralID { get; set; }
        public string Path { get; set; }
        public string DocumentID { get; set; }

    }

    public class DXCodeMappingList1
    {
        public long ReferralDXCodeMappingID { get; set; }
        public long ReferralID { get; set; }
        public string DXCodeID { get; set; }
        public int? Precedence { get; set; }
        public string DXCodeName { get; set; }
        public string DXCodeWithoutDot { get; set; }
        public string DxCodeType { get; set; }
        public string Description { get; set; }
        public string DxCodeShortName { get; set; }
    }

    public class ReferralEmail
    {
        public string ReferralID { get; set; }
        public string Email { get; set; }

    }

    public class GetReferralStatusModel
    {
        public int ReferralStatusID { get; set; }
        public string Status { get; set; }
        public int StatusCount { get; set; }
        public int TotalStatus { get; set; }
        public int Count { get; set; }
    }

    public class GetReferralCareTypeModel
    {
        public int CareTypeID { get; set; }
        public string CareType { get; set; }
        public int CareTypeCount { get; set; }
        public int TotalCareType { get; set; }
        public int Count { get; set; }
    }

    public class GetReferralAuthorizationCodeDetailsModel
    {
        public string Name { get; set; }
        public long Value { get; set; }
        // public int ReferralBillingAuthorizationID { get; set; }
        //public string AuthorizationCode { get; set; }
        public string CareType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
    }
    public class ReferralBillingAuthorizations
    {
        public long ReferralBillingAuthorizationID { get; set; }
        public long ReferralID { get; set; }
        public string AuthorizationCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public long? AllowedTime { get; set; }
        public int UnitType { get; set; }
        public int MaxUnit { get; set; }
        public int DailyUnitLimit { get; set; }
        public int UnitLimitFrequency { get; set; }
        public string ServiceCode { get; set; }
        public string CareType { get; set; }
        public long CareTypeID { get; set; }
    }
    public class GetPayorIdentificationNumberModel
    {
        public string PayorIdentificationNumber { get; set; }
    }
    public class DXcodeListModel
    {
        public long ReferralDXCodeMappingID { get; set; }
        public long ReferralID { get; set; }
        public String DXCodeID { get; set; }
        public string Precedence { get; set; }
        public string DXCodeName { get; set; }
        public string DXCodeWithoutDot { get; set; }
        public string Description { get; set; }

        public bool IsChecked { get; set; }
    }
    public class DxChangeSortingOrderModel
    {
        public long ReferralDXCodeMappingID { get; set; }
        public long originID { get; set; }
        public long destinationID { get; set; }
    }
    public class DxModel
    {
        public string ReferralBillingAuthorizationID { get; set; }
        public string DXCodeID { get; set; }
    }

}
