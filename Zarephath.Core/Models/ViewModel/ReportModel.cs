using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ExportToExcel;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{

    public class ReportModel
    {
        public ReportModel()
        {
            ReferralStatuses = new List<ReferralStatus>();
            Region = new List<RegionListModel>();
            Agencies = new List<AgencyListModel>();
            ScheduleStatusListModel = new List<ScheduleStatusListModel>();
            Payors = new List<PayorListModel>();
            Assignee = new List<EmployeeModel>();
            AssigneeList = new List<EmployeeDropDownModel>();
            AgencyLocations = new List<AgencyLocationListModel>();
            Facillator = new List<CaseManagerListModel>();
            Employees = new List<NameValueData>();
            NotifyCaseManager = new List<NameValueData>();

            Checklist = new List<NameValueData>();
            ClinicalReview = new List<NameValueData>();

            Services = new List<NameValueData>();
            SearchReportModel = new SearchReportModel();
            Draft = new List<NameValueData>();
            DeleteFilter = new List<NameValueData>();
            ExpireDateFilter = new List<NameValueData>();
            FacilityList = new List<NameValueData>();

            BXContractStatusList = new List<NameValueData>();



            SearchRespiteUsageModel = new SearchRespiteUsageModel();
            SearchEncounterPrintModel = new SearchEncounterPrintModel();
            SearchSnapshotPrintModel = new SearchSnapshotPrintModel();
            SearchDTRPrintModel = new SearchDTRPrintModel();
            SearchGeneralNoticeModel = new SearchGeneralNoticeModel();
            SearchDspRosterModel = new SearchDspRosterModel();
            SearchScheduleAttendanceModel = new SearchScheduleAttendanceModel();
            SearchReqDocsForAttendanceModel = new SearchReqDocsForAttendanceModel();
            SearchLSTeamMemberCaseloadReport = new SearchLSTeamMemberCaseloadReport();
            SearchBXContractStatusReport = new SearchBXContractStatusReport();
            SearchRequestClientListModel = new SearchRequestClientListModel();
        }

        public List<ReferralStatus> ReferralStatuses { get; set; }
        public List<RegionListModel> Region { get; set; }
        public List<AgencyListModel> Agencies { get; set; }
        public List<ScheduleStatusListModel> ScheduleStatusListModel { get; set; }

        public List<PayorListModel> Payors { get; set; }
        public List<EmployeeModel> Assignee { get; set; }
        public List<EmployeeDropDownModel> AssigneeList { get; set; }
        public List<AgencyLocationListModel> AgencyLocations { get; set; }
        public List<CaseManagerListModel> Facillator { get; set; }
        public List<NameValueData> Employees { get; set; }

        public List<NameValueData> NotifyCaseManager { get; set; }


        public List<NameValueData> Checklist { get; set; }
        public List<NameValueData> ClinicalReview { get; set; }
        public List<NameValueData> Services { get; set; }
        public List<NameValueData> Draft { get; set; }
        public SearchReportModel SearchReportModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public List<NameValueData> ExpireDateFilter { get; set; }
        public List<NameValueData> FacilityList { get; set; }


        [Ignore]
        public List<NameValueData> BXContractStatusList { get; set; }


        [Ignore]
        public SearchRespiteUsageModel SearchRespiteUsageModel { get; set; }
        [Ignore]
        public SearchEncounterPrintModel SearchEncounterPrintModel { get; set; }
        [Ignore]
        public SearchSnapshotPrintModel SearchSnapshotPrintModel { get; set; }

        [Ignore]
        public SearchDTRPrintModel SearchDTRPrintModel { get; set; }
        [Ignore]
        public SearchGeneralNoticeModel SearchGeneralNoticeModel { get; set; }
        [Ignore]
        public SearchDspRosterModel SearchDspRosterModel { get; set; }
        [Ignore]
        public SearchScheduleAttendanceModel SearchScheduleAttendanceModel { get; set; }
        [Ignore]
        public SearchReqDocsForAttendanceModel SearchReqDocsForAttendanceModel { get; set; }

        [Ignore]
        public SearchLSTeamMemberCaseloadReport SearchLSTeamMemberCaseloadReport { get; set; }
        [Ignore]
        public SearchBXContractStatusReport SearchBXContractStatusReport { get; set; }

        [Ignore]
        public SearchRequestClientListModel SearchRequestClientListModel { get; set; }
    }

    public class DownloadFileModel
    {
        public string AbsolutePath { get; set; }
        public string VirtualPath { get; set; }
        public string FileName { get; set; }
    }

    public class PayorListModel
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
    }

    public class RegionListModel
    {
        public long RegionID { get; set; }
        public string RegionName { get; set; }
    }

    public class CaseManagerListModel
    {
        public long CaseManagerID { get; set; }
        public string Name { get; set; }
    }

    public class AgencyListModel
    {
        public long AgencyID { get; set; }
        public string NickName { get; set; }
    }

    public class ScheduleStatusListModel
    {
        public long ScheduleStatusID { get; set; }
        public string ScheduleStatusName { get; set; }
    }

    public class AgencyLocationListModel
    {
        public long AgencyLocationID { get; set; }
        public string LocationName { get; set; }
        public long AgencyID { get; set; }
    }

    #region All Report Search Model

    public class SearchReportModel
    {
        public long ReferralStatusID { get; set; }
        public long AgencyID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long RegionID { get; set; }
        public long PayorID { get; set; }
        public long AssigneeID { get; set; }
        public string ClientName { get; set; }
        public int NotifyCaseManagerID { get; set; }
        public int ChecklistID { get; set; }
        public int ClinicalReviewID { get; set; }
        public long CaseManagerID { get; set; }
        public int ServiceID { get; set; }
        public long AgencyLocationID { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsSaveAsDraft { get; set; }
        public int IsDeleted { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public long ScheduleStatusID { get; set; }
        public int CheckExpireorNot { get; set; }
    }

    public class SearchRequestClientListModel
    {
        public long ReferralStatusID { get; set; }
        public long RegionID { get; set; }
        public string ClientName { get; set; }
        public int IsDeleted { get; set; }
    }



    public class SearchRespiteUsageModel
    {
        public long AgencyID { get; set; }
        public int IsDeleted { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
        public string FromMonth { get; set; }
        public string FromYear { get; set; }
        public string FisaclYear { get; set; }
        public long RegionID { get; set; }
        public long ReferralStatusID { get; set; }
    }

    public class SearchEncounterPrintModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ReferralID { get; set; }
        public int IsDeleted { get; set; }
    }

    public class SearchSnapshotPrintModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ReferralID { get; set; }
        public long FacilityID { get; set; }
        public long CreatedBy { get; set; }
        public string ReferralMonthlySummariesIDs { get; set; }
        public int IsDeleted { get; set; }
    }

    public class SearchDTRPrintModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ReferralID { get; set; }
        public long DriverID { get; set; }
        public int IsDeleted { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string PickUpAddress { get; set; }
        public string DropOffAddress { get; set; }
    }

    public class SearchGeneralNoticeModel
    {
        public long AgencyID { get; set; }
        public long RegionID { get; set; }
        public long PayorID { get; set; }
        public long ReferralStatusID { get; set; }
        public long ReferralID { get; set; }
        public int IsDeleted { get; set; }
    }

    public class SearchDspRosterModel
    {
        public long AgencyID { get; set; }
        public long PayorID { get; set; }
        public DateTime? ReferralStartDate { get; set; }
        public DateTime? ReferralEndDate { get; set; }
        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }
        public int IsDeleted { get; set; }
        public string MISSING { get; set; }
        public string EXPIRED { get; set; }
        public List<string> ReferralStatusIDs { get; set; }
    }

    public class SetLSTeamMemberCaseloadPageModel
    {
        public SetLSTeamMemberCaseloadPageModel()
        {
            Payors = new List<PayorModel>();
            ReferralStatuses = new List<ReferralStatus>();
            Facillator = new List<CaseManagerModel>();
            Agencies = new List<AgencyModel>();
            Employees = new List<NameValueData>();
            SearchLSTMCaseloadListModel = new SearchLSTeamMemberCaseloadReport();
        }
        public List<PayorModel> Payors { get; set; }
        public List<ReferralStatus> ReferralStatuses { get; set; }
        public List<CaseManagerModel> Facillator { get; set; }
        public List<AgencyModel> Agencies { get; set; }
        public List<NameValueData> Employees { get; set; }

        [Ignore]
        public SearchLSTeamMemberCaseloadReport SearchLSTMCaseloadListModel { get; set; }


    }



    public class SearchLSTeamMemberCaseloadReport
    {
        public long RegionID { get; set; }
        public DateTime? ReferralStartDate { get; set; }
        public DateTime? ReferralEndDate { get; set; }
        public string ClientName { get; set; }
        public string Parent { get; set; }


        public long ReferralStatusID { get; set; }
        public long LoggedInID { get; set; }
        public string AHCCCSID { get; set; }
        public long PayorID { get; set; }
        public long AgencyID { get; set; }
        public long CaseManagerID { get; set; }
        public DateTime? ReferralFromDate { get; set; }
        public DateTime? ReferralToDate { get; set; }

        public DateTime? ServiceFromDate { get; set; }
        public DateTime? ServiceToDate { get; set; }

        public DateTime? CFServiceFromDate { get; set; }
        public DateTime? CFServiceToDate { get; set; }

        public DateTime? ACFromDate { get; set; }
        public DateTime? ACToDate { get; set; }

        public DateTime? OMCompletedFromDate { get; set; }
        public DateTime? OMCompletedToDate { get; set; }

        public DateTime? OMNextDueFromDate { get; set; }
        public DateTime? OMNextDueToDate { get; set; }

        public string ReferralLSTMCaseloadsComment { get; set; }

        public bool ViewAllPermission { get; set; }

        public long CaseLoadID { get; set; }

    }

    public class SearchBXContractStatusReport
    {
        public int BXContractStatus { get; set; }
        public DateTime? WarningStartDate { get; set; }
        public DateTime? WarningEndDate { get; set; }

        public string ClientName { get; set; }
        public long RegionID { get; set; }
        public long AgencyID { get; set; }
        public long ReferralStatusID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long PayorID { get; set; }
        public int ServiceID { get; set; }
    }

    #endregion

    #region List Model

    public class ClientStatusListModel
    {
        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        public string Agency { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string Payor { get; set; }

        [CreateExcelFile.ExcelHead("Region", typeof(Resource))]
        public string Region { get; set; }

        [CreateExcelFile.ExcelHead("LastAttendedDate", typeof(Resource))]
        public DateTime Last_Attended_Date { get; set; }

        [CreateExcelFile.ExcelHead("Status", typeof(Resource))]
        public string Status { get; set; }
    }




    public class RequestClientListPageModel
    {
        public List<RequestClientListModel> RequestClientList { get; set; }
        public RequestClientFacility Facility { get; set; }
    }

    public class RequestClientHeader
    {
        public string Header { get; set; }
    }

    public class RequestClientFacility
    {
        public string FacilityBillingName { get; set; }
        public string AHCCCSID { get; set; }
        public string NPI { get; set; }
    }


    public class RequestClientListModel
    {
        [CreateExcelFile.ExcelHead("ClientNumber", typeof(Resource))]
        public string ReferralID { get; set; }

        [CreateExcelFile.ExcelHead("ClientNameLabel", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("DXCode", typeof(Resource))]
        public string DXCode { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("CISID", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string Payor { get; set; }

        [CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        public string Agency { get; set; }

        [CreateExcelFile.ExcelHead("CaseManager", typeof(Resource))]
        public string CaseManager { get; set; }


        [CreateExcelFile.ExcelHead("ClientDoblbl", typeof(Resource))]
        public string Birthdate { get; set; }

        [CreateExcelFile.ExcelHead("PlacementAddress", typeof(Resource))]
        public string PlacementFullAddress { get; set; }
    }

















    public class ReferralDetailsListModel
    {
        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        public string Agency { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string Payor { get; set; }

        [CreateExcelFile.ExcelHead("ReferralDate", typeof(Resource))]
        public DateTime ReferralDate { get; set; }

        [CreateExcelFile.ExcelHead("Status", typeof(Resource))]
        public string Status { get; set; }
    }

    public class ClientInformationListModel
    {
        [CreateExcelFile.ExcelHead("Status", typeof(Resource))]
        public string Status { get; set; }

        [CreateExcelFile.ExcelHead("FirstName", typeof(Resource))]
        public string FirstName { get; set; }

        [CreateExcelFile.ExcelHead("MiddleName", typeof(Resource))]
        public string MiddleName { get; set; }

        [CreateExcelFile.ExcelHead("LastName", typeof(Resource))]
        public string LastName { get; set; }

        [CreateExcelFile.ExcelHead("NickName", typeof(Resource))]
        public string ClientNickName { get; set; }

        [CreateExcelFile.ExcelHead("Birthdate", typeof(Resource))]
        public DateTime Birthdate { get; set; }

        [CreateExcelFile.ExcelHead("Age", typeof(Resource))]
        public decimal Age { get; set; }

        [CreateExcelFile.ExcelHead("Gender", typeof(Resource))]
        public string Gender { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSEnrollDateNew", typeof(Resource))]
        public DateTime AHCCCSEnrollDate { get; set; }

        [CreateExcelFile.ExcelHead("Population", typeof(Resource))]
        public string Population { get; set; }

        [CreateExcelFile.ExcelHead("HealthPlan", typeof(Resource))]
        public string HealthPlan { get; set; }

        [CreateExcelFile.ExcelHead("Title", typeof(Resource))]
        public string Title { get; set; }

        [CreateExcelFile.ExcelHead("RecordRequestEmail", typeof(Resource))]
        public string RecordRequestEmail { get; set; }

        [CreateExcelFile.ExcelHead("MonthlySummaryEmail", typeof(Resource))]
        public string MonthlySummaryEmail { get; set; }

        [CreateExcelFile.ExcelHead("RateCode", typeof(Resource))]
        public string RateCode { get; set; }

        [CreateExcelFile.ExcelHead("RateCodeStartDate", typeof(Resource))]
        public DateTime RateCodeStartDate { get; set; }

        [CreateExcelFile.ExcelHead("RateCodeEndDate", typeof(Resource))]
        public DateTime RateCodeEndDate { get; set; }

        [CreateExcelFile.ExcelHead("LanguagePreferance", typeof(Resource))]
        public string LanguageName { get; set; }

        [CreateExcelFile.ExcelHead("Region", typeof(Resource))]
        public string RegionName { get; set; }

        [CreateExcelFile.ExcelHead("Caseloads", typeof(Resource))]
        public string CaseLoads { get; set; }

        [CreateExcelFile.ExcelHead("OrientationDate", typeof(Resource))]
        public DateTime OrientationDate { get; set; }

        [CreateExcelFile.ExcelHead("PickUpLocation", typeof(Resource))]
        public string PickUpLocation { get; set; }

        [CreateExcelFile.ExcelHead("DropOffLocation", typeof(Resource))]
        public string DropOffLocation { get; set; }

        [CreateExcelFile.ExcelHead("Frequancy", typeof(Resource))]
        public string Code { get; set; }

        [CreateExcelFile.ExcelHead("NeedPrivateRoom", typeof(Resource))]
        public string NeedPrivateRoom { get; set; }

        [CreateExcelFile.ExcelHead("NextScheduleStartDate", typeof(Resource))]
        public string NextScheduleStartDate { get; set; }

        [CreateExcelFile.ExcelHead("NextScheduleEndDate", typeof(Resource))]
        public string NextScheduleEndDate { get; set; }

        [CreateExcelFile.ExcelHead("PlacementN", typeof(Resource))]
        public string PlacementRequirement { get; set; }

        [CreateExcelFile.ExcelHead("BehavioralIssue", typeof(Resource))]
        public string BehavioralIssue { get; set; }

        [CreateExcelFile.ExcelHead("PlacementName", typeof(Resource))]
        public string PlacementName { get; set; }
        [CreateExcelFile.ExcelHead("PlacementPhone", typeof(Resource))]
        public string PlacmentPhone { get; set; }
        [CreateExcelFile.ExcelHead("PlacementFullAddress", typeof(Resource))]
        public string PlacementFullAddress { get; set; }
        [CreateExcelFile.ExcelHead("PlacementEmail", typeof(Resource))]
        public string PlacementEmail { get; set; }
        [CreateExcelFile.ExcelHead("LegalGuardianName", typeof(Resource))]
        public string LegalGuardianName { get; set; }
        [CreateExcelFile.ExcelHead("LegalGuardianPhone", typeof(Resource))]
        public string LegalGuardianPhone { get; set; }
        [CreateExcelFile.ExcelHead("LegalGuardianFullAddress", typeof(Resource))]
        public string LegalGuardianFullAddress { get; set; }
        [CreateExcelFile.ExcelHead("LegalGuardianEmail", typeof(Resource))]
        public string LegalGuardianEmail { get; set; }

        [CreateExcelFile.ExcelHead("EmergencyContact", typeof(Resource))]
        public string EmergencyContact { get; set; }

        [CreateExcelFile.ExcelHead("CaseManager", typeof(Resource))]
        public string Casemanager { get; set; }

        [CreateExcelFile.ExcelHead("CMAgency", typeof(Resource))]
        public string AgencyName { get; set; }

        [CreateExcelFile.ExcelHead("CMEmail", typeof(Resource))]
        public string CMEmail { get; set; }

        [CreateExcelFile.ExcelHead("CMPhone", typeof(Resource))]
        public string Phone { get; set; }

        [CreateExcelFile.ExcelHead("CMFax", typeof(Resource))]
        public string CMFax { get; set; }

        [CreateExcelFile.ExcelHead("CareConsent", typeof(Resource))]
        public string CareConsent { get; set; }

        [CreateExcelFile.ExcelHead("SelfAdministrationofMedication", typeof(Resource))]
        public string SelfAdministrationofMedication { get; set; }

        [CreateExcelFile.ExcelHead("HealthInformationDisclosuren", typeof(Resource))]
        public string HealthInformationDisclosure { get; set; }

        [CreateExcelFile.ExcelHead("AdmissionRequirements", typeof(Resource))]
        public string AdmissionRequirements { get; set; }

        [CreateExcelFile.ExcelHead("AdmissionOrientation", typeof(Resource))]
        public string AdmissionOrientation { get; set; }

        [CreateExcelFile.ExcelHead("ZarephathCrisisPlan", typeof(Resource))]
        public string ZarephathCrisisPlan { get; set; }

        [CreateExcelFile.ExcelHead("PermissionforVoiceMail", typeof(Resource))]
        public string PermissionForVoiceMail { get; set; }

        [CreateExcelFile.ExcelHead("PermissionForEmail", typeof(Resource))]
        public string PermissionForEmail { get; set; }

        [CreateExcelFile.ExcelHead("PermissionForSMS", typeof(Resource))]
        public string PermissionForSMS { get; set; }

        [CreateExcelFile.ExcelHead("PermissionForMail", typeof(Resource))]
        public string PermissionForMail { get; set; }

        [CreateExcelFile.ExcelHead("PCMStatus", typeof(Resource))]
        public string PCMStatus { get; set; }

        [CreateExcelFile.ExcelHead("PCMVoiceMail", typeof(Resource))]
        public string PCMVoiceMail { get; set; }

        [CreateExcelFile.ExcelHead("PCMEmail", typeof(Resource))]
        public string PCMEmail { get; set; }

        [CreateExcelFile.ExcelHead("PCMSMS", typeof(Resource))]
        public string PCMSMS { get; set; }

        [CreateExcelFile.ExcelHead("PCMMail", typeof(Resource))]
        public string PCMMail { get; set; }


        [CreateExcelFile.ExcelHead("PHI", typeof(Resource))]
        public string PHI { get; set; }

        [CreateExcelFile.ExcelHead("PHIExpirationDate", typeof(Resource))]
        public string PHIExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("PHIAgencyName", typeof(Resource))]
        public string PHIAgencyName { get; set; }

        [CreateExcelFile.ExcelHead("ProgramName", typeof(Resource))]
        public string ProgrameName { get; set; }

        [CreateExcelFile.ExcelHead("ZSPRespite", typeof(Resource))]
        public string ZSPRespite { get; set; }

        [CreateExcelFile.ExcelHead("ZSPRespiteExpirationDate", typeof(Resource))]
        public string ZSPRespiteExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("ZSPRespiteGuardianSignature", typeof(Resource))]
        public string ZSPRespiteGuardianSignature { get; set; }

        [CreateExcelFile.ExcelHead("ZSPRespiteBHPSigned", typeof(Resource))]
        public string ZSPRespiteBHPSigned { get; set; }

        [CreateExcelFile.ExcelHead("ZSPLifeSkills", typeof(Resource))]
        public string ZSPLifeSkills { get; set; }

        [CreateExcelFile.ExcelHead("ZSPLifeSkillsExpirationDate", typeof(Resource))]
        public string ZSPLifeSkillsExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("ZSPLifeSkillsGuardianSignature", typeof(Resource))]
        public string ZSPLifeSkillsGuardianSignature { get; set; }

        [CreateExcelFile.ExcelHead("ZSPLifeSkillsBHPSigned", typeof(Resource))]
        public string ZSPLifeSkillsBHPSigned { get; set; }

        [CreateExcelFile.ExcelHead("ZSPCounselling", typeof(Resource))]
        public string ZSPCounselling { get; set; }

        [CreateExcelFile.ExcelHead("ZSPCounsellingExpirationDate", typeof(Resource))]
        public string ZSPCounsellingExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("ZSPCounsellingGuardianSignature", typeof(Resource))]
        public string ZSPCounsellingGuardianSignature { get; set; }

        [CreateExcelFile.ExcelHead("ZSPCounsellingBHPSigned", typeof(Resource))]
        public string ZSPCounsellingBHPSigned { get; set; }

        [CreateExcelFile.ExcelHead("ACAssessment", typeof(Resource))]
        public string ACAssessment { get; set; }

        [CreateExcelFile.ExcelHead("ACAssessmentExpirationDate", typeof(Resource))]
        public string ACAssessmentExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("ROIFromAgency", typeof(Resource))]
        public string AROI { get; set; }

        [CreateExcelFile.ExcelHead("ROIFromAgenycExpirationDate", typeof(Resource))]
        public string AROIExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("ROIAgencyName", typeof(Resource))]
        public string AROIAgencyName { get; set; }

        [CreateExcelFile.ExcelHead("NetworkCrisisPlan", typeof(Resource))]
        public string NetworkCrisisPlan { get; set; }

        [CreateExcelFile.ExcelHead("NetworkServicePlan", typeof(Resource))]
        public string NetworkServicePlan { get; set; }

        [CreateExcelFile.ExcelHead("NSPExpirationDate", typeof(Resource))]
        public string NSPExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("NSPGuardianSignature", typeof(Resource))]
        public string NSPGuardianSignature { get; set; }

        public string NSPBHPSigned { get; set; }

        public string NSPSPidentifyService { get; set; }

        [CreateExcelFile.ExcelHead("BXAssessment", typeof(Resource))]
        public string BXAssessment { get; set; }

        [CreateExcelFile.ExcelHead("BXAssessmentExpirationDate", typeof(Resource))]
        public string BXAssessmentExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("BXAssessmentBHPSigned", typeof(Resource))]
        public string BXAssessmentBHPSigned { get; set; }

        [CreateExcelFile.ExcelHead("DXCodeName", typeof(Resource))]
        public string DXCodeName { get; set; }

        [CreateExcelFile.ExcelHead("Demographic", typeof(Resource))]
        public string Demographic { get; set; }

        [CreateExcelFile.ExcelHead("DemographicExpirationDate", typeof(Resource))]
        public string DemographicExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("SNCD", typeof(Resource))]
        public string SNCD { get; set; }

        [CreateExcelFile.ExcelHead("SNCDCompletedDate", typeof(Resource))]
        public DateTime SNCDExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string PayorName { get; set; }

        [CreateExcelFile.ExcelHead("PayorEffectiveStartDate", typeof(Resource))]
        public DateTime PayorEffectiveDate { get; set; }

        [CreateExcelFile.ExcelHead("PayorEffectiveEndDate", typeof(Resource))]
        public DateTime PayorEffectiveEndDate { get; set; }

        [CreateExcelFile.ExcelHead("ReferralDate", typeof(Resource))]
        public DateTime ReferralDate { get; set; }

        [CreateExcelFile.ExcelHead("ReferralSourceName", typeof(Resource))]
        public string ReferralSourceName { get; set; }

        [CreateExcelFile.ExcelHead("FirstDOS", typeof(Resource))]
        public DateTime FirstDOS { get; set; }

        [CreateExcelFile.ExcelHead("ClosureDate", typeof(Resource))]
        public DateTime ClosureDate { get; set; }

        [CreateExcelFile.ExcelHead("ClosureReason", typeof(Resource))]
        public string ClosureReason { get; set; }

        [CreateExcelFile.ExcelHead("RespiteHoursLimit", typeof(Resource))]
        public int Respite
        {
            get { return 600; }
        }
        [CreateExcelFile.ExcelHead("UsedRespiteHours", typeof(Resource))]
        public int UsedRespiteHours { get; set; }

        [CreateExcelFile.ExcelHead("AvailableRespiteHours", typeof(Resource))]
        public int AvailableRespiteHours
        {
            get { return Respite - UsedRespiteHours; }
        }

        [CreateExcelFile.ExcelHead("ReferralSiblingMappingValue", typeof(Resource))]
        public string ReferralSiblingMappingValue { get; set; }



    }

    public class InternalServicePlanListModel
    {
        [CreateExcelFile.ExcelHead("Status", typeof(Resource))]
        public string Status { get; set; }

        [CreateExcelFile.ExcelHead("Name", typeof(Resource))]
        public string Name { get; set; }

        [CreateExcelFile.ExcelHead("NextScheduledRespiteAttendance", typeof(Resource))]
        public DateTime NextAttDate { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("Birthdate", typeof(Resource))]
        public DateTime Birthdate { get; set; }

        [CreateExcelFile.ExcelHead("LanguagePreferance", typeof(Resource))]
        public string LanguageName { get; set; }

        [CreateExcelFile.ExcelHead("RSP", typeof(Resource))]
        public string RespiteService { get; set; }

        [CreateExcelFile.ExcelHead("RSPExpire", typeof(Resource))]
        public DateTime ZSPRespiteExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("RSPGuard", typeof(Resource))]
        public string ZSPRespiteGuardianSignature { get; set; }

        [CreateExcelFile.ExcelHead("RSPbhp", typeof(Resource))]
        public string ZSPRespiteBHPSigned { get; set; }

        [CreateExcelFile.ExcelHead("LSSP", typeof(Resource))]
        public string LifeSkillsService { get; set; }

        [CreateExcelFile.ExcelHead("LSSPExpire", typeof(Resource))]
        public DateTime ZSPLifeSkillsExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("LSSPGuard", typeof(Resource))]
        public string ZSPLifeSkillsGuardianSignature { get; set; }

        [CreateExcelFile.ExcelHead("LSSPbhp", typeof(Resource))]
        public string ZSPLifeSkillsBHPSigned { get; set; }

        [CreateExcelFile.ExcelHead("CSP", typeof(Resource))]
        public string ZSPCounselling { get; set; }

        [CreateExcelFile.ExcelHead("CSPExpire", typeof(Resource))]
        public DateTime ZSPCounsellingExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("CSPGuard", typeof(Resource))]
        public string ZSPCounsellingGuardianSignature { get; set; }

        [CreateExcelFile.ExcelHead("CSPbhp", typeof(Resource))]
        public string ZSPCounsellingBHPSigned { get; set; }

        //[CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        //public string AgencyName { get; set; }

        //[CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        //public string PayorName { get; set; }



        //[CreateExcelFile.ExcelHead("CaseManager", typeof(Resource))]
        //public string Casemanager { get; set; }

        //[CreateExcelFile.ExcelHead("CareConsent", typeof(Resource))]
        //public string CareConsent { get; set; }

        //[CreateExcelFile.ExcelHead("SelfAdministrationofMedication", typeof(Resource))]
        //public string SelfAdministrationofMedication { get; set; }

        //[CreateExcelFile.ExcelHead("HealthInformationDisclosure", typeof(Resource))]
        //public string HealthInformationDisclosure { get; set; }

        //[CreateExcelFile.ExcelHead("PHI", typeof(Resource))]
        //public string PHI { get; set; }

        //[CreateExcelFile.ExcelHead("AdmissionRequirements", typeof(Resource))]
        //public string AdmissionRequirements { get; set; }

        //[CreateExcelFile.ExcelHead("AdmissionOrientation", typeof(Resource))]
        //public string AdmissionOrientation { get; set; }

        //[CreateExcelFile.ExcelHead("PermissionforVoiceMail", typeof(Resource))]
        //public string PermissionForVoiceMail { get; set; }

        //[CreateExcelFile.ExcelHead("ZarephathCrisisPlan", typeof(Resource))]
        //public string ZarephathCrisisPlan { get; set; }

        //[CreateExcelFile.ExcelHead("NCPExpirationDate", typeof(Resource))]
        //public string NCPExpirationDate { get; set; }

        //[CreateExcelFile.ExcelHead("RespiteService", typeof(Resource))]
        //public string RespiteService { get; set; }

        //[CreateExcelFile.ExcelHead("ACAssessment", typeof(Resource))]
        //public string ACAssessment { get; set; }

        //[CreateExcelFile.ExcelHead("PermissionForEmail", typeof(Resource))]
        //public string PermissionForEmail { get; set; }

        //[CreateExcelFile.ExcelHead("PermissionForSMS", typeof(Resource))]
        //public string PermissionForSMS { get; set; }

    }

    public class AttendanceListModel
    {
        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCS_ID { get; set; }

        [CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        public string NickName { get; set; }

        [CreateExcelFile.ExcelHead("ScheduleStartDate", typeof(Resource))]
        public DateTime Schedule_StartDate { get; set; }

        [CreateExcelFile.ExcelHead("ScheduleEndDate", typeof(Resource))]
        public DateTime Schedule_EndDate { get; set; }

        [CreateExcelFile.ExcelHead("Status", typeof(Resource))]
        public string Status { get; set; }

        [CreateExcelFile.ExcelHead("CancelledReason", typeof(Resource))]
        public string CancelReason { get; set; }

        [CreateExcelFile.ExcelHead("LastAttendedDate", typeof(Resource))]
        public DateTime Last_Attended_Date { get; set; }
    }

    public class TempRespiteusageListModel
    {
        [CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        public string Agency { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string PayorName { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string Name { get; set; }

        [CreateExcelFile.ExcelHead("ImportHours", typeof(Resource))]
        public decimal ImportHours { get; set; }


        [CreateExcelFile.ExcelHead("Oct", typeof(Resource))]
        public decimal Oct { get; set; }

        [CreateExcelFile.ExcelHead("Nov", typeof(Resource))]
        public decimal Nov { get; set; }

        [CreateExcelFile.ExcelHead("Dec", typeof(Resource))]
        public decimal Dec { get; set; }

        [CreateExcelFile.ExcelHead("Jan", typeof(Resource))]
        public decimal Jan { get; set; }

        [CreateExcelFile.ExcelHead("Feb", typeof(Resource))]
        public decimal Feb { get; set; }

        [CreateExcelFile.ExcelHead("Mar", typeof(Resource))]
        public decimal Mar { get; set; }

        [CreateExcelFile.ExcelHead("Apr", typeof(Resource))]
        public decimal Apr { get; set; }

        [CreateExcelFile.ExcelHead("May", typeof(Resource))]
        public decimal May { get; set; }

        [CreateExcelFile.ExcelHead("Jun", typeof(Resource))]
        public decimal Jun { get; set; }

        [CreateExcelFile.ExcelHead("Jul", typeof(Resource))]
        public decimal Jul { get; set; }

        [CreateExcelFile.ExcelHead("Aug", typeof(Resource))]
        public decimal Aug { get; set; }

        [CreateExcelFile.ExcelHead("Sep", typeof(Resource))]
        public decimal Sep { get; set; }


        [CreateExcelFile.ReportIgnore]
        public DateTime StartDate { get; set; }

        [CreateExcelFile.ReportIgnore]
        public DateTime EndDate { get; set; }

        [CreateExcelFile.ExcelHead("FiscalYear", typeof(Resource))]
        public decimal FiscalYear
        {
            get
            {
                if (DateTime.Now.Date >= StartDate && DateTime.Now.Date <= EndDate)
                    return Convert.ToDecimal(Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov + Dec + ImportHours);

                return Convert.ToDecimal(Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov + Dec);
            }
        }

        public decimal Reamaining
        {
            get { return Convert.ToDecimal(Convert.ToDecimal(ConfigSettings.RespiteUsageLimit - FiscalYear)); }
        }
    }
    public class RespiteusageListModel
    {
        [CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        public string Agency { get; set; }

        [CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        public string PayorName { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSID", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }

        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string Name { get; set; }


        [CreateExcelFile.ExcelHead("Oct", typeof(Resource))]
        public decimal Oct { get; set; }

        [CreateExcelFile.ExcelHead("Nov", typeof(Resource))]
        public decimal Nov { get; set; }

        [CreateExcelFile.ExcelHead("Dec", typeof(Resource))]
        public decimal Dec { get; set; }

        [CreateExcelFile.ExcelHead("Jan", typeof(Resource))]
        public decimal Jan { get; set; }

        [CreateExcelFile.ExcelHead("Feb", typeof(Resource))]
        public decimal Feb { get; set; }

        [CreateExcelFile.ExcelHead("Mar", typeof(Resource))]
        public decimal Mar { get; set; }

        [CreateExcelFile.ExcelHead("Apr", typeof(Resource))]
        public decimal Apr { get; set; }

        [CreateExcelFile.ExcelHead("May", typeof(Resource))]
        public decimal May { get; set; }

        [CreateExcelFile.ExcelHead("Jun", typeof(Resource))]
        public decimal Jun { get; set; }

        [CreateExcelFile.ExcelHead("Jul", typeof(Resource))]
        public decimal Jul { get; set; }

        [CreateExcelFile.ExcelHead("Aug", typeof(Resource))]
        public decimal Aug { get; set; }

        [CreateExcelFile.ExcelHead("Sep", typeof(Resource))]
        public decimal Sep { get; set; }


        [CreateExcelFile.ReportIgnore]
        public DateTime StartDate { get; set; }

        [CreateExcelFile.ReportIgnore]
        public DateTime EndDate { get; set; }

        [CreateExcelFile.ExcelHead("FiscalYear", typeof(Resource))]
        public decimal FiscalYear
        {
            get
            {
                return Convert.ToDecimal(Jan + Feb + Mar + Apr + May + Jun + Jul + Aug + Sep + Oct + Nov + Dec);
            }
        }

        public decimal Reamaining
        {
            get { return Convert.ToDecimal(Convert.ToDecimal(ConfigSettings.RespiteUsageLimit - FiscalYear)); }
        }
    }

    public class EncounterPrintListModel
    {
        public string ClientName { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string BillingProviderName { get; set; }
        public string ServiceDate { get; set; }
        public string Createdby { get; set; }
        public string Updatedby { get; set; }
        public string SignedBy { get; set; }

        public DateTime CreatedDate1 { get; set; }
        public DateTime UpdatedDate1 { get; set; }

        public string CreatedDate { get { return Common.GetLocalFromUtc(CreatedDate1).ToString("MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture); } }
        public string UpdatedDate { get { return Common.GetLocalFromUtc(UpdatedDate1).ToString("MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture); } }


        public string DXCodeName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string PosID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ServiceCodeType { get; set; }

        public string POSDetail { get; set; }
        public string NoteDetails { get; set; }
        public string CalculatedUnit { get; set; }
        public string Assessment { get; set; }
        public string ActionPlan { get; set; }
        public string Startingodometer { get; set; }
        public int UnitType { get; set; }
        public string Endingodometer { get; set; }
        public string EmpSignature { get; set; }
        public string DiagnosesStringAppend { get; set; }
        public string LineItemStringAppend { get; set; }
        public string PrintDate { get; set; }
        public string ImageTag { get; set; }
        public string DAPData { get; set; }
        public string DAPAssessment { get; set; }
        public string DAPPlan { get; set; }
        public int CountPage { get; set; }
        public int TotalPage { get; set; }
        public string SearchFromDate { get; set; }
        public string SearchToDate { get; set; }
        public string LateEntry { get; set; }
        public string LateEntryText { get; set; }
        public string CredentialID { get; set; }
        public string Time { get; set; }

        public string RandomGroupID { get; set; }

    }

    public class GeneralNoticeModel
    {
        CacheHelper _cacheHelper = new CacheHelper();
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ClientNickName { get; set; }
        public string Birthdate { get; set; }
        public decimal Age { get; set; }
        public string Gender { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string ParentName { get; set; }
        public string ParentAddress { get; set; }
        public string ParentCity { get; set; }
        public string ParentStateName { get; set; }
        public string ParentZipCode { get; set; }
        public string ParentFullAddress { get; set; }
        public string ParentPhone { get; set; }
        public string ParentEmail { get; set; }

        public int CountPage { get; set; }
        public int TotalPage { get; set; }
        public string DiagnosesStringAppend { get; set; }
        public string ImageTag { get; set; }
        public string FaceBookImage
        {
            get
            {
                return "<img src='" + _cacheHelper.SiteBaseURL + Constants.FaceBookLogoImage + "' height='30' width='30' style='float:left;'/>";
            }
        }
        public string TagZerpathLogoImage
        {
            get
            {
                return "<img src='" + _cacheHelper.SiteBaseURL + Constants.ZerpathLogoImage + "' width='300' style='float:right;'/>";
            }
        }
    }

    [TableName("Life Skills OM")]
    public class LifeSkillsOutcomeMeasurementsListModel
    {
        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("WorkCommunity", typeof(Resource))]
        public int? WorkCommunity { get; set; }

        [CreateExcelFile.ExcelHead("ScheduledAppointment", typeof(Resource))]
        public int? ScheduledAppointment { get; set; }

        [CreateExcelFile.ExcelHead("AskForHelp", typeof(Resource))]
        public int? AskForHelp { get; set; }

        [CreateExcelFile.ExcelHead("CommunicateEffectively", typeof(Resource))]
        public int? CommunicateEffectively { get; set; }

        [CreateExcelFile.ExcelHead("PositiveBehavior", typeof(Resource))]
        public int? PositiveBehavior { get; set; }

        [CreateExcelFile.ExcelHead("QualityFriendship", typeof(Resource))]
        public int? QualityFriendship { get; set; }

        [CreateExcelFile.ExcelHead("RespectOther", typeof(Resource))]
        public int? RespectOther { get; set; }

        [CreateExcelFile.ExcelHead("StickUp", typeof(Resource))]
        public int? StickUp { get; set; }

        [CreateExcelFile.ExcelHead("LifeSkillGoals", typeof(Resource))]
        public int? LifeSkillGoals { get; set; }

        [CreateExcelFile.ExcelHead("StayOutOfTrouble", typeof(Resource))]
        public int? StayOutOfTrouble { get; set; }

        [CreateExcelFile.ExcelHead("SuccessfulInSchool", typeof(Resource))]
        public int? SuccessfulInSchool { get; set; }

        [CreateExcelFile.ExcelHead("SuccessfulInLiving", typeof(Resource))]
        public int? SuccessfulInLiving { get; set; }

        [CreateExcelFile.ExcelHead("AdulthoodNeeds", typeof(Resource))]
        public int? AdulthoodNeeds { get; set; }

    }

    public class DspRosterList
    {
        public DspRosterList()
        {
            DspRosterListModel = new List<DspRosterListModel>();
            ClosureDspRosterListModel = new List<ClosureDspRosterListModel>();
        }

        public List<DspRosterListModel> DspRosterListModel { get; set; }
        public List<ClosureDspRosterListModel> ClosureDspRosterListModel { get; set; }
    }

    [TableName("DSP Roster")]
    public class DspRosterListModel
    {

        [CreateExcelFile.ExcelHead("ReferralStatus", typeof(Resource))]
        public string ReferralStatus { get; set; }

        [CreateExcelFile.ExcelHead("ProgramName", typeof(Resource))]
        public string ProgrameName { get; set; }


        [CreateExcelFile.ExcelHead("Prioritization", typeof(Resource))]
        public string Prioritization { get; set; }



        [CreateExcelFile.ExcelHead("ReferralDate", typeof(Resource))]
        public DateTime ReferralDate { get; set; }

        [CreateExcelFile.ExcelHead("ReferralAccepted", typeof(Resource))]
        public string ReferralAccepted { get; set; }

        //WILL BE BLANK
        [CreateExcelFile.ExcelHead("DenialReason", typeof(Resource))]
        public string DenialReason { get; set; }


        [CreateExcelFile.ExcelHead("Dateoffirstservice", typeof(Resource))]
        public DateTime FirstDOS { get; set; }

        //WILL BE BLANK
        [CreateExcelFile.ExcelHead("ReferralResponse", typeof(Resource))]
        public string ReferralResponse { get; set; }


        [CreateExcelFile.ExcelHead("LastName", typeof(Resource))]
        public string LastName { get; set; }

        [CreateExcelFile.ExcelHead("FirstName", typeof(Resource))]
        public string FirstName { get; set; }

        [CreateExcelFile.ExcelHead("DOBShort", typeof(Resource))]
        public DateTime Dob { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }


        [CreateExcelFile.ExcelHead("QSP", typeof(Resource))]
        public string Agency { get; set; }


        [CreateExcelFile.ExcelHead("CaseManager", typeof(Resource))]
        public string CaseManager { get; set; }

        //WILL BE BLANK
        [CreateExcelFile.ExcelHead("CASIILevel", typeof(Resource))]
        public string CASIILevel { get; set; }


        [CreateExcelFile.ExcelHead("ClosureDate", typeof(Resource))]
        public DateTime ClosureDate { get; set; }

        [CreateExcelFile.ExcelHead("ClosureReason", typeof(Resource))]
        public string ClosureReason { get; set; }


        [CreateExcelFile.ExcelHead("AdditionalNoteForClosureDenial", typeof(Resource))]
        public string AdditionalNote { get; set; }

        [CreateExcelFile.ExcelHead("LengthofStay", typeof(Resource))]
        public string LengthofStay { get; set; }


        [CreateExcelFile.ExcelHead("DischargePrioritization", typeof(Resource))]
        public string DischargePrioritization { get; set; }

        [CreateExcelFile.ExcelHead("MMWIAClosureNotes", typeof(Resource))]
        public string MMWIAClosureNotes { get; set; }

        [CreateExcelFile.ExcelHead("Comments", typeof(Resource))]
        public string Comments { get; set; }


    }

    [TableName("Closure Tab")]
    public class ClosureDspRosterListModel
    {

        [CreateExcelFile.ExcelHead("ReferralStatus", typeof(Resource))]
        public string ReferralStatus { get; set; }

        [CreateExcelFile.ExcelHead("ProgramName", typeof(Resource))]
        public string ProgrameName { get; set; }


        [CreateExcelFile.ExcelHead("Prioritization", typeof(Resource))]
        public string Prioritization { get; set; }

        [CreateExcelFile.ExcelHead("ReferralDate", typeof(Resource))]
        public DateTime ReferralDate { get; set; }

        [CreateExcelFile.ExcelHead("ReferralAccepted", typeof(Resource))]
        public string ReferralAccepted { get; set; }

        //WILL BE BLANK
        [CreateExcelFile.ExcelHead("DenialReason", typeof(Resource))]
        public string DenialReason { get; set; }


        [CreateExcelFile.ExcelHead("Dateoffirstservice", typeof(Resource))]
        public DateTime FirstDOS { get; set; }

        //WILL BE BLANK
        [CreateExcelFile.ExcelHead("ReferralResponse", typeof(Resource))]
        public string ReferralResponse { get; set; }


        [CreateExcelFile.ExcelHead("LastName", typeof(Resource))]
        public string LastName { get; set; }

        [CreateExcelFile.ExcelHead("FirstName", typeof(Resource))]
        public string FirstName { get; set; }

        [CreateExcelFile.ExcelHead("DOBShort", typeof(Resource))]
        public DateTime Dob { get; set; }

        [CreateExcelFile.ExcelHead("CISNumberLabel", typeof(Resource))]
        public string CISNumber { get; set; }


        [CreateExcelFile.ExcelHead("QSP", typeof(Resource))]
        public string Agency { get; set; }


        [CreateExcelFile.ExcelHead("CaseManager", typeof(Resource))]
        public string CaseManager { get; set; }

        //WILL BE BLANK
        [CreateExcelFile.ExcelHead("CASIILevel", typeof(Resource))]
        public string CASIILevel { get; set; }


        [CreateExcelFile.ExcelHead("ClosureDate", typeof(Resource))]
        public DateTime ClosureDate { get; set; }

        [CreateExcelFile.ExcelHead("ClosureReason", typeof(Resource))]
        public string ClosureReason { get; set; }


        [CreateExcelFile.ExcelHead("AdditionalNoteForClosureDenial", typeof(Resource))]
        public string AdditionalNote { get; set; }

        [CreateExcelFile.ExcelHead("LengthofStay", typeof(Resource))]
        public string LengthofStay { get; set; }


        [CreateExcelFile.ExcelHead("DischargePrioritization", typeof(Resource))]
        public string DischargePrioritization { get; set; }

        [CreateExcelFile.ExcelHead("MMWIAClosureNotes", typeof(Resource))]
        public string MMWIAClosureNotes { get; set; }

        [CreateExcelFile.ExcelHead("Comments", typeof(Resource))]
        public string Comments { get; set; }


    }

    [TableName("LS Team Member Caseloads")]
    public class LSTeamMemberCaseloadListModel
    {
        [CreateExcelFile.ExcelHead("LastName", typeof(Resource))]
        public string LastName { get; set; }

        [CreateExcelFile.ExcelHead("FirstName", typeof(Resource))]
        public string FirstName { get; set; }

        [CreateExcelFile.ExcelHead("Birthdate", typeof(Resource))]
        public string Birthdate { get; set; }

        [CreateExcelFile.ExcelHead("Age", typeof(Resource))]
        public string Age { get; set; }

        [CreateExcelFile.ExcelHead("CaseManager", typeof(Resource))]
        public string Casemanager { get; set; }

        [CreateExcelFile.ExcelHead("ServicePlanCompleted", typeof(Resource))]
        public string ServicePlanCompleted { get; set; }

        [CreateExcelFile.ExcelHead("ServicePlanDue", typeof(Resource))]
        public string ServicePlanDue { get; set; }

        [CreateExcelFile.ExcelHead("AnsellCaseyCompleted", typeof(Resource))]
        public string ACServicePlanCompleted { get; set; }

        [CreateExcelFile.ExcelHead("AnsellCaseyDue", typeof(Resource))]
        public string ACServicePlanDue { get; set; }

        [CreateExcelFile.ExcelHead("ACGra", typeof(Resource))]
        public string LastCompletedDate { get; set; }

        [CreateExcelFile.ExcelHead("OMLastCompleted", typeof(Resource))]
        public string OMLastCompleted { get; set; }

        [CreateExcelFile.ExcelHead("OMNextDue", typeof(Resource))]
        public string OMNextDue
        {
            get
            {
                if (!string.IsNullOrEmpty(OMLastCompleted))
                {
                    return Convert.ToDateTime(OMLastCompleted).AddMonths(3).ToString(Constants.DbDateFormat, CultureInfo.InvariantCulture);
                }
                return null;
            }
        }

        [CreateExcelFile.ExcelHead("NumberOfContactAttempt", typeof(Resource))]
        public string NumberOfContactAttempt { get; set; }


        [CreateExcelFile.ExcelHead("LastAttemptedDate", typeof(Resource))]
        public string LastAttemptedDate { get; set; }


        [CreateExcelFile.ExcelHead("Parent", typeof(Resource))]
        public string Parent { get; set; }


    }

    public class LSTeamMemberCaseloadListPageModel
    {
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Status { get; set; }
        public long Assignee { get; set; }
        public string AssigneeName { get; set; }
        public string ContractName { get; set; }
        public string FaciliatorName { get; set; }
        public string CompanyName { get; set; }
        public string ReferralLSTMCaseloadsComment { get; set; }

        public DateTime? ReferralDate { get; set; }

        public DateTime? ServicePlanDue { get; set; }
        public DateTime? CFServicePlanDue { get; set; }
        public bool EnrollToCFServicePlan { get; set; }
        public DateTime? ACServicePlanDue { get; set; }
        public DateTime? OMLastCompleted { get; set; }
        public string OMNextDue { get; set; }


        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }

        public bool IsDeleted { get; set; }

        public DateTime? Dob { get; set; }

        public string Age { get; set; }
        public string Gender { get; set; }

        public string CaseLoads { get; set; }

        public string CMPhone { get; set; }
        public string CMEmail { get; set; }
        public string ParentPhone1 { get; set; }
        public string ParentPhone2 { get; set; }
        public string ParentEmail { get; set; }

        public long Row { get; set; }
        public int Count { get; set; }

    }

    [TableName("BX Contracts Tracking")]
    public class BXContractStatusListModel
    {
        [CreateExcelFile.ExcelHead("ClientN", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("DateofWarning", typeof(Resource))]
        public DateTime WarningDate { get; set; }

        [CreateExcelFile.ExcelHead("BxWarningContract", typeof(Resource))]
        public string WarningReason { get; set; }

        [CreateExcelFile.ExcelHead("CMNotifiedDate", typeof(Resource))]
        public DateTime CaseManagerNotifyDate { get; set; }

        [CreateExcelFile.ExcelHead("WarningStatus", typeof(Resource))]
        public string WarningStatus { get; set; }

        [CreateExcelFile.ExcelHead("SuspentionType", typeof(Resource))]
        public string SuspentionType { get; set; }

        [CreateExcelFile.ExcelHead("SuspentionLength", typeof(Resource))]
        public string SuspentionLength { get; set; }

        [CreateExcelFile.ExcelHead("ReturnEligibleDate", typeof(Resource))]
        public DateTime ReturnEligibleDate { get; set; }

        [CreateExcelFile.ExcelHead("ActiveWarnings", typeof(Resource))]
        public string ActiveCount { get; set; }


    }

    #endregion

    #region Schedule Attendance Model

    public class SceduleAttendanceModel
    {
        public SceduleAttendanceModel()
        {
            SchedulAttendanceFacilityNameListModel = new List<SchedulAttendanceFacilityNameListModel>();
            SchedulAttendanceListModel = new List<SchedulAttendanceListModel>();
            SchedulAttendanceCancelListModel = new List<SchedulAttendanceListModel>();
            SearchScheduleAttendanceModel = new SearchScheduleAttendanceModel();
        }
        public List<SchedulAttendanceFacilityNameListModel> SchedulAttendanceFacilityNameListModel { get; set; }
        public List<SchedulAttendanceListModel> SchedulAttendanceListModel { get; set; }
        public List<SchedulAttendanceListModel> SchedulAttendanceCancelListModel { get; set; }
        public SearchScheduleAttendanceModel SearchScheduleAttendanceModel { get; set; }
    }

    public class SchedulAttendanceFacilityNameListModel
    {
        public string FacilityHouseName { get; set; }
        public List<SchedulAttendanceListModel> SchedulFacilityListModel { get; set; }
    }

    public class SchedulAttendanceListModel
    {
        public string FacilityHouseName { get; set; }
        public string RegionName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClientName { get; set; }

        //public string ClientName
        //{
        //    get { return Common.GetGeneralNameFormat(FirstName, LastName); }
        //}

        public string AHCCCSID { get; set; }
        public float Age { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }

        public string ParentName { get; set; }
        public string ParentPhone1 { get; set; }
        public string ParentPhone2 { get; set; }

        public string StrParentPhone1
        {
            get
            {
                string phone1 = null;
                if (!string.IsNullOrEmpty(ParentPhone1))
                {
                    phone1 = Convert.ToInt64(ParentPhone1).ToString("(###) ###-####");
                }
                return phone1;
            }
        }

        public string StrParentPhone2
        {
            get
            {
                string phone2 = null;
                if (!string.IsNullOrEmpty(ParentPhone2))
                {
                    phone2 = Convert.ToInt64(ParentPhone2).ToString("(###) ###-####");
                }
                return phone2;
            }
        }

        public string ParentAddress { get; set; }
        public string ParentCity { get; set; }
        public string ParentState { get; set; }
        public string ParentZipCode { get; set; }

        public string PlacementRequirement { get; set; }
        public string BehavioralIssue { get; set; }
        public string DropOffLocationName { get; set; }
        public string PickUpLocationName { get; set; }
        public string Agency { get; set; }

        public string Code { get; set; }
        public DateTime? LastAttendedDate { get; set; }
        public DateTime? NextAttDate { get; set; }

        public string PayorName { get; set; }
        public string DXCodeName { get; set; }
        public string EmergencyContact { get; set; }

        public string ScheduleStatusName { get; set; }
        public string PickUpLocationCode { get; set; }
        public string DropOffLocationCode { get; set; }
        public long ScheduleStatusID { get; set; }
        public string Facilitator { get; set; }
        public string Comments { get; set; }
        public string CancelReason { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        public string PickUpDay
        {
            get { return Convert.ToString(Convert.ToDateTime(StartDate).DayOfWeek); }
        }

        public string DropOffDay
        {
            get { return Convert.ToString(Convert.ToDateTime(EndDate).DayOfWeek); }
        }

        public string Days
        {
            get
            {
                int Fromday = (int)(EnumDays)Enum.Parse(typeof(EnumDays), PickUpDay);
                int today = (int)(EnumDays)Enum.Parse(typeof(EnumDays), DropOffDay);
                return Fromday + " - " + today;
            }
        }

        public bool PermissionForVoiceMail { get; set; }

        public bool NeedPrivateRoom { get; set; }

    }

    public class SearchScheduleAttendanceModel
    {

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }

        public long RegionID { get; set; }
        public string RegionName { get; set; }
    }

    public class RequestedDocsForAttendanceListModel
    {
        [CreateExcelFile.ExcelHead("ClientLabel", typeof(Resource))]
        public string ClientName { get; set; }

        [CreateExcelFile.ExcelHead("AHCCCSNumber", typeof(Resource))]
        public string AHCCCSID { get; set; }

        [CreateExcelFile.ExcelHead("ScheduleStatus", typeof(Resource))]
        public string ScheduleStatusName { get; set; }

        [CreateExcelFile.ExcelHead("AssignedFacility", typeof(Resource))]
        public string FacilityHouseName { get; set; }

        [CreateExcelFile.ReportIgnore]
        public DateTime StartDate { get; set; }

        [CreateExcelFile.ReportIgnore]
        public DateTime EndDate { get; set; }

        public string PickUpDay
        {
            get
            {
                return Convert.ToString(Convert.ToDateTime(StartDate).DayOfWeek);
            }
        }

        public string DropOffDay
        {
            get
            {
                return Convert.ToString(Convert.ToDateTime(EndDate).DayOfWeek);
            }
        }

        [CreateExcelFile.ExcelHead("DropOffLocation", typeof(Resource))]
        public string DropOffLocationName { get; set; }

        [CreateExcelFile.ExcelHead("PickUpLocation", typeof(Resource))]
        public string PickUpLocationName { get; set; }

        [CreateExcelFile.ExcelHead("CareConsent", typeof(Resource))]
        public string CareConsent { get; set; }

        [CreateExcelFile.ExcelHead("ZSPRespiteExpirationDate", typeof(Resource))]
        public DateTime ZSPRespiteExpirationDate { get; set; }

        [CreateExcelFile.ExcelHead("ROIForEmergencyContact", typeof(Resource))]
        public string ROIForEmergencyContact
        {
            get
            {
                string value = Resource.No;
                if (!string.IsNullOrEmpty(EmergencyContact))
                {
                    if (EmergencyContact.Length > 0)
                        value = Resource.Yes;
                }
                return value;
            }
        }

        [CreateExcelFile.ExcelHead("EmergencyContact", typeof(Resource))]
        public string EmergencyContact { get; set; }



        //[CreateExcelFile.ExcelHead("DOBShort", typeof(Resource))]
        //public string Dob { get; set; }

        //[CreateExcelFile.ExcelHead("Age", typeof(Resource))]
        //public float Age { get; set; }

        //[CreateExcelFile.ExcelHead("Gender", typeof(Resource))]
        //public string Gender { get; set; }


        //[CreateExcelFile.ExcelHead("ZSPLifeSkillsExpirationDate", typeof(Resource))]
        //public string ZSPLifeSkillsExpirationDate { get; set; }

        //[CreateExcelFile.ExcelHead("ZSPCounsellingExpirationDate", typeof(Resource))]
        //public string ZSPCounsellingExpirationDate { get; set; }

        //[CreateExcelFile.ExcelHead("ConnectingFamiliesExpirationDate", typeof(Resource))]
        //public string ZSPConnectingFamiliesExpirationDate { get; set; }

        //[CreateExcelFile.ExcelHead("Region", typeof(Resource))]
        //public string RegionName { get; set; }

        //[CreateExcelFile.ExcelHead("Parent", typeof(Resource))]
        //public string ParentName { get; set; }

        //[CreateExcelFile.ReportIgnore]
        //public string ParentPhone1 { get; set; }

        //[CreateExcelFile.ReportIgnore]
        //public string ParentPhone2 { get; set; }

        //[CreateExcelFile.ExcelHead("Phone1", typeof(Resource))]
        //public string StrParentPhone1
        //{
        //    get
        //    {
        //        string phone1 = null;
        //        if (!string.IsNullOrEmpty(ParentPhone1))
        //        {
        //            phone1 = Convert.ToInt64(ParentPhone1).ToString("(###) ###-####");
        //        }
        //        return phone1;
        //    }
        //}

        //[CreateExcelFile.ExcelHead("Phone2", typeof(Resource))]
        //public string StrParentPhone2
        //{
        //    get
        //    {
        //        string phone2 = null;
        //        if (!string.IsNullOrEmpty(ParentPhone2))
        //        {
        //            phone2 = Convert.ToInt64(ParentPhone2).ToString("(###) ###-####");
        //        }
        //        return phone2;
        //    }
        //}

        //[CreateExcelFile.ExcelHead("Address", typeof(Resource))]
        //public string ParentAddress { get; set; }

        //[CreateExcelFile.ExcelHead("City", typeof(Resource))]
        //public string ParentCity { get; set; }

        //[CreateExcelFile.ExcelHead("State", typeof(Resource))]
        //public string ParentState { get; set; }

        //[CreateExcelFile.ExcelHead("ZipCode", typeof(Resource))]
        //public string ParentZipCode { get; set; }


        //[CreateExcelFile.ExcelHead("ScheduleStartDate", typeof(Resource))]
        //public string StartDate { get; set; }
        //[CreateExcelFile.ExcelHead("ScheduleEndDate", typeof(Resource))]
        //public string EndDate { get; set; }

        //public string Days
        //{
        //    get
        //    {
        //        int Fromday = (int)(EnumDays)Enum.Parse(typeof(EnumDays), PickUpDay);
        //        int today = (int)(EnumDays)Enum.Parse(typeof(EnumDays), DropOffDay);
        //        return Fromday + " - " + today;
        //    }
        //}

        //[CreateExcelFile.ExcelHead("PlacementRequirement", typeof(Resource))]
        //public string PlacementRequirement { get; set; }

        //[CreateExcelFile.ExcelHead("BehavioralIssue", typeof(Resource))]
        //public string BehavioralIssue { get; set; }



        //[CreateExcelFile.ExcelHead("CaseManager", typeof(Resource))]
        //public string Facilitator { get; set; }

        //[CreateExcelFile.ExcelHead("Agency", typeof(Resource))]
        //public string Agency { get; set; }

        //[CreateExcelFile.ExcelHead("Payor", typeof(Resource))]
        //public string PayorName { get; set; }

    }

    public class SearchReqDocsForAttendanceModel
    {

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }

        public long RegionID { get; set; }
        public string RegionName { get; set; }
    }

    public enum EnumDays
    {
        Friday = 1,
        Saturday = 2,
        Sunday = 3,
        Monday = 4,
        Tuesday = 5,
        Wednesday = 6,
        Thursday = 7,
    }

    public enum PayFrequency
    {
        Biweekly = 1,
        Weekly = 2,
        Semimonthly = 3,
        Monthly = 4,
        Quarterly = 5
    }

    #endregion

    #region DTR Print Report Model

    public class DTRPrintListModel
    {
        public long NoteID { get; set; }
        public string ClientName { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string ServiceDate { get; set; }
        public string DXCodeName { get; set; }
        public string StartTime { get; set; }

        public DateTime? DateStartTime { get; set; }


        public string EndTime { get; set; }
        public string CalculatedUnit { get; set; }
        public string Startingodometer { get; set; }
        public string Endingodometer { get; set; }
        public string EmpSignature { get; set; }
        public string PrintDate { get; set; }
        public int CountPage { get; set; }
        public int TotalPage { get; set; }
        public string CredentialID { get; set; }
        public string AHCCCSLogoImage { get; set; }
        public string DriverName { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string PickUpAddress { get; set; }
        public string DropOffAddress { get; set; }
        public string RoundTrip { get; set; }
        public string OneWay { get; set; }
        public string MultiStops { get; set; }
        public string EscortName { get; set; }
        public string RelationShip { get; set; }
        public string Dob { get; set; }
        public string DTRLOGLIST { get; set; }
        public string MailingAddress { get; set; }
        public int Tripmile
        {
            get { return Convert.ToInt32(Endingodometer) - Convert.ToInt32(Startingodometer); }
        }
        public string RandomGroupID { get; set; }
        public string ServiceCode { get; set; }
        public bool IsUsed { get; set; }
    }

    public class DTRGroupByModel
    {
        public string DriverName { get; set; }
        public string ServiceDate { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public string AHCCCSID { get; set; }
    }

    public class DTRGroupListModel
    {
        public DTRGroupByModel DtrGroupByModel { get; set; }
        public List<DTRPrintListModel> DtrPrintListModel { get; set; }
    }

    #endregion




    public class Edi277RedableFileModel
    {
        public string Source { get; set; }
        public string TraceNumber { get; set; }
        public string ReceiptDate { get; set; }
        public string ProcessDate { get; set; }
        public string Receiver { get; set; }
        public string TotalAcceptedClaims { get; set; }
        public string TotalAcceptedAmount { get; set; }
        public string TotalRejectedClaims { get; set; }
        public string TotalRejectedAmount { get; set; }
        public string Provider { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AHCCCSID { get; set; }
        public string Status { get; set; }
        public string CSCC { get; set; }
        public string CSC { get; set; }
        public string EIC { get; set; }
        public string Action { get; set; }
        public string Amount { get; set; }
        public string Message { get; set; }
        public string BatchNoteID { get; set; }
        public string BatchID { get; set; }
        public string NoteID { get; set; }
        public string ClaimNumber { get; set; }
        public string PayorClaimNumber { get; set; }
        public string ServiceDate { get; set; }

    }




    public class SearchBillingSummaryReport
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class BillingSummaryListPageModel
    {
        public string ClientName { get; set; }
        public string Gender { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Payor { get; set; }
        public string BatchTypeName { get; set; }
        public string ClaimStatus { get; set; }
        public string ClaimNumber { get; set; }
        public string PayorClaimNumber { get; set; }
        public string BilledAmount { get; set; }
        public string AllowedAmount { get; set; }
        public string PaidAmount { get; set; }
        public string ClaimAdjustmentGroupCode { get; set; }
        public string ClaimAdjustmentReason { get; set; }
        public string LoadDate { get; set; }
    }

    public class SetEmployeeVisitListPage
    {
        public SetEmployeeVisitListPage()
        {
            SearchEmployeeVisitListPage = new SearchEmployeeVisitListPage();
            DeleteFilter = new List<NameValueData>();
            ActionFilter = new List<NameValueData>();
            SearchEmployeeVisitNoteListPage = new SearchEmployeeVisitNoteListPage();
            DeleteVisitNoteFilter = new List<NameValueData>();
            EmployeeList = new List<EmployeeListModel>();
            ReferralList = new List<ReferralListModel>();
            //CareTypeList = new List<CareTypeListModel>();
            SearchRefCalender = new SearchRefCalender();
            PayorList = new List<PayorModelList>();
            CareTypeList = new List<CareTypemodel>();
            ServiceTypeList = new List<ServiceTypeModel>();
            ReferralPayorList = new List<ReferralPayorList>();
            SelectedEmployeeList = new List<EmployeeListModel>();
            PCADetail = new PCADetail();
            TaskList = new List<TaskLists>();
            ConclusionList = new List<TaskLists>();
            DeviationList = new List<DeviationModel>();
            FacilityList = new List<FacilityList>();
        }
        [Ignore]
        public SearchEmployeeVisitListPage SearchEmployeeVisitListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public List<NameValueData> ActionFilter { get; set; }
        [Ignore]
        public SearchEmployeeVisitNoteListPage SearchEmployeeVisitNoteListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteVisitNoteFilter { get; set; }
        public List<EmployeeListModel> EmployeeList { get; set; }
        public List<ReferralListModel> ReferralList { get; set; }

        // public List<CareTypeListModel> CareTypeList { get; set; }

        [Ignore]
        public SearchRefCalender SearchRefCalender { get; set; }
        [Ignore]
        public bool IsPartial { get; set; }


        public List<PayorModelList> PayorList { get; set; }
        public List<CareTypemodel> CareTypeList { get; set; }
        public List<ServiceTypeModel> ServiceTypeList { get; set; }
        public List<ReferralPayorList> ReferralPayorList { get; set; }
        [Ignore]
        public List<EmployeeListModel> SelectedEmployeeList { get; set; }
        [Ignore]
        public PCADetail PCADetail { get; set; }
        //public List<Categories> PCATaskList { get; internal set; }
        [Ignore]
        public List<TaskLists> TaskList { get; set; }
        [Ignore]
        public List<TaskLists> ConclusionList { get; set; }
        [Ignore]
        public List<DeviationModel> DeviationList { get; set; }
        public List<FacilityList> FacilityList { get; set; }
        //public List<TypesOfTimeSheet> TimeSheetList { get; set; }

    }

    public class SetNurseSignaturePage
    {
        public SetNurseSignaturePage()
        {
            SearchNurceTimesheetListPage = new SearchNurceTimesheetListPage();
            EmployeeList = new List<EmployeeListModel>();
            ReferralList = new List<ReferralListModel>();
            CareTypeList = new List<CareTypemodel>();
            //TypesOfTimeSheet = new List<TypesOfTimeSheet>();
        }
        [Ignore]
        public SearchNurceTimesheetListPage SearchNurceTimesheetListPage { get; set; }
        public List<EmployeeListModel> EmployeeList { get; set; }
        public List<ReferralListModel> ReferralList { get; set; }
        public List<CareTypemodel> CareTypeList { get; set; }
    }
    public class SearchNurceTimesheetListPage
    {
        public string EmployeeIDs { get; set; }
        public string ReferralIDs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CareTypeIDs { get; set; }
        public string StatusId { get; set; }
    }

    public class DeviationModel
    {
        public long DeviationID { get; set; }
        public long DeviationNoteID { get; set; }
        public long EmployeeID { get; set; }
        public string DeviationNotes { get; set; }
        public string DeviationType { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    }
    public class FacilityList
    {
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }

    }
    public class ReferralPayorList
    {
        public long ReferralID { get; set; }
        public string PayorID { get; set; }
        public string PayorName { get; set; }
    }
    public class CareTypemodel
    {
        public long CareTypeID { get; set; }
        public string CareType { get; set; }
    }
    //public class TypesOfTimeSheet
    //{
    //    public long TimeSheetID { get; set; }
    //    public string TimeSheetType { get; set; }
    //}
    public class PayorModelList
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
    }

    public class SetPatientTimeSheetPage
    {
        public SetPatientTimeSheetPage()
        {
            EmployeeList = new List<EmployeeListModel>();
            ReferralList = new List<ReferralListModel>();
            SearchPatientTimeSheetListPage = new SearchEmployeeVisitListPage();
        }
        public List<EmployeeListModel> EmployeeList { get; set; }
        public List<ReferralListModel> ReferralList { get; set; }
        [Ignore]
        public SearchEmployeeVisitListPage SearchPatientTimeSheetListPage { get; set; }
    }

    public class SetEmployeeBillingReportListPage
    {
        public SetEmployeeBillingReportListPage()
        {
            SearchEmployeeBillingReportListPage = new SearchEmployeeBillingReportListPage();
            EmployeeList = new List<EmployeeListModel>();
        }
        public SearchEmployeeBillingReportListPage SearchEmployeeBillingReportListPage { get; set; }
        public List<EmployeeListModel> EmployeeList { get; set; }
    }

    public class EmployeeListModel
    {
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }

    public class ReferralListModel
    {
        public long ReferralID { get; set; }
        public string ReferralName { get; set; }
        public string ServiceType { get; set; }
    }
    public class CareTypeListModel
    {
        public string VisitTaskID { get; set; }
        public long CareType { get; set; }
        public string VisitTaskType { get; set; }
    }

    public class TokenData
    {
        public long EmployeeId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireLogin { get; set; }
    }

    public class SearchEmployeeBillingReportListPage
    {
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PayFrequency { get; set; }

    }

    public class EmployeeBillingReportListModel
    {
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(EmployeeID.ToString()); } }

        public string EmployeeName { get; set; }
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }

        //public long BillableMinute { get; set; }
        public long AllocatedHour { get; set; }
        public long PTOHour { get; set; }
        public long WorkingHour { get; set; }

        public string AllocatedHourInStr { get; set; }
        public string PTOHourInStr { get; set; }
        public string WorkingHourInStr { get; set; }
        public string RegHourInStr { get; set; }
        public string OvertimeHoursInStr { get; set; }
        public string RegularPayHours { get; set; }
        public string OvertimePayHours { get; set; }
        //public string RegHourInStr { get; set; }
        //public string OTHourInStr { get; set; }

        public int Row { get; set; }
        public int Count { get; set; }

        //public string BillableTime = string.Format("{0} hrs {1} min", BillableMinute / 60, BillableMinute % 60);
    }


    public class SearchEmployeeVisitListPage
    {
        public long EmployeeVisitID { get; set; }
        public string ClaimActionStatus { get; set; }
        public string EmployeeIDs { get; set; }
        public string ReferralIDs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessage = "Time is Invalid")]
        public string StrStartTime
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    StartTime = timeOnly.TimeOfDay;
                }
            }
        }
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessage = "Time is Invalid")]
        public string StrEndTime
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    EndTime = timeOnly.TimeOfDay;
                }
            }
        }

        public TimeSpan StartTimeEdit { get; set; }
        public TimeSpan EndTimeEdit { get; set; }
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessage = "Time is Invalid")]
        public string StrStartTimeEdit
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    StartTimeEdit = timeOnly.TimeOfDay;
                }
            }
        }
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessage = "Time is Invalid")]
        public string StrEndTimeEdit
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    EndTimeEdit = timeOnly.TimeOfDay;
                }
            }
        }

        public int IsDeleted { get; set; }
        public int ActionTaken { get; set; }
        public bool ExecludeIncompletePending { get; set; } = false;
        public string ListOfIdsInCsv { get; set; }
        public string AdditionalNote { get; set; }
        public string PayorIDs { get; set; }
        public string CareTypeIDs { get; set; }
        public string ServiceTypeID { get; set; }
        public int IsAuthExpired { get; set; }
    }

    public class SearchMissingTimeSheetListPage
    {
        public string EmployeeIDs { get; set; }
        public string ReferralIDs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class EmployeeVisitListModel
    {
        public long EmployeeVisitID { get; set; }
        public string EncryptedEmployeeVisitID { get { return Crypto.Encrypt(EmployeeVisitID.ToString()); } }
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(EmployeeID.ToString()); } }
        public string Name { get; set; }
        public string ClaimStatus { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public string PatientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool SurveyCompleted { get; set; }
        public string SurveyComment { get; set; }
        public bool IsPCACompleted { get; set; }
        public bool IsByPassClockIn { get; set; }
        public bool IsByPassClockOut { get; set; }
        public string ByPassReasonClockIn { get; set; }
        public string ByPassReasonClockOut { get; set; }
        public bool IVRClockIn { get; set; }
        public bool IVRClockOut { get; set; }
        public int ActionTaken { get; set; }
        public string RejectReason { get; set; }
        public bool IsApprovalRequired { get; set; }
        public bool IsEarlyClockIn { get; set; }
        public string EarlyClockInComment { get; set; }
        public bool IsEarlyClockOut { get; set; }
        public string EarlyClockOutComment { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        public int PayorID { get; set; }
        public string PayorName { get; set; }
        public int CareTypeID { get; set; }
        public string CareType { get; set; }
        public string ClockInTimeLatitude { get; set; }
        public string ClockInTimeLongitude { get; set; }
        public string ClockOutTimeLatitude { get; set; }
        public string ClockOutTimeLongitude { get; set; }
        public string SignedLatitude { get; set; }
        public string SignedLongitude { get; set; }
        public string PatientLatitude { get; set; }
        public string PatientLongitude { get; set; }
        public long TotalcreatedTask { get; set; }
        public long PayorCount { get; set; }
        public string TimeSheetCompleteHours { get; set; }
        public string TotalAmount { get; set; }
        public long ARPcount { get; set; }
        public long ARAcount { get; set; }
        public long ARRcount { get; set; }
        public long DeniedCount { get; set; }
        public long InvalidVisitCount { get; set; }
        public long PaidCount { get; set; }
        public long BilledCount { get; set; }
        public long NotBilledCount { get; set; }
        public long IsPCACompletedcount { get; set; }
        public long NotIsPCACompletedcount { get; set; }
        public bool IsSigned { get; set; }
        public string BeneficiaryID { get; set; }
        public string PlaceOfService { get; set; }
        public string HHA_PCA_NP { get; set; }
        public DateTime Date { get; set; }

        public string ServiceDate
        {

            get { return Date.ToString(Constants.GlobalDateFormat); }
        }
        public string DayOfWeek { get; set; }
        public long WorkingHour { get; set; }
        public long ScheduleTime { get; set; }

        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }
        public string FacilityName { get; set; }
        public long ScheduleID { get; set; }
        public bool IsAuthExpired { get; set; }
        public double? Distance { get; set; }
        public string StrDistance
        {
            get
            {
                if (Distance == null)
                    return Resource.NALbl;

                return String.Format("+{0}", Convert.ToString(Math.Floor(Distance.Value)));
            }
        }
    }

    public class GroupTimesheetListModel
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public string PatientName { get; set; }
        public string PatAddress { get; set; }

        public long FacilityID { get; set; }
        public string FacilityName { get; set; }

        public long? EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(EmployeeID.ToString()); } }
        public string EmployeeName { get; set; }

        public string EmpEmail { get; set; }
        public string EmpMobile { get; set; }
        public string EmpAddress { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ScheduleStatusID { get; set; }
        public string ScheduleStatusName { get; set; }
        public string PlacementRequirement { get; set; }
        public string Comments { get; set; }

        public long PickUpLocation { get; set; }
        public string PickupLocationName { get; set; }
        public long DropOffLocation { get; set; }
        public string DropOffLocationName { get; set; }

        public string ParentName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public long RegionID { get; set; }
        public bool PermissionForEmail { get; set; }
        public bool PermissionForSMS { get; set; }
        public bool PermissionForVoiceMail { get; set; }
        public string WhoCancelled { get; set; }
        public DateTime? WhenCancelled { get; set; }
        public string CancelReason { get; set; }
        public string BehavioralIssue { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsReschedule { get; set; }

        public string RegionName { get; set; }
        public bool PermissionForMail { get; set; }

        public string Age { get; set; }

        public bool EmailSent { get; set; }
        public bool SMSSent { get; set; }
        public bool NoticeSent { get; set; }

        public bool PCMVoiceMail { get; set; }
        public bool PCMMail { get; set; }
        public bool PCMSMS { get; set; }
        public bool PCMEmail { get; set; }

        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public long CareTypeID { get; set; }
        public string CareType { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }


        public bool IsChecked { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }
    }

    public class SetEmployeeVisitNoteListPage
    {
        public SetEmployeeVisitNoteListPage()
        {
            SearchEmployeeVisitNoteListPage = new SearchEmployeeVisitNoteListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchEmployeeVisitNoteListPage SearchEmployeeVisitNoteListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchEmployeeVisitNoteListPage
    {
        public long EmployeeVisitNoteID { get; set; }
        public long EmployeeVisitID { get; set; }
        public string Name { get; set; }
        public string PatientName { get; set; }
        public string VisitTaskDetail { get; set; }
        public string Description { get; set; }
        public long? ServiceTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }
    public class EmployeeVisitNoteListModel
    {
        public long EmployeeVisitNoteID { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public string Name { get; set; }
        public string PatientName { get; set; }
        public string VisitTaskDetail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public long ServiceTime { get; set; }
        public long TimeInMinutes { get; set; }
        public int TaskForms { get; set; }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }

    public class ApprovalVisit
    {
        public long EmployeeVisitID { get; set; }
        public string EncryptedEmployeeVisitID { get { return Crypto.Encrypt(EmployeeVisitID.ToString()); } }
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(EmployeeID.ToString()); } }
        public string Name { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public string PatientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public bool IsChecked { get; set; }
        public bool CanApprove { get; set; }

        public int PayorID { get; set; }
        public string PayorName { get; set; }
        public int CareTypeID { get; set; }
        public string CareType { get; set; }

        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }
        public long ScheduleID { get; set; }
    }

    public class ApproveVisit
    {
        public long EmployeeVisitID { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public string ApproveNote { get; set; }
    }

    public class ApproveVisitList
    {
        public List<ApproveVisit> List { get; set; }
    }

    public class NurseSignatureItem
    {
        public long EmployeeVisitID { get; set; }
        public string EncryptedEmployeeVisitID { get { return Crypto.Encrypt(EmployeeVisitID.ToString()); } }
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(EmployeeID.ToString()); } }
        public string Name { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public string PatientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public bool IsChecked { get; set; }
        public bool CanApprove { get; set; }

        public int PayorID { get; set; }
        public string PayorName { get; set; }
        public int CareTypeID { get; set; }
        public string CareType { get; set; }
        public string ApproveNote { get; set; }

        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }
        public long ScheduleID { get; set; }
        public int IsApproved { get; set; }
        public string ClaimStatus { get; set; }
        public bool SurveyCompleted { get; set; }
        public string SurveyComment { get; set; }
        public bool IsPCACompleted { get; set; }
        public bool IsSigned { get; set; }
        public bool IsByPassClockIn { get; set; }
        public bool IsByPassClockOut { get; set; }
        public string ByPassReasonClockIn { get; set; }
        public string ByPassReasonClockOut { get; set; }
        public int ActionTaken { get; set; }
        public bool IsApprovalRequired { get; set; }
        public bool IsEarlyClockIn { get; set; }
        public string EarlyClockInComment { get; set; }
        public bool IsEarlyClockOut { get; set; }
        public string EarlyClockOutComment { get; set; }
        public long TotalcreatedTask { get; set; }
        public int AnyActionMissing { get; set; }
        public string ApproveBy { get; set; }
        public int Count { get; set; }
    }

    public class NurseSignatureVisit
    {
        public long EmployeeVisitID { get; set; }
        public string SignNote { get; set; }
    }

    public class NurseSignature
    {
        public List<NurseSignatureVisit> List { get; set; }
        public string Signature { get; set; }
    }

    public class VisitTaskDocument
    {
        public long EmployeeVisitNoteID { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public long VisitTaskID { get; set; }
        public string VisitTaskDetail { get; set; }
        public long TaskFormMappingID { get; set; }
        public string EBFormID { get; set; }
        public long ReferralDocumentID { get; set; }
        public long ComplianceID { get; set; }
        public long ReferralTaskFormMappingID { get; set; }
        public string OrbeonFormID { get; set; }
        public string DocFileName { get; set; }
        public string DocFilePath { get; set; }
        public string FormName { get; set; }
        public string FormLongName { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormId { get; set; }
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public bool IsOrbeonForm { get; set; }
    }

    public class EmployeeVisitNoteForm
    {
        public long EmployeeVisitID { get; set; }
        public long ReferralTaskFormMappingID { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public long TaskFormMappingID { get; set; }
        public long ReferralDocumentID { get; set; }
        public long ComplianceID { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public string OrbeonFormID { get; set; }
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
    }

    public class EmployeeVisitNoteFormList
    {
        public long TaskFormMappingID { get; set; }
        public bool IsRequired { get; set; }
        public long VisitTaskID { get; set; }
        public string EBFormID { get; set; }
        public long ReferralDocumentID { get; set; }
        public long ComplianceID { get; set; }
        public long ReferralTaskFormMappingID { get; set; }
        public string OrbeonFormID { get; set; }
        public string Name { get; set; }
        public string FormLongName { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormId { get; set; }
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public bool IsOrbeonForm { get; set; }
    }

    public class MissingTimeSheetModel
    {
        public long ScheduleId { get; set; }
        public long EmployeeId { get; set; }
        public long ReferralId { get; set; }
        public long CareTypeId { get; set; }
        public TimeSpan? ClockInTime { get; set; }
        public TimeSpan? ClockOutTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CareType { get; set; }
        public string EmployeeName { get; set; }
        public string PatientName { get; set; }

    }
    public class EmployeeVisitNoteListResultModel
    {
        public EmployeeVisitNoteListResultModel()
        {
            listmodel = new List<EmployeeVisitNoteListModel>();
            EmployeeVisitNoteList = new Page<EmployeeVisitNoteListModel>();
        }
        public int Result { get; set; }
        public List<EmployeeVisitNoteListModel> listmodel { get; set; }
        [Ignore]
        public Page<EmployeeVisitNoteListModel> EmployeeVisitNoteList { get; set; }
    }

    public class EmployeeVisitModel
    {
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public long EmployeeVisitID { get; set; }
        public DateTime? ClockInDate { get; set; }
        public DateTime? ClockOutDate { get; set; }
        public bool SurveyCompleted { get; set; }
        public bool IsPCACompleted { get; set; }

    }
    public class EmployeeVisitPayermodal
    {
        public long EmployeeVisitID { get; set; }
        public long ReferralPayorID { get; set; }
    }

    public class EmployeeVisitPayorAutherizationCode
    {
        public long EmployeeVisitID { get; set; }
        public long PayorID { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public long EmployeeID { get; set; }
    }

    public class VisitTime
    {
        public string ClockInTime { get; set; }
        public string ClockOutTime { get; set; }
        public bool SurveyCompleted { get; set; }
        public bool IsPCACompleted { get; set; }

        public int Result { get; set; }
    }

    public class PayorAndAutherizationCodeResult
    {
        public string AuthorizationCode { get; set; }
        public string PayorName { get; set; }
        public string Name { get; set; }
    }

    public class EmployeeVisitConclusionModel
    {
        public string VisitTaskDetail { get; set; }
        public string Description { get; set; }
        public string AlertComment { get; set; }
        public bool Desc { get; set; }
        public long EmployeeVisitNoteID { get; set; }
        public long ReferralTaskMappingID { get; set; }
    }

    public class VisitReferral
    {
        public long ReferralTaskMappingID { get; set; }
        public string VisitTaskDetail { get; set; }
        public string Description { get; set; }
        public string AlertComment { get; set; }

    }


    public class AddNoteModel
    {
        public List<VisitReferral> VisitTaskList { get; set; }
        public List<NameValueData> HourList { get; set; }
        public List<NameValueData> MinuteList { get; set; }
        public List<NameValueDataInString> YesNoList { get; set; }
    }
    public class AddDeviationModel
    {
        public List<VisitReferral> VisitTaskList { get; set; }
        public List<NameValueData> HourList { get; set; }
        public List<NameValueData> MinuteList { get; set; }
    }

    public class VisitNoteModel
    {
        public long EmployeeVisitID { get; set; }
        public long EmployeeVisitNoteID { get; set; }
        //     public long? ReferralTaskMappingID { get; set; }
        public string ReferralTaskMappingID { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }

    public class VisitConclusionModel
    {
        public long EmployeeVisitID { get; set; }
        public long EmployeeVisitNoteID { get; set; }
        public long? ReferralTaskMappingID { get; set; }
        public string Answer { get; set; }
        public string AlertComment { get; set; }
        public string Description { get; set; }
    }

    public class ChangeConclusionModel
    {
        public long EmployeeVisitNoteID { get; set; }
        public bool Description { get; set; }
    }

    public class ByPassDetailModel
    {
        public bool IsByPassClockIn { get; set; }
        public bool IsByPassClockOut { get; set; }
        public bool IsApprove { get; set; }
        public string ByPassReasonClockIn { get; set; }
        public string ByPassReasonClockOut { get; set; }
        public long EmployeeVisitID { get; set; }
        public string RejectReason { get; set; }
        public int ActionTaken { get; set; }
    }

    //Total active Referral List model
    public class PatientTotalReportListModel
    {
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }

        public string Name { get; set; }
        public string Gender { get; set; }
        public string AHCCCSID { get; set; }
        public string HealthPlan { get; set; }

        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }

        public int Row { get; set; }
        public int Count { get; set; }

        //public string BillableTime = string.Format("{0} hrs {1} min", BillableMinute / 60, BillableMinute % 60);
    }
    public class DeviationModels
    {
        public DeviationModels()
        {
            DeviationNoteModelList = new List<DeviationNoteModel>();
            DeviationList = new List<DeviationModel>();

        }

        public List<DeviationNoteModel> DeviationNoteModelList { get; set; }
        public List<DeviationModel> DeviationList { get; set; }

    }
    public class DeviationNotesModel
    {
        public long EmployeeVisitID { get; set; }
        public long DeviationID { get; set; }
        public long DeviationNoteID { get; set; }
        public long EmployeeID { get; set; }
        public string DeviationNotes { get; set; }
        public Boolean IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }
    public class DeviationNoteModel
    {
        public long DeviationID { get; set; }
        public long DeviationNoteID { get; set; }
        public long EmployeeID { get; set; }
        public string DeviationNotes { get; set; }
        public string DeviationType { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public int DeviationTime { get; set; }
        public List<WorkingHourm> WorkingHourm { get; set; }
    }
    public class WorkingHourm
    {
        public int WorkingHour { get; set; }
        public int ScheduleTime { get; set; }
    }

    //Class for Search Active patient from startDate to EndDate
    public class SetReferralTotalReportListPage
    {
        public SetReferralTotalReportListPage()
        {
            SearchPatientTotalReportListPage = new SearchPatientTotalReportListPage();
        }
        public SearchPatientTotalReportListPage SearchPatientTotalReportListPage { get; set; }
    }

    //Method for Search Active patient
    public class SearchPatientTotalReportListPage
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class PatientListModel
    {
        public PatientListModel()
        {
            patientTotalReportListModel = new List<PatientTotalReportListModel>();

        }
        public List<PatientTotalReportListModel> patientTotalReportListModel { get; set; }
    }
    public class DMASFormsModel
    {
        public DMASFormsModel()
        {
            DMASForm_90NewModel = new List<DMASForm_90NewModel>();
            DMASForm_90NewModelList = new List<DMASForm_90NewModelList>();
            ConclusionModelList = new List<ConclusionModel>();


        }
        public List<DMASForm_90NewModel> DMASForm_90NewModel { get; set; }
        public List<DMASForm_90NewModelList> DMASForm_90NewModelList { get; set; }
        [Ignore]
        public List<DateTimeModel> DateTimeModel { get; set; }
        [Ignore]
        public List<AdditionalNoteModel> AdditionalNote { get; set; }
        public List<ConclusionModel> ConclusionModelList { get; set; }

    }

    public class AdditionalNoteModel
    {
        public string AdditionalNote { get; set; }
        public string AdditionalNote1 { get; set; }
    }
    public class ConclusionModel
    {
        public long EmployeeVisitID { get; set; }
        public string Description { get; set; }
        public string VisitTaskDetail { get; set; }
        public string CareType { get; set; }
        public long VisitTaskID { get; set; }

        public string VisitTaskType { get; set; }
        public string Notes { get; set; }
        public string AlertComment { get; set; }
        public string Survey { get; set; }
        public DateTime? SurveyDate { get; set; }
        public string SurveyDates { get; set; }
        //public string SurveyDates
        //{
        //    get
        //    {
        //        return Convert.ToDateTime(SurveyDate).ToShortDateString();
        //    }
        //}
    }
}
public class SearchDMASfORM
{
    public string CareTypeID { get; set; }
    public string CareType { get; set; }
    public long ReferralID { get; set; }
    public string ReferralName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ServiceTypeID { get; set; }
    public long EmployeeID { get; set; }
    public string EmployeeName { get; set; }

    public string Dates { get; set; }
}

public class DMASForms90ModelList
{
    public DMASForms90ModelList()
    {

        DMASForm_90NewModel = new List<DMASForm_90NewModel>();
        DMASForm_90DistList = new List<DMASForm_90DistList>();
    }

    public List<DMASForm_90NewModel> DMASForm_90NewModel { get; set; }
    public List<DMASForm_90DistList> DMASForm_90DistList { get; set; }
}

public class DMASForm_90FormListModel
{
    public long EmployeeVisitID { get; set; }
    public long ReferralIDs { get; set; }
    public string CreatedDay { get; set; }
    public string EncryptedEmployeeVisitID { get { return Crypto.Encrypt(EmployeeVisitID.ToString()); } }
    public long EmployeeID { get; set; }
    public string EncryptedEmployeeID { get { return Crypto.Encrypt(EmployeeID.ToString()); } }
    public string Name { get; set; }
    public long ReferralID { get; set; }
    public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
    public string PatientName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? ClockInTime { get; set; }
    public TimeSpan? ClockOutTime { get; set; }
    public bool IsDeleted { get; set; }
    public bool SurveyCompleted { get; set; }
    public bool IsPCACompleted { get; set; }
    public bool IsByPassClockIn { get; set; }
    public bool IsByPassClockOut { get; set; }
    public string ByPassReasonClockIn { get; set; }
    public string ByPassReasonClockOut { get; set; }
    public bool IVRClockIn { get; set; }
    public bool IVRClockOut { get; set; }
    public int ActionTaken { get; set; }
    public string RejectReason { get; set; }
    public bool IsApprovalRequired { get; set; }
    public string VisitTaskType { get; set; }
    public string VisitTaskDetail { get; set; }

    public int Row { get; set; }
    public int Count { get; set; }
}
public class DMASForm_90NewModelList
{
    public string ActivityName { get; set; }
    public long ActivityId { get; set; }
    public string monday { get; set; }
    public string Tuesday { get; set; }
    public string Wednesday { get; set; }
    public string Thursday { get; set; }
    public string Friday { get; set; }
    public string Saturday { get; set; }
    public string Sunday { get; set; }
}
public class DMASForm_90NewModel
{
    public string VisitTaskDetail { get; set; }
    public long VisitTaskID { get; set; }
    public long CareType { get; set; }
    public string CareTypeName { get; set; }
    public long ReferralID { get; set; }
    public long ScheduleID { get; set; }
    public string VisitTaskCategoryName { get; set; }
    //public DateTime ClockInTime { get; set; }
    //public string ClockInTimes
    //{
    //    get
    //    {
    //        return Convert.ToDateTime(ClockInTime).ToShortTimeString();
    //    }
    //}

    //public DateTime ClockOutTime { get; set; }
    //public string ClockOutTimes
    //{
    //    get
    //    {
    //        return Convert.ToDateTime(ClockOutTime).ToShortTimeString();
    //    }
    //}
    public bool IVRClockOut { get; set; }
    public bool IVRClockIn { get; set; }
    public DateTime ScheduleDate { get; set; }
    public string ScheduleDates
    {
        get
        {
            return Convert.ToDateTime(ScheduleDate).ToShortDateString();
        }
    }
    public DateTime ScheduleEndDate { get; set; }
    public string ScheduleEndDates
    {
        get
        {
            return Convert.ToDateTime(ScheduleEndDate).ToShortDateString();
        }
    }
    public string isDone { get; set; }
    public string ReferralName { get; set; }
    public string EmployeeName { get; set; }
    public string AlertComment { get; set; }
    public string CreatedDay { get; set; }
    public string Notes { get; set; }
    public string survey { get; set; }
    public long Phone1 { get; set; }
    public long ServiceTime { get; set; }
    public string PatientSignature { get; set; }
    public DateTime PatientSignatureDate { get; set; }
    public string PatientSignatureDates
    {
        get
        {
            return Convert.ToDateTime(PatientSignatureDate).ToShortDateString();
        }
    }
    public long EmployeeSignatureID { get; set; }
    public string SignaturePath { get; set; }
    public DateTime EmployeeSignatureDate { get; set; }
    public string EmployeeSignatureDates
    {
        get
        {
            return Convert.ToDateTime(EmployeeSignatureDate).ToShortDateString();
        }
    }

}
//public class DMASForm_90NewModel
//{
//    public string EmployeeName { get; set; }
//    public long ReferralID { get; set; }
//    // public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
//    public string ReferralName { get; set; }
//    public string ScheduleDate { get; set; }
//    public string ScheduleEndDate { get; set; }
//    public string ClockInTime { get; set; }
//    public string ClockInTimes
//    {
//        get
//        {
//            return Convert.ToDateTime(ClockInTime).ToShortTimeString();
//        }
//    }
//    public string ClockOutTime { get; set; }
//    public string ClockOutTimes
//    {
//        get
//        {
//            return Convert.ToDateTime(ClockOutTime).ToShortTimeString();
//        }
//    }
//    public bool IVRClockIn { get; set; }
//    public bool IVRClockOut { get; set; }
//    public string VisitTaskType { get; set; }
//    public string VisitTaskDetail { get; set; }
//    public string AlertComment { get; set; }
//    public long ServiceTime { get; set; }
//    public string isDone { get; set; }
//    public string VisitTaskCategoryName { get; set; }
//    public long ScheduleID { get; set; }
//    public long Caretype { get; set; }
//    public string CaretypeName { get; set; }
//    public long VisitTaskID { get; set; }
//    public string StartDate { get; set; }
//    public string EndDate { get; set; }
//    public string CreatedDay { get; set; }
//    public long Phone1 { get; set; }
//    public string PatientSignature { get; set; }
//    public string PatientSignatureDate { get; set; }
//    public string PatientSignatureDates
//    {
//        get
//        {
//            return Convert.ToDateTime(PatientSignatureDate).ToShortDateString();
//        }
//    }
//    public string ServiceTimeMonday { get; set; }
//    public string ServiceTimeTuesday { get; set; }
//    public string ServiceTimeWednesday { get; set; }
//    public string ServiceTimeThursday { get; set; }
//    public string ServiceTimeFriday { get; set; }
//    public string ServiceTimeSaturday { get; set; }
//    public string ServiceTimeSunday { get; set; }


//    public int Row { get; set; }
//    public int Count { get; set; }
//    //   public List<DateTime> DateTime { get; set; }
//}
public class DateTimeModel
{

    public string DayOfWeek { get; set; }
    public string Dates { get; set; }

    //   public List<DateTime> DateTime { get; set; }
}
public class ServiceTimeModel
{

    public string ServiceTimeMonday { get; set; }
    public string ServiceTimeTuesday { get; set; }
    public string ServiceTimeWednesday { get; set; }
    public string ServiceTimeThursday { get; set; }
    public string ServiceTimeFriday { get; set; }
    public string ServiceTimeSaturday { get; set; }
    public string ServiceTimeSunday { get; set; }

}

public class DMASForm_90DistList
{
    public string ReferralName { get; set; }
    public string ScheduleDate { get; set; }
    public string ScheduleEndDate { get; set; }
    public long ScheduleID { get; set; }
    public long VisitTaskID { get; set; }

    public int Row { get; set; }
    public int Count { get; set; }
}
public class ReportCareTypeListModel
{
    public string Title { get; set; }
    public long DDMasterID { get; set; }
    //public string VisitTaskType { get; set; }
}

public class WeeklyReportModel
{
    //public WeeklyReportModel()
    //{

    //    StartDayofWeekModel = new List<StartDayofWeekModel>();
    //    LastDayofWeekModel = new List<LastDayofWeekModel>();
    //}

    public List<StartDayofWeekModel> StartDayofWeekModel { get; set; }
    public List<LastDayofWeekModel> LastDayofWeekModel { get; set; }
}
public class StartDayofWeekModel
{
    public string ReferralName { get; set; }
    public string ReferralID { get; set; }
    public string startDate { get; set; }
    public string EndDate { get; set; }
    public string Title { get; set; }
    public string ReferralFirstName { get; set; }
    public string ReferralLastName { get; set; }
    public string ServiceTyeID { get; set; }
    //public long CareTypeID { get; set; }
    public long EmployeeID { get; set; }
    public string EmployeeName { get; set; }
}
public class LastDayofWeekModel
{
    public string ReferralName { get; set; }
    public string startDate { get; set; }
    public string EndDate { get; set; }
    public string Title { get; set; }
}

public class MissingTimesheetModel
{
    public long ScheduleId { get; set; }
    public long EmployeeId { get; set; }
    public long ReferralId { get; set; }
    public long CareTypeId { get; set; }
    public TimeSpan ClockInTime { get; set; }
    public TimeSpan ClockOutTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CareType { get; set; }
    public string EmployeeName { get; set; }
    public string PatientName { get; set; }
}
public class DMASFormsDaysModel
{
    public DateTime? ScheduleDate { get; set; }
    public DateTime ClockInTime { get; set; }
    public DateTime ClockOutTime { get; set; }
    public long CareTypeId { get; set; }
}
public class ReportInfoModel
{
    public int ReportId { get; set; }
    public string ReportName { get; set; }
    public string ReportDescription { get; set; }
    public string ReportURL { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string ReportSummary { get; set; }
}
public class ReportMasterModel
{
    public long ReportID { get; set; }
    public string ReportName { get; set; }
    public string SqlString { get; set; }
    public string ReportDescription { get; set; }
    public string DataSet { get; set; }
    public string RDL_FileName { get; set; }
    public string Category { get; set; }
    public bool IsDisplay { get; set; }
    public int Count { get; set; }

}
public class EmployeeModel
{
    public long EmployeeID { get; set; }
    public string EmployeeName { get; set; }

}

public class CategoryModel
{
    public string Name { get; set; }
    public long Value { get; set; }
    public string CareType { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string ServiceName { get; set; }
    public string ServiceCode { get; set; }

}


public class PriorAuthorizationModel
{
    public string ReferralBillingAuthorizationName { get; set; }
    public string Available { get; set; }
    public string Allocated { get; set; }
    public string Used { get; set; }
    public string Remaining { get; set; }
    public string Unallocated { get; set; }


    public DateTime StartDate { get; set; }
    public string StartDates
    {
        get
        {
            if (StartDate != null)
            {
                return StartDate.ToString("MM/dd/yyyy");

            }
            return string.Empty;
        }
    }
    public DateTime EndDate { get; set; }
    public string EndDates
    {
        get
        {
            if (EndDate != null)
            {
                return EndDate.ToString("MM/dd/yyyy");

            }
            return string.Empty;
        }
    }
    public string ServiceCode { get; set; }
    public string CareType { get; set; }

}

public class FacilityDetailModel
{
    public string ReferralId { get; set; }
    public string TIN_Number { get; set; }
    public string EIN_Number { get; set; }
    public int Type { get; set; }
}