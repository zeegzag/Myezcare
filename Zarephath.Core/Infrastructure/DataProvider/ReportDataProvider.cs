using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using ExportToExcel;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;
using iTextSharp.text.pdf;
using PetaPoco;
using Document = iTextSharp.text.Document;
using Zarephath.Core.Infrastructure.Utility.Fcm;
using System.Data.SqlClient;
using System.Data;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class ReportDataProvider : BaseDataProvider, IReportDataProvider
    {
        CacheHelper _cacheHelper = new CacheHelper();

        #region Set Report Page

        public ServiceResponse SetReportPage(long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            ReportModel reportModel = GetMultipleEntity<ReportModel>(StoredProcedure.SetReportPage);

            reportModel.NotifyCaseManager = Common.SetYesNoAllList();
            reportModel.Checklist = Common.SetYesNoAllList();
            reportModel.ClinicalReview = Common.SetYesNoAllList();
            reportModel.Services = Common.SetServicesFilter();
            reportModel.Draft = Common.SetDraftFilter();
            reportModel.SearchReportModel.ServiceID = -1;
            reportModel.SearchReportModel.ChecklistID = -1;
            reportModel.SearchReportModel.ClinicalReviewID = -1;
            reportModel.SearchReportModel.NotifyCaseManagerID = -1;
            reportModel.SearchReportModel.IsSaveAsDraft = -1;
            reportModel.DeleteFilter = Common.SetDeleteFilter();
            reportModel.ExpireDateFilter = Common.SetExpireDateFilter();
            reportModel.BXContractStatusList = Common.SetBXContractStatusList();
            reportModel.SearchBXContractStatusReport.BXContractStatus = -1;
            reportModel.SearchBXContractStatusReport.ServiceID = -1;
            reportModel.SearchReportModel.CheckExpireorNot = -1;
            reportModel.SearchReportModel.IsDeleted = 0;
            reportModel.SearchRequestClientListModel.ReferralStatusID = 1;
            reportModel.SearchRequestClientListModel.IsDeleted = 0;
            reportModel.SearchScheduleAttendanceModel.RegionID = 1;

            //string ids = "1,5";
            reportModel.SearchDspRosterModel.ReferralStatusIDs = Constants.DspRosterStatus.Split(',').ToList();
            //reportModel.SearchDTRPrintModel.DriverID = -1;


            if (Common.HasPermission(Constants.Permission_View_Assinged_Referral) && !Common.HasPermission(Constants.Permission_View_All_Referral))
            {
                reportModel.SearchReportModel.AssigneeID = loggedInId;
            }
            response.Data = reportModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion

        #region  Get Client Status List

        public ServiceResponse GetClientStatusReport(SearchReportModel searchReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReportModel != null)
                SetSearchFilterForReport(searchReportModel, searchList);

            List<ClientStatusListModel> totalData = GetEntityList<ClientStatusListModel>(StoredProcedure.Rpt_GetClientSatus, searchList);

            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_ClientStatus, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region  Get Request Clinet List

        public ServiceResponse GetRequestClientListReport(SearchRequestClientListModel searchModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Parameter
            if (searchModel != null)
            {
                searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchModel.ReferralStatusID) });
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchModel.ClientName) });
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchModel.RegionID) });
                searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchModel.IsDeleted) });
            }

            #endregion

            RequestClientListPageModel totalData = GetMultipleEntity<RequestClientListPageModel>(StoredProcedure.Rpt_GetRequestClientList, searchList);

            int count = totalData.RequestClientList.Count;

            //totalData.RequestClientList.Insert(0, new RequestClientListModel() { ReferralID = "C" });
            //totalData.RequestClientList.Insert(totalData.RequestClientList.Count, new RequestClientListModel() { ReferralID = "H" });
            //if (totalData.Facility != null)
            //{
            //    totalData.RequestClientList.Insert(totalData.RequestClientList.Count, new RequestClientListModel()
            //    {
            //        ReferralID = totalData.Facility.FacilityBillingName,
            //        ClientName = totalData.Facility.AHCCCSID,
            //        DXCode = totalData.Facility.NPI,
            //        //CISNumber = totalData.Facility.EIN
            //    });
            //}

            RequestClientHeader topHeader = new RequestClientHeader() { Header = "C" };
            RequestClientHeader midHeader = new RequestClientHeader() { Header = "H" };
            List<object> data = new List<object>();
            data.Add(topHeader);
            data.AddRange(totalData.RequestClientList.Cast<object>());
            data.Add(midHeader);
            data.Add(totalData.Facility);


            if (count > 0)
            {
                string fileName = string.Format("{0}", Constants.ReportName_RequestClientList);
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_txt);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_txt);
                downloadFileModel.FileName = fileName + Constants.Extention_txt;
                CreateExcelFile.CreateTxtFromList(data, downloadFileModel.AbsolutePath, "\t");
                //CreateExcelFile.CreateExcelDocument(totalData.RequestClientList, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region Get Referral Details Report

        public ServiceResponse GetReferralDetailsReport(SearchReportModel searchReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReportModel != null)
                SetSearchFilterForReport(searchReportModel, searchList);

            List<ReferralDetailsListModel> totalData = GetEntityList<ReferralDetailsListModel>(StoredProcedure.Rpt_GetReferralDetail, searchList);

            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_ReferralDetails, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region Get Client Information Report

        public ServiceResponse GetClientInformationReport(SearchReportModel searchReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReportModel != null)
                SetSearchFilterForReport(searchReportModel, searchList);

            List<ClientInformationListModel> totalData = GetEntityList<ClientInformationListModel>(StoredProcedure.Rpt_GetClientInformation, searchList);

            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_ClientInformation, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region Get Internal ServicePlan Report

        public ServiceResponse GetInternalServicePlanReport(SearchReportModel searchReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            searchList.Add(new SearchValueData { Name = "CheckExpireorNot", Value = Convert.ToString(searchReportModel.CheckExpireorNot) });
            SetSearchFilterForReport(searchReportModel, searchList);

            List<InternalServicePlanListModel> totalData = GetEntityList<InternalServicePlanListModel>(StoredProcedure.Rpt_GetServicePlanExpiration, searchList);

            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_ServicePlanExpiration, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }

            return response;
        }

        #endregion

        #region Get Attendance Report

        public ServiceResponse GetAttendanceReport(SearchReportModel searchReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchReportModel != null)
            {
                SetSearchFilterForReport(searchReportModel, searchList);

                if (searchReportModel.ScheduleStatusID > 0)
                    searchList.Add(new SearchValueData { Name = "ScheduleStatusID", Value = Convert.ToString(searchReportModel.ScheduleStatusID) });
            }

            List<AttendanceListModel> totalData = GetEntityList<AttendanceListModel>(StoredProcedure.Rpt_GetAttendance, searchList);

            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_Attendance, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }

            return response;
        }

        #endregion

        #region Get Respite Usage Report

        public ServiceResponse GetRespiteUsageReport(SearchRespiteUsageModel searchRespiteUsageModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Set Search Parameter

            if (searchRespiteUsageModel.FisaclYear == null || Convert.ToInt32(searchRespiteUsageModel.FisaclYear) <= 0)
                searchRespiteUsageModel.FisaclYear = Convert.ToInt32(DateTime.Now.Year).ToString();

            if (searchRespiteUsageModel.AgencyID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchRespiteUsageModel.AgencyID) });

            if (searchRespiteUsageModel.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchRespiteUsageModel.RegionID) });

            if (searchRespiteUsageModel.ReferralStatusID > 0)
                searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchRespiteUsageModel.ReferralStatusID) });

            if (searchRespiteUsageModel.FisaclYear != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(Convert.ToString(Convert.ToInt32(searchRespiteUsageModel.FisaclYear) - 1) + "-" + ConfigSettings.ResetRespiteUsageMonth) + "-" + Constants.FiscalYearStartDate });

            if (searchRespiteUsageModel.FisaclYear != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(Convert.ToString(searchRespiteUsageModel.FisaclYear) + "-" + ConfigSettings.ResetRespiteUsageToMonth + "-" + Constants.FiscalYearEndDate) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchRespiteUsageModel.IsDeleted) });

            #endregion

            DateTime startDateTime = new DateTime(Convert.ToInt32(searchRespiteUsageModel.FisaclYear) - 1, ConfigSettings.ResetRespiteUsageMonth, Constants.FiscalYearStartDate);
            DateTime endDateTime = new DateTime(Convert.ToInt32(searchRespiteUsageModel.FisaclYear), ConfigSettings.ResetRespiteUsageToMonth, Constants.FiscalYearEndDate);

            string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);

            string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string fileName = string.Format("{0}_{1}_{2}-{3}_{4}", Constants.ReportName_RespiteUsage, Constants.FiscalYear, Convert.ToString(Convert.ToInt32(searchRespiteUsageModel.FisaclYear) - 1), searchRespiteUsageModel.FisaclYear, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
            var downloadFileModel = new DownloadFileModel();
            downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
            downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
            downloadFileModel.FileName = fileName + Constants.Extention_xlsx;

            //if (DateTime.Now.Date >= startDateTime && DateTime.Now.Date <= endDateTime)
            if (startDateTime.Year == 2016)
            {
                List<TempRespiteusageListModel> totalData = GetEntityList<TempRespiteusageListModel>(StoredProcedure.Rpt_GetRespiteUsage, searchList);
                if (totalData.Count > 0)
                    CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
            }
            else
            {
                List<RespiteusageListModel> totalData = GetEntityList<RespiteusageListModel>(StoredProcedure.Rpt_GetRespiteUsage, searchList);
                if (totalData.Count > 0)
                    CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);

            }
            response.IsSuccess = true;
            response.Data = downloadFileModel;
            return response;
        }

        #endregion

        #region Get Encounter Print Report

        public ServiceResponse GetEncounterPrintReport(SearchEncounterPrintModel searchEncounterPrintModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Parameter

            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchEncounterPrintModel.ReferralID) });
            if (searchEncounterPrintModel.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchEncounterPrintModel.StartDate).ToString(Constants.DbDateFormat) });

            if (searchEncounterPrintModel.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchEncounterPrintModel.EndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEncounterPrintModel.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "InternalMessaging", Value = Convert.ToString(Resource.InternalMessaging) });

            #endregion

            List<EncounterPrintListModel> encounterPrintList = GetEntityList<EncounterPrintListModel>(StoredProcedure.Rpt_EncounterPrint, searchList);


            if (encounterPrintList == null || encounterPrintList.Count == 0)
                return response;

            var groupEncounter = encounterPrintList.GroupBy(c => String.IsNullOrEmpty(c.RandomGroupID) ? Common.GenerateRandomNumber() : c.RandomGroupID).Select(grp => new
            {
                GroupID = grp.Key,
                Client = grp.ToList().First().ClientName,
                EncounterList = grp.ToList()
            }).ToList();


            //var data = encounterList;
            #region Code
            string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
            string path = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = string.Format("{0}_{1}", Constants.ReportName_EncounterPrint, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
            var downloadFileModel = new DownloadFileModel();
            downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", path, fileName, Constants.Extention_pdf);
            downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_pdf);
            downloadFileModel.FileName = fileName + Constants.Extention_pdf;


            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData
                      { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.Print_Notices).ToString() } });

            string encounterPrintHtml = emailTemplate.EmailTemplateBody;
            encounterPrintHtml = Regex.Replace(encounterPrintHtml, "<hr(.*?)>", "<hr $1 />");
            encounterPrintHtml = Regex.Replace(encounterPrintHtml, "<br(.*?)>", "<br $1 />");
            string pdfHtmlString = "";
            DateTime dt = DateTime.Now;
            int i = 1;


            #endregion


            foreach (var encounterList in groupEncounter.OrderBy(c => c.Client))
            {
                var model = encounterList.EncounterList.First();
                StringBuilder str = new StringBuilder();
                if (model.DXCodeName != null)
                {
                    string[] dxcodenamesplit = model.DXCodeName.Split('~');

                    str = new StringBuilder();
                    str.Append("<table style='padding: 2px;'>");
                    str.Append("<tbody><tr><th style='font-size: 10px; font-family: 'Arial Narrow'; font-weight: normal; width: 70%'>Diagnosis</th>" +
                        "<th style='font-size: 10px; font-family:'Arial Narrow'; width: 150px; font-weight: normal;text-align:center;'>&nbsp;&nbsp;&nbsp;&nbsp;Precedence</th></tr>");
                    foreach (var s in dxcodenamesplit)
                    {
                        string[] dxcodenameanddescsplit = s.Split('|');

                        str.Append("<tr>");
                        str.Append("<td style='font-size:10px;font-family:'Arial Narrow';'>" + dxcodenameanddescsplit[0].ToString() + "</td>");
                        str.Append("<td style='font-size:10px;font-family:'Arial Narrow';text-align:center;'>&nbsp;&nbsp;&nbsp;&nbsp;" + dxcodenameanddescsplit[1].ToString() + "</td>");
                        str.Append("</tr>");
                    }
                    str.Append("</tbody>");
                    str.Append("</table>");
                }


                model.CountPage = i;
                model.TotalPage = groupEncounter.Count;

                if (model.ServiceCodeType == "3")
                {
                    model.StartTime = model.EndTime = "";
                }
                model.ServiceCode = model.ServiceCode ?? " - ";
                model.ServiceName = model.ServiceName ?? " - ";
                model.CalculatedUnit = model.CalculatedUnit ?? " - ";
                model.PosID = model.PosID == "0" ? " - " : model.PosID;
                model.POSDetail = model.POSDetail ?? " - ";

                if (string.IsNullOrEmpty(model.CISNumber))
                {
                    model.CISNumber = Resource.NA;
                }
                model.DiagnosesStringAppend = str.ToString();


                model.ImageTag = "<img src='" + ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket + "/" + model.EmpSignature + "' height='80' width='300' style='float:right;'/>";

                var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
                if (model.Assessment != null)
                    model.Assessment = reg.Replace(model.Assessment, "");
                if (model.ActionPlan != null)
                    model.ActionPlan = reg.Replace(model.ActionPlan, "");


                if (encounterList.EncounterList.Count > 0)
                {
                    str = new StringBuilder();
                    str.Append("<table style='width: 100%; border: 1px solid #ddd; border-collapse: collapse;' cellpadding='4' cellspacing='3'>");
                    str.Append("<tbody><tr style='border: 1px solid #ddd'><th style='font-size: 10px; border: 1px solid #ddd;width:50px'>CODE</th>" +
                               "<th style='font-size: 10px; border: 1px solid #ddd;'>DESC.</th><th style='font-size: 10px; border: 1px solid #ddd;width:50px'>UNITS</th>" +
                               "<th style='font-size: 10px; border: 1px solid #ddd;width:120px'>TIME</th><th style='font-size: 10px; border: 1px solid #ddd;width:30px'>POS</th>" +
                               "<th style='font-size: 10px; border: 1px solid #ddd;'>POS DETAIL</th></tr>");

                    foreach (var lineItem in encounterList.EncounterList)
                    {
                        if (lineItem.ServiceCodeType == "3")
                        {
                            lineItem.StartTime = lineItem.EndTime = "";
                        }
                        lineItem.ServiceCode = lineItem.ServiceCode ?? " - ";
                        lineItem.ServiceName = lineItem.ServiceName ?? " - ";
                        lineItem.CalculatedUnit = lineItem.CalculatedUnit ?? " - ";
                        lineItem.PosID = lineItem.PosID == "0" ? " - " : lineItem.PosID;
                        lineItem.POSDetail = lineItem.POSDetail ?? " - ";


                        str.Append("<tr style='border: 1px solid #ddd'>");
                        str.Append("<td style='font-size: 10px; border: 1px solid #ddd; font-family: 'Arial Narrow'; text-align: left;'>" + lineItem.ServiceCode + "</td>");
                        str.Append("<td style='font-size: 10px; border: 1px solid #ddd; font-family: 'Arial Narrow'; text-align: left'>" + lineItem.ServiceName + "</td>");
                        str.Append("<td style='font-size: 10px; border: 1px solid #ddd; font-family: 'Arial Narrow'; text-align: left;'>" + lineItem.CalculatedUnit + "</td>");
                        str.Append("<td style='font-size: 10px; border: 1px solid #ddd; font-family: 'Arial Narrow'; text-align: left;'>" + lineItem.StartTime + " - " + lineItem.EndTime + "</td>");
                        str.Append("<td style='font-size: 10px; border: 1px solid #ddd; font-family: 'Arial Narrow'; text-align: left;'>" + lineItem.PosID + "</td>");
                        str.Append("<td style='font-size: 10px; border: 1px solid #ddd; font-family: 'Arial Narrow'; text-align: left;'>" + lineItem.POSDetail + "</td>");
                        str.Append("</tr>");

                        if (lineItem.NoteDetails != null)
                            lineItem.NoteDetails = reg.Replace(lineItem.NoteDetails, "");

                        str.Append("<tr style='border: 1px solid #ddd'> <td colspan='6'>");
                        str.Append("<span style='clear: both; float: left; width: 100%; display: block; padding: 5px 0px;'><u><b style='font-size: 10px;'>DAP Data:</b></u>" +
                                    "<span style='font-size: 10px;'>" + lineItem.NoteDetails + "</span></span>");
                        str.Append("</td></tr>");

                        if (lineItem.UnitType != (int)EnumUnitType.Time)
                        {
                            str.Append("<tr style='border: 1px solid #ddd'> <td colspan='6'>");
                            str.Append("<span style='clear: both; float: left; width: 100%; display: block; padding: 5px 0px;'><u><b style='font-size: 10px;'>Starting Odometer</b></u>" +
                                        "<span style='font-size: 10px;'>:&nbsp;" + lineItem.Startingodometer + "</span>&nbsp;&nbsp;&nbsp;&nbsp; <u><b style='font-size: 10px;padding-left:20px;'>Ending Odometer</b></u>" +
                                        "<span style='font-size: 10px;'>:&nbsp;" + lineItem.Endingodometer + "</span></span>");
                            str.Append("</td></tr>");
                        }

                    }
                    str.Append("</tbody>");
                    str.Append("</table>");

                    model.LineItemStringAppend = str.ToString();
                }



                model.DAPData = model.NoteDetails == null ? model.DAPData = "" : model.DAPData = Resource.DAPData;
                model.DAPAssessment = model.Assessment == null ? model.DAPAssessment = "" : model.DAPAssessment = Resource.DAPAssessment;
                model.DAPPlan = model.ActionPlan == null ? model.DAPPlan = "" : model.DAPPlan = Resource.DAPPlan;




                i++;
                model.PrintDate = String.Format("{0:MM'/'dd'/'yyyy HH:mm:ss tt}", dt);
                //    model.PrintDate = String.Format("{0:yyyy'/'MM'/'dd  HH:mm:ss tt}", dt);

                if (searchEncounterPrintModel.StartDate != null)
                {
                    model.SearchFromDate = Convert.ToDateTime(searchEncounterPrintModel.StartDate).ToString("MM'/'dd'/'yyyy");
                }
                else
                {
                    model.SearchFromDate = Resource.NA;
                }

                if (searchEncounterPrintModel.EndDate != null)
                {
                    model.SearchToDate = Convert.ToDateTime(searchEncounterPrintModel.EndDate).ToString("MM'/'dd'/'yyyy");
                }
                else
                {
                    model.SearchToDate = Resource.NA;
                }
                if (Convert.ToInt32(model.LateEntry) >= 7)
                {
                    StringBuilder strnew = new StringBuilder();
                    strnew.Append(@"<table style='width:100%'>
                                      <tr><td style='border:1px solid black;text-align:center;font-size: 8.2pt;padding:8px;padding-top:10px' align='center'><b>LATE ENTRY</b></td></tr>
                                      </table>");

                    model.LateEntryText = strnew.ToString();
                }
                pdfHtmlString = pdfHtmlString + TokenReplace.ReplaceTokens(encounterPrintHtml, model);

            }


            Byte[] bytes = Common.ReturnByteArrayFromStringForitextSharpPDF(pdfHtmlString);
            File.WriteAllBytes(downloadFileModel.AbsolutePath, bytes);
            response.IsSuccess = true;
            response.Data = downloadFileModel;


            return response;
        }

        #endregion

        #region Get Snapshot Print Report

        public ServiceResponse GetSnapshotPrintReport(SearchSnapshotPrintModel searchEncounterPrintModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();


            var searchlist = new List<SearchValueData>
                    {

                         new SearchValueData {
                            Name = "IsFromMonthlyService", Value = "0"
                        }
                        //  new SearchValueData {
                        //    Name = "ScheduleStatusID", Value =
                        //    Convert.ToString((int)ScheduleStatus.ScheduleStatuses.Confirmed) 
                        //}
                        //new SearchValueData {
                        //    Name = "ReferralStatusIDs", Value =Convert.ToString((int)Common.ReferralStatusEnum.Active)
                        //},
                        //new SearchValueData {
                        //    Name = "PayorIDs", Value =
                        //    Convert.ToString( (int)Payor.PayorCode.CAZ + "," +(int)Payor.PayorCode.MMIC  + "," +(int) Payor.PayorCode.UHC) 
                        //},
                    };


            if (searchEncounterPrintModel.StartDate.HasValue)
                searchlist.Add(new SearchValueData { Name = "FromDate", Value = searchEncounterPrintModel.StartDate.Value.ToString(Constants.DbDateFormat) });
            if (searchEncounterPrintModel.EndDate.HasValue)
                searchlist.Add(new SearchValueData { Name = "ToDate", Value = searchEncounterPrintModel.EndDate.Value.ToString(Constants.DbDateFormat) });
            if (searchEncounterPrintModel.ReferralID > 0)
                searchlist.Add(new SearchValueData { Name = "ReferralID", Value = searchEncounterPrintModel.ReferralID.ToString() });
            if (searchEncounterPrintModel.FacilityID > 0)
                searchlist.Add(new SearchValueData { Name = "FacilityID", Value = searchEncounterPrintModel.FacilityID.ToString() });
            if (searchEncounterPrintModel.CreatedBy > 0)
                searchlist.Add(new SearchValueData { Name = "CreatedBy", Value = searchEncounterPrintModel.CreatedBy.ToString() });
            if (!string.IsNullOrEmpty(searchEncounterPrintModel.ReferralMonthlySummariesIDs))
                searchlist.Add(new SearchValueData { Name = "ReferralMonthlySummariesIDs", Value = searchEncounterPrintModel.ReferralMonthlySummariesIDs });


            AttandanceMonthlySummaryModel model = GetMultipleEntity<AttandanceMonthlySummaryModel>(StoredProcedure.GetAttendanceNotificationList, searchlist);
            List<AttandanceNotificationEmailListModel> AttandanceNotificationList = new List<AttandanceNotificationEmailListModel>();


            AttandanceNotificationList.AddRange(model.Attandance);
            //AttandanceNotificationList.AddRange(model.NonAttandance);

            if (!AttandanceNotificationList.Any())
            {
                response.IsSuccess = false;
                //response.Message = Resource.NoReferralFound;
                return response;
            }

            ICronJobDataProvider cronjobDataProvider = new CronJobDataProvider();
            DownloadFileModel downloadFileModel = cronjobDataProvider.GenratePdfMonthlySummary(AttandanceNotificationList);

            response.IsSuccess = true;
            response.Data = downloadFileModel;


            return response;
        }

        #endregion

        #region Get DTR Print Report

        public ServiceResponse GetDTRPrintReport(SearchDTRPrintModel searchDTRPrintModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Parameter

            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchDTRPrintModel.ReferralID) });
            if (searchDTRPrintModel.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchDTRPrintModel.StartDate).ToString(Constants.DbDateFormat) });

            if (searchDTRPrintModel.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchDTRPrintModel.EndDate).ToString(Constants.DbDateFormat) });

            if (searchDTRPrintModel.DriverID > 0)
                searchList.Add(new SearchValueData { Name = "DriverID", Value = searchDTRPrintModel.DriverID.ToString() });

            if (searchDTRPrintModel.VehicleNumber != null)
                searchList.Add(new SearchValueData { Name = "VehicleNumber", Value = searchDTRPrintModel.VehicleNumber });

            if (searchDTRPrintModel.VehicleType != null)
                searchList.Add(new SearchValueData { Name = "VehicleType", Value = searchDTRPrintModel.VehicleType });

            if (searchDTRPrintModel.PickUpAddress != null)
                searchList.Add(new SearchValueData { Name = "PickUpAddress", Value = searchDTRPrintModel.PickUpAddress });

            if (searchDTRPrintModel.DropOffAddress != null)
                searchList.Add(new SearchValueData { Name = "DropOffAddress", Value = searchDTRPrintModel.DropOffAddress });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchDTRPrintModel.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "InternalMessaging", Value = Convert.ToString(Resource.InternalMessaging) });

            #endregion

            #region Getting List for DTR Print and also Doing Group By With Driver & Client Details.

            List<DTRPrintListModel> dtrPrintList = GetEntityList<DTRPrintListModel>(StoredProcedure.Rpt_DTRPrint, searchList);

            if (dtrPrintList == null || dtrPrintList.Count == 0)
                return response;

            List<DTRGroupListModel> groupDTR = dtrPrintList.GroupBy(m => new
            {
                m.DriverName,
                m.VehicleNumber,
                m.VehicleType,
                m.ServiceDate,
                m.AHCCCSID
            }).Select(m => new DTRGroupListModel
            {
                DtrGroupByModel = new DTRGroupByModel
                {
                    DriverName = m.Key.DriverName,
                    VehicleNumber = m.Key.VehicleNumber,
                    VehicleType = m.Key.VehicleType,
                    ServiceDate = m.Key.ServiceDate,
                    AHCCCSID = m.Key.AHCCCSID
                },
                DtrPrintListModel = m.ToList()
            }).OrderBy(m => m.DtrGroupByModel.ServiceDate).ToList();

            #endregion

            #region Code Set File Name and Getting Email Template
            string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
            string path = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileName = string.Format("{0}_{1}", Constants.ReportName_DTRPrint, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
            var downloadFileModel = new DownloadFileModel();
            downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", path, fileName, Constants.Extention_pdf);
            downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_pdf);
            downloadFileModel.FileName = fileName + Constants.Extention_pdf;


            EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData
                      { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.DTR_Print).ToString() } });

            string encounterPrintHtml = emailTemplate.EmailTemplateBody;
            encounterPrintHtml = Regex.Replace(encounterPrintHtml, "<hr(.*?)>", "<hr $1 />");
            encounterPrintHtml = Regex.Replace(encounterPrintHtml, "<br(.*?)>", "<br $1 />");
            string pdfHtmlString = "";

            int i = 1;

            #endregion

            int j = 1;//, pageno = 0;

            #region New Code

            foreach (var models in groupDTR)
            {
                DTRPrintListModel list = new DTRPrintListModel();

                list.DriverName = models.DtrGroupByModel.DriverName;
                list.VehicleNumber = models.DtrGroupByModel.VehicleNumber;
                list.VehicleType = models.DtrGroupByModel.VehicleType;
                list.ServiceDate = models.DtrGroupByModel.ServiceDate;
                list.AHCCCSLogoImage = "<img src='" + _cacheHelper.SiteBaseURL + Constants.AHCCCSLogoImage + "' width='50px' style='float:right;'/>";

                #region  set the value of variable

                StringBuilder str = new StringBuilder();

                int index = 1;
                int pageno = 0;
                ;
                int count = models.DtrPrintListModel.GroupBy(f => f.RandomGroupID != null ? (object)f.RandomGroupID : new object(), key => new { Object = key }).Count();
                decimal totalPages = Math.Floor(Convert.ToDecimal(count / 3));
                totalPages = models.DtrPrintListModel.Count % 3 == 0 ? totalPages : totalPages + 1;



                foreach (var item in models.DtrPrintListModel.OrderBy(m => m.DateStartTime).ToList())
                {
                    if (item != null && !item.IsUsed)
                    {
                        str.Append("<table style='width: 100%; padding-top:30px; border: 1px solid ; border-collapse: collapse;' cellpadding='4' cellspacing='3'>");

                        str.Append("<tbody>" +
                                   "<tr style='border: 1px solid;'>" +
                                   "<th style='width: 24%; font-size: 12px; background-color:#E5E7E9; border: 1px ;font-family: 'Century Gothic';' colspan='3' ><strong>Recipient Name</strong></th>" +
                                   "<th style='width: 11%; font-size: 12px; background-color:#E5E7E9; border: 1px ;font-family: 'Century Gothic';' colspan='1'><strong>Pick up time</strong></th>" +
                                   "<th style='width: 11%; font-size: 12px; background-color:#E5E7E9; border: 1px ;font-family: 'Century Gothic';' colspan='1'><strong>Pick up odometer</strong></th>" +
                                   "<th style='width: 11%; font-size: 12px; background-color:#E5E7E9; border: 1px ;font-family: 'Century Gothic';' colspan='1'><strong>Drop off time</strong></th>" +
                                   "<th style='width: 11%; font-size: 12px; background-color:#E5E7E9; border: 1px ;font-family: 'Century Gothic';' colspan='1'><strong>Drop off odometer</strong></th>" +
                                   "<th style='width: 21%; font-size: 12px; background-color:#E5E7E9; border: 1px ;font-family: 'Century Gothic';' colspan='1'><strong>Recipient Signature</strong></th>" +
                                   "<th style='width: 11%; font-size: 12px; background-color:#E5E7E9; border: 1px ;font-family: 'Century Gothic';' colspan='1'><strong>Trip miles</strong></th>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='3'> " +
                                   item.ClientName + " </td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>" +
                                   item.StartTime + "</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>" +
                                   item.Startingodometer + "</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>" +
                                   item.EndTime + "</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>" +
                                   item.Endingodometer + "</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspban='1'></td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>" +
                                   item.Tripmile + " </td>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd;display:block;'>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='3'>&nbsp;&nbsp;</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>&nbsp;&nbsp;</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>&nbsp;&nbsp;</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>&nbsp;&nbsp;</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>&nbsp;&nbsp;</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>&nbsp;&nbsp;</td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='1'>&nbsp;&nbsp;</td>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td style='font-size: 12px;  background-color:#D7DBDD; border: 1px ;font-family: 'Century Gothic';' colspan='3'><strong>Pick up location <br></br>& address</strong></td>" +
                                   "<td style='font-size: 12px; border: 1px ;font-family: 'Century Gothic';' colspan='6'>" +
                                   item.PickUpAddress + "</td>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td style='font-size: 12px; border: 1px; background-color:#D7DBDD; font-family: 'Century Gothic';' colspan='3'><strong>Drop off location <br></br>& address</strong></td>" +
                                   "<td style='font-size: 12px; border: 1px;font-family: 'Century Gothic';' colspan='6'>" +
                                   item.DropOffAddress + "</td>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td colspan='6'></td>" +
                                   "<td style='font-size: 12px;font-family: 'Century Gothic';' colspan='3'><span style='Padding-left:100px'></span><span>Round Trip </span><span style='margin-left:5px;'><b><u>" +
                                   item.RoundTrip +
                                   "</u></b>&nbsp;&nbsp;&nbsp;</span><span style='margin-left:20px;text-align:'right''>One Way </span><span style='margin-left:5px;'><b><u>" +
                                   item.OneWay +
                                   "</u></b>&nbsp;&nbsp;&nbsp;</span><span style='padding-left:20px;'>Multiple Stops </span><span style='padding-left:5px;'><b><u>" +
                                   item.MultiStops + "</u></b></span>  </td>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td colspan='7'></td>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td style='font-size: 12px; font-family: 'Century Gothic';' colspan='4'><span style='padding-top: 15px'>AHCCCS #: </span><span><u>" +
                                   item.AHCCCSID + "</u></span> </td>" +
                                   "<td style='font-size: 12px; font-family: 'Century Gothic';' colspan='5'><span style='padding-left:30px;'>Mailing Address : </span><span style='padding-left:5px;'><u>" +
                                   item.MailingAddress + " </u></span></td>" +
                                   "</tr>");

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td style='font-size: 12px; font-family: 'Century Gothic';' colspan='9'><span>Date of Birth : </span><span><u>" +
                                   item.Dob + "</u></span> </td>" +
                                   "</tr>");

                        str.Append("<tr>" +
                                   "<td style='font-size: 12px; font-family: 'Century Gothic';' colspan='9'><span style='padding-top: 15px'>Reason for Visit/Diagnosis (Be specific) : </span><span><u>" +
                                   item.DXCodeName + "</u></span> </td>" +
                                   "</tr>");

                        #region  Check if null or not

                        if (item.EscortName == null)
                        {
                            item.EscortName = Resource.NA;
                        }
                        if (item.RelationShip == null)
                        {
                            item.RelationShip = Resource.NA;
                        }

                        #endregion

                        str.Append("<tr style='border: 1px solid #ddd'>" +
                                   "<td style='font-size: 12px; font-family: 'Century Gothic';' colspan='4'><span style='padding-top: 15px'>Name of Escort : </span><span><u>" +
                                   item.EscortName + "</u></span> </td>" +
                                   "<td style='font-size: 12px; font-family: 'Century Gothic';' colspan='5'><span style='padding-left:30px;'>Relationship  :</span><span style='padding-left:5px;'><u>" +
                                   item.RelationShip + " </u></span></td>" +
                                   "</tr>");

                        str.Append("</tbody>");
                        str.Append("</table>");
                        str.Append("<br></br>");

                        if (!string.IsNullOrEmpty(item.RandomGroupID))
                        {
                            foreach (var temp in models.DtrPrintListModel.Where(x => x.RandomGroupID == item.RandomGroupID && x.NoteID != item.NoteID).ToList())
                            {
                                temp.IsUsed = true;
                            }
                        }


                        #region Page Break

                        var groupCount = models.DtrPrintListModel.GroupBy(f => f.RandomGroupID != null ? (object)f.RandomGroupID : new object(), key => new { Object = key }).Count();

                        //int groupCount = models.DtrPrintListModel.GroupBy(c => c.RandomGroupID).ToList().Count;
                        //int groupCount = models.DtrPrintListModel.GroupBy(c => c.RandomGroupID).ToList().Count;

                        if (index % 3 == 0 && index < groupCount || index == groupCount)
                        {
                            pageno++;
                            str.Append("<div><span style='padding-left: 20px; font-size: 10px; font-family: 'Century Gothic';'>This is to certify that the information is true, and complete. I understand that payment and satisfaction of this claim will be from Federal and State funds,        and that any false claims, statements or documents, or concealment of a material fact, may be prosecuted under applicable Federal of State laws.</span></div>" +
                                       "<div style='padding-top: 30px;'>" +
                                       "<table width='100%'>" +
                                       "<tbody>" +
                                       "<tr style='padding-top: 10px; padding-left: 10px; padding-bottom: 10px'>" +
                                       "<td style='float: left; width: 50%'>" +
                                       "<span style='vertical-align: top'>" +
                                       "<span style='font-size: 12px; font-family: 'Century Gothic';float:left;'>Driver Signature</span>" +
                                       "<span>&nbsp;&nbsp;<img class='img-responsive signature-image' src='" + ConfigSettings.AmazonS3Url + ConfigSettings.ZarephathBucket + "/" + models.DtrPrintListModel.First().EmpSignature + "' width='150px' /></span>"
                                        + "</span>" +
                                        "</td>" +
                                       "<td style='text-align: right; width: 25%'><span style='font-size: 12px; font-family: 'Century Gothic';'>Date:&nbsp;&nbsp;<span style='padding-left: 10px;'><u>" + models.DtrGroupByModel.ServiceDate + "</u></span>" +
                                        "</span>" +
                                        "</td>" +
                                        "<td style='text-align: right; width: 25%'>" +
                                        //"<span style='font-size: 12px; font-family: 'Century Gothic';'>Page <u>" + pageno + "</u> of ##TotalPage##<span style='padding-left: 10px;'></span>" +
                                        "<span style='font-size: 12px; font-family: 'Century Gothic';'>Page <u>" + pageno + "</u> of <u>" + Convert.ToInt16(totalPages) + "</u><span style='padding-left: 10px;'></span>" +
                                        "</span>" +
                                        "</td>" +
                                       "</tr>" +
                                       "</tbody>" +
                                       "</table>" +
                                       "</div>");
                            str.Append("<div style='page-break-after: always'></div>");
                        }

                        #endregion

                        if (!item.IsUsed)
                        {
                            index++;
                            item.IsUsed = true;
                        }

                        //list.CountPage = j;
                        //list.TotalPage = groupDTR.Count;
                    }
                }

                #endregion

                j++;

                list.DTRLOGLIST = str.ToString();
                pdfHtmlString = pdfHtmlString + TokenReplace.ReplaceTokens(encounterPrintHtml, list);
            }

            //pdfHtmlString = pdfHtmlString.Replace("##TotalPage##", pageno.ToString());

            #endregion

            Byte[] bytes = Common.ReturnByteArrayFromStringForitextSharpPDF(pdfHtmlString);
            File.WriteAllBytes(downloadFileModel.AbsolutePath, bytes);
            response.IsSuccess = true;
            response.Data = downloadFileModel;
            return response;
        }

        #endregion

        #region Get General Notice Report

        public ServiceResponse GetGeneralNoticeReport(SearchGeneralNoticeModel searchGeneralNoticeModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Params
            if (searchGeneralNoticeModel.RegionID > 0) searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchGeneralNoticeModel.RegionID) });
            if (searchGeneralNoticeModel.AgencyID > 0) searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchGeneralNoticeModel.AgencyID) });
            if (searchGeneralNoticeModel.PayorID > 0) searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchGeneralNoticeModel.PayorID) });
            if (searchGeneralNoticeModel.ReferralStatusID > 0) searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchGeneralNoticeModel.ReferralStatusID) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(searchGeneralNoticeModel.ReferralID) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchGeneralNoticeModel.IsDeleted) });
            #endregion

            List<GeneralNoticeModel> genralNoticeList = GetEntityList<GeneralNoticeModel>(StoredProcedure.Rpt_GeneralNotice, searchList);

            if (genralNoticeList.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string path = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_GeneralNotice, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", path, fileName, Constants.Extention_pdf);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_pdf);
                downloadFileModel.FileName = fileName + Constants.Extention_pdf;

                EmailTemplate emailTemplate = GetEntity<EmailTemplate>(new List<SearchValueData> { new SearchValueData
                      { Name = "EmailTemplateTypeID", Value = ((int)EnumEmailType.General_Notice).ToString() } });

                //string encounterPrintHtml = Common.ReadHtmlFile(ConfigSettings.EncounterPdfHtmlPath);
                string generalNoticeHtml = emailTemplate.EmailTemplateBody;

                generalNoticeHtml = Regex.Replace(generalNoticeHtml, "<hr(.*?)>", "<hr $1 />");
                generalNoticeHtml = Regex.Replace(generalNoticeHtml, "<br(.*?)>", "<br $1 />");

                string pdfHtmlString = "";
                DateTime dt = DateTime.Now;
                int i = 1;

                foreach (var model in genralNoticeList)
                {
                    StringBuilder str = new StringBuilder();
                    model.CountPage = i;
                    model.TotalPage = genralNoticeList.Count;
                    model.DiagnosesStringAppend = str.ToString();
                    var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
                    i++;
                    pdfHtmlString = pdfHtmlString + TokenReplace.ReplaceTokens(generalNoticeHtml, model);
                }

                Byte[] bytes = Common.ReturnByteArrayFromStringForitextSharpPDF(pdfHtmlString);
                File.WriteAllBytes(downloadFileModel.AbsolutePath, bytes);

                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }



            return response;
        }

        #endregion

        #region Get DSP Roster Report

        public ServiceResponse GetDspRosterReport(SearchDspRosterModel searchDspRosterModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Parameter

            searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchDspRosterModel.AgencyID) });
            searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchDspRosterModel.PayorID) });

            if (searchDspRosterModel.ReferralStartDate != null)
                searchList.Add(new SearchValueData { Name = "ReferralStartDate", Value = Convert.ToDateTime(searchDspRosterModel.ReferralStartDate).ToString(Constants.DbDateFormat) });

            if (searchDspRosterModel.ReferralEndDate != null)
                searchList.Add(new SearchValueData { Name = "ReferralEndDate", Value = Convert.ToDateTime(searchDspRosterModel.ReferralEndDate).ToString(Constants.DbDateFormat) });

            if (searchDspRosterModel.ServiceStartDate != null)
                searchList.Add(new SearchValueData { Name = "ServiceStartDate", Value = Convert.ToDateTime(searchDspRosterModel.ServiceStartDate).ToString(Constants.DbDateFormat) });

            if (searchDspRosterModel.ServiceEndDate != null)
                searchList.Add(new SearchValueData { Name = "ServiceEndDate", Value = Convert.ToDateTime(searchDspRosterModel.ServiceEndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchDspRosterModel.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "MISSING", Value = Convert.ToString(Constants.Missing) });
            searchList.Add(new SearchValueData { Name = "EXPIRED", Value = Convert.ToString(Constants.Expired) });

            if (searchDspRosterModel.ReferralStatusIDs.Count > 0)
            {
                string ids = string.Join(",", new List<string>(searchDspRosterModel.ReferralStatusIDs).ToArray());
                searchList.Add(new SearchValueData
                {
                    Name = "ReferralStatusIDs",
                    Value = ids
                });
            }

            #endregion

            DspRosterList totalData = GetMultipleEntity<DspRosterList>(StoredProcedure.Rpt_GetDspRoster, searchList);

            if (totalData.DspRosterListModel.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_DspRoster, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;

                CreateExcelFile.CreateExcelDocumentOfTwoDifferentType(
                    new List<List<DspRosterListModel>> { totalData.DspRosterListModel }, downloadFileModel.AbsolutePath, new List<List<ClosureDspRosterListModel>> { totalData.ClosureDspRosterListModel });

                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region Set Search Parameter for List

        private static void SetSearchFilterForReport(SearchReportModel searchReportModel, List<SearchValueData> searchList)
        {
            if (searchReportModel.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchReportModel.RegionID) });

            if (searchReportModel.AgencyID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchReportModel.AgencyID) });

            if (searchReportModel.ReferralStatusID > 0)
                searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchReportModel.ReferralStatusID) });

            if (searchReportModel.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchReportModel.StartDate).ToString(Constants.DbDateFormat) });

            if (searchReportModel.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchReportModel.EndDate).ToString(Constants.DbDateFormat) });

            #region Extra Parameter

            if (searchReportModel.AssigneeID > 0)
            {
                searchList.Add(new SearchValueData { Name = "AssigneeID", Value = Convert.ToString(searchReportModel.AssigneeID) });
            }

            if (!string.IsNullOrEmpty(searchReportModel.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchReportModel.ClientName) });

            searchList.Add(new SearchValueData { Name = "NotifyCaseManagerID", Value = Convert.ToString(searchReportModel.NotifyCaseManagerID) });

            searchList.Add(new SearchValueData { Name = "ChecklistID", Value = Convert.ToString(searchReportModel.ChecklistID) });

            if (searchReportModel.AgencyLocationID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyLocationID", Value = Convert.ToString(searchReportModel.AgencyLocationID) });

            if (searchReportModel.CaseManagerID > 0)
                searchList.Add(new SearchValueData { Name = "CaseManagerID", Value = Convert.ToString(searchReportModel.CaseManagerID) });

            if (!string.IsNullOrEmpty(searchReportModel.AHCCCSID))
            {
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchReportModel.AHCCCSID) });
            }

            if (!string.IsNullOrEmpty(searchReportModel.CISNumber))
            {
                searchList.Add(new SearchValueData { Name = "CISNumber", Value = Convert.ToString(searchReportModel.CISNumber) });
            }
            searchList.Add(new SearchValueData { Name = "ClinicalReviewID", Value = Convert.ToString(searchReportModel.ClinicalReviewID) });

            if (searchReportModel.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchReportModel.PayorID) });

            if (searchReportModel.ServiceID != -1)
                searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchReportModel.ServiceID) });



            searchList.Add(new SearchValueData { Name = "IsSaveAsDraft", Value = Convert.ToString(searchReportModel.IsSaveAsDraft) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchReportModel.IsDeleted) });

            #endregion

        }

        #endregion

        #region Get Schedule Attendance Report

        public ServiceResponse ScheduleAttendanceReport(SearchScheduleAttendanceModel searchScheduleAttendanceModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Parameter

            searchList.Add(new SearchValueData { Name = "RegionID", Value = searchScheduleAttendanceModel.RegionID.ToString() });

            if (searchScheduleAttendanceModel.StartDate != null)
            {
                searchList.Add(new SearchValueData
                {
                    Name = "StartDate",
                    Value = Convert.ToDateTime(searchScheduleAttendanceModel.StartDate).ToString(Constants.DbDateFormat)
                });
            }
            else
            {
                searchList.Add(new SearchValueData
                {
                    Name = "StartDate",
                    Value = Convert.ToDateTime(DateTime.Now).ToString(Constants.DbDateFormat)
                });
            }
            if (searchScheduleAttendanceModel.EndDate != null)
                searchList.Add(new SearchValueData
                {
                    Name = "EndDate",
                    Value = Convert.ToDateTime(searchScheduleAttendanceModel.EndDate).ToString(Constants.DbDateFormat)
                });


            #endregion

            List<SchedulAttendanceListModel> schedulAttendanceListModel = GetEntityList<SchedulAttendanceListModel>(StoredProcedure.Rpt_GetScheuldeAttendanceList, searchList);
            SceduleAttendanceModel sceduleAttendanceModel = new SceduleAttendanceModel();

            if (schedulAttendanceListModel.Count > 0)
            {
                #region Genrate Html Desgine for CODE

                sceduleAttendanceModel.SearchScheduleAttendanceModel = searchScheduleAttendanceModel;

                List<long> ids = new List<long> { Convert.ToInt64(ScheduleStatus.ScheduleStatuses.Cancelled), Convert.ToInt64(ScheduleStatus.ScheduleStatuses.No_Show), Convert.ToInt64(ScheduleStatus.ScheduleStatuses.No_Confirmation) };

                sceduleAttendanceModel.SchedulAttendanceFacilityNameListModel =
                    schedulAttendanceListModel.Where(m => !ids.Contains(m.ScheduleStatusID)).GroupBy(p => new { p.FacilityHouseName },
                               p => p, (key, g) => new SchedulAttendanceFacilityNameListModel
                               {
                                   FacilityHouseName = key.FacilityHouseName,
                                   SchedulFacilityListModel = g.OrderBy(p => p.ClientName).ToList()
                               }).ToList();



                var regionName = "";

                var attendanceListModel = schedulAttendanceListModel.FirstOrDefault();
                if (attendanceListModel != null)
                {
                    regionName = attendanceListModel.RegionName;
                }
                sceduleAttendanceModel.SearchScheduleAttendanceModel.RegionName = regionName;

                if (searchScheduleAttendanceModel.StartDate != null)
                {
                    sceduleAttendanceModel.SearchScheduleAttendanceModel.StrStartDate = Convert.ToDateTime(searchScheduleAttendanceModel.StartDate).ToString(Constants.GlobalDateFormat).Replace("-", "/");
                }
                if (searchScheduleAttendanceModel.EndDate != null)
                {
                    sceduleAttendanceModel.SearchScheduleAttendanceModel.StrEndDate = Convert.ToDateTime(searchScheduleAttendanceModel.EndDate).ToString(Constants.GlobalDateFormat).Replace("-", "/");
                }

                sceduleAttendanceModel.SchedulAttendanceCancelListModel = schedulAttendanceListModel.Where(m => ids.Contains(m.ScheduleStatusID)).OrderBy(p => p.ClientName).ToList();

                sceduleAttendanceModel.SchedulAttendanceListModel = schedulAttendanceListModel;

                #endregion

                response.IsSuccess = true;
            }
            response.Data = sceduleAttendanceModel;
            return response;
        }

        public ServiceResponse GetRequiredDocsforAttendanceReport(SearchReqDocsForAttendanceModel searchReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            #region Search Parameter

            searchList.Add(new SearchValueData { Name = "RegionID", Value = searchReportModel.RegionID.ToString() });

            if (searchReportModel.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchReportModel.StartDate).ToString(Constants.DbDateFormat) });
            else
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(DateTime.Now).ToString(Constants.DbDateFormat) });

            if (searchReportModel.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchReportModel.EndDate).ToString(Constants.DbDateFormat) });


            #endregion

            List<RequestedDocsForAttendanceListModel> totalData = GetEntityList<RequestedDocsForAttendanceListModel>(StoredProcedure.Rpt_GetRequestedDocsForAttendanceList, searchList);

            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_RequestedDocsForAttendance, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }


        #endregion

        #region LS Team Member Caseloads Report

        public ServiceResponse GetLsTeamMemberCaseload()
        {
            ServiceResponse response = new ServiceResponse();
            SetLSTeamMemberCaseloadPageModel model = GetMultipleEntity<SetLSTeamMemberCaseloadPageModel>(StoredProcedure.SetLSTeamMemberCaseloadListPage);

            if (model.SearchLSTMCaseloadListModel == null)
                model.SearchLSTMCaseloadListModel = new SearchLSTeamMemberCaseloadReport();

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetLsTeamMemberCaseLoadList(SearchLSTeamMemberCaseloadReport searchModel, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Parameter
            if (searchModel != null)
                SetSearchFilterForReferralTrackingListPage(searchModel, searchList, loggedInId);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            #endregion

            List<LSTeamMemberCaseloadListPageModel> totalData = GetEntityList<LSTeamMemberCaseloadListPageModel>(StoredProcedure.GetLSTeamMemberCaseloadList, searchList);
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<LSTeamMemberCaseloadListPageModel> listModel = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = listModel;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForReferralTrackingListPage(SearchLSTeamMemberCaseloadReport searchModel, List<SearchValueData> searchList, long loggedInId)
        {

            //if (searchModel.ReferralStatusID > 0)
            searchList.Add(new SearchValueData
            {
                Name = "ReferralStatusID",
                Value =
                    Convert.ToString((int)ReferralStatus.ReferralStatuses.Active + "," + (int)ReferralStatus.ReferralStatuses.ConnectingFamilies
                    + "," + (int)ReferralStatus.ReferralStatuses.LifeSkillsOnly)
            });

            if (!string.IsNullOrEmpty(searchModel.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchModel.ClientName) });

            if (!string.IsNullOrEmpty(searchModel.Parent))
                searchList.Add(new SearchValueData { Name = "ParentName", Value = Convert.ToString(searchModel.Parent) });

            if (!string.IsNullOrEmpty(searchModel.AHCCCSID))
                searchList.Add(new SearchValueData { Name = "AHCCCSID", Value = Convert.ToString(searchModel.AHCCCSID) });
            if (searchModel.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchModel.PayorID) });
            if (searchModel.AgencyID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchModel.AgencyID) });
            if (searchModel.CaseManagerID > 0)
                searchList.Add(new SearchValueData { Name = "CaseManagerID", Value = Convert.ToString(searchModel.CaseManagerID) });

            if (searchModel.LoggedInID > 0)
                searchList.Add(new SearchValueData { Name = "LoggedInID", Value = Convert.ToString(searchModel.LoggedInID) });

            searchList.Add(new SearchValueData { Name = "ServiceID", Value = "1" });

            if (searchModel.ReferralFromDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ReferralFromDate", Value = searchModel.ReferralFromDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.ReferralToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ReferralToDate", Value = searchModel.ReferralToDate.Value.ToString(Constants.DbDateFormat) });

            if (searchModel.ServiceFromDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ServiceFromDate", Value = searchModel.ServiceFromDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.ServiceToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ServiceToDate", Value = searchModel.ServiceToDate.Value.ToString(Constants.DbDateFormat) });


            if (searchModel.CFServiceFromDate.HasValue)
                searchList.Add(new SearchValueData { Name = "CFServiceFromDate", Value = searchModel.CFServiceFromDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.CFServiceToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "CFServiceToDate", Value = searchModel.CFServiceToDate.Value.ToString(Constants.DbDateFormat) });


            if (searchModel.ACFromDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ACFromDate", Value = searchModel.ACFromDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.ACToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "ACToDate", Value = searchModel.ACToDate.Value.ToString(Constants.DbDateFormat) });


            if (searchModel.OMNextDueFromDate.HasValue)
                searchList.Add(new SearchValueData { Name = "OMNextDueFromDate", Value = searchModel.OMNextDueFromDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.OMNextDueToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "OMNextDueToDate", Value = searchModel.OMNextDueToDate.Value.ToString(Constants.DbDateFormat) });


            if (searchModel.OMCompletedFromDate.HasValue)
                searchList.Add(new SearchValueData { Name = "OMCompletedFromDate", Value = searchModel.OMCompletedFromDate.Value.ToString(Constants.DbDateFormat) });
            if (searchModel.OMCompletedToDate.HasValue)
                searchList.Add(new SearchValueData { Name = "OMCompletedToDate", Value = searchModel.OMCompletedToDate.Value.ToString(Constants.DbDateFormat) });

            if (searchModel.ReferralLSTMCaseloadsComment != null)
                searchList.Add(new SearchValueData { Name = "ReferralLSTMCaseloadsComment", Value = searchModel.ReferralLSTMCaseloadsComment });

            searchList.Add(new SearchValueData { Name = "ViewAllPermission", Value = searchModel.ViewAllPermission ? "1" : "0" });

            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = "0" });

            if (searchModel.CaseLoadID > 0)
                searchList.Add(new SearchValueData { Name = "CaseLoadID", Value = searchModel.CaseLoadID.ToString() });

        }

        public ServiceResponse GetLSTeamMemberCaseloadReport(SearchLSTeamMemberCaseloadReport searchLSTeamMemberCaseloadReport)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            #region Search Parameter

            searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchLSTeamMemberCaseloadReport.ClientName) });
            searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchLSTeamMemberCaseloadReport.RegionID) });
            searchList.Add(new SearchValueData { Name = "ServiceID", Value = "1" });

            if (searchLSTeamMemberCaseloadReport.ReferralStartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchLSTeamMemberCaseloadReport.ReferralStartDate).ToString(Constants.DbDateFormat) });

            if (searchLSTeamMemberCaseloadReport.ReferralEndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchLSTeamMemberCaseloadReport.ReferralEndDate).ToString(Constants.DbDateFormat) });

            #endregion

            List<LSTeamMemberCaseloadListModel> totalData = GetEntityList<LSTeamMemberCaseloadListModel>(StoredProcedure.Rpt_GetLSTeamMemberCaseload, searchList);

            if (totalData.Count > 0)
            {
                string fileName = string.Format("{0}_{1}", Constants.ReportName_LSTeamMemberCaseloads, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        public ServiceResponse SaveReferralLSTMCaseloadsComment(ReferralCommentModel referralCommentModel, long loggedInId)
        {
            ServiceResponse response = new ServiceResponse();

            Referral referral = GetEntity<Referral>(referralCommentModel.ReferralID);
            if (referral != null)
            {
                referral.ReferralLSTMCaseloadsComment = referralCommentModel.Comment;
                SaveObject(referral, loggedInId);

                response.IsSuccess = true;
                response.Message = Resource.ReferralLSTeamMemberCaseloadsCommentUpdated;
            }
            else
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        #endregion

        #region Get Life Skills Outcome Measurements Report

        public ServiceResponse GetLifeSkillsOutcomeMeasurementsReport(SearchReportModel searchReportModel)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (!string.IsNullOrEmpty(searchReportModel.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchReportModel.ClientName) });

            if (searchReportModel.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchReportModel.RegionID) });

            if (searchReportModel.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchReportModel.StartDate).ToString(Constants.DbDateFormat) });

            if (searchReportModel.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchReportModel.EndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "ServiceID", Value = "1" });

            List<LifeSkillsOutcomeMeasurementsListModel> totalData = GetEntityList<LifeSkillsOutcomeMeasurementsListModel>(StoredProcedure.Rpt_GetLifeSkillsOutcomeMeasurementsList, searchList);

            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_LifeSkillsOutcomeMeasurements, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region Get BX Contract Status Report

        public ServiceResponse GetBXContractStatusReport(SearchBXContractStatusReport searchBxContractStatusReport)
        {
            var response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();

            if (!string.IsNullOrEmpty(searchBxContractStatusReport.ClientName))
                searchList.Add(new SearchValueData { Name = "ClientName", Value = Convert.ToString(searchBxContractStatusReport.ClientName) });

            if (searchBxContractStatusReport.RegionID > 0)
                searchList.Add(new SearchValueData { Name = "RegionID", Value = Convert.ToString(searchBxContractStatusReport.RegionID) });

            if (searchBxContractStatusReport.AgencyID > 0)
                searchList.Add(new SearchValueData { Name = "AgencyID", Value = Convert.ToString(searchBxContractStatusReport.AgencyID) });

            if (searchBxContractStatusReport.PayorID > 0)
                searchList.Add(new SearchValueData { Name = "PayorID", Value = Convert.ToString(searchBxContractStatusReport.PayorID) });

            searchList.Add(new SearchValueData { Name = "ReferralStatusID", Value = Convert.ToString(searchBxContractStatusReport.ReferralStatusID) });

            searchList.Add(new SearchValueData { Name = "BXContractStatus", Value = Convert.ToString(searchBxContractStatusReport.BXContractStatus) });

            if (searchBxContractStatusReport.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchBxContractStatusReport.StartDate).ToString(Constants.DbDateFormat) });

            if (searchBxContractStatusReport.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchBxContractStatusReport.EndDate).ToString(Constants.DbDateFormat) });

            if (searchBxContractStatusReport.WarningStartDate != null)
                searchList.Add(new SearchValueData { Name = "WarningStartDate", Value = Convert.ToDateTime(searchBxContractStatusReport.WarningStartDate).ToString(Constants.DbDateFormat) });

            if (searchBxContractStatusReport.WarningEndDate != null)
                searchList.Add(new SearchValueData { Name = "WarningEndDate", Value = Convert.ToDateTime(searchBxContractStatusReport.WarningEndDate).ToString(Constants.DbDateFormat) });

            searchList.Add(new SearchValueData { Name = "ServiceID", Value = Convert.ToString(searchBxContractStatusReport.ServiceID) });

            List<BXContractStatusListModel> totalData = GetEntityList<BXContractStatusListModel>(StoredProcedure.Rpt_BehaviorContractTracking, searchList);

            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_BXContractsTracking, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region GetReferralInfo
        public List<ReferralDetailForNote> GetReferralInfo(int pageSize, string searchText = null)
        {
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "SearchText", Value = searchText},
                    new SearchValueData {Name = "PageSize", Value =pageSize.ToString()}
                };

            return GetEntityList<ReferralDetailForNote>(StoredProcedure.Rpt_GetNotePageReferrals, searchParam);
        }
        #endregion

        #region SPECIFIC REPORT ASKED  BY PALLAV
        public ServiceResponse GetBillingSummaryPage()
        {
            ServiceResponse response = new ServiceResponse();
            response.IsSuccess = true;
            return response;

        }

        public ServiceResponse GetBillingSummaryList(SearchBillingSummaryReport searchBillingSummaryReport)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            if (searchBillingSummaryReport.StartDate != null)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchBillingSummaryReport.StartDate).ToString(Constants.DbDateFormat) });
            if (searchBillingSummaryReport.EndDate != null)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchBillingSummaryReport.EndDate).ToString(Constants.DbDateFormat) });

            List<BillingSummaryListPageModel> totalData = GetEntityList<BillingSummaryListPageModel>(StoredProcedure.GetBillingSummary, searchList);
            if (totalData.Count > 0)
            {
                string reportExcelFileUploadPath = String.Format(_cacheHelper.ReportExcelFileUploadPath, _cacheHelper.Domain);
                string basePath = HttpContext.Current.Server.MapCustomPath(reportExcelFileUploadPath);
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fileName = string.Format("{0}_{1}", Constants.ReportName_BillingSummary, DateTime.Now.ToString(Constants.FileNameDateTimeFormat));
                var downloadFileModel = new DownloadFileModel();
                downloadFileModel.AbsolutePath = string.Format("{0}{1}{2}", basePath, fileName, Constants.Extention_xlsx);
                downloadFileModel.VirtualPath = string.Format("{0}{1}{2}", reportExcelFileUploadPath, fileName, Constants.Extention_xlsx);
                downloadFileModel.FileName = fileName + Constants.Extention_xlsx;
                CreateExcelFile.CreateExcelDocument(totalData, downloadFileModel.AbsolutePath);
                response.IsSuccess = true;
                response.Data = downloadFileModel;
            }
            return response;
        }

        #endregion

        #region Employee Visit Reports
        public ServiceResponse SetEmployeeVisitListPage(string data)
        {
            ServiceResponse response = new ServiceResponse();
            SetEmployeeVisitListPage model = GetMultipleEntity<SetEmployeeVisitListPage>(StoredProcedure.SetEmployeeVisitListPage);
            var pca = GeneratePcaTimeSheet(61, true);
            var setEmployeeVisitListPage = new SetEmployeeVisitListPage
            {
                DeleteFilter = Common.SetDeleteFilter(),
                ActionFilter = Common.GetListFromEnum<EmployeeVisit.BypassActions>(),
                SearchEmployeeVisitListPage = { IsDeleted = 0, ActionTaken = ((!string.IsNullOrEmpty(data)) && (data.ToUpper() == EmployeeVisit.BypassActions.Pending.ToString().ToUpper())) ? 1 : 0 },
                EmployeeList = model.EmployeeList,
                ReferralList = model.ReferralList,
                PayorList = model.PayorList,
                CareTypeList = model.CareTypeList,
                ServiceTypeList = model.ServiceTypeList,
                ReferralPayorList = model.ReferralPayorList,
                PCADetail = ((PCAModel)pca.Data).PCADetail,
                TaskList = ((PCAModel)pca.Data).TaskList,
                ConclusionList = ((PCAModel)pca.Data).ConclusionList,
                DeviationList = model.DeviationList,

            };
            response.Data = setEmployeeVisitListPage;
            return response;
        }

        public ServiceResponse GetEmployeeVisitList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize,
           string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEmployeeVisitListPage != null)
                SetSearchFilterForEmployeeVisitList(searchEmployeeVisitListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<EmployeeVisitListModel> totalData = GetEntityList<EmployeeVisitListModel>(StoredProcedure.GetEmployeeVisitList, searchList);
            if (!Common.HasPermission(Constants.AllRecordAccess))
            {
                totalData = totalData.Where(_ => _.EmployeeID == Convert.ToInt32(SessionHelper.LoggedInID)).ToList();
            }

            // the pending in employee visit report come with incomplete status when ISPACACompleted = 0
            if (searchEmployeeVisitListPage.ExecludeIncompletePending)
            {
                totalData = totalData.Where(_ => _.ActionTaken != 1).ToList();
            }
            if (searchEmployeeVisitListPage.IsAuthExpired == 1)
            {
                totalData = totalData.Where(_ => _.IsAuthExpired == true).ToList();
            }
            if (searchEmployeeVisitListPage.IsAuthExpired == 0)
            {
                totalData = totalData.Where(_ => _.IsAuthExpired == false).ToList();
            }
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<EmployeeVisitListModel> employeeVisitList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = employeeVisitList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SetNurseSignaturePage()
        {
            ServiceResponse response = new ServiceResponse();

            SetNurseSignaturePage model = GetMultipleEntity<SetNurseSignaturePage>(StoredProcedure.SetNurseSignaturePage);
            model.SearchNurceTimesheetListPage.StatusId = "1";
            response.Data = model;
            return response;
        }

        public ServiceResponse GetGroupTimesheetList(SearchGroupTimesheetListPage searchGroupTimesheetListPage, int pageIndex, int pageSize,
          string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchGroupTimesheetListPage != null)
                SetSearchFilterForGroupTimesheetList(searchGroupTimesheetListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));
            List<GroupTimesheetListModel> totalData = GetEntityList<GroupTimesheetListModel>(StoredProcedure.GetGroupTimesheetList, searchList);
            if (!Common.HasPermission(Constants.AllRecordAccess))
            {
                totalData = totalData.Where(_ => _.EmployeeID == Convert.ToInt32(SessionHelper.LoggedInID)).ToList();
            }
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SaveReferralActivityList(ReferralActivityModel saveModel, string[] referralIds, string Year, string Month)
        {
            ServiceResponse response = new ServiceResponse();
            DataTable dataTbl = Common.ListToDataTable(saveModel.ReferralActivityList);

            MyEzcareOrganization orgData = GetOrganizationConnectionString();
            SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.SaveReferralActivityList, con);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (string str in referralIds)
            {
                cmd.Parameters.Clear();
                var pList = new SqlParameter("@Type_ReferralActivityList", SqlDbType.Structured);
                pList.TypeName = "dbo.Type_ReferralActivity";
                pList.Value = dataTbl;
                cmd.Parameters.Add(pList);

                cmd.Parameters.AddWithValue("@ReferralId", str);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Year", Year);

                if (cmd.ExecuteNonQuery() <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "No data saved.";
                    //return response;
                }
            }

            response.Message = "Data saved successfully.";
            response.Data = true;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse AddReferralActivityNotes(ReferralActivityNotesModel referralActivityNotesModel, string Year, string Month, int referralId)
        {
            ServiceResponse response = new ServiceResponse();

            MyEzcareOrganization orgData = GetOrganizationConnectionString();
            SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.SaveReferralActivityNotes, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@Initials", referralActivityNotesModel.Initials);
            cmd.Parameters.AddWithValue("@Description", referralActivityNotesModel.Description);
            cmd.Parameters.AddWithValue("@ReferralId", referralId);
            cmd.Parameters.AddWithValue("@Month", Month);
            cmd.Parameters.AddWithValue("@Year", Year);

            if (cmd.ExecuteNonQuery() <= 0)
            {
                response.IsSuccess = false;
                response.Message = "No data saved.";
                //return response;
            }

            response.Message = "Data saved successfully.";
            response.Data = true;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse EditDeleteReferralActivityNotes(ReferralActivityNotesModel referralActivityNotesModel, int ReferralActivityNoteId, string AddOrEdit)
        {
            ServiceResponse response = new ServiceResponse();

            MyEzcareOrganization orgData = GetOrganizationConnectionString();
            SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.EditDeleteReferralActivityNotes, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Clear();
            if (AddOrEdit == "EDIT")
            {
                cmd.Parameters.AddWithValue("@Initials", referralActivityNotesModel.Initials);
                cmd.Parameters.AddWithValue("@Description", referralActivityNotesModel.Description);
            }
            cmd.Parameters.AddWithValue("@ReferralActivityNoteId", ReferralActivityNoteId);
            cmd.Parameters.AddWithValue("@AddOrEdit", AddOrEdit);

            if (cmd.ExecuteNonQuery() <= 0)
            {
                response.IsSuccess = false;
                response.Message = "No data saved.";
            }
            if (AddOrEdit == "EDIT")
            {
                response.Message = "Data saved successfully.";
            }
            if (AddOrEdit == "DELETE")
            {
                response.Message = "Deleted successfully.";
            }
            response.Data = true;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse SetReferralActivityPage()
        {
            ServiceResponse response = new ServiceResponse();
            //SetGroupTimesheetPage model = GetMultipleEntity<SetGroupTimesheetPage>(StoredProcedure.SetGroupTimesheetPage);

            ReferralActivityModel referralActivityModel = GetMultipleEntity<ReferralActivityModel>(StoredProcedure.SetReferralActivityPage);

            response.Data = referralActivityModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferral(string Year, string Month, int referralId)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (Month != null && Year != null)
            {
                searchList.Add(new SearchValueData { Name = "Month", Value = Convert.ToString(Month) });
                searchList.Add(new SearchValueData { Name = "Year", Value = Convert.ToString(Year) });
                searchList.Add(new SearchValueData { Name = "ReferralId", Value = Convert.ToString(referralId) });
            }

            List<ReferralActivityListModel> totalData = GetEntityList<ReferralActivityListModel>(StoredProcedure.GetReferralActivityById, searchList);

            response.Message = "Data fetched successfully.";
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralNotes(string Year, string Month, int referralId)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (Month != null && Year != null)
            {
                searchList.Add(new SearchValueData { Name = "Month", Value = Convert.ToString(Month) });
                searchList.Add(new SearchValueData { Name = "Year", Value = Convert.ToString(Year) });
                searchList.Add(new SearchValueData { Name = "ReferralId", Value = Convert.ToString(referralId) });
            }

            List<ReferralActivityNotesModel> totalData = GetEntityList<ReferralActivityNotesModel>(StoredProcedure.GetReferralNotesById, searchList);

            response.Message = "Data fetched successfully.";
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetReferralList(string Year, string Month, string AddOrEdit)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (Month != null && Year != null)
            {
                searchList.Add(new SearchValueData { Name = "Month", Value = Convert.ToString(Month) });
                searchList.Add(new SearchValueData { Name = "Year", Value = Convert.ToString(Year) });
                searchList.Add(new SearchValueData { Name = "AddOrEdit", Value = Convert.ToString(AddOrEdit) });
            }

            List<ReferralListModel> totalData = GetEntityList<ReferralListModel>(StoredProcedure.GetReferralsByMonthYear, searchList);

            response.Message = "Data fetched successfully.";
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }

        private bool ValidateReferralBillingAuthorization(long referralID, string scheduleIDs, string authType = null)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchParam = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID.ToString()}
                };
            if (!String.IsNullOrWhiteSpace(authType))
            {
                searchParam.Add(new SearchValueData { Name = "AuthType", Value = authType });
            }
            if (!String.IsNullOrWhiteSpace(scheduleIDs))
            {
                searchParam.Add(new SearchValueData { Name = "scheduleIDs", Value = scheduleIDs });
            }

            response = GetEntityList<ServiceResponse>(StoredProcedure.HC_ValidateReferralBillingAuthorization, searchParam).FirstOrDefault();
            return response.IsSuccess;
        }

        public ServiceResponse SaveGroupTimesheetList(SaveGroupTimesheetList saveGroupTimesheetList, long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();
            string scheduleIDs = string.Join(",", saveGroupTimesheetList.TimesheetDetails.Where(t => t.ScheduleID > 0).Select(t => t.ScheduleID).ToList());
            if (!ValidateReferralBillingAuthorization(0, scheduleIDs))
            {
                response.IsSuccess = false;
                response.Message = Resource.HCBillingAuthorizationValidation;
                return response;
            }
            saveGroupTimesheetList.TimesheetDetails?.ForEach(td =>
            {
                if (td.ClockInDateTime.HasValue)
                { td.ClockInDateTime = Common.LocalToOrgDateTime(td.ClockInDateTime.Value); }
                if (td.ClockOutDateTime.HasValue)
                { td.ClockOutDateTime = Common.LocalToOrgDateTime(td.ClockOutDateTime.Value); }
            });
            DataTable dataTbl = Common.ListToDataTable(saveGroupTimesheetList.TimesheetDetails);
            DataTable taskTbl = Common.ListToDataTable(saveGroupTimesheetList.TaskList);

            MyEzcareOrganization orgData = GetOrganizationConnectionString();
            SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.SaveGroupTimesheetList, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Remarks", saveGroupTimesheetList.Remarks);
            cmd.Parameters.AddWithValue("@SystemID", Common.GetHostAddress());
            cmd.Parameters.AddWithValue("@LoggedInID", loggedInID);

            var pList = new SqlParameter("@TimesheetDetails", SqlDbType.Structured);
            pList.TypeName = "dbo.GroupTimesheet";
            pList.Value = dataTbl;
            cmd.Parameters.Add(pList);

            var tList = new SqlParameter("@TaskList", SqlDbType.Structured);
            tList.TypeName = "dbo.GroupTask";
            tList.Value = taskTbl;
            cmd.Parameters.Add(tList);

            if (cmd.ExecuteNonQuery() <= 0)
            {
                response.IsSuccess = false;
                response.Message = "No data saved.";
                return response;
            }

            response.Message = "Data saved successfully.";
            response.Data = true;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForGroupTimesheetList(SearchGroupTimesheetListPage searchGroupTimesheetListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "EmployeeIDs", Value = Convert.ToString(searchGroupTimesheetListPage.EmployeeIDs) });
            searchList.Add(new SearchValueData { Name = "FacilityIDs", Value = Convert.ToString(searchGroupTimesheetListPage.FacilityIDs) });
            searchList.Add(new SearchValueData { Name = "ReferralIDs", Value = Convert.ToString(searchGroupTimesheetListPage.ReferralIDs) });

            if (searchGroupTimesheetListPage.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchGroupTimesheetListPage.StartDate).ToString(Constants.DbDateFormat) });
            if (searchGroupTimesheetListPage.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchGroupTimesheetListPage.EndDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "PayorIDs", Value = Convert.ToString(searchGroupTimesheetListPage.PayorIDs) });
            searchList.Add(new SearchValueData { Name = "CareTypeIDs", Value = Convert.ToString(searchGroupTimesheetListPage.CareTypeIDs) });
            searchList.Add(new SearchValueData { Name = "TypesOfTimeSheet", Value = Convert.ToString(searchGroupTimesheetListPage.TypesOfTimeSheet) });
        }

        public ServiceResponse GetMissingTimeSheetList(SearchMissingTimeSheetListPage searchMissingTSListPage, int pageIndex, int pageSize,
           string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchMissingTSListPage != null)
            {
                searchList.Add(new SearchValueData { Name = "EmployeeIDs", Value = Convert.ToString(searchMissingTSListPage.EmployeeIDs) });
                searchList.Add(new SearchValueData { Name = "ReferralIDs", Value = Convert.ToString(searchMissingTSListPage.ReferralIDs) });
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchMissingTSListPage.StartDate).ToString(Constants.DbDateFormat) });
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchMissingTSListPage.EndDate).ToString(Constants.DbDateFormat) });
            }
            //searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<MissingTimeSheetModel> totalData = GetEntityList<MissingTimeSheetModel>(StoredProcedure.GetMissingTimesheet, searchList);
            if (!Common.HasPermission(Constants.AllRecordAccess))
            {
                totalData = totalData.Where(_ => _.EmployeeId == Convert.ToInt32(SessionHelper.LoggedInID)).ToList();
            }
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.Count;

            Page<MissingTimeSheetModel> missingTSList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = missingTSList;
            response.IsSuccess = true;
            return response;
        }



        public ServiceResponse AddMissingTimeASheet(List<MissingTimesheetModel> missingTimesheetModels)
        {
            var response = new ServiceResponse();
            try
            {
                if (missingTimesheetModels != null && missingTimesheetModels.Count > 0)
                {
                    foreach (MissingTimesheetModel item in missingTimesheetModels)
                    {
                        List<SearchValueData> searchList = new List<SearchValueData>();
                        SetAddMissingTsFilter(item, searchList);
                        Object obj = new object();
                        List<Object> result = GetEntityList<Object>(StoredProcedure.API_SetEmployeeVisitTime, searchList);
                    }
                    response.IsSuccess = true;
                    response.Data = 1;
                    response.Message = "Record Added Successfully";
                }
                //List<ReferralPayorList> data = GetEntityList<ReferralPayorList>(StoredProcedure.SaveEmployeeVisitPayer, new List<SearchValueData>
                //{
                //    new SearchValueData {Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                //    new SearchValueData {Name = "ReferralPayorID",Value = Convert.ToString(model.ReferralPayorID)}
                //});

                //if (data.Count == 0)
                //{
                //    response.IsSuccess = false;
                //    response.Message = "RecordNotFound";
                //}
                //else
                //{
                //    response.IsSuccess = true;
                //    response.Data = data;
                //    response.Message = "RecordUpdatedSuccessfully";
                //}
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        private static void SetAddMissingTsFilter(MissingTimesheetModel missingTimesheetModel, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "ScheduleID", Value = Convert.ToString(missingTimesheetModel.ScheduleId) });
            searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(missingTimesheetModel.ReferralId) });
            searchList.Add(new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(missingTimesheetModel.EmployeeId) });
            searchList.Add(new SearchValueData { Name = "Type", Value = "0" });
            searchList.Add(new SearchValueData { Name = "IsByPass", Value = "True" });
            searchList.Add(new SearchValueData { Name = "IsCaseManagement", Value = "False" });
            searchList.Add(new SearchValueData { Name = "IsApprovalRequired", Value = "True" });
            searchList.Add(new SearchValueData { Name = "ActionTaken", Value = "" });
            searchList.Add(new SearchValueData { Name = "EarlyClockOutComment", Value = "" });
            searchList.Add(new SearchValueData { Name = "ByPassReason", Value = " " });
            searchList.Add(new SearchValueData { Name = "Lat", Value = "0.0" });
            searchList.Add(new SearchValueData { Name = "Long", Value = "0.0" });
            searchList.Add(new SearchValueData { Name = "BeforeClockIn", Value = "-15" });
            searchList.Add(new SearchValueData { Name = "Distance", Value = "50" });
            searchList.Add(new SearchValueData { Name = "SystemID", Value = "0.0.0.0" });
            searchList.Add(new SearchValueData { Name = "ClockInTime", Value = Convert.ToDateTime(missingTimesheetModel.StartDate).ToString(Constants.DbDateTimeFormat) });
            searchList.Add(new SearchValueData { Name = "ClockOutTime", Value = Convert.ToDateTime(missingTimesheetModel.EndDate).ToString(Constants.DbDateTimeFormat) });
        }

        private static void SetSearchFilterForEmployeeVisitList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, List<SearchValueData> searchList)
        {
            if (searchEmployeeVisitListPage.ActionTaken == 4 || searchEmployeeVisitListPage.ActionTaken == 5)
            {

                var IsPCACompleted = searchEmployeeVisitListPage.ActionTaken == 4 ? "0" : "1";
                searchList.Add(new SearchValueData { Name = "IsPCACompleted", Value = Convert.ToString(IsPCACompleted) });
                searchEmployeeVisitListPage.ActionTaken = 0;
            }
            if (!string.IsNullOrEmpty(searchEmployeeVisitListPage.ClaimActionStatus))
                searchList.Add(new SearchValueData { Name = "ClaimActionStatus", Value = Convert.ToString(searchEmployeeVisitListPage.ClaimActionStatus) });

            searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(searchEmployeeVisitListPage.EmployeeVisitID) });
            searchList.Add(new SearchValueData { Name = "EmployeeIDs", Value = Convert.ToString(searchEmployeeVisitListPage.EmployeeIDs) });
            searchList.Add(new SearchValueData { Name = "ReferralIDs", Value = Convert.ToString(searchEmployeeVisitListPage.ReferralIDs) });
            searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(searchEmployeeVisitListPage.StartTime) });
            searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(searchEmployeeVisitListPage.EndTime) });
            if (searchEmployeeVisitListPage.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchEmployeeVisitListPage.StartDate).ToString(Constants.DbDateFormat) });
            if (searchEmployeeVisitListPage.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchEmployeeVisitListPage.EndDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEmployeeVisitListPage.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "ActionTaken", Value = Convert.ToString(searchEmployeeVisitListPage.ActionTaken) });
            searchList.Add(new SearchValueData { Name = "PayorIDs", Value = Convert.ToString(searchEmployeeVisitListPage.PayorIDs) });
            searchList.Add(new SearchValueData { Name = "CareTypeIDs", Value = Convert.ToString(searchEmployeeVisitListPage.CareTypeIDs) });
            searchList.Add(new SearchValueData { Name = "ServiceTypeID", Value = Convert.ToString(searchEmployeeVisitListPage.ServiceTypeID) });
            if (searchEmployeeVisitListPage.IsAuthExpired >= 0)
            {
                searchList.Add(new SearchValueData { Name = "IsAuthExpired", Value = Convert.ToString(searchEmployeeVisitListPage.IsAuthExpired) });
            }
            if (searchEmployeeVisitListPage.ExecludeIncompletePending)
            {
                searchList.Add(new SearchValueData { Name = "ExecludeIncompletePending", Value = Convert.ToString(searchEmployeeVisitListPage.ExecludeIncompletePending) });
            }
        }
        public ServiceResponse SaveEmployeeVisitPayer(EmployeeVisitPayermodal model, long LoggedInID)
        {
            var response = new ServiceResponse();
            try
            {
                List<ReferralPayorList> data = GetEntityList<ReferralPayorList>(StoredProcedure.SaveEmployeeVisitPayer, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                    new SearchValueData {Name = "ReferralPayorID",Value = Convert.ToString(model.ReferralPayorID)}
                });

                if (data.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "RecordNotFound";
                }
                else
                {
                    response.IsSuccess = true;
                    response.Data = data;
                    response.Message = "RecordUpdatedSuccessfully";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse SaveEmployeeVisit(EmployeeVisitModel model, long LoggedInID)
        {
            var response = new ServiceResponse();
            try
            {
                TimeSpan StartTime = new TimeSpan();
                TimeSpan EndTime = new TimeSpan();
                if (!string.IsNullOrEmpty(model.StrStartTime))
                {
                    DateTime timeOnly = DateTime.ParseExact(model.StrStartTime, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    StartTime = timeOnly.TimeOfDay;
                }

                if (!string.IsNullOrEmpty(model.StrEndTime))
                {
                    DateTime timeOnly = DateTime.ParseExact(model.StrEndTime, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    EndTime = timeOnly.TimeOfDay;
                }

                model.ClockInDate = model.ClockInDate + StartTime;
                model.ClockOutDate = model.ClockOutDate + EndTime;

                VisitTime data = GetEntity<VisitTime>(StoredProcedure.SaveEmployeeVisit, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                    new SearchValueData {Name = "ClockInTime",Value = Convert.ToDateTime(model.ClockInDate).ToString(Constants.DbDateTimeFormat)},//Convert.ToString(model.ClockInDate)},
                    new SearchValueData {Name = "ClockOutTime",Value = Convert.ToDateTime(model.ClockOutDate).ToString(Constants.DbDateTimeFormat)},//Convert.ToString(model.ClockOutDate)},
                   // new SearchValueData {Name = "BeforeClockIn",Value = Constants.BeforeClockInWeb},
                    new SearchValueData {Name = "UpdatedBy",Value = Convert.ToString(LoggedInID)}
                });

                if (data.Result == -1)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.EarlyClockInMessage;
                }
                else if (data.Result == -2)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.InvalidClockInClockOut;
                }
                else if (data.Result == -3)
                {
                    response.IsSuccess = false;
                    response.Message = "Please delete Deviation Notes";
                }
                else if (data.Result == -5)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.HCBillingAuthorizationValidation;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Data = data;
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse SavePCAComplete(EmployeeVisitModel model, long LoggedInID)
        {
            var response = new ServiceResponse();
            try
            {
                VisitTime data = GetEntity<VisitTime>(StoredProcedure.SavePCACompeleted, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                     new SearchValueData {Name = "UpdatedBy",Value = Convert.ToString(LoggedInID)}
                    //new SearchValueData {Name = "IsPCACompleted",Value = Convert.ToString(true)},
                    //new SearchValueData {Name = "SurveyCompleted",Value = Convert.ToString(true)},
                   
                });

                response.IsSuccess = true;
                response.Data = data;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse UpdateEmployeeVisitPayorAndAutherizationCode(EmployeeVisitPayorAutherizationCode model, long LoggedInID)
        {
            var response = new ServiceResponse();
            try
            {
                PayorAndAutherizationCodeResult data = GetEntity<PayorAndAutherizationCodeResult>(StoredProcedure.UpdateEmployeeVisitPayorAndAutherizationCode, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                    new SearchValueData {Name = "PayorID",Value = Convert.ToString(model.PayorID)},
                    new SearchValueData {Name = "ReferralBillingAuthorizationID",Value = Convert.ToString(model.ReferralBillingAuthorizationID)},
                    new SearchValueData {Name = "EmployeeID",Value = Convert.ToString(model.EmployeeID)},
                    new SearchValueData {Name = "UpdatedBy",Value = Convert.ToString(LoggedInID)}
                });
                response.IsSuccess = true;
                response.Data = data;
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse DeleteEmployeeVisit(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEmployeeVisitList(searchEmployeeVisitListPage, searchList);

            if (!string.IsNullOrEmpty(searchEmployeeVisitListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchEmployeeVisitListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<EmployeeVisitListModel> totalData = GetEntityList<EmployeeVisitListModel>(StoredProcedure.DeleteEmployeeVisit, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<EmployeeVisitListModel> employeeVisitList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = employeeVisitList;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);
            return response;
        }


        public ServiceResponse MarkEmployeeVisitAsComplete(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize,
       string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEmployeeVisitList(searchEmployeeVisitListPage, searchList);

            if (!string.IsNullOrEmpty(searchEmployeeVisitListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchEmployeeVisitListPage.ListOfIdsInCsv });
            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });
            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            List<EmployeeVisitListModel> totalData = GetEntityList<EmployeeVisitListModel>(StoredProcedure.MarkEmployeeVisitAsComplete, searchList);


            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);
            return response;
        }


        public ServiceResponse SetEmployeeVisitNoteList()
        {
            ServiceResponse response = new ServiceResponse();
            var setEmployeeVisitNoteListPage = new SetEmployeeVisitNoteListPage
            {
                DeleteFilter = Common.SetDeleteFilter(),
                SearchEmployeeVisitNoteListPage = { IsDeleted = 0 }
            };
            response.Data = setEmployeeVisitNoteListPage;
            return response;
        }

        public ServiceResponse GetEmployeeVisitNoteList(SearchEmployeeVisitNoteListPage searchEmployeeVisitNoteListPage, int pageIndex, int pageSize,
           string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEmployeeVisitNoteListPage != null)
                SetSearchFilterForEmployeeVisitNoteList(searchEmployeeVisitNoteListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<EmployeeVisitNoteListModel> totalData = GetEntityList<EmployeeVisitNoteListModel>(StoredProcedure.GetEmployeeVisitNoteList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<EmployeeVisitNoteListModel> employeeVisitNoteList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = employeeVisitNoteList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetVisitTaskDocumentList(long employeeVisitID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(employeeVisitID) });
                searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(SessionHelper.OrganizationId) });
                List<VisitTaskDocument> resultData = GetEntityList<VisitTaskDocument>(StoredProcedure.GetVisitTaskDocumentList, searchList);
                response.Data = resultData;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetVisitApprovalList(string employeeVisitIDs, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EmployeeVisitIDs", Value = employeeVisitIDs });
                searchList.Add(new SearchValueData { Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserId) });
                List<ApprovalVisit> resultData = GetEntityList<ApprovalVisit>(StoredProcedure.GetVisitApprovalList, searchList);
                response.Data = resultData;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse ApproveVisitList(ApproveVisitList approveVisitList, long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();

            DataTable listTbl = Common.ListToDataTable(approveVisitList.List);

            MyEzcareOrganization orgData = GetOrganizationConnectionString();
            SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.ApproveVisitList, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SystemID", Common.GetHostAddress());
            cmd.Parameters.AddWithValue("@LoggedInID", loggedInID);

            var tList = new SqlParameter("@List", SqlDbType.Structured);
            tList.TypeName = "dbo.ApproveVisit";
            tList.Value = listTbl;
            cmd.Parameters.Add(tList);

            if (cmd.ExecuteNonQuery() <= 0)
            {
                response.IsSuccess = false;
                response.Message = "No data saved.";
                return response;
            }

            response.Message = "Data saved successfully.";
            response.Data = true;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetNurseSignatureList(SearchNurceTimesheetListPage searchNurceTimesheetListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EmployeeIDs", Value = Convert.ToString(searchNurceTimesheetListPage.EmployeeIDs) });
                searchList.Add(new SearchValueData { Name = "ReferralIDs", Value = Convert.ToString(searchNurceTimesheetListPage.ReferralIDs) });

                if (searchNurceTimesheetListPage.StartDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchNurceTimesheetListPage.StartDate).ToString(Constants.DbDateFormat) });
                if (searchNurceTimesheetListPage.EndDate.HasValue)
                    searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchNurceTimesheetListPage.EndDate).ToString(Constants.DbDateFormat) });

                searchList.Add(new SearchValueData { Name = "CareTypeIDs", Value = Convert.ToString(searchNurceTimesheetListPage.CareTypeIDs) });
                if (!string.IsNullOrEmpty(searchNurceTimesheetListPage.StatusId))
                    searchList.Add(new SearchValueData { Name = "StatusId", Value = Convert.ToString(searchNurceTimesheetListPage.StatusId) });
                searchList.Add(new SearchValueData { Name = "LoggedInUserID", Value = Convert.ToString(loggedInUserId) });

                searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

                List<NurseSignatureItem> resultData = GetEntityList<NurseSignatureItem>(StoredProcedure.GetNurseSignatureList, searchList);

                int count = 0;
                if (resultData != null && resultData.Count > 0)
                    count = resultData.First().Count;

                Page<NurseSignatureItem> getReferralList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, resultData);

                response.Data = getReferralList;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse NurseSignature(NurseSignature nurseSignature, long loggedInID)
        {
            ServiceResponse response = new ServiceResponse();

            DataTable listTbl = Common.ListToDataTable(nurseSignature.List);

            MyEzcareOrganization orgData = GetOrganizationConnectionString();
            SqlConnection con = new SqlConnection(orgData.CurrentConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(StoredProcedure.NurseSignature, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SystemID", Common.GetHostAddress());
            cmd.Parameters.AddWithValue("@LoggedInID", loggedInID);
            cmd.Parameters.AddWithValue("@Signature", nurseSignature.Signature);

            var tList = new SqlParameter("@List", SqlDbType.Structured);
            tList.TypeName = "dbo.NurseSignatureVisit";
            tList.Value = listTbl;
            cmd.Parameters.Add(tList);

            if (cmd.ExecuteNonQuery() <= 0)
            {
                response.IsSuccess = false;
                response.Message = "No data saved.";
                return response;
            }

            response.Message = "Data saved successfully.";
            response.Data = true;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEmployeeVisitNoteList(SearchEmployeeVisitNoteListPage searchEmployeeVisitNoteListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "EmployeeVisitNoteID", Value = Convert.ToString(searchEmployeeVisitNoteListPage.EmployeeVisitNoteID) });
            searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(searchEmployeeVisitNoteListPage.EmployeeVisitID) });
            searchList.Add(new SearchValueData { Name = "Name", Value = Convert.ToString(searchEmployeeVisitNoteListPage.Name) });
            searchList.Add(new SearchValueData { Name = "PatientName", Value = Convert.ToString(searchEmployeeVisitNoteListPage.PatientName) });
            searchList.Add(new SearchValueData { Name = "VisitTaskDetail", Value = Convert.ToString(searchEmployeeVisitNoteListPage.VisitTaskDetail) });
            searchList.Add(new SearchValueData { Name = "Description", Value = Convert.ToString(searchEmployeeVisitNoteListPage.Description) });
            searchList.Add(new SearchValueData { Name = "ServiceTime", Value = Convert.ToString(searchEmployeeVisitNoteListPage.ServiceTime) });
            searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(Resource.Task) });
            if (searchEmployeeVisitNoteListPage.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchEmployeeVisitNoteListPage.StartDate).ToString(Constants.DbDateFormat) });
            if (searchEmployeeVisitNoteListPage.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchEmployeeVisitNoteListPage.EndDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEmployeeVisitNoteListPage.IsDeleted) });
        }

        public ServiceResponse DeleteEmployeeVisitNote(SearchEmployeeVisitNoteListPage searchEmployeeVisitNoteListPage, int pageIndex, int pageSize,
            string sortIndex, string sortDirection, long loggedInUserId)
        {
            var response = new ServiceResponse();

            List<SearchValueData> searchList = Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection);

            SetSearchFilterForEmployeeVisitNoteList(searchEmployeeVisitNoteListPage, searchList);

            if (!string.IsNullOrEmpty(searchEmployeeVisitNoteListPage.ListOfIdsInCsv))
                searchList.Add(new SearchValueData { Name = "ListOfIdsInCsv", Value = searchEmployeeVisitNoteListPage.ListOfIdsInCsv });

            searchList.Add(new SearchValueData { Name = "IsShowList", Value = Convert.ToString(true) });

            searchList.Add(new SearchValueData { Name = "loggedInID", Value = Convert.ToString(loggedInUserId) });

            //List<EmployeeVisitNoteListModel> totalData = GetEntityList<EmployeeVisitNoteListModel>(StoredProcedure.DeleteEmployeeVisitNote, searchList);

            EmployeeVisitNoteListResultModel data = GetMultipleEntity<EmployeeVisitNoteListResultModel>(StoredProcedure.DeleteEmployeeVisitNote, searchList);

            List<EmployeeVisitNoteListModel> totalData = data.listmodel;


            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;
            Page<EmployeeVisitNoteListModel> employeeVisitNoteList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            data.EmployeeVisitNoteList = employeeVisitNoteList;

            if (data.Result == -2)
            {
                response.Data = data;
                response.IsSuccess = false;
                response.Message = string.Format(Resource.TotalTimeExceeded, Resource.EmployeeVisit);
                return response;
            }

            /* Kundan/Pallav: 08-04-2020, not required while deleting visits notes.
            List<SearchValueData> NoteParams = new List<SearchValueData>();
            NoteParams.Add(new SearchValueData("EmployeeVisitID", Convert.ToString(searchEmployeeVisitNoteListPage.EmployeeVisitID)));
            NoteParams.Add(new SearchValueData("LoggedInID", Convert.ToString(SessionHelper.LoggedInID)));
            NoteParams.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
            NoteParams.Add(new SearchValueData("MacAddress", Common.GetMAcAddress()));
            AddNote(NoteParams);
            */
            response.Data = data;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);
            return response;
        }

        public ServiceResponse GetEmployeeVisitConclusionList(long EmployeeVisitId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(Resource.Conclusion) });
            searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(EmployeeVisitId) });
            List<EmployeeVisitConclusionModel> list = GetEntityList<EmployeeVisitConclusionModel>(StoredProcedure.GetEmployeeVisitConclusionList, searchList);
            if (list.Count > 0)
            {
                foreach (EmployeeVisitConclusionModel item in list)
                {
                    item.Description = item.Description != null ? (item.Description.ToUpper() == Constants.CapsYes ? Constants.CapsYes : Constants.CapsNo) : Constants.CapsNo;
                    item.Desc = item.Description != null ? (item.Description.ToUpper() == Constants.CapsYes ? true : false) : false;
                }
            }
            response.Data = list;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse ChangeConclusionAnswer(ChangeConclusionModel ConclusionAnswer)
        {
            var response = new ServiceResponse();
            try
            {
                var Desc = ConclusionAnswer.Description == true ? Constants.Yes : Constants.No;
                GetScalar(StoredProcedure.ChangeConclusionAnswer, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeVisitNoteID",Value = Convert.ToString(ConclusionAnswer.EmployeeVisitNoteID)},
                    new SearchValueData {Name = "Description",Value = Desc},
                    new SearchValueData {Name = "SystemID",Value = Common.GetHostAddress()},
                    new SearchValueData {Name = "LoggedInID",Value = Convert.ToString(SessionHelper.LoggedInID)}
                });
                response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.Conclusion);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GetGroupVisitTask(long careType)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(Resource.Task) });
            searchList.Add(new SearchValueData { Name = "CareType", Value = Convert.ToString(careType) });
            List<GroupVisitTask> VisitTaskList = GetEntityList<GroupVisitTask>(StoredProcedure.GetGroupVisitTask, searchList);
            //GroupVisitTaskModels VisitTaskList = GetMultipleEntity<GroupVisitTaskModels>(StoredProcedure.GetGroupVisitTask, searchList);
            GroupVisitTaskModel setModel = new GroupVisitTaskModel
            {
                VisitTaskList = VisitTaskList,
                HourList = Common.GetHourList(),
                MinuteList = Common.GetMinuteList()
            };

            response.Data = setModel;
            response.IsSuccess = true;
            return response;
        }
        public ServiceResponse GetGroupVisitTaskOptionList(string careType)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(Resource.Task) });
            searchList.Add(new SearchValueData { Name = "careType", Value = Convert.ToString(careType) });
            //List<GroupVisitTask> VisitTaskList = GetEntityList<GroupVisitTask>(StoredProcedure.GetGroupVisitTask, searchList);
            List<VisitTaskOptionList> VisitTaskOptionList = GetEntityList<VisitTaskOptionList>(StoredProcedure.GetGroupTaskOptionList, searchList);
            //GroupVisitTaskModels VisitTaskList = GetEntityList<GroupVisitTaskModels>(StoredProcedure.GetGroupVisitTask, searchList);


            response.Data = VisitTaskOptionList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetMappedVisitTask(long EmployeeVisitId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(Resource.Task) });
            searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(EmployeeVisitId) });
            List<VisitReferral> VisitTaskList = GetEntityList<VisitReferral>(StoredProcedure.GetMappedVisitTask, searchList);

            AddNoteModel setModel = new AddNoteModel
            {
                VisitTaskList = VisitTaskList,
                HourList = Common.GetHourList(),
                MinuteList = Common.GetMinuteList()
            };

            response.Data = setModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetMappedVisitConclusion(long EmployeeVisitId)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "VisitTaskType", Value = Convert.ToString(Resource.Conclusion) });
            searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(EmployeeVisitId) });
            List<VisitReferral> VisitTaskList = GetEntityList<VisitReferral>(StoredProcedure.GetMappedVisitTask, searchList);

            AddNoteModel setModel = new AddNoteModel
            {
                VisitTaskList = VisitTaskList,
                HourList = Common.GetHourList(),
                MinuteList = Common.GetMinuteList(),
                YesNoList = Common.SetYesNoStringList()
            };

            response.Data = setModel;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetMappedVisitTaskForms(long employeeVisitID, long referralTaskMappingID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(employeeVisitID) });
                searchList.Add(new SearchValueData { Name = "ReferralTaskMappingID", Value = Convert.ToString(referralTaskMappingID) });
                searchList.Add(new SearchValueData { Name = "OrganizationID", Value = Convert.ToString(SessionHelper.OrganizationId) });
                List<EmployeeVisitNoteFormList> visitTaskFormList = GetEntityList<EmployeeVisitNoteFormList>(StoredProcedure.GetReferralTaskForms, searchList);
                response.Data = visitTaskFormList;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SaveVisitNoteOrbeonForm(EmployeeVisitNoteForm form)
        {
            var response = new ServiceResponse();
            try
            {
                IReferralDataProvider _referralDataProvider = new ReferralDataProvider();
                if (form.ComplianceID == 0)
                {
                    long parentID = 0;

                    List<SearchValueData> searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData { Name = "DocumentName", Value = form.SectionName });
                    searchList.Add(new SearchValueData { Name = "ParentID", Value = Convert.ToString(0) });
                    var compliance = GetEntity<Compliance>(searchList);
                    if (compliance != null)
                    {
                        parentID = compliance.ComplianceID;
                    }
                    else
                    {
                        // Create Section
                        AddDirSubDirModal section = new AddDirSubDirModal()
                        {
                            UserType = Common.UserType.Referral.ToString(),
                            Name = form.SectionName,
                            Value = string.Format("#{0:X6}", new Random().Next(0x1000000)),
                            DocumentationType = (int)Common.DocumentationType.Internal,
                            SelectedRoles = new List<string>(),
                            HideIfEmpty = true
                        };
                        var secRes = _referralDataProvider.HC_SaveSectionNew(section, SessionHelper.RoleID);
                        if (!secRes.IsSuccess)
                        { return secRes; }

                        parentID = (long)secRes.Data;
                    }

                    searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData { Name = "DocumentName", Value = form.SubSectionName });
                    searchList.Add(new SearchValueData { Name = "ParentID", Value = Convert.ToString(parentID) });
                    compliance = GetEntity<Compliance>(searchList);
                    if (compliance != null)
                    {
                        form.ComplianceID = compliance.ComplianceID;
                    }
                    else
                    {
                        // Create Sub Section
                        AddDirSubDirModal subSection = new AddDirSubDirModal()
                        {
                            UserType = Common.UserType.Referral.ToString(),
                            Name = form.SubSectionName,
                            Value = string.Format("#{0:X6}", new Random().Next(0x1000000)),
                            DocumentationType = (int)Common.DocumentationType.Internal,
                            ParentID = parentID,
                            SelectedRoles = new List<string>(),
                            HideIfEmpty = true
                        };
                        var secRes = _referralDataProvider.HC_SaveSubSectionNew(subSection);
                        if (!secRes.IsSuccess)
                        { return secRes; }

                        form.ComplianceID = (long)secRes.Data;
                    }
                }

                // Save Referral Document Orbeon
                LinkDocModel linkDocModel = new LinkDocModel()
                {
                    DocumentID = form.OrbeonFormID,
                    ReferralDocumentID = form.ReferralDocumentID,
                    EmployeeID = form.EmployeeID,
                    ReferralID = form.ReferralID,
                    ComplianceID = form.ComplianceID
                };
                var res = _referralDataProvider.HC_GetOrbeonFormDetailsByID(linkDocModel, false);
                if (!res.IsSuccess)
                { return res; }

                // Add Referral Task Form
                var doc = res.Data as ReferralDocument;
                if (form.ReferralTaskFormMappingID == 0)
                {
                    List<SearchValueData> searchList = new List<SearchValueData>();
                    searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(form.EmployeeVisitID) });
                    searchList.Add(new SearchValueData { Name = "ReferralTaskMappingID", Value = Convert.ToString(form.ReferralTaskMappingID) });
                    searchList.Add(new SearchValueData { Name = "TaskFormMappingID", Value = Convert.ToString(form.TaskFormMappingID) });
                    searchList.Add(new SearchValueData { Name = "ReferralDocumentID", Value = Convert.ToString(doc.ReferralDocumentID) });
                    GetScalar(StoredProcedure.AddReferralTaskForm, searchList);
                    response.Message = string.Format(Resource.RecordCreatedSuccessfully, Resource.VisitTaskForm);
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.VisitTaskForm);
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse DeleteVisitNoteForm(long referralTaskFormMappingID)
        {
            var response = new ServiceResponse();
            try
            {
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralTaskFormMappingID", Value = Convert.ToString(referralTaskFormMappingID) });
                GetScalar(StoredProcedure.DeleteReferralTaskForm, searchList);
                response.Message = string.Format(Resource.DeletedSuccessfully, Resource.VisitTaskForm);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse GenerateBillingNote(long EmployeeVisitID)
        {
            var response = new ServiceResponse();
            var searchParameter = new List<SearchValueData>();
            try
            {
                searchParameter.Add(new SearchValueData("EmployeeVisitID", Convert.ToString(EmployeeVisitID)));
                searchParameter.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
                searchParameter.Add(new SearchValueData("MacAddress", Common.GetMAcAddress()));
                searchParameter.Add(new SearchValueData("LoggedInID", Convert.ToString(SessionHelper.LoggedInID)));

                TransactionResult result = AddNote(searchParameter);
                if (result.TransactionResultId == 1)
                {
                    response.IsSuccess = true;
                    response.Message = Resource.BillingNoteGenerateSuccessfully;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SaveVisitNote(VisitNoteModel model)
        {
            var response = new ServiceResponse();
            var searchParameter = new List<SearchValueData>();
            CacheHelper cache = new CacheHelper();
            try
            {
                int totalMinutes = (model.Hours * 60) + model.Minutes;

                if (totalMinutes == 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ServiceTimeValidation;
                    return response;
                }

                searchParameter.Add(new SearchValueData("EmployeeVisitID", Convert.ToString(model.EmployeeVisitID)));
                searchParameter.Add(new SearchValueData("EmployeeVisitNoteID", Convert.ToString(model.EmployeeVisitNoteID)));
                searchParameter.Add(new SearchValueData("ReferralTaskMappingID", Convert.ToString(model.ReferralTaskMappingID)));
                searchParameter.Add(new SearchValueData("ServiceTime", Convert.ToString(totalMinutes)));
                searchParameter.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
                searchParameter.Add(new SearchValueData("MacAddress", Common.GetMAcAddress()));
                searchParameter.Add(new SearchValueData("SetAsIncomplete", Convert.ToString(cache.PatientResignatureNeeded)));
                searchParameter.Add(new SearchValueData("LoggedInID", Convert.ToString(SessionHelper.LoggedInID)));


                int data = (int)GetScalar(StoredProcedure.SaveVisitNote, searchParameter);

                if (data == -1)
                {
                    response.Message = Resource.TaskAlreadyExists;
                    return response;
                }
                else if (data == -2)
                {
                    response.Message = Resource.TotalTimeExceeded;
                    return response;
                }
                else if (data == -3)
                {
                    response.Message = "Invoice generated and thier status is in paid or void so we can't update this task";
                    return response;
                }
                //response.IsSuccess = true;
                //response.Message = model.EmployeeVisitNoteID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Task) : string.Format(Resource.RecordSavedSuccessfully, Resource.Task);

                var notesrchParam = new List<SearchValueData>();
                notesrchParam.Add(new SearchValueData("EmployeeVisitID", Convert.ToString(model.EmployeeVisitID)));
                notesrchParam.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
                notesrchParam.Add(new SearchValueData("MacAddress", Common.GetMAcAddress()));
                notesrchParam.Add(new SearchValueData("LoggedInID", Convert.ToString(SessionHelper.LoggedInID)));

                TransactionResult result = AddNote(notesrchParam);
                if (result.TransactionResultId == 1)
                {
                    if (cache.PatientResignatureNeeded)
                    {
                        SendNotification(model);
                    }
                    response.IsSuccess = true;
                    response.Message = model.EmployeeVisitNoteID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Task) : string.Format(Resource.RecordSavedSuccessfully, Resource.Task);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SaveVisitNoteTimeSheet(VisitNoteModel model)
        {
            var response = new ServiceResponse();
            var searchParameter = new List<SearchValueData>();
            CacheHelper cache = new CacheHelper();
            try
            {
                int totalMinutes = (model.Hours * 60) + model.Minutes;

                if (totalMinutes == 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ServiceTimeValidation;
                    return response;
                }

                searchParameter.Add(new SearchValueData("EmployeeVisitID", Convert.ToString(model.EmployeeVisitID)));
                searchParameter.Add(new SearchValueData("EmployeeVisitNoteID", Convert.ToString(model.EmployeeVisitNoteID)));
                searchParameter.Add(new SearchValueData("ReferralTaskMappingID", Convert.ToString(model.ReferralTaskMappingID)));
                searchParameter.Add(new SearchValueData("ServiceTime", Convert.ToString(totalMinutes)));
                searchParameter.Add(new SearchValueData("SystemID", Common.GetHostAddress()));
                searchParameter.Add(new SearchValueData("MacAddress", Common.GetMAcAddress()));
                searchParameter.Add(new SearchValueData("SetAsIncomplete", Convert.ToString(cache.PatientResignatureNeeded)));
                searchParameter.Add(new SearchValueData("LoggedInID", Convert.ToString(SessionHelper.LoggedInID)));


                int data = (int)GetScalar(StoredProcedure.SaveVisitNote, searchParameter);

                if (data == -1)
                {
                    response.Message = Resource.TaskAlreadyExists;
                    return response;
                }
                else if (data == -2)
                {
                    response.Message = Resource.TotalTimeExceeded;
                    return response;
                }
                else if (data == -3)
                {
                    response.Message = "Invoice generated and thier status is in paid or void so we can't update this task";
                    return response;
                }

                response.IsSuccess = true;
                response.Message = model.EmployeeVisitNoteID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Task) : string.Format(Resource.RecordSavedSuccessfully, Resource.Task);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public void SendNotification(VisitNoteModel model)
        {
            var domainName = SessionHelper.DomainName;
            var notificationType = (int)Mobile_Notification.NotificationTypes.PatientResignature;

            PatientResignatureNotificationDetails detail = GetEntity<PatientResignatureNotificationDetails>(StoredProcedure.GetEmpDetailsForNotification, new List<SearchValueData>
                {
                    new SearchValueData("EmployeeVisitID", Convert.ToString(model.EmployeeVisitID)),
                    new SearchValueData("NotificationType",Convert.ToString(notificationType)),
                    new SearchValueData("ServerDateTime",DateTime.UtcNow.ToString(Constants.DbDateTimeFormat)),
                    new SearchValueData("NotificationStatus",Convert.ToString((int)Mobile_Notification.NotificationStatuses.Sent)),
                    new SearchValueData("LoggedInId",Convert.ToString(SessionHelper.LoggedInID)),
                });

            FcmManager fcmManager = new FcmManager(new FcmManagerOptions
            {
                AuthenticationKey = ConfigSettings.FcmAuthenticationKey,
                SenderId = ConfigSettings.FcmSenderId
            });

            if (detail.FcmTokenId != null && detail.DeviceType != null)
            {
                var fcmResponseIos = fcmManager.SendMessage(new FcmMessage
                {
                    RegistrationIds = new List<string> { detail.FcmTokenId },
                    Notification = detail.DeviceType.ToLower() == Constants.ios
                        ? new FcmNotification
                        {
                            Body = "Task has been updated by Admin",
                            Title = domainName
                        }
                        : null,
                    Data = new PatientResignatureNotificationModel
                    {
                        SiteName = domainName,
                        Body = "Task has been updated by Admin",
                        NotificationType = notificationType,
                        ReferralID = detail.ReferralID,
                        ScheduleID = detail.ScheduleID,
                        Editable = detail.Editable
                    },
                });
            }
        }

        public TransactionResult AddNote(List<SearchValueData> searchParams)
        {
            TransactionResult result = GetEntity<TransactionResult>(StoredProcedure.HC_AddNote, searchParams);
            return result;
        }

        public ServiceResponse SaveVisitConclusion(VisitConclusionModel model)
        {
            var response = new ServiceResponse();
            try
            {
                int data = (int)GetScalar(StoredProcedure.SaveVisitConclusion, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                    new SearchValueData {Name = "EmployeeVisitNoteID",Value = Convert.ToString(model.EmployeeVisitNoteID)},
                    new SearchValueData {Name = "ReferralTaskMappingID",Value = Convert.ToString(model.ReferralTaskMappingID)},
                    new SearchValueData {Name = "AlertComment",Value = Convert.ToString(model.AlertComment)},
                    new SearchValueData {Name = "Description",Value = model.Description},
                    new SearchValueData {Name = "SystemID",Value = Common.GetHostAddress()},
                    new SearchValueData {Name = "LoggedInID",Value = Convert.ToString(SessionHelper.LoggedInID)}
                });
                if (data == -1)
                {
                    response.Message = Resource.ConclusionAlreadyExists;
                    return response;
                }
                response.IsSuccess = true;
                response.Message = model.EmployeeVisitNoteID > 0 ? string.Format(Resource.RecordUpdatedSuccessfully, Resource.Conclusion) : string.Format(Resource.RecordSavedSuccessfully, Resource.Conclusion);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse SaveDeviationNotes(DeviationNotesModel model)
        {
            var response = new ServiceResponse();
            CacheHelper cache = new CacheHelper();
            try
            {
                int totalMinutes = (model.Hours * 60) + model.Minutes;

                if (totalMinutes == 0)
                {
                    response.IsSuccess = false;
                    response.Message = Resource.ServiceTimeValidation;
                    return response;
                }
                int data = (int)GetScalar(StoredProcedure.SaveDeviationNotes, new List<SearchValueData>
                {
                    new SearchValueData {Name = "EmployeeVisitID",Value = Convert.ToString(model.EmployeeVisitID)},
                    new SearchValueData {Name = "DeviationNotes",Value = Convert.ToString(model.DeviationNotes)},
                    new SearchValueData {Name = "DeviationID",Value = Convert.ToString(model.DeviationID)},
                    new SearchValueData {Name = "DeviationNoteID",Value = Convert.ToString(model.DeviationNoteID)},
                    new SearchValueData {Name = "DeviationTime",Value = Convert.ToString(totalMinutes)},
                //    new SearchValueData {Name = "LoggedInID",Value = Convert.ToString(SessionHelper.LoggedInID)}
                });
                if (data == -1)
                {
                    response.Message = "Deviation Note already exist";
                    return response;
                }
                else if (data == -2)
                {
                    response.Message = "Total Deviation time are exceeding";
                    return response;
                }
                response.Data = data;
                response.IsSuccess = true;
                response.Message = "Deviation Note Save Successfuly";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse DeleteDeviationNote(String ListOfIdsInCsv, long loggedInUserID)
        {
            var response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ListOfIdsInCsv", Value = Convert.ToString(ListOfIdsInCsv)},
                    new SearchValueData {Name = "loggedInID", Value = Convert.ToString(loggedInUserID)},
                };

            List<DeviationNoteModel> data = GetEntityList<DeviationNoteModel>(StoredProcedure.DeleteDeviationNote, searchlist);

            response.Data = data;
            response.IsSuccess = true;
            response.Message = string.Format(Resource.RecordUpdatedSuccessfully, Resource.EmployeeVisit);
            return response;
        }

        public ServiceResponse GetDeviationNotes(long EmployeeVisitID)
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
            {
                new SearchValueData {Name = "EmployeeVisitID", Value = Convert.ToString(EmployeeVisitID)},
                //new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(employeeId)},
                //new SearchValueData {Name = "LoggedInUser", Value = Convert.ToString(loggedInUserID)},
            };
            //  List<DeviationNoteModel> totalData = GetEntityList<DeviationNoteModel>(StoredProcedure.GetDeviationNotes, searchlist);
            DeviationModels totalData = GetMultipleEntity<DeviationModels>(StoredProcedure.GetDeviationNotes, searchlist);
            response.Data = totalData;
            return response;
        }

        public ServiceResponse BypassActionTaken(ByPassDetailModel model)
        {
            var response = new ServiceResponse();
            var searchParameter = new List<SearchValueData>();
            try
            {

                searchParameter.Add(new SearchValueData("EmployeeVisitID", Convert.ToString(model.EmployeeVisitID)));
                searchParameter.Add(new SearchValueData("RejectReason", model.IsApprove ? null : model.RejectReason));
                searchParameter.Add(new SearchValueData("ByPassReasonClockIn", model.ByPassReasonClockIn));
                searchParameter.Add(new SearchValueData("ActionTaken", model.IsApprove ? Convert.ToString((int)EmployeeVisit.BypassActions.Approved) : Convert.ToString((int)EmployeeVisit.BypassActions.Rejected)));

                GetScalar(StoredProcedure.HC_BypassActionTaken, searchParameter);

                response.IsSuccess = true;
                response.Message = Resource.BypassActionAppliedSuccessfully;
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse SaveByPassReasonNote(ByPassDetailModel model)
        {
            var response = new ServiceResponse();
            var searchParameter = new List<SearchValueData>();
            try
            {

                searchParameter.Add(new SearchValueData("EmployeeVisitID", Convert.ToString(model.EmployeeVisitID)));
                searchParameter.Add(new SearchValueData("RejectReason", model.IsApprove ? null : model.RejectReason));
                searchParameter.Add(new SearchValueData("ByPassReasonClockIn", model.ByPassReasonClockIn));
                searchParameter.Add(new SearchValueData("ByPassReasonClockOut", model.ByPassReasonClockOut));
                // searchParameter.Add(new SearchValueData("ActionTaken", model.IsApprove ? Convert.ToString((int)EmployeeVisit.BypassActions.Approved) : Convert.ToString((int)EmployeeVisit.BypassActions.Rejected)));

                GetScalar(StoredProcedure.HC_BypassActionTaken, searchParameter);

                response.IsSuccess = true;
                response.Message = Resource.BypassActionAppliedSuccessfully;
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }

        public ServiceResponse SetPatientTimeSheetPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetPatientTimeSheetPage model = GetMultipleEntity<SetPatientTimeSheetPage>(StoredProcedure.SetEmployeeVisitListPage);
            var setPatientTimeSheetPage = new SetPatientTimeSheetPage
            {
                SearchPatientTimeSheetListPage = { IsDeleted = 0 },
                EmployeeList = model.EmployeeList,
                ReferralList = model.ReferralList
            };
            response.Data = setPatientTimeSheetPage;
            return response;
        }

        #endregion

        public ServiceResponse GeneratePcaTimeSheet(long employeeVisitID, Boolean SimpleTaskType)
        {
            ServiceResponse response = new ServiceResponse();

            PCAModel pcaModel = GetMultipleEntity<PCAModel>(StoredProcedure.GetPCATimesheetDetail,
            new List<SearchValueData>
            {
                    new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(employeeVisitID) },
                    new SearchValueData { Name = "TaskType", Value = Convert.ToString(VisitTask.TaskType.Task) },
                    new SearchValueData { Name = "ConclusionType", Value = Convert.ToString(VisitTask.TaskType.Conclusion) }

            });
            if (pcaModel.TaskList.Count > 0)
            {
                // Start , Added by Sagar,22 Dec 2019 : Set condition for display ony Task List based on Simple and Details Task permissions
                if (SimpleTaskType)
                {
                    pcaModel.TaskList = pcaModel.TaskList.Where(x => x.SimpleTaskType == true).ToList();
                }
                // End 
            }
            pcaModel.PcaTaskList = GetCategoryGroup(pcaModel.TaskList);
            pcaModel.PcaConclusionList = GetCategoryGroup(pcaModel.ConclusionList);

            response.Data = pcaModel;

            return response;
        }
        public ServiceResponse GeneratePcaTimeSheetDayCare(long employeeVisitID, Boolean SimpleTaskType)
        {
            ServiceResponse response = new ServiceResponse();

            PCAModel pcaModel = GetMultipleEntity<PCAModel>(StoredProcedure.GetPCATimesheetDetailDayCare,
           new List<SearchValueData>
           {
                    new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(employeeVisitID) },
                    new SearchValueData { Name = "TaskType", Value = Convert.ToString(VisitTask.TaskType.Task) },
                    new SearchValueData { Name = "ConclusionType", Value = Convert.ToString(VisitTask.TaskType.Conclusion) }

           });
            if (pcaModel.TaskList.Count > 0)
            {
                // Start , Added by Sagar,22 Dec 2019 : Set condition for display ony Task List based on Simple and Details Task permissions
                if (SimpleTaskType)
                {
                    pcaModel.TaskList = pcaModel.TaskList.Where(x => x.SimpleTaskType == true).ToList();
                }
                // End 
            }
            pcaModel.PcaTaskList = GetCategoryGroup(pcaModel.TaskList);
            pcaModel.PcaConclusionList = GetCategoryGroup(pcaModel.ConclusionList);

            response.Data = pcaModel;

            return response;
        }

        private List<Categories> GetCategoryGroup(List<TaskLists> taskList)
        {
            List<Categories> categoryList = taskList.ToList().GroupBy(c => new
            {
                c.CategoryId,
                c.CategoryName,
            }).Select(grp => new Categories
            {
                CategoryId = grp.Key.CategoryId,
                CategoryName = grp.Key.CategoryName,
                TaskLists = grp.ToList()
            }).ToList();


            foreach (Categories category in categoryList)
            {
                List<SubCategory> subCategoryList = category.TaskLists.GroupBy(sc => new
                {
                    sc.SubCategoryId,
                    sc.SubCategoryName,
                }).Select(grp => new SubCategory
                {
                    SubCategoryId = grp.Key.SubCategoryId,
                    SubCategoryName = grp.Key.SubCategoryName,
                    TaskLists = grp.ToList()
                }).ToList();

                category.SubCategory.AddRange(subCategoryList);
            }

            return categoryList;
        }

        #region Employee Billing Report

        public ServiceResponse SetEmployeeBillingReportListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetEmployeeBillingReportListPage model = GetMultipleEntity<SetEmployeeBillingReportListPage>(StoredProcedure.SetEmployeeBillingListPage);
            // SetEmployeeBillingReportListPage model = new SetEmployeeBillingReportListPage();
            response.Data = model;
            return response;
        }

        public ServiceResponse GetEmployeeBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (searchEmployeeBillingReportPage != null)
                SetSearchFilterForEmployeeBillingReportList(searchEmployeeBillingReportPage, searchList);

            List<EmployeeBillingReportListModel> totalData = GetEntityList<EmployeeBillingReportListModel>(StoredProcedure.HC_GetEmployeeBillingReportList, searchList);
            if (!Common.HasPermission(Constants.AllRecordAccess))
            {
                totalData = totalData.Where(_ => _.EmployeeID == Convert.ToInt32(SessionHelper.LoggedInID)).ToList();
            }
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<EmployeeBillingReportListModel> employeeBillingReportList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = employeeBillingReportList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForEmployeeBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "EmployeeName", Value = Convert.ToString(searchEmployeeBillingReportPage.EmployeeName) });
            searchList.Add(new SearchValueData { Name = "WeekStartDay", Value = Convert.ToString(Common.GetCalWeekStartDay()) });

            if (searchEmployeeBillingReportPage.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchEmployeeBillingReportPage.StartDate) });

            if (searchEmployeeBillingReportPage.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchEmployeeBillingReportPage.EndDate) });
        }

        #endregion

        #region Patient Total Report

        public ServiceResponse SetPatientTotalReportListPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetReferralTotalReportListPage model = new SetReferralTotalReportListPage();
            response.Data = model;
            return response;
        }

        public ServiceResponse GetPatientTotalReportList(SearchPatientTotalReportListPage searchPatientTotalReportListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (searchPatientTotalReportListPage != null)
                SetSearchFilterForPatientTotalReportList(searchPatientTotalReportListPage, searchList);

            List<PatientTotalReportListModel> totalData = GetEntityList<PatientTotalReportListModel>(StoredProcedure.GetActiveReferralList, searchList);

            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<PatientTotalReportListModel> patienttotalReportList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = patienttotalReportList;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse GetPatientTotalReportListDownload(SearchPatientTotalReportListPage searchPatientTotalReportListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            if (searchPatientTotalReportListPage != null)
                SetSearchFilterForPatientTotalReportList(searchPatientTotalReportListPage, searchList);

            PatientListModel patientListModel = GetMultipleEntity<PatientListModel>(StoredProcedure.GetActiveReferralList, searchList);

            response.Data = patientListModel;
            return response;
        }

        private static void SetSearchFilterForPatientTotalReportList(SearchPatientTotalReportListPage searchPatientTotalReportListPage, List<SearchValueData> searchList)
        {
            if (searchPatientTotalReportListPage.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchPatientTotalReportListPage.StartDate) });

            if (searchPatientTotalReportListPage.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchPatientTotalReportListPage.EndDate) });
        }

        #endregion
        #region DMAS_Forms
        public ServiceResponse SetEmployeeVisitListPageDMAS(string data)
        {
            ServiceResponse response = new ServiceResponse();
            SetEmployeeVisitListPage model = GetMultipleEntity<SetEmployeeVisitListPage>(StoredProcedure.SetEmployeeVisitListPageDMAS);

            var setEmployeeVisitListPage = new SetEmployeeVisitListPage
            {
                DeleteFilter = Common.SetDeleteFilter(),
                ActionFilter = Common.GetListFromEnum<EmployeeVisit.BypassActions>(),
                SearchEmployeeVisitListPage = { IsDeleted = 0, ActionTaken = ((!string.IsNullOrEmpty(data)) && (data.ToUpper() == EmployeeVisit.BypassActions.Pending.ToString().ToUpper())) ? 1 : 0 },
                EmployeeList = model.EmployeeList,
                ReferralList = model.ReferralList,
                ServiceTypeList = model.ServiceTypeList
                // CareTypeList = model.CareTypeList
            };
            response.Data = setEmployeeVisitListPage;
            return response;
        }
        public ServiceResponse GetCaretype()
        {
            ServiceResponse response = new ServiceResponse();

            var searchlist = new List<SearchValueData>
            {
            };
            List<ReportCareTypeListModel> totalData = GetEntityList<ReportCareTypeListModel>(StoredProcedure.GetCaretype, searchlist);
            response.IsSuccess = true;
            response.Data = totalData;
            return response;
        }

        public ServiceResponse SetDMASForm_90FormList(string data)
        {
            ServiceResponse response = new ServiceResponse();
            SetEmployeeVisitListPage model = GetMultipleEntity<SetEmployeeVisitListPage>(StoredProcedure.DMASForm_90FormList);

            var setEmployeeVisitListPage = new SetEmployeeVisitListPage
            {
                DeleteFilter = Common.SetDeleteFilter(),
                ActionFilter = Common.GetListFromEnum<EmployeeVisit.BypassActions>(),
                SearchEmployeeVisitListPage = { IsDeleted = 0, ActionTaken = ((!string.IsNullOrEmpty(data)) && (data.ToUpper() == EmployeeVisit.BypassActions.Pending.ToString().ToUpper())) ? 1 : 0 },
                EmployeeList = model.EmployeeList,
                ReferralList = model.ReferralList
            };
            response.Data = setEmployeeVisitListPage;
            return response;
        }

        public ServiceResponse GetDMASForm_90FormList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, int pageIndex, int pageSize,
          string sortIndex, string sortDirection)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();

            if (searchEmployeeVisitListPage != null)
                SetSearchFilterForDMASForm_90FormList(searchEmployeeVisitListPage, searchList);

            searchList.AddRange(Common.SetPagerValues(pageIndex, pageSize, sortIndex, sortDirection));

            List<DMASForm_90FormListModel> totalData = GetEntityList<DMASForm_90FormListModel>(StoredProcedure.DMASFormList, searchList);
            if (!Common.HasPermission(Constants.AllRecordAccess))
            {
                totalData = totalData.Where(_ => _.EmployeeID == Convert.ToInt32(SessionHelper.LoggedInID)).ToList();
            }
            int count = 0;
            if (totalData != null && totalData.Count > 0)
                count = totalData.First().Count;

            Page<DMASForm_90FormListModel> employeeVisitList = GetPageInStoredProcResultSet(pageIndex, pageSize, count, totalData);
            response.Data = employeeVisitList;
            response.IsSuccess = true;
            return response;
        }

        private static void SetSearchFilterForDMASForm_90FormList(SearchEmployeeVisitListPage searchEmployeeVisitListPage, List<SearchValueData> searchList)
        {
            searchList.Add(new SearchValueData { Name = "EmployeeVisitID", Value = Convert.ToString(searchEmployeeVisitListPage.EmployeeVisitID) });
            searchList.Add(new SearchValueData { Name = "EmployeeIDs", Value = Convert.ToString(searchEmployeeVisitListPage.EmployeeIDs) });
            searchList.Add(new SearchValueData { Name = "ReferralIDs", Value = Convert.ToString(searchEmployeeVisitListPage.ReferralIDs) });
            searchList.Add(new SearchValueData { Name = "StartTime", Value = Convert.ToString(searchEmployeeVisitListPage.StartTime) });
            searchList.Add(new SearchValueData { Name = "EndTime", Value = Convert.ToString(searchEmployeeVisitListPage.EndTime) });
            if (searchEmployeeVisitListPage.StartDate.HasValue)
                searchList.Add(new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchEmployeeVisitListPage.StartDate).ToString(Constants.DbDateFormat) });
            if (searchEmployeeVisitListPage.EndDate.HasValue)
                searchList.Add(new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchEmployeeVisitListPage.EndDate).ToString(Constants.DbDateFormat) });
            searchList.Add(new SearchValueData { Name = "IsDeleted", Value = Convert.ToString(searchEmployeeVisitListPage.IsDeleted) });
            searchList.Add(new SearchValueData { Name = "ActionTaken", Value = Convert.ToString(searchEmployeeVisitListPage.ActionTaken) });
        }

        public ServiceResponse GetDMAS_90Forms(long ReferralID)
        {
            ServiceResponse response = new ServiceResponse();
            if (ReferralID >= 0)
            {
                var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(ReferralID)},
                    //new SearchValueData {Name = "EmployeeID", Value = Convert.ToString(employeeId)},
                };
                List<DMASForm_90FormListModel> totalData = GetEntityList<DMASForm_90FormListModel>(StoredProcedure.DMAS_90Forms, searchlist);
                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
                response.Message = Resource.GetReferralInternalMessageError;
            return response;
        }

        public ServiceResponse GetEmployeeByReferralID(string referralID, string StartDate = null, string EndDate = null)
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = referralID}
                };
            if (StartDate != null)
                searchlist.Add(new SearchValueData { Name = "StartDate", Value = StartDate });
            if (EndDate != null)
                searchlist.Add(new SearchValueData { Name = "EndDate", Value = EndDate });

            List<EmployeeListModel> employees = GetEntityList<EmployeeListModel>(StoredProcedure.GetPatientScheduledEmployees, searchlist);
            response.Data = employees;
            response.IsSuccess = true;
            return response;
        }

        public List<DMASFormsDaysModel> GetEmployeeVisitListDays(DateTime startDate, long referralID, long employeeID, int caretype)
        {
            ServiceResponse response = new ServiceResponse();
            List<SearchValueData> searchList = new List<SearchValueData>
                {
                    new SearchValueData {Name = "StartDate", Value = Convert.ToString(startDate)},
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(referralID)},
                   new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(employeeID) },
                   new SearchValueData { Name = "Caretype", Value = Convert.ToString(caretype)}
                };

            return GetEntityList<DMASFormsDaysModel>(StoredProcedure.WeeklyReportDMAS90GetDays, searchList);
        }

        public ServiceResponse GetEmployeeVisitList1(SearchDMASfORM searchEmployeeVisitNoteListPage, bool isConclusion = true)
        {
            ServiceResponse response = new ServiceResponse();
            DateTime startDate = Convert.ToDateTime(searchEmployeeVisitNoteListPage.StartDate);
            DateTime EndDate = Convert.ToDateTime(searchEmployeeVisitNoteListPage.EndDate);

            List<DateTime> dates = new List<DateTime>();
            if (startDate < EndDate)
            {
                for (DateTime day = startDate.Date; day.Date <= EndDate.Date; day = day.AddDays(1))
                {
                    dates.Add(day);
                }
            }
            List<SearchValueData> searchList = new List<SearchValueData>();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(searchEmployeeVisitNoteListPage.ReferralID)},
                    new SearchValueData {Name = "CareType", Value = Convert.ToString(searchEmployeeVisitNoteListPage.CareType)},
                   new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchEmployeeVisitNoteListPage.StartDate) },
                   new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchEmployeeVisitNoteListPage.EndDate)},
                   new SearchValueData { Name = "EmployeeID", Value = Convert.ToString(searchEmployeeVisitNoteListPage.EmployeeID)},
                   new SearchValueData { Name = "Dates", Value = Convert.ToString(searchEmployeeVisitNoteListPage.Dates)},
        };

            DMASFormsModel totalData = GetMultipleEntity<DMASFormsModel>(StoredProcedure.GetWeeklyReportDMAS90, searchlist);

            List<DMASForm_90NewModel> DMASForm_90NewModel = totalData.DMASForm_90NewModel;
            List<DMASForm_90NewModelList> DMASForm_90NewModelList = totalData.DMASForm_90NewModelList;
            List<ConclusionModel> ConclusionModelList = totalData.ConclusionModelList;

            if (isConclusion)
            {
                foreach (var item in ConclusionModelList.ToList())
                {
                    var lastDate = ConclusionModelList.LastOrDefault().SurveyDate;
                    if (item.SurveyDate == lastDate)
                    {
                        ConclusionModelList.Add(new ConclusionModel
                        {
                            Description = item.Description,
                            VisitTaskDetail = item.VisitTaskDetail,
                            CareType = item.CareType,
                            VisitTaskID = item.VisitTaskID,
                            VisitTaskType = item.VisitTaskType,
                            Notes = item.Notes,
                            AlertComment = item.AlertComment,
                            Survey = item.Survey,
                            SurveyDate = item.SurveyDate,
                            SurveyDates = Convert.ToDateTime(item.SurveyDate).ToShortDateString(),
                        });

                    }
                }
            }

            var DateTimeModel = new List<DateTimeModel>();
            for (int i = 0; i < dates.Count; i++)
            {
                var item = dates[i];
                var DateModel = new DateTimeModel()
                {
                    DayOfWeek = item.DayOfWeek.ToString(),
                    Dates = item.Date.ToString().Split(' ')[0].ToString()
                };
                DateTimeModel.Add(DateModel);
            }
            //  var AdditionalNoteModel = new List<AdditionalNoteModel>();
            var mylist = new List<DMASForm_90NewModelList>();
            totalData.DateTimeModel = DateTimeModel;
            //totalData.ServiceTimeModel = mylist;
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse GetENewDMAS90List(SearchDMASfORM searchEmployeeVisitNoteListPage)
        {
            ServiceResponse response = new ServiceResponse();

            List<SearchValueData> searchList = new List<SearchValueData>();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = Convert.ToString(searchEmployeeVisitNoteListPage.ReferralID)},
                    new SearchValueData {Name = "CareType", Value = Convert.ToString(searchEmployeeVisitNoteListPage.CareType)},
                   new SearchValueData { Name = "StartDate", Value = Convert.ToDateTime(searchEmployeeVisitNoteListPage.StartDate).ToString(Constants.DbDateFormat) },
                   new SearchValueData { Name = "EndDate", Value = Convert.ToDateTime(searchEmployeeVisitNoteListPage.EndDate).ToString(Constants.DbDateFormat) },
        };

            DMASForms90ModelList totalData = GetMultipleEntity<DMASForms90ModelList>(StoredProcedure.GetWeeklyReportDMAS90, searchlist);
            response.Data = totalData;
            response.IsSuccess = true;
            return response;

        }
        public ServiceResponse GetENewDMAS90ListNew(SearchDMASfORM searchEmployeeVisitNoteListPage)
        {

            ServiceResponse response = new ServiceResponse();
            var Referral = searchEmployeeVisitNoteListPage.ReferralName;
            if (!string.IsNullOrEmpty(Referral))
            {
                DateTime startDate = Convert.ToDateTime(searchEmployeeVisitNoteListPage.StartDate);
                DateTime EndDate = Convert.ToDateTime(searchEmployeeVisitNoteListPage.EndDate);
                List<DateTime> dates = new List<DateTime>();
                if (startDate < EndDate)
                {
                    for (DateTime day = startDate.Date; day.Date <= EndDate.Date; day = day.AddDays(1))
                    {
                        dates.Add(day);
                    }
                }
                DateTime[] StartDayofWeek = dates.Where(d => d.DayOfWeek == DayOfWeek.Monday).ToArray();
                DateTime[] LastDayofWeek = dates.Where(d => d.DayOfWeek == DayOfWeek.Sunday).ToArray();
                List<string> startMondays = new List<string>();
                int month = 0;
                var EndDates = "";
                var weeklyReport = new List<StartDayofWeekModel>();
                var lastdate = new List<LastDayofWeekModel>();
                for (int i = 0; i < StartDayofWeek.Length; i++)
                {
                    if (month <= StartDayofWeek[i].Month)
                    {
                        var item = StartDayofWeek[i];
                        // var EndDates = StartDayofWeek[i].AddDays(6).ToString();
                        if (i < (StartDayofWeek.Length - 1))
                        {
                            EndDates = StartDayofWeek[i].AddDays(6).ToString();
                        }
                        else
                        {
                            EndDates = searchEmployeeVisitNoteListPage.EndDate.ToString();
                        }
                        var startDayofWeekModel = new StartDayofWeekModel()
                        {
                            ReferralFirstName = Referral.Split(',')[0],
                            ReferralLastName = Referral.Split(',')[1],
                            ReferralID = Referral.Split(',')[2],
                            startDate = item.Date.ToString(),
                            EndDate = EndDates,
                            Title = searchEmployeeVisitNoteListPage.CareTypeID,
                            EmployeeID = searchEmployeeVisitNoteListPage.EmployeeID
                        };
                        if (!string.IsNullOrEmpty(Referral))
                        {
                            startDayofWeekModel.ServiceTyeID = Referral.Split(',')[2].ToString() == "" ? "0" : Referral.Split(',')[2].ToString();

                        }
                        weeklyReport.Add(startDayofWeekModel);

                    }
                }

                if (searchEmployeeVisitNoteListPage.ServiceTypeID != "0" && searchEmployeeVisitNoteListPage.ServiceTypeID != null)
                {
                    weeklyReport = weeklyReport.Where(x => x.ServiceTyeID == searchEmployeeVisitNoteListPage.ServiceTypeID).ToList();
                }
                WeeklyReportModel viewmodel = new WeeklyReportModel();
                viewmodel.StartDayofWeekModel = weeklyReport;
                viewmodel.LastDayofWeekModel = lastdate;



                response.Data = viewmodel;
                response.IsSuccess = true;

            }
            return response;

        }

        #endregion
        #region  weeklytimesheet
        public ServiceResponse WeeklyTimeSheet(string data)
        {
            ServiceResponse response = new ServiceResponse();
            SetEmployeeVisitListPage model = GetMultipleEntity<SetEmployeeVisitListPage>(StoredProcedure.SetEmployeeVisitListPageDMAS);

            var setEmployeeVisitListPage = new SetEmployeeVisitListPage
            {
                DeleteFilter = Common.SetDeleteFilter(),
                ActionFilter = Common.GetListFromEnum<EmployeeVisit.BypassActions>(),
                SearchEmployeeVisitListPage = { IsDeleted = 0, ActionTaken = ((!string.IsNullOrEmpty(data)) && (data.ToUpper() == EmployeeVisit.BypassActions.Pending.ToString().ToUpper())) ? 1 : 0 },
                EmployeeList = model.EmployeeList,
                ReferralList = model.ReferralList,
                ServiceTypeList = model.ServiceTypeList
                // CareTypeList = model.CareTypeList
            };
            response.Data = setEmployeeVisitListPage;
            return response;
        }
        public ServiceResponse GenerateWeeklyTimeSheetPdf(SearchDMASfORM searchEmployeeVisitNoteListPage)
        {
            ServiceResponse response = new ServiceResponse();
            DateTime startDate = Convert.ToDateTime(searchEmployeeVisitNoteListPage.StartDate);
            DateTime EndDate = Convert.ToDateTime(searchEmployeeVisitNoteListPage.EndDate);
            List<DateTime> dates = new List<DateTime>();
            if (startDate < EndDate)
            {
                for (DateTime day = startDate.Date; day.Date <= EndDate.Date; day = day.AddDays(1))
                {
                    dates.Add(day);
                }
            }
            List<SearchValueData> searchList = new List<SearchValueData>();
            var searchlist = new List<SearchValueData>
                {
                    new SearchValueData {Name = "ReferralID", Value = "13"},
                    new SearchValueData {Name = "CareType", Value = Convert.ToString(searchEmployeeVisitNoteListPage.CareType)},
                   new SearchValueData { Name = "StartDate", Value = Convert.ToString(searchEmployeeVisitNoteListPage.StartDate) },
                   new SearchValueData { Name = "EndDate", Value = Convert.ToString(searchEmployeeVisitNoteListPage.EndDate)},
        };

            DMASFormsModel totalData = GetMultipleEntity<DMASFormsModel>(StoredProcedure.GetWeeklyReportDMAS90, searchlist);
            List<DMASForm_90NewModel> DMASForm_90NewModel = totalData.DMASForm_90NewModel;
            List<DMASForm_90NewModelList> DMASForm_90NewModelList = totalData.DMASForm_90NewModelList;
            var DateTimeModel = new List<DateTimeModel>();
            for (int i = 0; i < dates.Count; i++)
            {
                var item = dates[i];
                var DateModel = new DateTimeModel()
                {
                    DayOfWeek = item.DayOfWeek.ToString(),
                    Dates = item.Date.ToString().Split(' ')[0].ToString()
                };
                DateTimeModel.Add(DateModel);
            }
            totalData.DateTimeModel = DateTimeModel;
            response.Data = totalData;
            response.IsSuccess = true;
            return response;
        }
        #endregion

        #region Report

        public ServiceResponse ReportMasterList(long LoggedInID)
        {
            ServiceResponse response = new ServiceResponse();
            var searchlist = new List<SearchValueData>
            {
                // new SearchValueData {Name = "loggedInUser", Value = Convert.ToString(LoggedInID)}
            };
            List<ReportMasterModel> totalData = GetEntityListAdmin<ReportMasterModel>(StoredProcedure.GetReportMaster, searchlist);

            if (Common.HasPermission(Constants.HC_Permission_DMASFrom))
            {
                response.IsSuccess = true;
                response.Data = totalData;
            }
            else
            {
                var RemoveList = totalData.Where(x => x.ReportName != "DMAS90").ToList();
                response.IsSuccess = true;
                response.Data = RemoveList;
            }
            return response;
        }
        public ServiceResponse GetEmployeeList()
        {
            ServiceResponse response = new ServiceResponse();
            try
            {

                var searchlist = new List<SearchValueData> { };
                List<EmployeeModel> totalData = GetEntityList<EmployeeModel>(StoredProcedure.EmployeeList, searchlist);

                response.IsSuccess = true;
                response.Data = totalData;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, ex.Message);
            }
            return response;
        }
        public ServiceResponse CategoryList(string Category, string EmployeeVisitIDList)
        {

            var response = new ServiceResponse();


            try
            {
                List<SearchValueData> searchParam = new List<SearchValueData>();
                searchParam.Add(new SearchValueData { Name = "Category", Value = Category });
                searchParam.Add(new SearchValueData { Name = "EmployeeVisitIDList", Value = EmployeeVisitIDList });
                List<CategoryModel> model = GetEntityList<CategoryModel>(StoredProcedure.CategoryList, searchParam);
                response.IsSuccess = true;
                response.Data = model;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Common.MessageWithTitle(Resource.Error, Resource.ExceptionMessage);
            }


            return response;


        }
        public ServiceResponse BulkUpdateVisitReport(string BulkType, string EmployeeVisitIDList, string Catrgory, long loggedInUserId)
        {
            var response = new ServiceResponse();
            try
            {
                GetEntityList<NameValueData>(StoredProcedure.BulkUpdateVisitReport,
                                           new List<SearchValueData>
                                               {
                                                    new SearchValueData {Name = "BulkType", Value = Convert.ToString(BulkType)},
                                                   new SearchValueData {Name = "EmployeeVisitIDList", Value = Convert.ToString(EmployeeVisitIDList)},
                                                   new SearchValueData {Name = "loggedInID", Value = Convert.ToString(loggedInUserId)},
                                                   new SearchValueData {Name = "VisitTaskValue", Value = Convert.ToString(Catrgory)},
                                               });
                response.IsSuccess = true;

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }

            return response;
        }

        public ServiceResponse HC_GetEmployeeReportsList(long loggedInUser, string reportName, string reportDescription, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReportName", Value = Convert.ToString(reportName) });
            searchList.Add(new SearchValueData { Name = "ReportDescription", Value = Convert.ToString(reportDescription) });
            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "loggedInUser", Value = Convert.ToString(loggedInUser) });
            //searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });

            List<ReportMasterModel> data = GetEntityList<ReportMasterModel>(StoredProcedure.GetEmployeeReports, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<ReportMasterModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetPatientReportsList(long loggedInUser, string reportName, string reportDescription, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReportName", Value = Convert.ToString(reportName) });
            searchList.Add(new SearchValueData { Name = "ReportDescription", Value = Convert.ToString(reportDescription) });
            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "loggedInUser", Value = Convert.ToString(loggedInUser) });
            //searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });

            List<ReportMasterModel> data = GetEntityList<ReportMasterModel>(StoredProcedure.GetPatientReports, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<ReportMasterModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }

        public ServiceResponse HC_GetOtherReportsList(long loggedInUser, string reportName, string reportDescription, string sortDirection, string sortIndex, int pageIndex = 1, int pageSize = 10)
        {
            ServiceResponse response = new ServiceResponse();

            //Search List
            List<SearchValueData> searchList = new List<SearchValueData>();
            searchList.Add(new SearchValueData { Name = "ReportName", Value = Convert.ToString(reportName) });
            searchList.Add(new SearchValueData { Name = "ReportDescription", Value = Convert.ToString(reportDescription) });
            searchList.Add(new SearchValueData { Name = "SortExpression", Value = Convert.ToString(sortDirection) });
            searchList.Add(new SearchValueData { Name = "SortType", Value = Convert.ToString(sortIndex) });
            searchList.Add(new SearchValueData { Name = "loggedInUser", Value = Convert.ToString(loggedInUser) });
            //searchList.Add(new SearchValueData { Name = "PageSize", Value = Convert.ToString(pageSize) });

            List<ReportMasterModel> data = GetEntityList<ReportMasterModel>(StoredProcedure.GetOtherReports, searchList);

            int count = 0;
            if (data != null && data.Count > 0)
                count = data.First().Count;
            Page<ReportMasterModel> model = GetPageInStoredProcResultSet(1, 10, count, data);

            response.Data = model;
            response.IsSuccess = true;
            return response;
        }
        #endregion

        public ServiceResponse SetGroupTimesheetPage()
        {
            ServiceResponse response = new ServiceResponse();
            SetGroupTimesheetPage model = GetMultipleEntity<SetGroupTimesheetPage>(StoredProcedure.SetGroupTimesheetPage);


            response.Data = model;
            response.IsSuccess = true;
            return response;
        }


        public ServiceResponse PrioAuthorization(long ReferralID, long BillingAuthorizationID)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                //Search List
                List<SearchValueData> searchList = new List<SearchValueData>();
                searchList.Add(new SearchValueData { Name = "ReferralID", Value = Convert.ToString(ReferralID) });
                searchList.Add(new SearchValueData { Name = "BillingAuthorizationID", Value = Convert.ToString(BillingAuthorizationID) });
                PriorAuthorizationModel totalData = GetEntity<PriorAuthorizationModel>(StoredProcedure.API_UniversalPriorAuthorization, searchList);
                response.Data = totalData;
                response.IsSuccess = true;
                //return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "";

            }
            return response;
        }
    }
}
