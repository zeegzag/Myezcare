using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IReportDataProvider
    {

        #region Set Report Page

        ServiceResponse SetReportPage(long loggedInId);

        #endregion

        #region  Get Client Status List

        ServiceResponse GetClientStatusReport(SearchReportModel searchReportModel);


        #endregion

        #region  Get Request ClientList

        ServiceResponse GetRequestClientListReport(SearchRequestClientListModel searchReportModel);


        #endregion



        #region  Get Referral Details Report

        ServiceResponse GetReferralDetailsReport(SearchReportModel searchReportModel);

        #endregion

        #region  Get Client Information Report

        ServiceResponse GetClientInformationReport(SearchReportModel searchReportModel);

        #endregion

        #region  Get Internal ServicePlan Report

        ServiceResponse GetInternalServicePlanReport(SearchReportModel searchReportModel);

        #endregion

        #region  Get Attendance Report

        ServiceResponse GetAttendanceReport(SearchReportModel searchReportModel);

        #endregion

        #region  Get Respite Usage Report

        ServiceResponse GetRespiteUsageReport(SearchRespiteUsageModel searchRespiteUsageModel);

        #endregion

        #region  Get Encounter Print Report
        ServiceResponse GetEncounterPrintReport(SearchEncounterPrintModel searchEncounterPrintModel);
        #endregion

        #region  Get Snapshot Print Report
        ServiceResponse GetSnapshotPrintReport(SearchSnapshotPrintModel searchEncounterPrintModel);
        #endregion

        #region  Get DTR Print Report
        ServiceResponse GetDTRPrintReport(SearchDTRPrintModel searchDTRPrintModel);
        #endregion

        #region  Get General Notice Report

        ServiceResponse GetGeneralNoticeReport(SearchGeneralNoticeModel searchGeneralNoticeModel);

        #endregion

        #region  Get DSP RosterReport Report

        ServiceResponse GetDspRosterReport(SearchDspRosterModel searchDspRosterModel);

        #endregion

        #region  Get Schedule Attendance  Report

        ServiceResponse ScheduleAttendanceReport(SearchScheduleAttendanceModel SearchScheduleAttendanceModel);
        ServiceResponse GetRequiredDocsforAttendanceReport(SearchReqDocsForAttendanceModel searchReportModel);
        #endregion

        #region LS Team Member Caseload
        ServiceResponse GetLsTeamMemberCaseload();
        ServiceResponse GetLsTeamMemberCaseLoadList(SearchLSTeamMemberCaseloadReport searchLSTeamMemberCaseloadReport, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId);
        ServiceResponse GetLSTeamMemberCaseloadReport(SearchLSTeamMemberCaseloadReport searchLSTeamMemberCaseloadReport);
        ServiceResponse SaveReferralLSTMCaseloadsComment(ReferralCommentModel referralCommentModel, long loggedInId);
        #endregion

        #region Get Life Skills Outcome Measurements

        ServiceResponse GetLifeSkillsOutcomeMeasurementsReport(SearchReportModel searchReportModel);

        #endregion

        #region Get BX Contract Status Report

        ServiceResponse GetBXContractStatusReport(SearchBXContractStatusReport searchBxContractStatusReport);

        #endregion

        List<ReferralDetailForNote> GetReferralInfo(int pageSize, string searchText = null);



        #region SPECIFIC REPORT ASKED  BY PALLAV

        ServiceResponse GetBillingSummaryPage();
        ServiceResponse GetBillingSummaryList(SearchBillingSummaryReport searchBillingSummaryReport);
        #endregion

        #region Employee Visit Reports

        ServiceResponse SetEmployeeVisitListPageDMAS(string data);
        ServiceResponse WeeklyTimeSheet(string data);
        ServiceResponse SetEmployeeVisitListPage(string data);
        ServiceResponse GetEmployeeVisitList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse SetNurseSignaturePage();
        ServiceResponse GetReferralList(string Year, string Month, string AddOrEdit);
        ServiceResponse GetReferral(string Year, string Month, int referralId);
        ServiceResponse GetReferralNotes(string Year, string Month, int referralId);
        ServiceResponse SaveReferralActivityList(ReferralActivityModel referralActivityModel, string[] referralIds, string Year, string Month);
        ServiceResponse AddReferralActivityNotes(ReferralActivityNotesModel referralActivityNotesModel, string Year, string Month, int referralId);
        ServiceResponse EditDeleteReferralActivityNotes(ReferralActivityNotesModel referralActivityNotesModel, int ReferralActivityNoteId, string AddOrEdit);
        ServiceResponse GetGroupTimesheetList(SearchGroupTimesheetListPage searchGroupTimesheetListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse SaveGroupTimesheetList(SaveGroupTimesheetList saveGroupTimesheetList, long loggedInID);
        ServiceResponse SaveEmployeeVisit(EmployeeVisitModel model, long LoggedInID);
        ServiceResponse SavePCAComplete(EmployeeVisitModel model, long LoggedInID);
        ServiceResponse SaveEmployeeVisitPayer(EmployeeVisitPayermodal model, long LoggedInID);
        ServiceResponse UpdateEmployeeVisitPayorAndAutherizationCode(EmployeeVisitPayorAutherizationCode model, long LoggedInID);
        ServiceResponse DeleteEmployeeVisit(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        ServiceResponse MarkEmployeeVisitAsComplete(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);

        ServiceResponse GetMissingTimeSheetList(SearchMissingTimeSheetListPage SearchMissingTSListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse AddMissingTimeASheet(List<MissingTimesheetModel> missingTimesheetModel);

        ServiceResponse SetEmployeeVisitNoteList();
        ServiceResponse GetEmployeeVisitNoteList(SearchEmployeeVisitNoteListPage searchEmployeeVisitNoteListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse GetVisitTaskDocumentList(long employeeVisitID);
        ServiceResponse GetVisitApprovalList(string employeeVisitIDs, long loggedInUserId);
        ServiceResponse ApproveVisitList(ApproveVisitList approveVisitList, long loggedInID);
        ServiceResponse GetNurseSignatureList(SearchNurceTimesheetListPage searchNurceTimesheetListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection,long loggedInUserId);
        ServiceResponse NurseSignature(NurseSignature nurseSignature, long loggedInID);
        ServiceResponse DeleteEmployeeVisitNote(SearchEmployeeVisitNoteListPage searchEmployeeVisitNoteListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);
        ServiceResponse DeleteDeviationNote(String ListOfIdsInCsv, long loggedInUserId);
        ServiceResponse GetEmployeeVisitConclusionList(long EmployeeVisitId);
        ServiceResponse ChangeConclusionAnswer(ChangeConclusionModel ConclusionAnswer);
        ServiceResponse GetGroupVisitTask(long careType);
        ServiceResponse GetGroupVisitTaskOptionList(string careType);
        ServiceResponse GetMappedVisitTask(long EmployeeVisitId);
        ServiceResponse GetMappedVisitConclusion(long EmployeeVisitId);
        ServiceResponse GetMappedVisitTaskForms(long employeeVisitID, long referralTaskMappingID);
        ServiceResponse SaveVisitNoteOrbeonForm(EmployeeVisitNoteForm form);
        ServiceResponse DeleteVisitNoteForm(long referralTaskFormMappingID);
        ServiceResponse GenerateBillingNote(long EmployeeVisitID);
        ServiceResponse SaveVisitNote(VisitNoteModel model);
        ServiceResponse SaveVisitNoteTimeSheet(VisitNoteModel model);
        ServiceResponse SaveVisitConclusion(VisitConclusionModel model);
        ServiceResponse BypassActionTaken(ByPassDetailModel model);
        ServiceResponse SaveByPassReasonNote(ByPassDetailModel model);
        ServiceResponse GeneratePcaTimeSheet(long employeeVisitID, Boolean GeneratePcaTimeSheet);
        ServiceResponse GeneratePcaTimeSheetDayCare(long employeeVisitID, Boolean GeneratePcaTimeSheet);
        ServiceResponse SaveDeviationNotes(DeviationNotesModel model);
        ServiceResponse GetDeviationNotes(long EmployeeVisitID);
        ServiceResponse SetPatientTimeSheetPage();
        //ServiceResponse GetPatientList();
        ServiceResponse ReportMasterList(long LoggedInID);
        ServiceResponse GetEmployeeList();
        ServiceResponse CategoryList(string Category, string EmployeeVisitIDList);
        ServiceResponse BulkUpdateVisitReport(string BulkType, string EmployeeVisitIDList, string Catrgory, long loggedInUserId);
        ServiceResponse HC_GetEmployeeReportsList(long loggedInUser, string reportName, string reportDescription, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetPatientReportsList(long loggedInUser, string reportName, string reportDescription, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse HC_GetOtherReportsList(long loggedInUser, string reportName, string reportDescription, string sortDirection, string sortIndex, int pageIndex, int pageSize);
        ServiceResponse PrioAuthorization(long ReferralID, long BillingAuthorizationID);
        #endregion

        #region Employee Billing Report
        ServiceResponse SetEmployeeBillingReportListPage();
        ServiceResponse GetEmployeeBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        #endregion

        #region Patient Total Report
        ServiceResponse SetPatientTotalReportListPage();
        ServiceResponse GetPatientTotalReportList(SearchPatientTotalReportListPage searchPatientTotalReportListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);

        ServiceResponse GetPatientTotalReportListDownload(SearchPatientTotalReportListPage searchPatientTotalReportListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        #endregion

        #region

        ServiceResponse SetDMASForm_90FormList(string data);
        ServiceResponse GetDMAS_90Forms(long referralId);
        ServiceResponse GetEmployeeVisitList1(SearchDMASfORM searchEmployeeVisitNoteListPage, bool isConclusion = true);
        ServiceResponse GetDMASForm_90FormList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse GetENewDMAS90List(SearchDMASfORM searchEmployeeVisitNoteListPage);
        ServiceResponse GetENewDMAS90ListNew(SearchDMASfORM searchEmployeeVisitNoteListPage);
        ServiceResponse GenerateWeeklyTimeSheetPdf(SearchDMASfORM searchEmployeeVisitNoteListPage);
        ServiceResponse GetCaretype();
        ServiceResponse GetEmployeeByReferralID(string referralID, string StartDate = null, string EndDate = null);

        List<DMASFormsDaysModel> GetEmployeeVisitListDays(DateTime startDate, long referralID, long employeeID, int caretype);
        #endregion

        ServiceResponse SetGroupTimesheetPage();

        ServiceResponse SetReferralActivityPage();
    }
}
