using System;
using System.Collections.Generic;
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
    public class PatientTimeSheetController : BaseController
    {
        private IReportDataProvider _reportDataProvider;

        [HttpGet]
        public ActionResult PatientTimeSheet()
        {
            _reportDataProvider = new ReportDataProvider();
            var model = (SetPatientTimeSheetPage)_reportDataProvider.SetPatientTimeSheetPage().Data;
            return View(model);
        }

        [HttpPost]
        public ContentResult GetPatientTimeSheetList(SearchEmployeeVisitListPage SearchPatientTimeSheetListPage, int pageIndex = 1,int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _reportDataProvider = new ReportDataProvider();
            SearchPatientTimeSheetListPage.ReferralIDs = SessionHelper.LoggedInID.ToString();
            return CustJson(_reportDataProvider.GetEmployeeVisitList(SearchPatientTimeSheetListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpGet]
        public ActionResult GeneratePatientTimeSheetPdf(string id)
        {
            CacheHelper _cacheHelper= new CacheHelper();
            long employeeVisitID = Convert.ToInt64(Crypto.Decrypt(id));

            if (employeeVisitID == 0)
                return null;
            string url = string.Format("{0}{1}{2}", _cacheHelper.SiteBaseURL, Constants.HC_GeneratePcaTimeSheet, employeeVisitID);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "PcaTimeSheet", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat)); ;
            return fileResult;

        }
    }
}
