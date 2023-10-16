using System;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;


namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class ServiceCodeController : BaseController
    {
        private IServiceCodeDataProvider _iServiceCodeDataProvider;

        #region ADD ServiceCode

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public ActionResult AddServiceCode(string id)
        {
            _iServiceCodeDataProvider =new ServiceCoceDataProvider();
            long serviceCodeId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iServiceCodeDataProvider.HC_SetServiceCode(serviceCodeId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public ActionResult PartialAddServiceCode(string id)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            long serviceCodeId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iServiceCodeDataProvider.HC_SetServiceCode(serviceCodeId);
            ViewBag.IsPartialView = true;
            return View("AddServiceCode", response.Data);
            //return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public JsonResult GetServiceCode(string id)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            long serviceCodeId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iServiceCodeDataProvider.HC_SetServiceCode(serviceCodeId);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public JsonResult AddServiceCode(AddServiceCodeModel addServiceCodeModel)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            ServiceResponse response = _iServiceCodeDataProvider.HC_AddServiceCode(addServiceCodeModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region List Service Code Detail

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_List)]
        public ActionResult ServiceCodeList()
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            var response = _iServiceCodeDataProvider.HC_SetServiceCodeList();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_List)]
        public JsonResult GetServiceCodeList(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            var response = _iServiceCodeDataProvider.HC_GetServiceCodeList(searchServiceCodeListPage, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public JsonResult GetModifierList(SearchModifierModel searchModifierModel)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            var response = _iServiceCodeDataProvider.HC_GetModifierList(searchModifierModel);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public JsonResult GetModifierByServiceCode(long serviceCodeID)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            var response = _iServiceCodeDataProvider.HC_GetModifierByServiceCode(serviceCodeID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public JsonResult SaveModifier(Modifier modifier)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            return Json(_iServiceCodeDataProvider.HC_SaveModifier(modifier, SessionHelper.LoggedInID));
        }
           
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public JsonResult DeleteModifier(SearchModifierModel searchModifierModel)
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            return Json(_iServiceCodeDataProvider.HC_DeleteModifier(searchModifierModel));
        }

        #endregion

        #region Delete Service Code
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_ServiceCode_AddUpdate)]
        public ContentResult DeleteServiceCode(SearchServiceCodeListPage searchServiceCodeListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iServiceCodeDataProvider = new ServiceCoceDataProvider();
            return CustJson(_iServiceCodeDataProvider.DeleteServiceCode(searchServiceCodeListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }
        #endregion
    }
}
