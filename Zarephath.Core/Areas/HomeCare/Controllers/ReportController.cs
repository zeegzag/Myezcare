using iTextSharp.text;
using iTextSharp.text.pdf;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class ReportController : BaseController
    {
        CacheHelper _cacheHelper = new CacheHelper();
        private IReportDataProvider _reportDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_EmployeeVisitReports)]
        public ActionResult EmployeeVisitList(string id)
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetEmployeeVisitListPage)_reportDataProvider.SetEmployeeVisitListPage(id).Data;
            model.IsPartial = false;
            return View(model);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Reports)]
        public ContentResult GetEmployeeVisitList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetEmployeeVisitList(searchEmployeeVisitListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_NurseSignature)]
        public ActionResult NurseSignature()
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetNurseSignaturePage)_reportDataProvider.SetNurseSignaturePage().Data;

            return View(model);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ActionResult GroupTimesheet()
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetGroupTimesheetPage)_reportDataProvider.SetGroupTimesheetPage().Data;
            return View(model);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ActionResult ReferralActivity()
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (ReferralActivityModel)_reportDataProvider.SetReferralActivityPage().Data;

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult SaveReferralActivity(ReferralActivityModel referralActivityModel, string[] refIds, string Year, string Month)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.SaveReferralActivityList(referralActivityModel, refIds, Year, Month));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult AddReferralActivityNotes(ReferralActivityNotesModel referralActivityNotesModel, string Year, string Month, int referralId)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.AddReferralActivityNotes(referralActivityNotesModel, Year, Month, referralId));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult EditDeleteReferralActivityNotes(ReferralActivityNotesModel referralActivityNotesModel, int ReferralActivityNoteId, string AddOrEdit)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.EditDeleteReferralActivityNotes(referralActivityNotesModel, ReferralActivityNoteId, AddOrEdit));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult GetReferrals(string Year, string Month, string AddOrEdit)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetReferralList(Year, Month, AddOrEdit));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult GetReferral(string Year, string Month, int referralId)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetReferral(Year, Month, referralId));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult GetReferralNotes(string Year, string Month, int referralId)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetReferralNotes(Year, Month, referralId));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult GetGroupTimesheetList(SearchGroupTimesheetListPage searchGroupTimesheetListPage, int pageIndex = 1,
                                                    int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetGroupTimesheetList(searchGroupTimesheetListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_GroupTimesheet)]
        public ContentResult SaveGroupTimesheetList(SaveGroupTimesheetList saveGroupTimesheetList)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.SaveGroupTimesheetList(saveGroupTimesheetList, SessionHelper.LoggedInID));
        }



        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_EmployeeVisitReports)]
        public ActionResult GetMissingTimeSheet(string id)
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetEmployeeVisitListPage)_reportDataProvider.SetEmployeeVisitListPage(id).Data;
            model.IsPartial = false;
            return View(model);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Reports)]
        public ContentResult GetMissingTimeSheet(SearchMissingTimeSheetListPage searchMissingTSListPage, int pageIndex = 1,
                                            int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetMissingTimeSheetList(searchMissingTSListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        public ContentResult AddMissingTimeASheet(List<MissingTimesheetModel> missingTimesheetModels)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.AddMissingTimeASheet(missingTimesheetModels));
        }


        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public ActionResult PartialEmployeeTimeSheet(string id)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.SetEmployeeVisitListPage(null);
            var model = (SetEmployeeVisitListPage)response.Data;
            model.SearchRefCalender.EmployeeID = new List<string>() { id };
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("EmployeeVisitList", response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult PartialReferralTimeSheet(string id)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.SetEmployeeVisitListPage(null);
            var model = (SetEmployeeVisitListPage)response.Data;
            model.SearchRefCalender.ReferralID = new List<string>() { id };
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("EmployeeVisitList", response.Data);
        }


        [HttpPost]
        public JsonResult SaveEmployeeVisit(EmployeeVisitModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveEmployeeVisit(model, SessionHelper.LoggedInID));
        }
        [HttpPost]
        public JsonResult SavePCAComplete(EmployeeVisitModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SavePCAComplete(model, SessionHelper.LoggedInID));
        }
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_EmployeeVisitReports)]
        public ActionResult GeneratePcaTimeSheetForm(string id)
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetEmployeeVisitListPage)_reportDataProvider.SetEmployeeVisitListPage(id).Data;
            model.IsPartial = false;
            return View(model);
        }


        [HttpPost]
        public JsonResult SaveEmployeeVisitPayer(EmployeeVisitPayermodal model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveEmployeeVisitPayer(model, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public ContentResult UpdateEmployeeVisitPayorAutherizationCode(EmployeeVisitPayorAutherizationCode employeeVisitPayorAutherizationCode)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.UpdateEmployeeVisitPayorAndAutherizationCode(employeeVisitPayorAutherizationCode, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public ContentResult DeleteEmployeeVisit(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.DeleteEmployeeVisit(searchEmployeeVisitListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }


        [HttpPost]
        public ContentResult MarkAsCompleteEmployeeVisit(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.MarkEmployeeVisitAsComplete(searchEmployeeVisitListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_EmployeeVisitReports)]
        public ActionResult EmployeeVisitNoteList()
        {
            _reportDataProvider = new ReportDataProvider();
            return View(_reportDataProvider.SetEmployeeVisitNoteList().Data);
        }

        [HttpPost]
        public ContentResult GetEmployeeVisitNoteList(SearchEmployeeVisitNoteListPage searchEmployeeVisitNoteListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetEmployeeVisitNoteList(searchEmployeeVisitNoteListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        public ContentResult GetVisitTaskDocumentList(long employeeVisitID)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetVisitTaskDocumentList(employeeVisitID));
        }

        [HttpPost]
        public ContentResult GetVisitApprovalList(string employeeVisitIDs)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetVisitApprovalList(employeeVisitIDs, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public ContentResult ApproveVisitList(ApproveVisitList approveVisitList)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.ApproveVisitList(approveVisitList, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public ContentResult GetNurseSignatureList(SearchNurceTimesheetListPage searchNurceTimesheetListPage, int pageIndex = 1,
                                                    int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetNurseSignatureList(searchNurceTimesheetListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }


        [HttpPost]
        public ContentResult NurseSignature(NurseSignature nurseSignature)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.NurseSignature(nurseSignature, SessionHelper.LoggedInID));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_DxCode_AddUpdate)]
        public ContentResult DeleteEmployeeVisitNote(SearchEmployeeVisitNoteListPage searchEmployeeVisitNoteListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.DeleteEmployeeVisitNote(searchEmployeeVisitNoteListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public JsonResult GetEmployeeVisitConclusionList(string EmployeeVisitID)
        {
            long EmployeeVisitId = Convert.ToInt64(EmployeeVisitID);
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.GetEmployeeVisitConclusionList(EmployeeVisitId));
        }

        [HttpPost]
        public JsonResult ChangeConclusionAnswer(ChangeConclusionModel ConclusionAnswer)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.ChangeConclusionAnswer(ConclusionAnswer));
        }

        [HttpPost]
        public JsonResult GetGroupVisitTask(long careType)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.GetGroupVisitTask(careType));
        }
        [HttpPost]
        public JsonResult GetGroupVisitTaskOptionList(string careType)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.GetGroupVisitTaskOptionList(careType));
        }

        [HttpPost]
        public JsonResult GetMappedVisitTask(string EmployeeVisitID)
        {
            long EmployeeVisitId = Convert.ToInt64(EmployeeVisitID);
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.GetMappedVisitTask(EmployeeVisitId));
        }

        [HttpPost]
        public JsonResult GetMappedVisitConclusion(string EmployeeVisitID)
        {
            long EmployeeVisitId = Convert.ToInt64(EmployeeVisitID);
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.GetMappedVisitConclusion(EmployeeVisitId));
        }

        [HttpPost]
        public JsonResult GetMappedVisitTaskForms(long employeeVisitID, long referralTaskMappingID)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.GetMappedVisitTaskForms(employeeVisitID, referralTaskMappingID));
        }

        [HttpPost]
        public JsonResult SaveVisitNoteOrbeonForm(EmployeeVisitNoteForm form)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveVisitNoteOrbeonForm(form));
        }

        [HttpPost]
        public JsonResult DeleteVisitNoteForm(long referralTaskFormMappingID)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.DeleteVisitNoteForm(referralTaskFormMappingID));
        }

        [HttpPost]
        public JsonResult GenerateBillingNote(long EmployeeVisitID)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.GenerateBillingNote(EmployeeVisitID));
        }

        [HttpPost]
        public JsonResult SaveVisitNote(VisitNoteModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveVisitNote(model));
        }

        [HttpPost]
        public JsonResult SaveVisitNoteTimeSheet(VisitNoteModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveVisitNoteTimeSheet(model));
        }

        [HttpPost]
        public JsonResult SaveVisitConclusion(VisitConclusionModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveVisitConclusion(model));
        }
        [HttpPost]
        public JsonResult SaveDeviationNotes(DeviationNotesModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveDeviationNotes(model));
        }

        [HttpPost]
        public JsonResult GetDeviationNotes(long EmployeeVisitID)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GetDeviationNotes(EmployeeVisitID);
            return Json(response, JsonRequestBehavior.AllowGet);
            // return Json(_reportDataProvider.GetDeviationNotes(model));
        }


        [HttpPost]
        public JsonResult BypassActionTaken(ByPassDetailModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.BypassActionTaken(model));
        }
        [HttpPost]
        public JsonResult SaveByPassReasonNote(ByPassDetailModel model)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.SaveByPassReasonNote(model));
        }
        public JsonResult DeleteDeviationNote(String ListOfIdsInCsv)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.DeleteDeviationNote(ListOfIdsInCsv, SessionHelper.LoggedInID));
        }

        // added by "Boolean SimpleTaskType = false" Sagar - 22 Dec 2019 : Dispay the Task List based on Simple and Details Task Permission
        [HttpGet]
        public ActionResult GeneratePcaTimeSheet(string id, Boolean SimpleTaskType = false)

        {
            long employeeVisitID = Convert.ToInt64(id);
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GeneratePcaTimeSheet(employeeVisitID, SimpleTaskType);
            return View(response.Data);
        }
        [HttpGet]
        public ActionResult GeneratePcaTimeSheetDayCare(string id, Boolean SimpleTaskType = false)

        {
            long employeeVisitID = Convert.ToInt64(id);
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GeneratePcaTimeSheetDayCare(employeeVisitID, SimpleTaskType);
            return View(response.Data);
        }

        [HttpPost]
        public async Task<JsonResult> GenerateMultiplePcaTimeSheet(List<string> model)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                string fullPath = Server.MapCustomPath(String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles);

                foreach (string id in model)
                {
                    await GeneratePcaTimeSheetPdf(id, true);
                }


                string fileName = String.Format("{0}_{1}.pdf", "MergedTimeSheet", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
                DirectoryInfo di = new DirectoryInfo(fullPath);
                string[] filePaths = di.GetFiles("MultiTimeSheet*").Select(f => f.FullName).ToArray();
                string outputFullPath = String.Format("{0}{1}", fullPath, fileName);

                Common.MergePDFs(outputFullPath, filePaths);
                MultiplePCA detail = new MultiplePCA();
                detail.FileName = fileName;
                detail.FilePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles + fileName;

                response.Data = detail;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }
            return Json(response);
        }

        [HttpGet]
        public ActionResult Download(string fpath, string fname, bool d)
        {
            Common.SendFileBytesToResponse(Server.MapCustomPath(fpath), fname, d);

            string fullPath = Server.MapCustomPath(String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles);
            DirectoryInfo di = new DirectoryInfo(fullPath);
            foreach (FileInfo file in di.GetFiles("MultiTimeSheet*"))
            { file.Delete(); }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> GeneratePcaTimeSheetPdf(string id, bool isMultiple = false)
        {

            long employeeVisitID = Convert.ToInt64(Crypto.Decrypt(id));

            //long employeeVisitID = Convert.ToInt64(id);

            if (employeeVisitID == 0)
                return null;
            if (SessionHelper.IsDayCare)
            {
                string url = string.Format("{0}{1}", _cacheHelper.SiteBaseURL, Constants.HC_GeneratePcaTimeSheetDaycare);
                // Start Added by Sagar 22 Dec 2019 : check Simple and Details Task Permission and Pass the boolean parameter for generate the PDF file

                Boolean SimpleTaskType = false;
                if (Common.HasPermission(Constants.Empoyee_TimeSheet_SimpleTask) && !Common.HasPermission(Constants.Empoyee_TimeSheet_DetailTask))
                    SimpleTaskType = true;
                else if (!Common.HasPermission(Constants.Empoyee_TimeSheet_SimpleTask) && Common.HasPermission(Constants.Empoyee_TimeSheet_DetailTask))
                    SimpleTaskType = false;
                else if (!Common.HasPermission(Constants.Empoyee_TimeSheet_SimpleTask) && !Common.HasPermission(Constants.Empoyee_TimeSheet_DetailTask))
                    SimpleTaskType = false;

                // end 

                SelectHtmlToPdf data = new SelectHtmlToPdf();
                url += "?id=" + employeeVisitID + "&SimpleTaskType=" + SimpleTaskType;
                byte[] pdf = await data.GeneratePDFAsync(url);
                // return resulted pdf document
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                string fileName = String.Format("{0}_{1}.pdf", "TimeSheet", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
                if (isMultiple)
                {
                    string fullPath = Server.MapCustomPath(String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles);
                    System.IO.File.WriteAllBytes(String.Format("{0}Multi{1}", fullPath, fileName), pdf);
                }
                else
                {
                    fileResult.FileDownloadName = fileName;
                }
                return fileResult;
            }
            else
            {
                string url = string.Format("{0}{1}", _cacheHelper.SiteBaseURL, Constants.HC_GeneratePcaTimeSheet);
                // Start Added by Sagar 22 Dec 2019 : check Simple and Details Task Permission and Pass the boolean parameter for generate the PDF file

                Boolean SimpleTaskType = false;
                if (Common.HasPermission(Constants.Empoyee_TimeSheet_SimpleTask) && !Common.HasPermission(Constants.Empoyee_TimeSheet_DetailTask))
                    SimpleTaskType = true;
                else if (!Common.HasPermission(Constants.Empoyee_TimeSheet_SimpleTask) && Common.HasPermission(Constants.Empoyee_TimeSheet_DetailTask))
                    SimpleTaskType = false;
                else if (!Common.HasPermission(Constants.Empoyee_TimeSheet_SimpleTask) && !Common.HasPermission(Constants.Empoyee_TimeSheet_DetailTask))
                    SimpleTaskType = false;

                // end 

                SelectHtmlToPdf data = new SelectHtmlToPdf();
                url += "?id=" + employeeVisitID + "&SimpleTaskType=" + SimpleTaskType;
                byte[] pdf = await data.GeneratePDFAsync(url);
                // return resulted pdf document
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                string fileName = String.Format("{0}_{1}.pdf", "TimeSheet", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
                if (isMultiple)
                {
                    string fullPath = Server.MapCustomPath(String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles);
                    System.IO.File.WriteAllBytes(String.Format("{0}Multi{1}", fullPath, fileName), pdf);
                }
                else
                {
                    fileResult.FileDownloadName = fileName;
                }
                return fileResult;
            }



        }

        [HttpGet]
        public ActionResult GeneratePcaTimeSheetPdf01()
        {

            string url = "https://www.policybazaar.com/motor-insurance/car-insurance/";

            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "TimeSheet", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            return fileResult;

        }


        #region Employee Billing Report
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_EmployeeBillingReports)]
        public ActionResult EmployeeBillingReport()
        {
            _reportDataProvider = new ReportDataProvider();
            SetEmployeeBillingReportListPage model = (SetEmployeeBillingReportListPage)_reportDataProvider.SetEmployeeBillingReportListPage().Data;
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_EmployeeBillingReports)]
        public ActionResult Export(List<EmployeeBillingReportListModel> employeeBillingReportDetails, SearchEmployeeBillingReportListPage searchEmployeeBillingReportListPage, string PayFrequency)
        {
            if (PayFrequency != null && employeeBillingReportDetails != null)
            {
                string charPayFreq = PayFrequency.Substring(0, 1);
                DateTime dtStartDate = Convert.ToDateTime(searchEmployeeBillingReportListPage.StartDate);
                DateTime dtEndDate = Convert.ToDateTime(searchEmployeeBillingReportListPage.EndDate);

                string startDate = dtStartDate.Month + "/" + dtStartDate.Day + "/" + dtStartDate.Year;
                string endDate = dtEndDate.Month + "/" + dtEndDate.Day + "/" + dtEndDate.Year;

                DataTable table = new DataTable();
                //columns  
                table.Columns.Add("Company Code", typeof(string));
                table.Columns.Add("Pay Frequency", typeof(string));
                table.Columns.Add("Pay Period Start Date", typeof(string));
                table.Columns.Add("Pay Period End Date", typeof(string));
                table.Columns.Add("Employee ID", typeof(string));
                table.Columns.Add("Earnings Code", typeof(string));
                table.Columns.Add("Pay Hours", typeof(string));
                table.Columns.Add("Dollars", typeof(string));
                table.Columns.Add("Separate Check", typeof(string));
                table.Columns.Add("Worked Department", typeof(string));
                table.Columns.Add("Rate Code", typeof(string));

                foreach (var rowData in employeeBillingReportDetails)
                {
                    if ((rowData.RegHourInStr != null || rowData.OvertimeHoursInStr != null) && PayFrequency != null)
                    {
                        if (rowData.RegHourInStr != null && rowData.RegHourInStr != "hrs " && rowData.RegHourInStr != "0hrs ")
                        {
                            DataRow dtRegRow = table.NewRow();
                            dtRegRow["Company Code"] = "ADPRUNCC";
                            dtRegRow["Pay Frequency"] = charPayFreq;
                            dtRegRow["Pay Period Start Date"] = startDate.ToString().Length == 1 ? ("0" + startDate.ToString()) : startDate.ToString();
                            dtRegRow["Pay Period End Date"] = endDate.ToString().Length == 1 ? ("0" + endDate.ToString()) : endDate.ToString();
                            dtRegRow["Employee ID"] = rowData.EmployeeID;
                            dtRegRow["Earnings Code"] = "REG";
                            dtRegRow["Pay Hours"] = rowData.RegHourInStr.Replace("hrs ", "");
                            dtRegRow["Dollars"] = rowData.RegularPayHours;
                            dtRegRow["Separate Check"] = "0";
                            dtRegRow["Worked Department"] = "0";
                            dtRegRow["Rate Code"] = "BASE";
                            table.Rows.Add(dtRegRow);
                        }
                        if (rowData.OvertimeHoursInStr != null && rowData.OvertimeHoursInStr != "hrs " && rowData.OvertimeHoursInStr != "0hrs ")
                        {
                            DataRow dtOVTRow = table.NewRow();
                            dtOVTRow["Company Code"] = "ADPRUNCC";
                            dtOVTRow["Pay Frequency"] = charPayFreq;
                            dtOVTRow["Pay Period Start Date"] = startDate.ToString().Length == 1 ? ("0" + startDate.ToString()) : startDate.ToString();
                            dtOVTRow["Pay Period End Date"] = endDate.ToString().Length == 1 ? ("0" + endDate.ToString()) : endDate.ToString();
                            dtOVTRow["Employee ID"] = rowData.EmployeeID;
                            dtOVTRow["Earnings Code"] = "OVT";
                            dtOVTRow["Pay Hours"] = rowData.OvertimeHoursInStr.Replace("hrs ", "");
                            dtOVTRow["Dollars"] = rowData.OvertimePayHours;
                            dtOVTRow["Separate Check"] = "0";
                            dtOVTRow["Worked Department"] = "0";
                            dtOVTRow["Rate Code"] = "BASE";
                            table.Rows.Add(dtOVTRow);
                        }

                    }
                }
                string path = Server.MapCustomPath("~/Assets/Files") + "\\EmployeeBillingReport.csv";

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                StreamWriter sw = new StreamWriter(path, false);
                //headers  
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    sw.Write(table.Columns[i]);
                    if (i < table.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in table.Rows)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < table.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            return new EmptyResult();
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_EmployeeBillingReports)]
        public ContentResult GetEmployeeBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetEmployeeBillingReportList(searchEmployeeBillingReportListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        // Total active Referral Report 


        [HttpGet]
        public ActionResult GeneratePatientActivePdfurl(string StartDate, string EndDate)
        {
            CacheHelper _cacheHelper = new CacheHelper();

            string url = string.Format("{0}{1}?Startdate={2}&EndDate={3}", _cacheHelper.SiteBaseURL, Constants.GeneratePatientActivePdf, StartDate, EndDate);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "ActivePatientDetail", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat)); ;
            return fileResult;

        }

        [HttpGet]
        public ActionResult GeneratePatientActivePdf(string StartDate, string EndDate)
        {
            SearchPatientTotalReportListPage searchPatientTotalReportListPage = new SearchPatientTotalReportListPage();
            if (StartDate != "" & EndDate != "")
            {
                searchPatientTotalReportListPage.StartDate = Convert.ToDateTime(StartDate);
                searchPatientTotalReportListPage.EndDate = Convert.ToDateTime(EndDate);
            }
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GetPatientTotalReportListDownload(searchPatientTotalReportListPage, 1, 1000, "ReferralID", "DESC");
            return View(response.Data);
        }
        public ActionResult TotalPatientReport()
        {
            _reportDataProvider = new ReportDataProvider();
            SetReferralTotalReportListPage model = (SetReferralTotalReportListPage)_reportDataProvider.SetPatientTotalReportListPage().Data;
            return View(model);
        }

        public ContentResult GetPatientTotalReportList(SearchPatientTotalReportListPage searchPatientTotalReportListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetPatientTotalReportList(searchPatientTotalReportListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }
        #endregion

        #region Demo Combine Multiple File


        public void demo()
        {
            //string[] filePaths = Directory.GetFiles(Server.MapCustomPath(String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles));
            string[] filePaths = Directory.GetFiles("D:\\Test");
        }

        #endregion

        // for DMAS fprms For Varginia 
        #region DMAD Forms
        [HttpGet]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_DMAS_90Reports)]
        public ActionResult DMAS_90Form(string id)
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetEmployeeVisitListPage)_reportDataProvider.SetEmployeeVisitListPageDMAS(id).Data;
            model.IsPartial = false;
            return View(model);
        }
        [HttpGet]
        public JsonResult GetCaretype()
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GetCaretype();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Reports)]
        public ContentResult GetDMASForm_90FormList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex = 1,
                                            int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetDMASForm_90FormList(searchEmployeeVisitListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpGet]
        public JsonResult GetEmployeeByReferralID(string ReferralID)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GetEmployeeByReferralID(ReferralID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GenerateDMAS_90FormsPdfURL(string ReferralID, string StartDate, string EndDate, string CareType, string AdditionalNote, string AdditionalNote1, string EmployeeID)
        {
            CareType = string.IsNullOrEmpty(CareType) ? "0" : CareType;
            List<string> urls = new List<string>();
            //string url = string.Format("{0}{1}?Startdate={2}&EndDate={3}&ReferralID={4}&CareType={5}&AdditionalNote={6}&AdditionalNotes1={7}", _cacheHelper.SiteBaseURL, Constants.GenerateForm90Pdf, StartDate, EndDate, ReferralID, CareType, AdditionalNote, AdditionalNote1);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = null;
            _reportDataProvider = new ReportDataProvider();
            if (string.IsNullOrEmpty(EmployeeID) || EmployeeID == "0")
            {
                ServiceResponse response = _reportDataProvider.GetEmployeeByReferralID(ReferralID, StartDate, EndDate);
                List<EmployeeListModel> employees = (List<EmployeeListModel>)response.Data;
                foreach (var employee in employees)
                {
                    List<DMASFormsDaysModel> days = _reportDataProvider.GetEmployeeVisitListDays(Convert.ToDateTime(StartDate), Convert.ToInt32(ReferralID), employee.EmployeeID, Convert.ToInt32(CareType));
                    if (days != null && days.Count > 0)
                    {
                        while (days.Count(x => x.ScheduleDate != null) > 0)
                        {
                            string current = string.Empty;
                            StringBuilder dates = new StringBuilder();
                            int count = days.Count();
                            long careTypeId = 0;
                            for (int i = 0; i < count; i++)
                            {
                                string date = days[i].ScheduleDate?.ToString("dd/MM/yyyy");
                                if (days[i].ScheduleDate == null || current == date)
                                    continue;

                                careTypeId = days[i].CareTypeId;
                                string time = days[i].ClockInTime.ToLongTimeString();
                                DateTime longDate = DateTime.ParseExact(date + " " + time, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                                dates.Append(longDate.ToString());
                                dates.Append(",");

                                current = date;
                                days[i].ScheduleDate = null;
                            }
                            dates = dates.Remove(dates.Length - 1, 1);
                            urls.Add(string.Format("{0}{1}?Startdate={2}&EndDate={3}&ReferralID={4}&CareType={5}&AdditionalNote={6}&AdditionalNotes1={7}&EmployeeID={8}&Dates={9}", _cacheHelper.SiteBaseURL, Constants.GenerateForm90Pdf, StartDate, EndDate, ReferralID, careTypeId, AdditionalNote, AdditionalNote1, employee.EmployeeID, dates));
                        }
                    }
                }

                pdf = data.GenerateHtmlUrlsToPdf(urls);
            }
            else
            {
                List<DMASFormsDaysModel> days = _reportDataProvider.GetEmployeeVisitListDays(Convert.ToDateTime(StartDate), Convert.ToInt32(ReferralID), Convert.ToInt32(EmployeeID), Convert.ToInt32(CareType));
                if (days != null && days.Count > 0)
                {
                    while (days.Count(x => x.ScheduleDate != null) > 0)
                    {
                        string current = string.Empty;
                        StringBuilder dates = new StringBuilder();
                        int count = days.Count();
                        long careTypeId = 0;
                        for (int i = 0; i < count; i++)
                        {
                            string date = days[i].ScheduleDate?.ToString("dd/MM/yyyy");
                            if (days[i].ScheduleDate == null || current == date)
                                continue;

                            careTypeId = days[i].CareTypeId;
                            string time = days[i].ClockInTime.ToLongTimeString();
                            DateTime longDate = DateTime.ParseExact(date + " " + time, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                            dates.Append(longDate.ToString());
                            dates.Append(",");

                            current = date;
                            days[i].ScheduleDate = null;
                        }
                        dates = dates.Remove(dates.Length - 1, 1);
                        urls.Add(string.Format("{0}{1}?Startdate={2}&EndDate={3}&ReferralID={4}&CareType={5}&AdditionalNote={6}&AdditionalNotes1={7}&EmployeeID={8}&Dates={9}", _cacheHelper.SiteBaseURL, Constants.GenerateForm90Pdf, StartDate, EndDate, ReferralID, careTypeId, AdditionalNote, AdditionalNote1, EmployeeID, dates));
                    }
                }
                //string url = string.Format("{0}{1}?Startdate={2}&EndDate={3}&ReferralID={4}&CareType={5}&AdditionalNote={6}&AdditionalNotes1={7}&EmployeeID={8}&Dates={9}", _cacheHelper.SiteBaseURL, Constants.GenerateForm90Pdf, StartDate, EndDate, ReferralID, CareType, AdditionalNote, AdditionalNote1, EmployeeID, dates);
                pdf = data.GenerateHtmlUrlsToPdf(urls);
            }

            //byte[] pdf = data.GenerateHtmlUrlsToPdf(urls);
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "DMAS_90Forms", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat)); ;
            return fileResult;

        }

        [HttpGet]
        public ActionResult GenerateDMAS_90FormsPdf(SearchDMASfORM searchEmployeeVisitNoteListPage)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GetEmployeeVisitList1(searchEmployeeVisitNoteListPage, false);
            return View(response.Data);

        }

        //[HttpGet]
        //public ActionResult DMAS_97_ABForms(string id)
        //{
        //    //_reportDataProvider = new ReportDataProvider();
        //    //var model = (SetEmployeeVisitListPage)_reportDataProvider.SetEmployeeVisitListPage(id).Data;
        //    //model.IsPartial = false;
        //    //return View(model);
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult DMAS_99Forms(string id)
        //{

        //    //_reportDataProvider = new ReportDataProvider();
        //    //var model = (SetEmployeeVisitListPage)_reportDataProvider.SetEmployeeVisitListPage(id).Data;
        //    //model.IsPartial = false;
        //    //return View(model);
        //    return View();
        //}

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Reports)]
        public ContentResult GetENewDMAS90List(SearchDMASfORM searchEmployeeVisitNoteListPage)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetENewDMAS90List(searchEmployeeVisitNoteListPage));
        }
        [HttpPost]
        public ContentResult GetENewDMAS90ListNew(SearchDMASfORM searchEmployeeVisitNoteListPage)
        {
            _reportDataProvider = new ReportDataProvider();
            return CustJson(_reportDataProvider.GetENewDMAS90ListNew(searchEmployeeVisitNoteListPage));
        }


        #endregion

        #region  weeklytimesheet

        [HttpGet]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_DMAS_90Reports)]
        public ActionResult WeeklyTimeSheet(string id)
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetEmployeeVisitListPage)_reportDataProvider.WeeklyTimeSheet(id).Data;
            model.IsPartial = false;
            return View(model);
        }
        [HttpGet]
        public ActionResult GenerateWeeklyTimeSheetPdf(SearchDMASfORM searchEmployeeVisitNoteListPage)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GetEmployeeVisitList1(searchEmployeeVisitNoteListPage);
            return View(response.Data);

        }
        [HttpGet]
        public ActionResult GenerateWeeklyTimeSheetPdfURL(string ReferralID, string StartDate, string EndDate, string CareType, string AdditionalNote, string AdditionalNote1)
        {
            List<string> urls = new List<string>();
            _reportDataProvider = new ReportDataProvider();
            List<DMASFormsDaysModel> days = _reportDataProvider.GetEmployeeVisitListDays(Convert.ToDateTime(StartDate), Convert.ToInt32(ReferralID), 0, Convert.ToInt32(CareType));
            if (days != null && days.Count > 0)
            {
                while (days.Count(x => x.ScheduleDate != null) > 0)
                {
                    string current = string.Empty;
                    StringBuilder dates = new StringBuilder();
                    int count = days.Count();

                    for (int i = 0; i < count; i++)
                    {
                        string date = days[i].ScheduleDate?.ToString("dd/MM/yyyy");
                        if (days[i].ScheduleDate == null || current == date)
                            continue;

                        string time = days[i].ClockInTime.ToLongTimeString();
                        DateTime longDate = DateTime.ParseExact(date + " " + time, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                        dates.Append(longDate.ToString());
                        dates.Append(",");

                        current = date;
                        days[i].ScheduleDate = null;
                    }
                    dates = dates.Remove(dates.Length - 1, 1);
                    urls.Add(string.Format("{0}{1}?Startdate={2}&EndDate={3}&ReferralID={4}&CareType={5}&AdditionalNote={6}&AdditionalNotes1={7}&EmployeeID={8}&Dates={9}", _cacheHelper.SiteBaseURL, Constants.GenerateForm90Pdf, StartDate, EndDate, ReferralID, CareType, AdditionalNote, AdditionalNote1, 0, dates));
                }
            }
            //string url = string.Format("{0}{1}?Startdate={2}&EndDate={3}&ReferralID={4}&CareType={5}&AdditionalNote={6}&AdditionalNotes1={7}", _cacheHelper.SiteBaseURL, Constants.GenerateWeeklyTimeSheetPdf, StartDate, EndDate, ReferralID, CareType, AdditionalNote, AdditionalNote1);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlsToPdf(urls);
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "WeeklyTimeSheet", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat)); ;
            return fileResult;

        }
        #endregion
        #region ReportingServiceReportMaster

        public ActionResult ReportMaster()
        {
            string ReportDescription = "";
            string rootURL = Request.Url.GetLeftPart(UriPartial.Authority);
            string ReportWebFormURL = rootURL + "/Report/Template?ReportName=";

            string tt = HttpRuntime.AppDomainAppPath;
            var rptInfo = new ReportInfoModel
            {
                ReportName = "",
                ReportDescription = ReportDescription,
                ReportURL = ReportWebFormURL,
            };
            return View(rptInfo);
        }
        [HttpGet]
        public JsonResult ReportMasterList()
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.ReportMasterList(SessionHelper.LoggedInID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetEmployeeList()
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.GetEmployeeList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CategoryList(string Category, string EmployeeVisitIDList)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.CategoryList(Category, EmployeeVisitIDList);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BulkUpdateVisitReport(string BulkType, string EmployeeVisitIDList, string Catrgory)
        {
            _reportDataProvider = new ReportDataProvider();
            return Json(_reportDataProvider.BulkUpdateVisitReport(BulkType, EmployeeVisitIDList, Catrgory, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public JsonResult GetEmployeeReportsList(string reportName, string reportDescription, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.HC_GetEmployeeReportsList(SessionHelper.LoggedInID, reportName, reportDescription, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientReportsList(string reportName, string reportDescription, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.HC_GetPatientReportsList(SessionHelper.LoggedInID, reportName, reportDescription, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetOtherReportsList(string reportName, string reportDescription, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.HC_GetOtherReportsList(SessionHelper.LoggedInID, reportName, reportDescription, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }
        #endregion
        [HttpPost]
        public JsonResult PrioAuthorization(long ReferralID, long BillingAuthorizationID)
        {
            _reportDataProvider = new ReportDataProvider();
            ServiceResponse response = _reportDataProvider.PrioAuthorization(ReferralID, BillingAuthorizationID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
