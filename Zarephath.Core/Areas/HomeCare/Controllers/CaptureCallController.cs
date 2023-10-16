using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class CaptureCallController : BaseController
    {
        private ICaptureCallDataProvider _captureCallDataProvider;

        #region Add Call Capture Cal

        //public ActionResult AddCaptureCall()
        //{
        //    //_captureCallDataProvider = new CaptureCallDataProvider();
        //    //ServiceResponse response = _captureCallDataProvider.SetAddCaseManagerPage(caseManagerID, agencyID, agencyLocationID);
        //    //return ShowUserFriendlyPages(response) ?? View(response.Data);
        //    return View();
        //}


        public ActionResult AddCaptureCall(string id)
        {

            long capturecallID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _captureCallDataProvider = new CaptureCallDataProvider();
            ServiceResponse response = _captureCallDataProvider.AddCaptureCall(capturecallID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Physician_AddUpdate)]
        public ActionResult PartialAddCaptureCall(string id)
        {

            long capturecallID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _captureCallDataProvider = new CaptureCallDataProvider();
            ServiceResponse response = _captureCallDataProvider.AddCaptureCall(capturecallID);
            ViewBag.IsPartialView = true;
            return View("AddCaptureCall", response.Data);
        }

        [HttpPost]
        public JsonResult AddCaptureCall(CaptureCalls capturecall, string EmployeesIDs, string RoleIds,string RelatedWithPatient)
        {
            _captureCallDataProvider = new CaptureCallDataProvider();
            return Json(_captureCallDataProvider.AddCaptureCall(capturecall,EmployeesIDs,RoleIds, RelatedWithPatient, SessionHelper.LoggedInID));
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_CaptureCall_List)]
        public ActionResult CaptureCallList()
        {
            _captureCallDataProvider = new CaptureCallDataProvider();
            return View(_captureCallDataProvider.SetCaptureCallListPage().Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Physician_List)]
        public ContentResult GetCaptureCallList(SearchCaptureCallListPage searchCaptureCallListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _captureCallDataProvider = new CaptureCallDataProvider();
            return CustJson(_captureCallDataProvider.GetCaptureCallList(searchCaptureCallListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        public ContentResult DeleteCaptureCall(SearchCaptureCallListPage searchCaptureCallListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _captureCallDataProvider = new CaptureCallDataProvider();
            return CustJson(_captureCallDataProvider.DeleteCapture(searchCaptureCallListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        #endregion
        [HttpPost]
        public JsonResult ConvertToReferral(ConvertToReferralModel capturecall)
        {
            _captureCallDataProvider = new CaptureCallDataProvider();
            return Json(_captureCallDataProvider.ConvertToReferral(capturecall, SessionHelper.LoggedInID));
        }
    }
}
