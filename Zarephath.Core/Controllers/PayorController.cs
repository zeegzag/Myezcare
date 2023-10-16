using System;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;


namespace Zarephath.Core.Controllers
{
    public class PayorController : BaseController
    {
        IPayorDataProvider _iPayorDataProvider;

        #region Payor Detail

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_AddUpdate)]
        public ActionResult AddPayor(string id)
        {
            _iPayorDataProvider = new PayorDataProvider();
            long payorId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iPayorDataProvider.SetAddPayorPage(payorId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_AddUpdate)]
        public JsonResult SetAddPayorPage(string id)
        {
            _iPayorDataProvider = new PayorDataProvider();
            long payorId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _iPayorDataProvider.SetAddPayorPage(payorId);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_AddUpdate)]
        public JsonResult AddPayorDetail(AddPayorModel addPayorModel)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.AddPayorDetail(addPayorModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion

        #region  Add Service Code

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_ServiceCode_Mapping)]
        public JsonResult AddServiceCodeDetail(PayorServiceCodeMapping addPayorServiceCodeMappingModel)
        {
            _iPayorDataProvider = new PayorDataProvider();
            ServiceResponse response = _iPayorDataProvider.AddServiceCode(addPayorServiceCodeMappingModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        //SetServiceCodeMapping
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_ServiceCode_Mapping)]
        public JsonResult GetServiceCodeMappingList(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            ServiceResponse response = new ServiceResponse();
            _iPayorDataProvider = new PayorDataProvider();
            string payorId = Convert.ToString(Crypto.Decrypt(encryptedPayorId));
            response = _iPayorDataProvider.GetServiceCodeMappingList(payorId, searchServiceCodeMappingList, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_ServiceCode_Mapping)]
        public JsonResult DeleteServiceCodeMapping(string encryptedPayorId, SearchServiceCodeMappingList searchServiceCodeMappingList, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iPayorDataProvider = new PayorDataProvider();
            string payorId = Convert.ToString(Crypto.Decrypt(encryptedPayorId));
            ServiceResponse response = _iPayorDataProvider.DeleteServiceCodeMapping(payorId, searchServiceCodeMappingList, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_ServiceCode_Mapping)]
        public JsonResult GetServiceCodeList(string searchText, int pageSize)
        {
            _iPayorDataProvider = new PayorDataProvider();
            return Json(_iPayorDataProvider.GetServiceCodeList(searchText, pageSize));
        }

        #endregion

        #region List Payor Detail

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_List)]
        public ActionResult PayorList()
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _iPayorDataProvider.SetPayorListPage();
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_List)]
        public JsonResult GetPayorList(SearchPayorListPage searchPayorListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _iPayorDataProvider.GetPayorList(searchPayorListPage, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Payor_Delete)]
        public JsonResult DeletePayorList(SearchPayorListPage searchPayorListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iPayorDataProvider = new PayorDataProvider();
            var response = _iPayorDataProvider.DeletePayor(searchPayorListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion
    }
}
