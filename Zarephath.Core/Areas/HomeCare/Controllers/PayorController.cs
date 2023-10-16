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
    [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
    public class PayorController : BaseController
    {
        IPayorDataProvider _iPayorDataProvider;

        #region Payor Detail

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public ActionResult AddPayor(string id)
        {
            _iPayorDataProvider = new PayorDataProvider();
            long payorId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iPayorDataProvider.HC_SetAddPayorPage(payorId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public ActionResult PartialAddPayor(string id)
        {
            _iPayorDataProvider = new PayorDataProvider();
            long payorId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iPayorDataProvider.HC_SetAddPayorPage(payorId);
            ViewBag.IsPartialView = true;
            return View("AddPayor", response.Data);
            //  return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult SetAddPayorPage(string id)
        {
            _iPayorDataProvider = new PayorDataProvider();
            long payorId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iPayorDataProvider.HC_SetAddPayorPage(payorId);
            return JsonSerializer(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult SearchPayor(HC_SearchPayorModel model)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.SearchPayor(model);
            return JsonSerializer(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult PayorEnrollment(HC_PayorEnrollmentModel model)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.PayorEnrollment(model);
            return JsonSerializer(response);
        }

        

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult AddPayorDetail(AddPayorModel addPayorModel)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_AddPayorDetail(addPayorModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region  Add Service Code

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult AddServiceCodeDetail(PayorServiceCodeMapping addPayorServiceCodeMappingModel)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_AddPayorServiceCode(addPayorServiceCodeMappingModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        //SetServiceCodeMapping
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult GetServiceCodeMappingList(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            _iPayorDataProvider = new PayorDataProvider();
            string payorId = Convert.ToString(Crypto.Decrypt(encryptedPayorId));
            response = _iPayorDataProvider.HC_GetServiceCodeMappingList(payorId, searchServiceCodeMappingList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult DeleteServiceCodeMapping(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iPayorDataProvider = new PayorDataProvider();
            string payorId = Convert.ToString(Crypto.Decrypt(encryptedPayorId));
            ServiceResponse response = _iPayorDataProvider.HC_DeleteServiceCodeMapping(payorId, searchServiceCodeMappingList, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult GetServiceCodeList(string searchText, int pageSize)
        {
            _iPayorDataProvider = new PayorDataProvider();
            return Json(_iPayorDataProvider.HC_GetServiceCodeList(searchText, pageSize));
        }

        [HttpPost]
        public JsonResult AddServiceCode(SerivceCodeModifierModel SerivceCodeModifierModel)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_AddServiceCode(SerivceCodeModifierModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        #endregion

        #region Service Code Mapping
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult AddServiceCodeDetailNew(PayorServiceCodeMapping addPayorServiceCodeMappingModel)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_AddPayorServiceCodeNew(addPayorServiceCodeMappingModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult GetServiceCodeMappingListNew(SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            _iPayorDataProvider = new PayorDataProvider();
            response = _iPayorDataProvider.HC_GetServiceCodeMappingListNew(searchServiceCodeMappingList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_ServiceCode_Mapping)]
        public JsonResult DeleteServiceCodeMappingNew(SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_DeleteServiceCodeMappingNew(searchServiceCodeMappingList, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }
        #endregion

        #region List Payor Detail

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_List)]
        public ActionResult PayorList()
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _iPayorDataProvider.HC_SetPayorListPage();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_List)]
        public JsonResult GetPayorList(SearchPayorListPage searchPayorListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _iPayorDataProvider.HC_GetPayorList(searchPayorListPage, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_Delete)]
        public JsonResult DeletePayorList(SearchPayorListPage searchPayorListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _iPayorDataProvider.HC_DeletePayor(searchPayorListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion

        #region Save Billing Setting for 837 File
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult GetPayorBillingSetting(long PayorID)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_GetPayorBillingSetting(PayorID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult SavePayorBillingSetting(PayorEdi837Setting payorEdi837Setting)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_SavePayorBillingSetting(payorEdi837Setting);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult GetAllBillingSettings(BillingSettingModel billingSettingModel)

        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_GetAllBillingSetting(billingSettingModel);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Payor_AddUpdate)]
        public JsonResult SaveBillingSetting(BillingSettingModel billingSettingModel)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.HC_SaveBillingSetting(billingSettingModel);
            return JsonSerializer(response);
        }

        #endregion
    }
}
