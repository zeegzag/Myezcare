using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{

    public class ReportController : BaseController
    {
        IReportDataProvider _iReportDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Client_Status + "," + Constants.Permission_Report_Referral_Detail + "," + Constants.Permission_Report_Client_Information
        + "," + Constants.Permission_Report_Internal_Service_Plan_Expiration_Dates + "," + Constants.Permission_Report_Respite_Usage + "," + Constants.Permission_Report_Attendance
        + "," + Constants.Permission_Report_Behaviour_Contract + "," + Constants.Permission_Report_Encounter_Print + "," + Constants.Permission_Report_DSP_Roster
        + "," + Constants.Permission_Report_Schedule_Attendance + "," + Constants.Permission_Report_LSOutcomeMeasurementsReport + "," + Constants.Permission_Report_LSTeamMemberCaseloads
        + "," + Constants.Permission_Report_GeneralNotice)]
        public ActionResult Index()
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.SetReportPage(SessionHelper.LoggedInID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Client_Status)]
        public JsonResult GetClientStatusReport(SearchReportModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetClientStatusReport(searchReportModel);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_RequestClientListReport)]
        public JsonResult GetRequestClientListReport(SearchRequestClientListModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetRequestClientListReport(searchReportModel);
            return JsonSerializer(response);
        }

        


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Referral_Detail)]
        public JsonResult GetReferralDetailsReport(SearchReportModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetReferralDetailsReport(searchReportModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Client_Information)]
        public JsonResult GetClientInformationReport(SearchReportModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetClientInformationReport(searchReportModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Internal_Service_Plan_Expiration_Dates)]
        public JsonResult GetInternalServicePlanReport(SearchReportModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetInternalServicePlanReport(searchReportModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Attendance)]
        public JsonResult GetAttendanceReport(SearchReportModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetAttendanceReport(searchReportModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Respite_Usage)]
        public JsonResult GetRespiteUsageReport(SearchRespiteUsageModel searchRespiteUsageModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetRespiteUsageReport(searchRespiteUsageModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Behaviour_Contract)]
        public JsonResult GetBehaviourContractReport()
        {
            return null;
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Encounter_Print)]
        public JsonResult GetEncounterPrintReport(SearchEncounterPrintModel searchEncounterPrintModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetEncounterPrintReport(searchEncounterPrintModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Snapshot_Print)]
        public JsonResult GetSnapshotPrintReport(SearchSnapshotPrintModel searchSnapshotPrintModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetSnapshotPrintReport(searchSnapshotPrintModel);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_DTR_Print)]
        public JsonResult GetDTRPrintReport(SearchDTRPrintModel searchDTRPrintModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetDTRPrintReport(searchDTRPrintModel);
            return JsonSerializer(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_GeneralNotice)]
        public JsonResult GetGeneralNoticeReport(SearchGeneralNoticeModel searchGeneralNoticeModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetGeneralNoticeReport(searchGeneralNoticeModel);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_DSP_Roster)]
        public JsonResult GetDspRosterReport(SearchDspRosterModel searchDspRosterModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetDspRosterReport(searchDspRosterModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Schedule_Attendance)]
        public ActionResult ScheduleAttendanceReportPrint(SearchScheduleAttendanceModel searchScheduleAttendanceModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.ScheduleAttendanceReport(searchScheduleAttendanceModel);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Required_Documents_For_Attendance)]
        public JsonResult GetRequiredDocsforAttendanceReport(SearchReqDocsForAttendanceModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetRequiredDocsforAttendanceReport(searchReportModel);
            return JsonSerializer(response);
        }



        [HttpGet]
        public ActionResult Download(string vpath, string fname)
        {
            Common.SendFileBytesToResponse(Server.MapCustomPath(vpath), fname);
            return null;
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public JsonResult GetReferralInfo(string searchText, int pageSize)
        {
            _iReportDataProvider = new ReportDataProvider();
            return Json(_iReportDataProvider.GetReferralInfo(pageSize, searchText));
        }


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Report_LSTeamMemberCaseloads_View_All + "," + Constants.Permission_Report_LSTeamMemberCaseloads_View_Assigned)]
        public ViewResult LsTeamMemberCaseload()
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetLsTeamMemberCaseload();
            return View((SetLSTeamMemberCaseloadPageModel)response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_LSTeamMemberCaseloads_View_All + "," + Constants.Permission_Report_LSTeamMemberCaseloads_View_Assigned)]
        public JsonResult GetLsTeamMemberCaseLoadList(SearchLSTeamMemberCaseloadReport SearchLSTMCaseloadModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iReportDataProvider = new ReportDataProvider();

            SearchLSTMCaseloadModel.LoggedInID = SessionHelper.LoggedInID;
            var strPermissions = Constants.Permission_Report_LSTeamMemberCaseloads_View_All.Split(',');
            SearchLSTMCaseloadModel.ViewAllPermission = SessionHelper.Permissions.Any(permission => strPermissions.Contains(permission.PermissionID.ToString()));

            var response = _iReportDataProvider.GetLsTeamMemberCaseLoadList(SearchLSTMCaseloadModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_LSTeamMemberCaseloads_View_All + "," + Constants.Permission_Report_LSTeamMemberCaseloads_View_Assigned)]
        public JsonResult GetLSTeamMemberCaseloadReport(SearchLSTeamMemberCaseloadReport searchLSTeamMemberCaseloadReport)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetLSTeamMemberCaseloadReport(searchLSTeamMemberCaseloadReport);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_LSTeamMemberCaseloads_View_All + "," + Constants.Permission_Report_LSTeamMemberCaseloads_View_Assigned)]
        public JsonResult SaveReferralLstmCaseloadsComment(ReferralCommentModel referralCommentModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.SaveReferralLSTMCaseloadsComment(referralCommentModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_LSOutcomeMeasurementsReport)]
        public JsonResult GetLifeSkillsOutcomeMeasurementsReport(SearchReportModel searchReportModel)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetLifeSkillsOutcomeMeasurementsReport(searchReportModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Report_Attendance)]
        public JsonResult GetBXContractStatusReport(SearchBXContractStatusReport searchBxContractStatusReport)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetBXContractStatusReport(searchBxContractStatusReport);
            return JsonSerializer(response);
        }




        #region SPECIFIC REPORT ASKED  BY PALLAV
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult BillingSummary()
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetBillingSummaryPage();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        public JsonResult GetBillingSummaryList(SearchBillingSummaryReport searchBillingSummaryReport)
        {
            _iReportDataProvider = new ReportDataProvider();
            var response = _iReportDataProvider.GetBillingSummaryList(searchBillingSummaryReport);
            return JsonSerializer(response);
        }

        #endregion
    }
}

